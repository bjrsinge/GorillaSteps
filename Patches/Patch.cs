using GorillaSteps.Scripts;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using GorillaLocomotion.Climbing;
using TMPro;

namespace GorillaSteps.Patches
{
    [HarmonyPatch(typeof(GorillaTagger), "ProcessHandTapping"), HarmonyWrapSafe]
    class HandTapPatch : MonoBehaviour
    {
        public static int steps_count;
        static float lastTap = -1f;

        static void Postfix(GorillaTagger __instance, bool isHandTouching, bool wasHandTouching, ref float lastTapTime, GorillaVelocityTracker handVelocityTracker)
        {
            int audioClipIndex = __instance.audioClipIndex;
            Vector3 avg_vel = handVelocityTracker.GetAverageVelocity(true, 0.03f, false);

            float current = Time.time;
            if ((!wasHandTouching && isHandTouching) && (current - lastTap > 0.2f) && (avg_vel.magnitude > 1.2f) && !(audioClipIndex == 67 || audioClipIndex == 66 || audioClipIndex == 271))
            {
                lastTap = current;
                steps_count++;
                Plugin.steps_text.GetComponent<TextMeshPro>().text = $"{steps_count}";
                if (steps_count % 100 == 0) { DataSystem.SaveData(); }
            }
        }
    }
}

