using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;
using TUNING;
using UnityEngine;

namespace SlimeGrowerMod
{
	public class SlimeGrowerConfig : IBuildingConfig
	{
		private static readonly List<Storage.StoredItemModifier> PollutedWaterStorageModifiers = new List<Storage.StoredItemModifier>()
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal
		};

		public const string ID = "SlimeGrower";

		public override BuildingDef CreateBuildingDef()
		{
			string id = "SlimeGrower";
			int width = 1;
			int height = 2;
			string anim = "algaefarm_kanim";
			int hitpoints = 30;
			float construction_time = 30f;
			float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
			string[] farmable = MATERIALS.FARMABLE;
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
			EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, farmable, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tieR0, 0.2f);
			buildingDef.Floodable = false;
			buildingDef.MaterialCategory = MATERIALS.FARMABLE;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
			buildingDef.InputConduitType = ConduitType.Liquid;
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
			SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_bubbles", NOISE_POLLUTION.NOISY.TIER0);
			SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_algae_in", NOISE_POLLUTION.NOISY.TIER0);
			SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_algae_out", NOISE_POLLUTION.NOISY.TIER0);
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{

			Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go, false);
			defaultStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			/*
			Storage storage1 = go.AddOrGet<Storage>();
			storage1.showInUI = true;

			Storage storage2 = go.AddComponent<Storage>();
			storage2.capacityKg = 5f;
			storage2.showInUI = true;
			storage2.SetDefaultStoredItemModifiers(PollutedWaterStorageModifiers);
			storage2.allowItemRemoval = false;
			storage2.storageFilters = new List<Tag> { ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag };
			
			ManualDeliveryKG manualDeliveryKg1 = go.AddOrGet<ManualDeliveryKG>();
			manualDeliveryKg1.SetStorage(storage1);
			manualDeliveryKg1.requestedItemTag = new Tag("Algae");
			manualDeliveryKg1.capacity = 90f;
			manualDeliveryKg1.refillMass = 18f;
			manualDeliveryKg1.choreTypeIDHash = Db.Get().ChoreTypes.OperateFetch.IdHash;

			ManualDeliveryKG manualDeliveryKg2 = go.AddComponent<ManualDeliveryKG>();
			manualDeliveryKg2.SetStorage(storage1);
			manualDeliveryKg2.requestedItemTag = new Tag("Water");
			manualDeliveryKg2.capacity = 360f;
			manualDeliveryKg2.refillMass = 72f;
			manualDeliveryKg2.allowPause = true;
			manualDeliveryKg2.choreTypeIDHash = Db.Get().ChoreTypes.OperateFetch.IdHash;
			*/
			SlimeGrower algaeHabitat = go.AddOrGet<SlimeGrower>();
			//Operational algaeHabitato = go.AddOrGet<Operational>();

			ElementConverter elementConverter = go.AddComponent<ElementConverter>();
			elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
			{
				new ElementConverter.ConsumedElement(new Tag("Water"), 2f)
				//,new ElementConverter.ConsumedElement(new Tag("Oxygen"), 0.1f)
			};
			elementConverter.outputElements = new ElementConverter.OutputElement[2]
			{
				new ElementConverter.OutputElement(0.01f, SimHashes.ChlorineGas, 303.15f, false, 0.0f, 1f, false, 1f, byte.MaxValue, 0),
				new ElementConverter.OutputElement(1.5f, SimHashes.SlimeMold , 303.15f, true, 0.0f, 1f, false, 1f, byte.MaxValue, 0)
				//new ElementConverter.OutputElement(0.2003333f, SimHashes.ToxicSand , 303.15f, true, 0.0f, 1f, false, 1f, byte.MaxValue, 0),
				//new ElementConverter.OutputElement(0.03333f, SimHashes.Phosphorite , 303.15f, true, 0.0f, 1f, false, 1f, byte.MaxValue, 0)
			};

			ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
			conduitConsumer.conduitType = ConduitType.Liquid;
			conduitConsumer.consumptionRate = 10f;
			conduitConsumer.capacityKG = 200f;
			conduitConsumer.capacityTag = GameTags.AnyWater;
			conduitConsumer.forceAlwaysSatisfied = true;
			conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

			ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
			elementConsumer.elementToConsume = SimHashes.Oxygen;
			elementConsumer.consumptionRate = 3.333333f;
			elementConsumer.consumptionRadius = (byte)3;
			elementConsumer.showInStatusPanel = true;
			elementConsumer.sampleCellOffset = new Vector3(0.0f, 1f, 0.0f);
			elementConsumer.isRequired = true;

			ElementDropper elementDropper = go.AddComponent<ElementDropper>();
			elementDropper.emitMass = 100f;
			elementDropper.emitTag = new Tag("SlimeMold");
			elementDropper.emitOffset = new Vector3(0.0f, 0.0f, 0.0f);
			/*
			PassiveElementConsumer passiveElementConsumer = go.AddComponent<PassiveElementConsumer>();
			passiveElementConsumer.elementToConsume = SimHashes.Water;
			passiveElementConsumer.consumptionRate = 1.2f;
			passiveElementConsumer.consumptionRadius = (byte)1;
			passiveElementConsumer.showDescriptor = false;
			passiveElementConsumer.storeOnConsume = true;
			passiveElementConsumer.capacityKG = 360f;
			passiveElementConsumer.showInStatusPanel = false;
			*/
			go.AddOrGet<AnimTileable>();
			go.AddOrGet<Operational>();

			Prioritizable.AddRef(go);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);
		}
	}


	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class RadiumFuelMod_GeneratedBuildings_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Debug.Log(" === GeneratedBuildings Prefix === " + SlimeGrowerConfig.ID);
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SLIMEGROWER.NAME", "Slimer Grower");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SLIMEGROWER.DESC", "The Slime Grower grows Slime and Chlorine from water. Needs Light.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SLIMEGROWER.EFFECT", "The Slime Grower grows Slime and Chlorine from water. Needs Light.");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);

			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"Oxygen").Equals(po.category)).data;
			category.Add(SlimeGrowerConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(SlimeGrowerConfig));

		}
	} 

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class RadiumFuelMod_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["FarmingTech"]);
			ls.Add(SlimeGrowerConfig.ID);
			Database.Techs.TECH_GROUPING["FarmingTech"] = (string[])ls.ToArray();

			//Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
		}
	}



	[HarmonyPatch(typeof(KSerialization.Manager), "GetType", new Type[] { typeof(string) })]
	public static class SlimeGrowerS
	{
		[HarmonyPostfix]
		public static void GetType(string type_name, ref Type __result)
		{
			if (type_name == "SlimeGrower.SlimeGrower") {
				__result = typeof(SlimeGrower);
			}
		}
	}
}