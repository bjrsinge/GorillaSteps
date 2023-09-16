using GorillaSteps.Scripts;
using HarmonyLib;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GorillaSteps.Patches
{
    [HarmonyPatch(typeof(VRRig), "PlayHandTapLocal"), HarmonyWrapSafe]
    class HandTapPatch : MonoBehaviour
    {
        public static int stepsCount;
        public static bool running = false;

        static void Postfix(VRRig __instance)
        {
            if (__instance.isOfflineVRRig)
            {
                stepsCount++;
                if (!running) { Plugin.asset.GetComponentInChildren<Text>().text = $"{stepsCount}"; }
                if (stepsCount % 10 == 0) { DataSystem.SaveData(); }
                /* if (stepsCount == 10000)
                {
                    __instance.StartCoroutine(TenKSteps());
                }
                */
            }
        }

        /*
        static IEnumerator TenKSteps()
        {
            running = true;
            Plugin.asset.GetComponentInChildren<Text>().text = "10000 steps!";
            yield return new WaitForSeconds(3f);
            Plugin.asset.GetComponentInChildren<Text>().text = $"{stepsCount}";
            running = false;          
        }
        */
    }
}
