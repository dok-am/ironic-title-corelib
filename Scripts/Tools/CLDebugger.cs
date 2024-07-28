using UnityEngine;

namespace IT.CoreLib.Tools
{

    public static class CLDebug 
    {

#pragma warning disable CS0162 // Unreachable code detected

        /// <summary>
        /// Use it to switch on/off logs
        /// TODO: move it somewhere not static,
        /// but for now - supress warning here
        /// </summary>
        private const bool DEBUGGER_ENABLED = true;
        private const bool BOOT_DEBUGGER_ENABLED = true;

        public static void BootLog(string log)
        {
            if (!BOOT_DEBUGGER_ENABLED)
                return;


            Log(log, "BOOT", "orange");

        }

        public static void Log(string log, string section = null, string color = "white")
        {
            if (!DEBUGGER_ENABLED)
                return;

            string prefix = "";
            if (section != null)
                prefix += $"<color=orange><b>[{section}]</b></color> ";

            Debug.Log($"{prefix}{log}");
        }

#pragma warning restore CS0162 // Unreachable code detected

    }

}
