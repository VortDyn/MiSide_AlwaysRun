using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace MiSide_AlwaysRun;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static ManualLogSource Log;

    internal static Harmony harmony = new Harmony("alwaysrun");

    public override void Load()
    {
        Log = base.Log;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        Harmony.DEBUG = true;

        harmony.PatchAll(typeof(Patch_PlayerMove));
    }

    internal static class Patch_PlayerMove {
        [HarmonyPatch(typeof(PlayerMove), "RunNeed")]
        [HarmonyPostfix]
        public static void Postfix_RunNeed(PlayerMove __instance)
        {
            __instance.needRun = true;
            __instance.keyRun.gameObject.SetActive(true);
            __instance.keyRun.hide = true;
        }
        
        [HarmonyPatch(typeof(PlayerMove), "Awake")]
        [HarmonyPostfix]
        public static void Postfix_Awake(PlayerMove __instance)
        {
            __instance.RunNeed(true);
        }
    }
}