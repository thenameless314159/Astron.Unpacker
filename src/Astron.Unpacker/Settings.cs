using System;
using System.Collections.Generic;
using System.Text;

namespace Astron.Unpacker
{
    public class Settings
    {
        public bool   Debug           { get; set; }
        public string D2PFilesFolder  { get; set; }
        public string DlmOutputFolder { get; set; }

        public Settings()
        {
        }

        public Settings(string d2PFilesFolder)
        {
            Debug = false;
            D2PFilesFolder = d2PFilesFolder;
            DlmOutputFolder = @".\dlm\";
        }

        public Settings(bool debug, string d2PFilesFolder, string dlmOutputFolder)
        {
            Debug = debug;
            D2PFilesFolder = d2PFilesFolder;
            DlmOutputFolder = dlmOutputFolder;
        }
    }
}
