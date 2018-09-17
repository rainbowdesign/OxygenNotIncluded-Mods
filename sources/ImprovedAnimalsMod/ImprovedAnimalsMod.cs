using Harmony;
using TUNING;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Klei;

namespace AnimalsMod
{
	#region animals

	[HarmonyPatch(typeof(BaseHatchConfig), "BasicRockDiet", null)]
	public static class HatchMod
	{
		public static void Postfix(List<Diet.Info> __result)
		{
			foreach (Diet.Info resu in __result)
			{
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Diamond.CreateTag() }, SimHashes.FoolsGold.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Regolith.CreateTag() }, SimHashes.Carbon.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
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
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.SolidCarbonDioxide.CreateTag() }, SimHashes.MoltenTungsten.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Clay.CreateTag() }, SimHashes.Mercury.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Brick.CreateTag() }, SimHashes.Unobtanium.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Tungsten.CreateTag() }, SimHashes.Katairite.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				//__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Katairite.CreateTag() }, SimHashes.Aerogel.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Aerogel.CreateTag() }, SimHashes.Brick.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.BleachStone.CreateTag() }, SimHashes.Cuprite.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.MaficRock.CreateTag() }, SimHashes.GoldAmalgam.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
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
			__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Propane.CreateTag() }, SimHashes.ChlorineGas.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f ));
			__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Carbon.CreateTag() }, SimHashes.SlimeMold.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 100000f));
			__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.SedimentaryRock.CreateTag() }, SimHashes.SlimeMold.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 100000f));
			__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.IgneousRock.CreateTag() }, SimHashes.SlimeMold.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 100000f));
			__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Obsidian.CreateTag() }, SimHashes.SlimeMold.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 100000f));
			__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Granite.CreateTag() }, SimHashes.SlimeMold.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 100000f));

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
			__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Algae.CreateTag() }, SimHashes.DirtyWater.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.SlimeLung.Id, 100000f));
			__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.ToxicSand.CreateTag() }, SimHashes.DirtyWater.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.SlimeLung.Id, 100000f));
			__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Fertilizer.CreateTag() }, SimHashes.Water.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0));
			__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.FoolsGold.CreateTag() }, SimHashes.Lime.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 1000000));
		}
	}
}
#endregion