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
    public class LuaFunctions
    {
        public static Element GetElement(string elementname)
        {
            return ElementLoader.FindElementByName(elementname);
        }
        public static bool AddRecipe(string Fabricatorid, List<string> ingredients, List<float> ingredientsamount, List<string> results, List<float> resultsamount, float recipetime=40f)
        {
            int ingreds= ingredients.Count;
            if ( ingredientsamount.Count< ingredients.Count) ingreds = ingredientsamount.Count;
            int resus = results.Count;
            if (resultsamount.Count < results.Count) resus = resultsamount.Count;
            List< ComplexRecipe.RecipeElement> ingredientslist= new List<ComplexRecipe.RecipeElement>();
            List<ComplexRecipe.RecipeElement> resultslist = new List<ComplexRecipe.RecipeElement>();
            foreach (var i in ingreds.Enumerate()) { 
                ingredientslist.Add( new ComplexRecipe.RecipeElement(ElementLoader.FindElementByName(ingredients.ElementAt(i)).tag , ingredientsamount.ElementAt(i)));
            }
            foreach (var i in resus.Enumerate())
            {
                resultslist.Add(new ComplexRecipe.RecipeElement(ElementLoader.FindElementByName(ingredients.ElementAt(i)).tag, ingredientsamount.ElementAt(i)));
            }
            Co.Add.Recipe(Fabricatorid, ingredientslist.ToArray(), resultslist.ToArray(), recipetime);
            return true;
        }
            public static bool AddResearch(string research)
        {
                Tech techtoadd;
            foreach (Tech resource in Db.Get().Techs.resources)
            {
                if (resource.Id == research) { techtoadd = resource;
            Research.Instance.Get(techtoadd).Purchased();
                    Debug.Log("[Lua] AddResearch successfully added: " + research);
                    return true;
                    }
            }
            Debug.Log("[Lua] AddResearch did not find: "+ research);
            return false;
        }
        public static string GetRandomResearch()
        {
            return Db.Get().Techs.resources.RandomElement().Id;
        }
        public static string GetRandomAvailableResearch()
        {
            int counter=0;
            while (counter++ < 100)
            {
                var tech = Db.Get().Techs.resources.RandomElement();
                if (tech.ArePrerequisitesComplete()) return tech.Id;
            }
            Debug.Log("[Lua] GetRandomAvailableResearch failed returning InteriorDecor ");
            return "InteriorDecor";
        }

        public static int GetCycleNumber()
        {
            return GameClock.Instance.GetCycle();
        }
        public static float GetCycleTime()
        {
            return GameClock.Instance.GetCurrentCycleAsPercentage();
        }

    }
}