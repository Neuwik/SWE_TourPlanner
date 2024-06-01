using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_TourPlanner_WPF.Models
{
    public class TourPlannerConfig
    {
        private static string configPath = "Configs/config_TourPlanner.json";
        public static string ConfigFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configPath);

        public DataAccessLayerConfig DataAccessLayer { get; set; }
        public BusinessLayerConfig BusinessLayer { get; set; }
        public ViewModelConfig ViewModel { get; set; }
    }

    public class DataAccessLayerConfig
    {
        public string ConnectionString { get; set; }
    }

    public class BusinessLayerConfig
    {
        public string ReportFolderPath { get; set; }
        public string ExportFolderPath { get; set; }
        public string ImportFolderPath { get; set; }
        public string ORS_ApiKey { get; set; }
    }

    public class ViewModelConfig
    {
        public string DirectionsFilePath { get; set; }
        public string LeafletFilePath { get; set; }
    }
}
