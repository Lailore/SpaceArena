using System;
using JetBrains.Diagnostics;
using UnityEngine;

namespace Feito {
    public class UnityLogger : ILog {
        public bool IsEnabled(LoggingLevel level) {
            return true;
        }

        public void Log(LoggingLevel level, string message, Exception exception = null) {
            if (exception != null) {
                Debug.LogException(exception);
            } else {
                switch (level) {
                    case LoggingLevel.FATAL:
                    case LoggingLevel.ERROR:
                        Debug.LogError(message);
                        return;
                    case LoggingLevel.WARN:
                        Debug.LogWarning(message);
                        return;
                    case LoggingLevel.INFO:
                        Debug.Log(message);
                        return;
                    case LoggingLevel.VERBOSE:
                    case LoggingLevel.TRACE:
                    case LoggingLevel.OFF:
                    default:
                        return;
                }
            }
        }

        public string Category => "All";
    }
}