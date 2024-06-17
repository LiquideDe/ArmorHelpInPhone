using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LoadGuns
{
    public List<SaveLoadGun> GetGuns()
    {
        List<SaveLoadGun> guns = new List<SaveLoadGun>();
        if(Application.platform == RuntimePlatform.Android)
        {
            var info = new DirectoryInfo(Application.persistentDataPath);
            var fileInfo = info.GetFiles("*.gun");
            byte[] jsonByte = null;
            foreach(FileInfo file in fileInfo)
            {
                jsonByte = File.ReadAllBytes(file.FullName);
                string jsonData = Encoding.UTF8.GetString(jsonByte);
                guns.Add(JsonUtility.FromJson<SaveLoadGun>(jsonData));
            }
        }
        else
        {
            //string filePath = Path.Combine(Application.dataPath, "StreamingAssets", name + ".json");
            var files = Directory.GetFiles($"{Application.dataPath}/StreamingAssets","*.gun");
            foreach(string path in files)
            {
                string loadData = File.ReadAllText(path);
                guns.Add(JsonUtility.FromJson<SaveLoadGun>(loadData));
            }
        }

        return guns;
    }

    public void SetGun(SaveLoadGun gun)
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
