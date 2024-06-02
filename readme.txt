To get the project up and running you need to do the following:

Config
-) instructions in the "Config_Explanation.txt"
-) All the paths in the Config are relative to the "AppDomain.CurrentDomain.BaseDirectory"

Setup the Docker DB:
-) instructions in the "DockerCommands.txt"
-) If you change the docker run command then you also need to edit the Config accordingly.

Export and Import:
-) The JSON Export exports a the selected Tour, as a json File, to the "EmportFolderPath" (Config)
-) The JSON Import imports all the Tours (as a json Files) from the "ImportFolderPath" (Config)
-) When the Tour already exists (a tour with the same ID) then the JSON Import creates a new Tour.

