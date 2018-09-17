using Harmony;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using ModHelper;

namespace AutoSweeperMod
{
		[HarmonyPatch(typeof(SolidTransferArmConfig), "DoPostConfigureComplete", null)]
		public static class AutoSweeperMod
		{
			public static float sweeperpower = 120f;
			public static int sweeperrange = 14;

			public static void Postfix(GameObject go)
			{
			ModHelper.ModHelper.ExposeConfig("sweeperpower", sweeperpower, "AutoSweeperMod");
			ModHelper.ModHelper.ExposeConfig("sweeperrange", sweeperrange, "AutoSweeperMod");
				go.AddOrGet<SolidTransferArm>().pickupRange = sweeperrange;
				go.AddOrGet<StationaryChoreRangeVisualizer>().range = sweeperrange;
				//configfile._streamwriter.Write("Load autosweeper");
				//Debug.Log("Autosweepermod");
				//configfile.moddebuglog("Autosweeper Loaded");
			}
		}

		[HarmonyPatch(typeof(SolidTransferArmConfig), "DoPostConfigurePreview", null)]
		public static class AutoSweeperModb
		{
			public static void Postfix(GameObject go)
			{
				go.AddOrGet<StationaryChoreRangeVisualizer>().range = AutoSweeperMod.sweeperrange;
			}
		}
		[HarmonyPatch(typeof(SolidTransferArmConfig), "CreateBuildingDef", null)]
		public static class AutoSweeperModc
		{
			public static void Postfix(BuildingDef __result)
			{
				__result.EnergyConsumptionWhenActive = AutoSweeperMod.sweeperpower;
			}
		}/*
	[HarmonyPatch(typeof(SolidTransferArmConfig), "DoPostConfigureUnderConstruction", null)]
	public static class AutoSweeperModd
	{
		public static void Postfix(GameObject go)
		{
			go.AddOrGet<StationaryChoreRangeVisualizer>().range = AutoSweeperMod.sweeperrange;
		}
	}*/
}


