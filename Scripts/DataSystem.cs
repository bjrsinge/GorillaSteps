using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GorillaSteps.Scripts
{
    internal class DataSystem
    {
        public static void SaveData()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string path = Application.streamingAssetsPath + "/GorillaStepsSaveData.bjrsinge";

            FileStream fileStream = new FileStream(path, FileMode.Create);
            PlayerData playerData = new PlayerData();

            binaryFormatter.Serialize(fileStream, playerData);
            fileStream.Close();
        }

        public static PlayerData GetPlayerData()
        {
            string path = Application.streamingAssetsPath + "/GorillaStepsSaveData.bjrsinge";
            if (File.Exists(path))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(path, FileMode.Open);

                PlayerData playerData = binaryFormatter.Deserialize(fileStream) as PlayerData;
                fileStream.Close();

                return playerData;
            }
            else
            {
#if DEBUG
                Debug.Log("No savefile found");
#endif

                SaveData();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(path, FileMode.Open);

                PlayerData playerData = binaryFormatter.Deserialize(fileStream) as PlayerData;
                fileStream.Close();

                return playerData;
            }
        }
    }
}
