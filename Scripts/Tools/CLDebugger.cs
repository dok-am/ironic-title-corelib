using UnityEditor;
using UnityEngine;

namespace IT.CoreLib.Tools
{
#pragma warning disable CS0162 // Unreachable code detected
    [InitializeOnLoad]
    public static class CLDebug 
    {
        public const string DEBUGGER_LOG_KEY = "CLDEBUGGER_LOG_ENABLED";
        public const string DEBUGGER_BOOT_LOG_KEY = "CLDEBUGGER_BOOT_LOG_ENABLED";


        private static bool IsDebuggerEnabled => PlayerPrefs.GetInt(CLDebug.DEBUGGER_LOG_KEY) != 0;
        private static bool IsBootDebuggerEnabled => PlayerPrefs.GetInt(CLDebug.DEBUGGER_BOOT_LOG_KEY) != 0;


        public static void BootLog(string log)
        {
            if (!IsBootDebuggerEnabled)
                return;


            Log(log, "BOOT", "orange");

        }

        public static void Log(string log, string section = null, string color = "white")
        {
            if (!IsDebuggerEnabled)
                return;

            string prefix = "";
            if (section != null)
                prefix += $"<color=orange><b>[{section}]</b></color> ";

            Debug.Log($"{prefix}{log}");
        }
    }
#pragma warning restore CS0162 // Unreachable code detected
}
