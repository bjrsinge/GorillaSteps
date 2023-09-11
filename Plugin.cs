using BepInEx;
using GorillaSteps.Patches;
using GorillaSteps.Scripts;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Utilla;

namespace GorillaSteps
{

    [BepInDependency("dev.auros.bepinex.bepinject")]
    [BepInPlugin("bjrsinge.gorillasteps", "GorillaSteps", "1.0.4")]
    public class Plugin : BaseUnityPlugin
    {
        private bool ModInitialized;
        public static GameObject asset;

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
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            PlayerData playerData = DataSystem.GetPlayerData();
            HandTapPatch.stepsCount = playerData.steps;

            var bundle = LoadAssetBundle("GorillaSteps.Bundle.watchbundleagain");
            asset = Instantiate(bundle.LoadAsset<GameObject>("thewatch3"));
            DontDestroyOnLoad(asset);

            asset.transform.SetParent(GorillaTagger.Instance.rightHandTransform, false);
            asset.AddComponent<MeshRenderer>();
            asset.GetComponent<MeshRenderer>().enabled = true;
            asset.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
            asset.transform.localPosition = new Vector3(-0.01f /* c'est bon je pense */, 0f, -0.15f);
            asset.transform.localRotation = new Quaternion(GorillaLocomotion.Player.Instance.rightHandFollower.transform.rotation.x + 30f, GorillaLocomotion.Player.Instance.rightHandFollower.transform.rotation.y, GorillaLocomotion.Player.Instance.rightHandFollower.transform.rotation.z + 90f, 0);
            asset.GetComponentInChildren<BoxCollider>().enabled = false;

            asset.GetComponentInChildren<Text>().font = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/NameTagAnchor/NameTagCanvas/Text").GetComponent<Text>().font;
            asset.GetComponentInChildren<Text>().text = HandTapPatch.stepsCount.ToString();

            ModInitialized = true;
        }

        void Update()
        {
            if (ModInitialized)
            {
                asset.GetComponentInChildren<Text>().text = $"{HandTapPatch.stepsCount}";
            }
        }
    }
}