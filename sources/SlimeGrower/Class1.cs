using Harmony;
using TUNING;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Klei;

//todo add new elements to get from melting or cooling algae-> water, -> propane -> mercury , liquid iron ->  electrum  , liquid gold -> pyrite edit electrum to superheatable
//bonus: pacu in algae out polluted water
//bonus: tropical pacu in algae out water
//bonus: last pacu in algae out copper
//bonus: increase heavy wire power to 50k
// electrum-> liquid gold pyrite -> iron

//wishlist: add trait tag in settings mod 
//Wishlist: change air cooler to work with electricity
//Wishlist: a power sensor that tells a building how much power it should consume for heating / cooling / else

namespace LavaHeaterMod
{
	#region lavaheater
	[HarmonyPatch(typeof(LogicTemperatureSensorConfig), "DoPostConfigureComplete", new System.Type[] { typeof(GameObject) })]
	public static class LogicTemperatureSensormod
	{
		public static void Postfix(LogicTemperatureSensorConfig __instance, GameObject go)
		{
			AccessTools.Field(typeof(LogicTemperatureSensor), "maxTemp").SetValue((object)go.AddOrGet<LogicTemperatureSensor>(), (object)3358.15f);

		}
	}
	[HarmonyPatch(typeof(LiquidHeaterConfig), "ConfigureBuildingTemplate", null)]
	public static class LiquidHeaterLogicMod
	{
		public static void Postfix(LiquidHeaterConfig __instance, GameObject go)
		{
			AccessTools.Field(typeof(SpaceHeater), "targetTemperature").SetValue((object)go.AddOrGet<SpaceHeater>(), (object)10000358.15f);
		}
	}
	[HarmonyPatch(typeof(LiquidHeaterConfig), "CreateBuildingDef", null)]
	public static class LiquidHeaterMod
	{
		public static void Postfix(BuildingDef __result)
		{
			//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
			__result.OverheatTemperature = 1000398.15f;
			__result.ExhaustKilowattsWhenActive = 20000f;
			__result.SelfHeatKilowattsWhenActive = 3640f;
		}
	}
	[HarmonyPatch(typeof(SpaceHeaterConfig), "ConfigureBuildingTemplate", null)]
	public static class SpaceHeaterLogicMod
	{
		public static void Postfix(SpaceHeaterConfig __instance, GameObject go)
		{
			AccessTools.Field(typeof(SpaceHeater), "targetTemperature").SetValue((object)go.AddOrGet<SpaceHeater>(), (object)10000358.15f);
		}
	}
	[HarmonyPatch(typeof(SpaceHeaterConfig), "CreateBuildingDef", null)]
	public static class SpaceHeaterMod
	{
		public static void Postfix(BuildingDef __result)
		{
			//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
			__result.OverheatTemperature = 1000398.15f;
			__result.EnergyConsumptionWhenActive = 960f;
			__result.ExhaustKilowattsWhenActive = 20000f;
			__result.SelfHeatKilowattsWhenActive = 3640f;
			__result.Floodable = true;
		}
	}
	/*
	[HarmonyPatch(typeof(WireHighWattageConfig), "DoPostConfigureComplete", null)]
	public static class WireHighWattageMod
	{
		public static void Postfix(SpaceHeaterConfig __instance, GameObject go)
		{
			AccessTools.Field(typeof(WireHighWattageConfig), "DoPostConfigureComplete").SetValue((object)go.AddOrGet<WireHighWattageConfig>(), (object)Wire.WattageRating.Max50000);
		}
	}*/
	[HarmonyPatch(typeof(LiquidPumpConfig), "CreateBuildingDef", null)]
	public static class LiquidPumpmod
	{
		public static void Postfix(BuildingDef __result)
		{
			//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
			__result.BaseMeltingPoint = 1000398.15f;
			__result.OverheatTemperature = 1000398.15f;
		}
	}

