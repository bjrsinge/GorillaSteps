using GorillaSteps.Scripts;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace GorillaSteps.Patches
{
    [HarmonyPatch(typeof(VRRig), "PlayHandTapLocal"), HarmonyWrapSafe]
    class HandTapPatch : MonoBehaviour
    {
        public static int stepsCount;

        static void Postfix(VRRig __instance, int soundIndex)
        {          
            if (__instance.isOfflineVRRig)
            {
                Debug.Log(soundIndex);
                if (!(soundIndex == 67 || soundIndex == 66 || soundIndex == 270))
                {
                    stepsCount++;
                    Plugin.steps_text.GetComponent<Text>().text = $"{stepsCount}";
                    if (stepsCount % 10 == 0) { DataSystem.SaveData(); }
                }
            }
        }
    }
}

