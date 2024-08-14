using UnityEngine;
using UnityEditor;
using IT.CoreLib.Tools;

[InitializeOnLoad]
public static class EditorMenu 
{
    private const string CLDEBUG_LOG_MENU_NAME = "CoreLib/Enable CLDebug log";
    private const string CLDEBUG_BOOT_LOG_MENU_NAME = "CoreLib/Enable CLDebug boot log";

    static EditorMenu()
    {        
        Menu.SetChecked(CLDEBUG_LOG_MENU_NAME, PlayerPrefs.GetInt(CLDebug.DEBUGGER_LOG_KEY) != 0);
        Menu.SetChecked(CLDEBUG_BOOT_LOG_MENU_NAME, PlayerPrefs.GetInt(CLDebug.DEBUGGER_BOOT_LOG_KEY) != 0);
    }

    [MenuItem(CLDEBUG_LOG_MENU_NAME)]
    public static void EnableDebug()
    {
        int newValue = PlayerPrefs.GetInt(CLDebug.DEBUGGER_LOG_KEY) == 0 ? 1 : 0;
        PlayerPrefs.SetInt(CLDebug.DEBUGGER_LOG_KEY, newValue);
        PlayerPrefs.Save();
        Menu.SetChecked(CLDEBUG_LOG_MENU_NAME, newValue != 0);
    }

    [MenuItem(CLDEBUG_BOOT_LOG_MENU_NAME)]
    public static void EnableBootDebug()
    {
        int newValue = PlayerPrefs.GetInt(CLDebug.DEBUGGER_BOOT_LOG_KEY) == 0 ? 1 : 0;
        PlayerPrefs.SetInt(CLDebug.DEBUGGER_BOOT_LOG_KEY, newValue);
        PlayerPrefs.Save();
        Menu.SetChecked(CLDEBUG_BOOT_LOG_MENU_NAME, newValue != 0);
    }
}
