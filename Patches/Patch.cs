using GorillaSteps.Scripts;
using HarmonyLib;
using UnityEngine.UI;

namespace GorillaSteps.Patches
{
    [HarmonyPatch(typeof(VRRig), "PlayHandTapLocal"), HarmonyWrapSafe]
    class HandTapPatch
    {
        public static int stepsCount;

        static void Postfix(VRRig __instance)
        {
            if (__instance.isOfflineVRRig)
            {
                stepsCount++;
                Plugin.asset.GetComponentInChildren<Text>().text = $"{stepsCount}";
                DataSystem.SaveData();
            }
        }
    }
}