	[HarmonyPatch(typeof(GasPumpConfig), "CreateBuildingDef", null)]
	public static class GasPumpMod
	{
		public static void Postfix(BuildingDef __result)
		{
			//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
			__result.BaseMeltingPoint = 1000398.15f;
			__result.OverheatTemperature = 1000398.15f;
		}
	}
	#endregion
	#region animalmod
	[HarmonyPatch(typeof(BaseHatchConfig), "BasicRockDiet", null)]
	public static class HatchMod
	{
		public static void Postfix(List<Diet.Info> __result)
		{
			foreach (Diet.Info resu in __result)
			{
				__result.Add(new Diet.Info((TagBits)SimHashes.Diamond.CreateTag(), SimHashes.FoolsGold.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info((TagBits)SimHashes.Regolith.CreateTag(), SimHashes.Carbon.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				break;
			}
		}
	}
	[HarmonyPatch(typeof(BaseHatchConfig), "MetalDiet", null)]
	public static class MetalHatchMod
	{
		public static void Postfix(List<Diet.Info> __result)
		{
			foreach (Diet.Info resu in __result)
			{
				__result.Add(new Diet.Info((TagBits)SimHashes.SolidCarbonDioxide.CreateTag(), SimHashes.MoltenTungsten.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info((TagBits)SimHashes.SolidPropane.CreateTag(), SimHashes.SolidMercury.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info((TagBits)SimHashes.SolidMercury.CreateTag(), SimHashes.Unobtanium.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info((TagBits)SimHashes.Tungsten.CreateTag(), SimHashes.Katairite.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info((TagBits)SimHashes.Katairite.CreateTag(), SimHashes.Aerogel.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info((TagBits)SimHashes.Aerogel.CreateTag(), SimHashes.Brick.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info((TagBits)SimHashes.BleachStone.CreateTag(), SimHashes.Copper.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				break;
			}
			__result = new List<Diet.Info>();
		}
	}
	[HarmonyPatch(typeof(BaseHatchConfig), "HardRockDiet", null)]
	public static class RockHatchMod
	{
		public static void Postfix(List<Diet.Info> __result)
		{

			Diet.Info resu = null;
			foreach (Diet.Info resua in __result)
			{
				resu = resua;
				break;
			}

			__result.Clear();
			__result.Add(new Diet.Info((TagBits)SimHashes.Propane.CreateTag(), SimHashes.ChlorineGas.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
			__result.Add(new Diet.Info((TagBits)SimHashes.Carbon.CreateTag(), SimHashes.SlimeMold.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 100000f));
			__result.Add(new Diet.Info((TagBits)SimHashes.SedimentaryRock.CreateTag(), SimHashes.SlimeMold.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 100000f));
			__result.Add(new Diet.Info((TagBits)SimHashes.IgneousRock.CreateTag(), SimHashes.SlimeMold.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 100000f));
			__result.Add(new Diet.Info((TagBits)SimHashes.Obsidian.CreateTag(), SimHashes.SlimeMold.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 100000f));
			__result.Add(new Diet.Info((TagBits)SimHashes.Granite.CreateTag(), SimHashes.SlimeMold.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 100000f));

		}
	}

	[HarmonyPatch(typeof(BaseHatchConfig), "VeggieDiet", null)]
	public static class SageHatchMod
	{
		public static void Postfix(List<Diet.Info> __result)
		{

			Diet.Info resu = null;
			foreach (Diet.Info resua in __result)
			{
				resu = resua;
				break;
			}
			__result.Clear();
			__result.Add(new Diet.Info((TagBits)SimHashes.Algae.CreateTag(), SimHashes.DirtyWater.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.SlimeLung.Id, 100000f));
			__result.Add(new Diet.Info((TagBits)SimHashes.ToxicSand.CreateTag(), SimHashes.DirtyWater.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.SlimeLung.Id, 100000f));
			__result.Add(new Diet.Info((TagBits)SimHashes.Fertilizer.CreateTag(), SimHashes.Water.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0));
			__result.Add(new Diet.Info((TagBits)SimHashes.FoolsGold.CreateTag(), SimHashes.Lime.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 1000000));

		}
	}
	#endregion

	#region elementconversionmod

	[HarmonyPatch(typeof(MetalRefineryConfig), "ConfigureBuildingTemplate", null)]
	public static class RefineryFoolsgoldMod
	{
		public static void Postfix()
		{
			ComplexRecipe.RecipeElement recipeElement5 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.MoltenGold).tag, 200f);
			ComplexRecipe.RecipeElement recipeElement6 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.FoolsGold).tag, 50f);
			
			ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[1] { recipeElement5 };
			ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1] { recipeElement6 };
			string str1 = ComplexRecipeManager.MakeRecipeID("MetalRefinery", (IList<ComplexRecipe.RecipeElement>)ingredients1, (IList<ComplexRecipe.RecipeElement>)results1);
			new ComplexRecipe(str1, ingredients1, results1)
			{
				time = 40f,
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
			ComplexRecipe.RecipeElement recipeElement1 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.MoltenGold).tag, 25f);
			ComplexRecipe.RecipeElement recipeElement2 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.RefinedCarbon).tag, 200f);
			ComplexRecipe.RecipeElement recipeElement3 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.FoolsGold).tag, 50f);
			ComplexRecipe.RecipeElement recipeElement4 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Electrum).tag, 75f);
			ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[3]			{	  recipeElement1,recipeElement2, recipeElement3};
			ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1] { recipeElement4 };
			string str1 = ComplexRecipeManager.MakeRecipeID("MetalRefinery", (IList<ComplexRecipe.RecipeElement>)ingredients1, (IList<ComplexRecipe.RecipeElement>)results1);
			new ComplexRecipe(str1, ingredients1, results1) { 
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
			ComplexRecipe.RecipeElement recipeElement2 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Wolframite).tag, 50f);
			ComplexRecipe.RecipeElement recipeElement3 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Gold).tag, 50f);
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
	[HarmonyPatch(typeof(PolymerizerConfig), "CreateBuildingDef", null)]
	public static class PolimerizerMod
	{
		public static void Postfix(BuildingDef __result)
		{
			__result.MaterialCategory = new string[1]
	{
	  "FoolsGold"
	};
			__result.BaseMeltingPoint = 1000398.15f;
			__result.OverheatTemperature = 1000398.15f;
			__result.Floodable = true;
		}
	}

	[HarmonyPatch(typeof(SteamTurbineConfig), "CreateBuildingDef", null)]
	public static class SteamTurbineMod
	{
		public static void Postfix(BuildingDef __result)
		{

			//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
			__result.MaterialCategory = new string[2]
	{
	  "Tungsten",
	  "Plastic"
	};
			Debug.Log("Lavaheater Mod Loaded");
		}

	}
	#endregion

}