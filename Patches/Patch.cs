using GorillaSteps.Scripts;
using HarmonyLib;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GorillaSteps.Patches
{
    [HarmonyPatch(typeof(VRRig), "PlayHandTapLocal"), HarmonyWrapSafe]
    class HandTapPatch
    {
        public static int stepsCount;
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void Prefix(VRRig __instance)
        {
            if (__instance.isOfflineVRRig)
            {
                stepsCount++;
                Debug.Log("patch ran");
                DataSystem.SaveData();
            }
        }
    }
}
