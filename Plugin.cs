using BepInEx;
using GorillaSteps.Patches;
using GorillaSteps.Scripts;
using Photon.Pun;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using TMPro;
using Unity.XR.Oculus.Input;
using UnityEngine;

namespace GorillaSteps
{

    //[BepInDependency("dev.auros.bepinex.bepinject")]
    [BepInPlugin("com.bjrsinge.gorillatag.gorillasteps", "GorillaSteps", "1.1.9")]
    public class Plugin : BaseUnityPlugin
    {
        public GameObject asset, stats, screen;
        public static TextMeshPro steps_text;
        public TextMeshPro time_text, code_text, date_text, playtime_text;
        public static bool init;
        public bool load, can_load = true, right_btn;
        public string date, time, last_date, last_time, code, last_code, playtime_string;
        public int playtime, seconds, minutes, hours, days;
        public float timer;

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
            steps_text = screen.transform.Find("steps_text").GetComponent<TextMeshPro>();
            stats = screen.transform.Find("stats").gameObject;
            time_text = stats.transform.Find("time").GetComponent<TextMeshPro>();
            code_text = stats.transform.Find("code").GetComponent<TextMeshPro>();
            playtime_text = stats.transform.Find("playtime").GetComponent<TextMeshPro>();
            date_text = stats.transform.Find("date").GetComponent<TextMeshPro>();

            //watch position
            asset.transform.SetParent(GorillaTagger.Instance.rightHandTransform, false);
            asset.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
            if (Info.Location.StartsWith("C:\\Program Files\\Oculus\\Software\\Software\\another-axiom-gorilla-tag")) 
            {
                asset.transform.localPosition = new Vector3(0.13f, -0.02f, -0.01f);
                asset.transform.localEulerAngles = new Vector3(85f, 180f, 0f);
            }
            else
            {
                asset.transform.localPosition = new Vector3(0.13f, -0.02f, -0.05f);
                asset.transform.localEulerAngles = new Vector3(30f, 180f, 0f);
            }

            //set fonts
            steps_text.font = GorillaTagger.Instance.offlineVRRig.playerText1.font;
            time_text.font = GorillaTagger.Instance.offlineVRRig.playerText1.font;
            code_text.font = GorillaTagger.Instance.offlineVRRig.playerText1.font;
            playtime_text.font = GorillaTagger.Instance.offlineVRRig.playerText1.font;
            date_text.font = GorillaTagger.Instance.offlineVRRig.playerText1.font;

            //set text
            steps_text.text = playerData.steps.ToString();

            stats.SetActive(false);
            steps_text.gameObject.SetActive(true);

            StartCoroutine("Playtime");
        }

        void LoadStats(bool toggle)
        {
            if (!toggle)
            {
                stats.SetActive(true);
                steps_text.gameObject.SetActive(false);
            }
            else
            {
                stats.SetActive(false);
                steps_text.gameObject.SetActive(true);
            }
        }

        public void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
            if (init) { asset.SetActive(false); }
        }

        public void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            if (init) { asset.SetActive(true); }
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            SetupBundle();
            init = true;
        }

        void Update()
        {
            if (!init) return;

            right_btn = ControllerInputPoller.instance.rightControllerSecondaryButton;

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
                playtime_text.text = playtime_string;

                time = DateTime.Now.ToString("t");
                date = DateTime.Now.ToString("d");
                code = PhotonNetwork.CurrentRoom != null ? PhotonNetwork.CurrentRoom.Name : "NONE";

                if (time != last_time)
                {
                    time_text.text = time;
                    last_time = time;
                }

                if (date != last_date)
                {
                    date_text.text = date;
                    last_date = date;
                }

                if (code != last_code)
                {
                    code_text.text = code;
                    last_code = code;
                }
            }
        }
    }
}