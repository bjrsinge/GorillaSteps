using GorillaSteps.Scripts;
using HarmonyLib;
using UnityEngine.UI;

namespace GorillaSteps.Patches
{
    [HarmonyPatch(typeof(VRRig), "PlayHandTapLocal")]
    class HandTapPatch
    {
        public static int stepsCount;
        public static int soundIndex;

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
