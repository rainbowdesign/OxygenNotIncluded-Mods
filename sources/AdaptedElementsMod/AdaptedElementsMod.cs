using Harmony;
using UnityEngine;
using System.Collections.Generic;

namespace AdaptedElementsMod
{
	#region AdaptedElementsMod

	[HarmonyPatch(typeof(MetalRefineryConfig), "ConfigureBuildingTemplate", null)]
	public static class RefineryFoolsgoldMod
	{
		public static void Postfix()
		{
			ComplexRecipe.RecipeElement recipeElement5 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Iron).tag, 50f);
			ComplexRecipe.RecipeElement recipeElement6 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.FoolsGold).tag, 50f);

			ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[1] { recipeElement5 };
			ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1] { recipeElement6 };
			string str1 = ComplexRecipeManager.MakeRecipeID("MetalRefinery", (IList<ComplexRecipe.RecipeElement>)ingredients1, (IList<ComplexRecipe.RecipeElement>)results1);
			new ComplexRecipe(str1, ingredients1, results1)
			{
				time = 200f,
				useResultAsDescription = true,
				description = string.Format((string)STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, (object)ElementLoader.GetElement(recipeElement5.material).name, (object)ElementLoader.GetElement(recipeElement6.material).name)
			}.fabricators = new List<Tag>()
	{
	  TagManager.Create("MetalRefinery")
	};
		}
	}
	[HarmonyPatch(typeof(MetalRefineryConfig), "ConfigureBuildingTemplate", null)]
	public static class RefineryelectrumMod
	{
		public static void Postfix()
		{
			ComplexRecipe.RecipeElement recipeElement1 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.SolidCarbonDioxide).tag, 25f);
			ComplexRecipe.RecipeElement recipeElement2 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.RefinedCarbon).tag, 200f);
			ComplexRecipe.RecipeElement recipeElement3 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.FoolsGold).tag, 50f);
			ComplexRecipe.RecipeElement recipeElement4 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Electrum).tag, 75f);
			ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[3] { recipeElement1, recipeElement2, recipeElement3 };
			ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1] { recipeElement4 };
			string str1 = ComplexRecipeManager.MakeRecipeID("MetalRefinery", (IList<ComplexRecipe.RecipeElement>)ingredients1, (IList<ComplexRecipe.RecipeElement>)results1);
			new ComplexRecipe(str1, ingredients1, results1)
			{
				time = 40f,
				useResultAsDescription = true,
				description = string.Format((string)STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, (object)ElementLoader.GetElement(recipeElement4.material).name, (object)ElementLoader.GetElement(recipeElement3.material).name)
			}.fabricators = new List<Tag>()
	{
	  TagManager.Create("MetalRefinery")
	};
		}
	}
	[HarmonyPatch(typeof(GlassForgeConfig), "ConfigureBuildingTemplate", null)]
	public static class RefineryTungstenMod
	{
		public static void Postfix()
		{
			ComplexRecipe.RecipeElement recipeElement0 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.SolidPetroleum).tag, 50f);
			ComplexRecipe.RecipeElement recipeElement1 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Electrum).tag, 50f);
			ComplexRecipe.RecipeElement recipeElement2 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Aerogel).tag, 50f);
			ComplexRecipe.RecipeElement recipeElement3 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Radium).tag, 50f);
			ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[3] { recipeElement1, recipeElement3, recipeElement0 };
			ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1] { recipeElement2 };
			string str1 = ComplexRecipeManager.MakeRecipeID("MetalRefinery", (IList<ComplexRecipe.RecipeElement>)ingredients1, (IList<ComplexRecipe.RecipeElement>)results1);
			new ComplexRecipe(str1, ingredients1, results1)
			{
				time = 40f,
				useResultAsDescription = true,
				description = string.Format((string)STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, (object)ElementLoader.GetElement(recipeElement2.material).name, (object)ElementLoader.GetElement(recipeElement3.material).name)
			}.fabricators = new List<Tag>()
	{
	  TagManager.Create("GlassForge")
	};
		}

	}
	/*
	[HarmonyPatch(typeof(PolymerizerConfig), "CreateBuildingDef", null)]
	public static class PolimerizerMod
	{
		public static void Postfix(BuildingDef __result)
		{
			__result.MaterialCategory = new string[1]
	{
	  "FoolsGold"
	};
		}
	}*/

	[HarmonyPatch(typeof(SteamTurbineConfig), "CreateBuildingDef", null)]
	public static class SteamTurbineMod
	{
		public static void Postfix(BuildingDef __result)
		{
			ElementLoader.FindElementByHash(SimHashes.FoolsGold).highTempTransitionTarget = SimHashes.Radium;
			ElementLoader.FindElementByHash(SimHashes.FoolsGold).highTemp = 10000f;
			ElementLoader.FindElementByHash(SimHashes.Aerogel).materialCategory = ElementLoader.FindElementByHash(SimHashes.FoolsGold).materialCategory;
			ElementLoader.FindElementByHash(SimHashes.Aerogel).disabled = ElementLoader.FindElementByHash(SimHashes.FoolsGold).disabled;
			ElementLoader.FindElementByHash(SimHashes.Aerogel).attributeModifiers = ElementLoader.FindElementByHash(SimHashes.FoolsGold).attributeModifiers;
			ElementLoader.FindElementByHash(SimHashes.Aerogel).name = "Iridium";
			ElementLoader.FindElementByHash(SimHashes.Aerogel).oreTags = ElementLoader.FindElementByHash(SimHashes.FoolsGold).oreTags;
			ElementLoader.FindElementByHash(SimHashes.Aerogel).tag = ElementLoader.FindElementByHash(SimHashes.FoolsGold).tag;
			ElementLoader.FindElementByHash(SimHashes.Aerogel).thermalConductivity = 0.000001f;
			ElementLoader.FindElementByHash(SimHashes.Aerogel).highTemp = 1000000f;

			//ElementLoader.FindElementByHash(SimHashes.Cuprite).highTempTransitionTarget = SimHashes.
			//ElementLoader.FindElementByHash(SimHashes.Wolframite).lowTemp = 150f;
			//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
			__result.MaterialCategory = new string[2]
	{
	  "Iridium",
	  "Plastic"
	};
			Debug.Log("Lavaheater Mod Loaded");
		}

	}
	#endregion

}
