using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.Networking;

public class ListGuns : MonoBehaviour
{
    [SerializeField] LoadSlot loadSlotExample;
    [SerializeField] Transform content;
    [SerializeField] TextMeshProUGUI text;
    public delegate void ReturnGun(SaveLoadGun loadGun);
    ReturnGun returnGun;

    public delegate void CreateGun();
    CreateGun createGun;
    List<LoadSlot> loadSlots = new List<LoadSlot>();
    public void SetParams(ReturnGun returnGun, CreateGun createGun)
    {
        gameObject.SetActive(true);
        this.returnGun = returnGun;
        this.createGun = createGun;

        Application.logMessageReceived += HandleLog;
        CheckDefaultGuns();
        var guns = new LoadGuns().GetGuns();
        foreach(SaveLoadGun gun in guns)
        {
            loadSlots.Add(Instantiate(loadSlotExample, content));
            loadSlots[^1].SetParams(gun, GetGun, RemoveJson);
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        text.text += logString +"\n";
        //text.text += stackTrace + "\n";
    }

    private void GetGun(SaveLoadGun loadGun)
    {
        returnGun?.Invoke(loadGun);
        Cancel();
    }

    private void RemoveJson(string name)
    {
        /*
        */
        if(Application.platform == RuntimePlatform.Android)
        {
            var info = new DirectoryInfo(Application.persistentDataPath);
            var fileInfo = info.GetFiles($"{name}.gun");
            File.Delete(fileInfo[0].FullName);
            Debug.Log("Удалось удалить");
        }
        else
        {
            string filePath = Path.Combine(Application.dataPath, "StreamingAssets", name + ".gun");
            File.Delete(filePath);
        }
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }

    public void NotThisWindow()
    {
        createGun?.Invoke();
        Cancel();
    }

    private void CheckDefaultGuns()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            var info = new DirectoryInfo(Application.persistentDataPath);
            var fileInfo = info.GetFiles("*.gun");
            if(fileInfo.Length < 1)
            {
                LoadDefaultGuns();
            }
        }
        else
        {
            var files = Directory.GetFiles($"{Application.dataPath}/StreamingAssets", "*.gun");
            if(files.Length < 1)
            {
                LoadDefaultGuns();
            }
        }
    }

    private void LoadDefaultGuns()
    {
        var lasgun = new SaveLoadGun();
        lasgun.name = "Лазган";
        lasgun.autoFire = 0;
        lasgun.maxClip = 60;
        lasgun.semiAutoFire = 3;
        lasgun.totalAmmo = 180;
        lasgun.type = 3;
        lasgun.singleFire = true;

        var autopistol = new SaveLoadGun();
        autopistol.name = "Автопистолет";
        autopistol.autoFire = 6;
        autopistol.maxClip = 18;
        autopistol.semiAutoFire = 0;
        autopistol.totalAmmo = 54;
        autopistol.type = 1;
        autopistol.singleFire = true;

        var revolver = new SaveLoadGun();
        revolver.name = "Стаб-револьвер";
        revolver.autoFire = 0;
        revolver.maxClip = 6;
        revolver.semiAutoFire = 0;
        revolver.totalAmmo = 18;
        revolver.type = 0;
        revolver.singleFire = true;

        var shotgun = new SaveLoadGun();
        shotgun.name = "Дробовик";
        shotgun.autoFire = 0;
        shotgun.maxClip = 8;
        shotgun.semiAutoFire = 0;
        shotgun.totalAmmo = 24;
        shotgun.type = 5;
        shotgun.singleFire = true;

        var autogun = new SaveLoadGun();
        autogun.name = "Автоган";
        autogun.autoFire = 10;
        autogun.maxClip = 30;
        autogun.semiAutoFire = 3;
        autogun.totalAmmo = 90;
        autogun.type = 2;
        autogun.singleFire = true;

        var lGuns = new LoadGuns();
        lGuns.SetGun(lasgun);
        lGuns.SetGun(autopistol);
        lGuns.SetGun(revolver);
        lGuns.SetGun(shotgun);
        lGuns.SetGun(autogun);
    }
}
