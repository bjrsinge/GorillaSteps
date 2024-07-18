using BepInEx;
using GorillaSteps.Patches;
using GorillaSteps.Scripts;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace GorillaSteps
{

    [BepInDependency("dev.auros.bepinex.bepinject")]
    [BepInPlugin("bjrsinge.gorillasteps", "GorillaSteps", "1.0.7")]
    public class Plugin : BaseUnityPlugin
    {
        public static GameObject asset;
        public bool init;
        public GameObject btn;

        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void SetupBundle()
        {
            PlayerData playerData = DataSystem.GetPlayerData();
            HandTapPatch.stepsCount = playerData.steps;

            var bundle = LoadAssetBundle("GorillaSteps.watchbundleagain");
            asset = Instantiate(bundle.LoadAsset<GameObject>("thewatch3"));

            asset.transform.SetParent(GorillaTagger.Instance.rightHandTransform, false);
            asset.AddComponent<MeshRenderer>();
            asset.GetComponent<MeshRenderer>().enabled = true;
            asset.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
            asset.transform.localPosition = new Vector3(0.13f, -0.02f, -0.05f);
            asset.transform.localEulerAngles = new Vector3(GorillaLocomotion.Player.Instance.rightHandFollower.transform.rotation.x + 30f, 180f, 0f);
            asset.GetComponentInChildren<BoxCollider>().enabled = false;

            asset.GetComponentInChildren<Text>().font = GorillaTagger.Instance.offlineVRRig.playerText.font;
            asset.GetComponentInChildren<Text>().text = playerData.steps.ToString();
            asset.GetComponentInChildren<Text>().transform.localEulerAngles = new Vector3(0f, 90f, -90f);
        }

        public void OnDisable()
        {
            if (init) { asset.SetActive(false); }
        }

        public void OnEnable()
        {
            if (init) { asset.SetActive(true); }
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            SetupBundle();
            init = true;
        }

        public static void BtnFunc()
        {
            if (asset.GetComponentInChildren<Text>().text != HandTapPatch.stepsCount.ToString())
            {
                asset.GetComponentInChildren<Text>().text = "testing";
            }
            else if (asset.GetComponentInChildren<Text>().text != "testing")
            {
                asset.GetComponentInChildren<Text>().text = HandTapPatch.stepsCount.ToString();
            }
        }
    }
}
