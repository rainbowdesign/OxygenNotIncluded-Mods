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
    public class ScriptingCore
    {

        #region Init

        static Dictionary<int,ScriptingCore> scriptdictionary= new Dictionary<int, ScriptingCore>();
        static public Dictionary<string,Delegate> functiondict= new Dictionary<string, Delegate>();
        static public Dictionary<string, DynValue> typedict = new Dictionary<string, DynValue>();
        static int scripts=0;
        static string scriptpath = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace(".dll", "");

        public static void ScriptInit() { 
            RegisterTypes();
            new ScriptingCore("init" );
        }
        public static void OnEvent(string eventtype)
        {
            foreach (var ev in scriptdictionary) { 
                if (ev.Value.scriptexecutiontimes== eventtype)
                    ev.Value.EvalText(ev.Value.scriptCode);
            }
        }

        string scriptfolder;
        string scriptfile;
        string scriptfileraw;
        string scriptname;
        string scriptexecutiontimes;
        string scriptCode;
        public ScriptingCore( string scriptfilen, string scriptexecutiontimesi="loaded" , string scriptfolderi="",string scriptnamei = "start")
        {
            DebugLog("Load Script " + scriptfilen + " Script Execution "  + scriptexecutiontimes );
            Script.GlobalOptions.RethrowExceptionNested = true;
            scriptexecutiontimes= scriptexecutiontimesi;
            scriptfolder = scriptfolderi;
            if (scriptnamei != null) { scriptname = scriptnamei; } else { scriptname = "start"; }
            if (scriptfilen != null) { scriptfileraw = scriptfilen; } else { scriptfileraw = "start"; }
            if(string.IsNullOrEmpty( scriptfolder)) scriptfile = scriptpath +  Path.DirectorySeparatorChar + scriptfilen + ".lua";
            else         scriptfile = scriptpath+ Path.DirectorySeparatorChar+ scriptfolder + Path.DirectorySeparatorChar + scriptfilen + ".lua";
            scriptCode = File.ReadAllText(scriptfile);
            if(scriptexecutiontimes=="loaded")EvalText(scriptCode);
            else scriptdictionary.Add(scripts++, this);
            
        }

        #endregion
        #region lua
        public bool safemode= false;
        public delegate R Func<P0, P1, P2, P3, P4, P5, R>( P0 p0, P1 p1, P2 p2, P3 p3, P4 p4,  P5 p5);
        public void EvalText(string scriptCode)
        {
            DebugLog("start script " + scriptfile + " scriptname " + scriptname + " ## " + scriptCode);
            if (safemode)
            {
                try
                {
                    DoScript(scriptCode);
                }
                catch (ScriptRuntimeException ex)
                {
                    Debug.Log("[LuaException]" + ex.DecoratedMessage);
                }
            }
            else DoScript(scriptCode);
        }

        static public void RegisterTypes()
        {
            LuaInterface.RegisterType<Element>("Element");
            LuaInterface.RegisterType<ElementLoader>("ElementLoader");
        }
        public void DoScript(string scriptCode)
        {//this is the Core of the scripting engine it does the actual work.

            Script script = new Script(); //Generating the script instance
            
            UserData.RegisterAssembly(); //registering the asseblys you need to add [MoonSharpUserData] to the data you want to expose more under http://www.moonsharp.org/objects.html
            

            foreach (var i in typedict)
            {
                script.Globals.Set(i.Key,  i.Value);
            }
            script.Globals["AddResearch"] = (Func<string, bool>)LuaFunctions.AddResearch;//exposing a function so it can be called from lua syntax is: Func<parameters, returntype>
            script.Globals["AddRecipe"] = (Func<string, List<string>, List<float>, List<string>, List<float>, float, bool>)LuaFunctions.AddRecipe;
            script.Globals["GetCycleNumber"] = (Func<int>)LuaFunctions.GetCycleNumber;
            script.Globals["GetCycleTime"] = (Func<float>)LuaFunctions.GetCycleTime;
            script.Globals["GetElement"] = (Func<string, Element>)LuaFunctions.GetElement;

            //Basic Functions
            script.Globals["DebugLog"] = (Func<string, bool>)DebugLog;
            //scripthandling
            script.Globals["NewScript"] = (Func<string, string, string, string, int>)NewScript;
            script.Globals["RemoveScript"] = (Func<string, string, string, string>)RemoveScript;
            script.Globals["HasScript"] = (Func<string, string, string, bool>)HasScript;
            foreach (var i in functiondict)
            {
                script.Globals[i.Key] = i.Value;
            }

            script.DoString(scriptCode); //The command to load scriptCode as module more under http://www.moonsharp.org/scriptloaders.html
            DynValue res = script.Call(script.Globals[scriptname]); //Calling the function inside the code you should define a default value here More about dynvalue http://www.moonsharp.org/dynvalue.html

        }
        #endregion
        #region Basic Functions
        public bool DebugLog(string logentry)
        {
            Debug.Log("[LuaLog] " + logentry);
            return true;
        }

        public int NewScript( string scriptfilen, string scriptexecutiontimesi = "loaded", string scriptfolderi = "", string scriptnamei = "start")
        {
            new ScriptingCore( scriptfilen, scriptexecutiontimesi, scriptfolderi, scriptnamei); return 0;
        }

        string RemoveScript(string scriptfilen,string scriptfolderi="", string scriptnamei="start" )
        {
            foreach (var n2 in scriptdictionary)
            {
                var n = n2.Value;
                if (n.scriptfolder == scriptfolderi && n.scriptfileraw == scriptfilen && n.scriptname == scriptnamei)
                    scriptdictionary.Remove(n2.Key);
            }
            return scriptfolderi;
        }

        bool HasScript(string scriptfolderi, string scriptfolderi = "", string scriptnamei = "start")
        {
            foreach (var n in scriptdictionary.Values)
            {
                if (n.scriptfolder == scriptfolderi && n.scriptfileraw == scriptfilen && n.scriptname == scriptnamei)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

    }
}
