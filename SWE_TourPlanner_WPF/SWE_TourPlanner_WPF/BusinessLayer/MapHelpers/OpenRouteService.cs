using Newtonsoft.Json;
using SWE_TourPlanner_WPF.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace SWE_TourPlanner_WPF.BusinessLayer.MapHelpers
{
    public class OpenRouteService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiKey;
        private readonly string _urlGeocode = "https://api.openrouteservice.org/geocode/search";
        private readonly string _urlDirections = "https://api.openrouteservice.org/v2/directions";

        static Dictionary<ETransportType, string> TransportTypeStringDictionary = new()
        {
            {ETransportType.Foot, "foot-walking"},
            {ETransportType.Bike, "cycling-regular"},
            {ETransportType.Car, "driving-car"}
        };

        public OpenRouteService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<string> GetDirectionsAsync(string startAddress, string endAddress, ETransportType transportType)
        {
            try
            {
                // Geocode addresses to get coordinates
                (double startLongitude, double startLatitude) = await GetGeocodeAsync(startAddress);
                (double endLongitude, double endLatitude) = await GetGeocodeAsync(endAddress);

                string sTransportType = TransportTypeStringDictionary.ContainsKey(transportType) ? TransportTypeStringDictionary[transportType] : TransportTypeStringDictionary[ETransportType.Car];

                //CultureInfo.InvariantCulture for punctuation
                string formattedStart = $"{startLongitude.ToString(CultureInfo.InvariantCulture)},{startLatitude.ToString(CultureInfo.InvariantCulture)}";
                string formattedEnd = $"{endLongitude.ToString(CultureInfo.InvariantCulture)},{endLatitude.ToString(CultureInfo.InvariantCulture)}";
                string requestUrl = $"{_urlDirections}/{sTransportType}?api_key={_apiKey}&start={formattedStart}&end={formattedEnd}";

                using (var response = await _httpClient.GetAsync(requestUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new HttpRequestException($"Failed to retrieve data. Status code: {response.StatusCode}, URL: {requestUrl}");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        private async Task<(double Longitude, double Latitude)> GetGeocodeAsync(string searchText)
        {
            try
            {
                string requestUrl = $"{_urlGeocode}?api_key={_apiKey}&text={searchText}";

                using (var response = await _httpClient.GetAsync(requestUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        var geocodeResult = JsonConvert.DeserializeObject<GeocodeResult>(responseData);
                        double longitude = geocodeResult.Features[0].Geometry.Coordinates[0];
                        double latitude = geocodeResult.Features[0].Geometry.Coordinates[1];
                        return (longitude, latitude);
                    }
                    else
                    {
                        throw new HttpRequestException($"Failed to retrieve data. Status code: {response.StatusCode}, URL: {requestUrl}");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}
