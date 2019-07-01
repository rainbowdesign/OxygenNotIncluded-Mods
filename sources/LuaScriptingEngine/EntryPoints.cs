using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using Harmony;
using MoonSharp.Interpreter;

namespace LuaScriptingEngine
{
    public class EntryPoints
    {
        public static bool loaded= false;
        [HarmonyPatch(typeof(MainMenu), "OnSpawn", null)]
        public static class Loadhook
        {
            public static void Postfix()
            {
                if (!loaded)
                {
                    ScriptingCore.ScriptInit();
                    ScriptingCore.OnEvent("loaded");
                    loaded=true;
                }
            }
        }
        [HarmonyPatch(typeof(WattsonMessage), "OnActivate", null)] //Suggestions on how to do this properly?
        public static class OnGameIsStarting
        {
            public static void Postfix()
            {
                ScriptingCore.OnEvent("maploaded");
            }
        }
        [HarmonyPatch(typeof(Game), "LateUpdate", null)]
        public static class Updatehook
        {
            public static void Postfix()
            {
                ScriptingCore.OnEvent("update");
            }
        }
    }
}