using Harmony;
using UnityEngine;
using System.Collections.Generic;

namespace easypowermod
{
	[HarmonyPatch(typeof(GeneratorConfig), "CreateBuildingDef", null)]
	public static class GeneratorConfigMOd
	{
		public static void Postfix(BuildingDef __result)
		{
			__result.GeneratorWattageRating = 60000f;
		}
	}
}
