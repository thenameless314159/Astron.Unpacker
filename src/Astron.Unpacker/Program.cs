using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Astron.Files;
using Astron.IoC;
using Astron.Logging;
using Astron.Unpacker.Logging;
using Astron.Unpacker.Managers;

using Newtonsoft.Json;

namespace Astron.Unpacker
{
    // TODO: handle exceptions on files not loaded or not existing
    public class Program
    {
        private const string _settingsPath            = @".\appsettings.json";
        private const int    _errorBadArguments       = 0xA0;
        private const int    _errorFileNotFound       = 0x3;

        static void Main(string[] args)
        {
            DrawAscii();
            var mainSw = Stopwatch.StartNew();

            try
            {
                #region Settings load logic
                Settings settings;
                if (args.Length > 0)
                {
                    if (!TryParseArgs(args, out settings)) Environment.Exit(_errorBadArguments);
                    CreateSettingsFile(settings);
                }
                else if(!TryLoadSettings(out settings)) Environment.Exit(_errorFileNotFound);

                var loggerBuilder = Logger.CreateBuilder()
                    .Register(new ConsoleOutputStrategy(false));

                if(settings.Debug)
                {
                    if (!Directory.Exists(@".\logs\"))
                        Directory.CreateDirectory(@".\logs\");

                    loggerBuilder.Register(new FileStrategy($@"logs\log-{DateTime.Now:dd-MM-yyyy-hh_mm_ss}.txt", 
                            LogLevel.Error));
                }
                ServiceLocator.Logger = loggerBuilder.Build();
                #endregion

                var container = Startup.ConfigureServices(new ContainerBuilder().Register(settings));
                var d2PFilePaths = Directory.GetFiles(settings.D2PFilesFolder, "*.d2p");
                var d2PFileManager = new D2PManager(container);

                d2PFileManager.UnpackAll(d2PFilePaths);
            }
            catch (Exception e)
            {
                ServiceLocator.Logger.Log<Program>(LogLevel.Fatal, e.ToString());
            }
            ServiceLocator.Logger.Log<Program>(LogLevel.Info, $"Execution took {mainSw.Elapsed:g}.");
            ServiceLocator.Logger.Save();
            Console.WriteLine();
            Console.WriteLine("--- Press any key to exit");
            Console.ReadLine();
        }

        static bool TryParseArgs(string[] args, out Settings settings) // Todo: add ask logic
        {
            settings = default;
            var filesPath = args[0];

            var pathValidation = new PathValidation();
            if (!pathValidation.IsValid(filesPath)) return false;
            if (filesPath.Last() != '\\') filesPath = $@"{filesPath}\"; // reformat
            settings = new Settings(filesPath);
            return true;
        }

        static bool TryLoadSettings(out Settings settings)
        {
            settings = default;
            if (!File.Exists(_settingsPath)) return false;

            var jsonText = File.ReadAllText(_settingsPath);
            settings = JsonConvert.DeserializeObject<Settings>(jsonText);
            return true;
        }

        static void CreateSettingsFile(Settings settings)
        {
            var jsonText = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(_settingsPath, jsonText);
        }

        static void DrawAscii()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"_______       _____                     
___    |________  /____________________ 
__  /| |_  ___/  __/_  ___/  __ \_  __ \
_  ___ |(__  )/ /_ _  /   / /_/ /  / / /
/_/  |_/____/ \__/ /_/    \____//_/ /_/ ");
            Console.WriteLine("GameData unpacker by NamelessK1NG - github.com/thenameless314159");
            Console.WriteLine();
        }
    }
}
