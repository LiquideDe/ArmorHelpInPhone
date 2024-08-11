using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LoadGuns
{

    public void SaveGun(SaveLoadGun gun)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string jsonDataString = JsonUtility.ToJson(gun, true);
            string path = Path.Combine(Application.persistentDataPath, $"{gun.name}.gun");
            byte[] jsonbytes = Encoding.UTF8.GetBytes(jsonDataString);
            File.WriteAllBytes(path, jsonbytes);
        }
        else
        {
            string jsonDataString = JsonUtility.ToJson(gun, true);
            string path = Path.Combine($"{Application.dataPath}/StreamingAssets", $"{gun.name}.gun");
            File.WriteAllText(path, jsonDataString);
        }
    }
}
