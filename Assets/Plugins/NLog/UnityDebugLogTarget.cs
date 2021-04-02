using NLog;
using NLog.Targets;
using UnityEngine;

namespace Plugins.NLog {
    [Target("UnityDebugLog")]
    public class UnityDebugLogTarget : TargetWithLayout {
        protected override void Write(LogEventInfo logEvent) {
            var logMessage = RenderLogEvent(Layout, logEvent);
            if (logEvent.Level <= LogLevel.Info)
                Debug.Log(logMessage);
            else if (logEvent.Level == LogLevel.Warn)
                Debug.LogWarning(logMessage);
            else
                Debug.LogError(logMessage);
        }
    }
}