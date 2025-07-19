using BepInEx;
using GorillaSteps.Patches;
using GorillaSteps.Scripts;
using Photon.Pun;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace GorillaSteps
{

    [BepInDependency("dev.auros.bepinex.bepinject")]
    [BepInPlugin("bjrsinge.gorillasteps", "GorillaSteps", "1.0.7")]
    public class Plugin : BaseUnityPlugin
    {
        public GameObject asset;
        public static GameObject steps_text;
        public GameObject stats;
        public GameObject screen;
        public GameObject time_text;
        public GameObject code_text;
        public GameObject date_text;
        public GameObject playtime_text;
        public static bool init;
        public bool load;
        public bool can_load = true;
        public bool left_btn;
        public bool right_btn;
        public string date;
        public string time;
        public string code;
        public string playtime_string;
        public int playtime = 0;
        public int seconds = 0;
        public int minutes = 0;
        public int hours = 0;
        public int days = 0;

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
            HandTapPatch.steps_count = playerData.steps;

            var bundle = LoadAssetBundle("GorillaSteps.updatedwatch");
            asset = Instantiate(bundle.LoadAsset<GameObject>("thewatch7"));

            //objects
            screen = asset.transform.Find("Cube (1)").gameObject;
            steps_text = screen.transform.Find("steps_text").gameObject;
            stats = screen.transform.Find("stats").gameObject;
            time_text = stats.transform.Find("time").gameObject;
            code_text = stats.transform.Find("code").gameObject;
            playtime_text = stats.transform.Find("playtime").gameObject;
            date_text = stats.transform.Find("date").gameObject;

            //watch position
            asset.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
            asset.transform.localPosition = new Vector3(0.13f, -0.02f, -0.05f);
            asset.transform.localEulerAngles = new Vector3(GorillaLocomotion.GTPlayer.Instance.rightHandFollower.transform.rotation.x + 30f, 180f, 0f);
            asset.transform.SetParent(GorillaTagger.Instance.rightHandTransform, false);
            //screen.GetComponentInChildren<Text>().transform.localEulerAngles = new Vector3(0f, 90f, -90f);

            //set fonts
            steps_text.GetComponent<TextMeshPro>().font = GorillaTagger.Instance.offlineVRRig.playerText1.font;
            time_text.GetComponent<TextMeshPro>().font = GorillaTagger.Instance.offlineVRRig.playerText1.font;
            code_text.GetComponent<TextMeshPro>().font = GorillaTagger.Instance.offlineVRRig.playerText1.font;
            playtime_text.GetComponent<TextMeshPro>().font = GorillaTagger.Instance.offlineVRRig.playerText1.font;
            date_text.GetComponent<TextMeshPro>().font = GorillaTagger.Instance.offlineVRRig.playerText1.font;

            steps_text.GetComponent<TextMeshPro>().outlineColor = GorillaTagger.Instance.offlineVRRig.playerText2.outlineColor;
            time_text.GetComponent<TextMeshPro>().outlineColor = GorillaTagger.Instance.offlineVRRig.playerText2.outlineColor;
            code_text.GetComponent<TextMeshPro>().outlineColor = GorillaTagger.Instance.offlineVRRig.playerText2.outlineColor;
            date_text.GetComponent<TextMeshPro>().outlineColor = GorillaTagger.Instance.offlineVRRig.playerText2.outlineColor;
            playtime_text.GetComponent<TextMeshPro>().outlineColor = GorillaTagger.Instance.offlineVRRig.playerText2.outlineColor;
            code_text.GetComponent<TextMeshPro>().outlineColor = GorillaTagger.Instance.offlineVRRig.playerText2.outlineColor;

            steps_text.GetComponent<TextMeshPro>().outlineWidth = GorillaTagger.Instance.offlineVRRig.playerText2.outlineWidth;
            time_text.GetComponent<TextMeshPro>().outlineWidth = GorillaTagger.Instance.offlineVRRig.playerText2.outlineWidth;
            code_text.GetComponent<TextMeshPro>().outlineWidth = GorillaTagger.Instance.offlineVRRig.playerText2.outlineWidth;
            date_text.GetComponent<TextMeshPro>().outlineWidth = GorillaTagger.Instance.offlineVRRig.playerText2.outlineWidth;
            playtime_text.GetComponent<TextMeshPro>().outlineWidth = GorillaTagger.Instance.offlineVRRig.playerText2.outlineWidth;
            code_text.GetComponent<TextMeshPro>().outlineWidth = GorillaTagger.Instance.offlineVRRig.playerText2.outlineWidth;


            //set text
            steps_text.GetComponent<TextMeshPro>().text = playerData.steps.ToString();

            stats.SetActive(false);
            steps_text.SetActive(true);

            StartCoroutine("Playtime");
        }

        void LoadStats(bool toggle)
        {
            if (!toggle)
            {
                stats.SetActive(true);
                steps_text.SetActive(false);
            }
            else
            {
                stats.SetActive(false);
                steps_text.SetActive(true);
            }
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

        void Update()
        {
            if (init)
            {
                right_btn = ControllerInputPoller.instance.rightControllerSecondaryButton;
                //set stats
                time = DateTime.Now.ToString("t");
                date = DateTime.Now.ToString("d");
                code = PhotonNetwork.CurrentRoom != null ? PhotonNetwork.CurrentRoom.Name : "NONE";
                //change stats text
                time_text.GetComponent<TextMeshPro>().text = time;
                date_text.GetComponent<TextMeshPro>().text = date;
                code_text.GetComponent<TextMeshPro>().text = code;
                playtime_text.GetComponent<TextMeshPro>().text = playtime_string;

                if (right_btn)
                {
                    if (can_load)
                    {
                        LoadStats(load);
                        load = !load;
                        can_load = false;
                    }
                }
                else
                {
                    can_load = true;
                }
            }
        }

        IEnumerator Playtime()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                playtime += 1;
                seconds = playtime % 60;
                minutes = playtime / 60 % 60;
                hours = playtime / 3600 % 24;
                days = playtime / 86400 % 365;
                playtime_string = $"{days}d:{hours}h:{minutes}m:{seconds}s";
            }
        }
    }
}