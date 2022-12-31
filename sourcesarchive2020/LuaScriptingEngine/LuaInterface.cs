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
    public static class LuaInterface {
        public static void AddFunction(string Functionnameinlua, Delegate Function)
        {
            ScriptingCore.functiondict.Add(Functionnameinlua,Function);
        }
        public static void RegisterType<T>(string typename) where T :  new()
        {
            UserData.RegisterType<T>();
            DynValue ele = UserData.Create(new T());
            if(ScriptingCore.typedict.ContainsKey(typename)) Debug.Log("The type "+ typename + " is already registered skipping it! "  );
            else ScriptingCore.typedict.Add(typename,ele);
        }
    }

}