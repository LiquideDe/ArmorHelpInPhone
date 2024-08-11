using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Zenject;
using System;

public class ListWithNewGunsPresenter : IPresenter
{
    public event Action<SaveLoadGun> SetThisGun;
    public event Action Close;
    public event Action OpenQr;
    public event Action OpenCreationPanel;
    private ListWithNewGunView _view;
    private AudioManager _audioManager;


    [Inject]
    private void Construct(AudioManager audioManager) => _audioManager = audioManager;

    public void Initialize(ListWithNewGunView view)
    {
        _view = view;
        CheckDefaultGuns();
        Subscribe();
        LoadGuns();
    }    

    private void Subscribe()
    {
        _view.ClosePanel += CloseDown;
        _view.OpenCreationPanel += OpenCreationPanelDown;
        _view.OpenQr += OpenQrDown;
        _view.UseThisGun += SetThisGunDown;
        _view.RemoveGun += RemoveThisGun;
    }

    private void Unscribe()
    {
        _view.ClosePanel -= CloseDown;
        _view.OpenCreationPanel -= OpenCreationPanelDown;
        _view.OpenQr -= OpenQrDown;
        _view.UseThisGun -= SetThisGunDown;
        _view.RemoveGun -= RemoveThisGun;
    }

    private void LoadGuns()
    {
        List<SaveLoadGun> guns = new List<SaveLoadGun>();
        if (Application.platform == RuntimePlatform.Android)
        {
            var info = new DirectoryInfo(Application.persistentDataPath);
            var fileInfo = info.GetFiles("*.gun");
            byte[] jsonByte = null;
            foreach (FileInfo file in fileInfo)
            {
                jsonByte = File.ReadAllBytes(file.FullName);
                string jsonData = Encoding.UTF8.GetString(jsonByte);
                guns.Add(JsonUtility.FromJson<SaveLoadGun>(jsonData));
            }
        }
        else
        {
            //string filePath = Path.Combine(Application.dataPath, "StreamingAssets", name + ".json");
            var files = Directory.GetFiles($"{Application.dataPath}/StreamingAssets", "*.gun");
            foreach (string path in files)
            {
                string loadData = File.ReadAllText(path);
                guns.Add(JsonUtility.FromJson<SaveLoadGun>(loadData));
            }
        }

        foreach(SaveLoadGun gun in guns)
        {
            _view.AddGunToList(gun);
        }
    }
    private void CheckDefaultGuns()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            var info = new DirectoryInfo(Application.persistentDataPath);
            var fileInfo = info.GetFiles("*.gun");
            if (fileInfo.Length < 1)
            {
                LoadDefaultGuns();
            }
        }
        else
        {
            var files = Directory.GetFiles($"{Application.dataPath}/StreamingAssets", "*.gun");
            if (files.Length < 1)
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
        lasgun.totalClips = 2;
        lasgun.type = 3;
        lasgun.singleFire = true;

        var autopistol = new SaveLoadGun();
        autopistol.name = "Автопистолет";
        autopistol.autoFire = 6;
        autopistol.maxClip = 18;
        autopistol.semiAutoFire = 0;
        autopistol.totalClips = 2;
        autopistol.type = 1;
        autopistol.singleFire = true;

        var revolver = new SaveLoadGun();
        revolver.name = "Стаб-револьвер";
        revolver.autoFire = 0;
        revolver.maxClip = 6;
        revolver.semiAutoFire = 0;
        revolver.totalClips = 2;
        revolver.type = 0;
        revolver.singleFire = true;

        var shotgun = new SaveLoadGun();
        shotgun.name = "Дробовик";
        shotgun.autoFire = 0;
        shotgun.maxClip = 8;
        shotgun.semiAutoFire = 0;
        shotgun.totalClips = 2;
        shotgun.type = 5;
        shotgun.singleFire = true;

        var autogun = new SaveLoadGun();
        autogun.name = "Автоган";
        autogun.autoFire = 10;
        autogun.maxClip = 30;
        autogun.semiAutoFire = 3;
        autogun.totalClips = 2;
        autogun.type = 2;
        autogun.singleFire = true;

        var lGuns = new LoadGuns();
        lGuns.SaveGun(lasgun);
        lGuns.SaveGun(autopistol);
        lGuns.SaveGun(revolver);
        lGuns.SaveGun(shotgun);
        lGuns.SaveGun(autogun);
    }
    private void SetThisGunDown(SaveLoadGun gun)
    {
        _audioManager.PlayDone();
        SetThisGun?.Invoke(gun);
        Unscribe();
        Debug.Log($"Удаляем лист");
        _view.DestroyView();
    }

    private void CloseDown()
    {
        _audioManager.PlayCancel();
        Unscribe();
        _view.DestroyView();
        Close?.Invoke();
    }

    private void OpenQrDown()
    {
        _audioManager.PlayClick();
        Unscribe();
        _view.DestroyView();
        OpenQr?.Invoke();
    }

    private void OpenCreationPanelDown()
    {
        _audioManager.PlayClick();
        Unscribe();
        _view.DestroyView();
        OpenCreationPanel?.Invoke();
    }

    private void RemoveThisGun(string name)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            var info = new DirectoryInfo(Application.persistentDataPath);
            var fileInfo = info.GetFiles($"{name}.gun");
            File.Delete(fileInfo[0].FullName);
        }
        else
        {
            string filePath = Path.Combine(Application.dataPath, "StreamingAssets", name + ".gun");
            File.Delete(filePath);
        }
    }
}
