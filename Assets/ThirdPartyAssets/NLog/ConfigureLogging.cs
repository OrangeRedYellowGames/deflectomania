using NLog;
using NLog.Config;
using UnityEngine;

// Adapted from https://github.com/NLog/NLog/issues/4112
namespace ThirdPartyAssets.NLog {
    public static class Bootstrap {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ConfigureLogging() {
            var config = new LoggingConfiguration();

            // Targets where to log to:
            var unityConsole = new UnityDebugLogTarget() {
                Name = "Unity", Layout = "[${longdate}] [${level:uppercase=true}] [${logger}] ${message}"
            };

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, unityConsole);

            // Apply config           
            LogManager.Configuration = config;
        }
    }
}