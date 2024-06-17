using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;
using System.IO;
using System.Text;

public class ArmorPresenter : IPresenter
{
    public event Action GoToArsenal;
    public event Action<SaveLoadArmor> GoToDamagePanel;
    private ArmorView _view;
    private AudioManager _audioManager;
    private int _wounds = 5;
    private SaveLoadArmor armor;

    [Inject]
    private void Construct(AudioManager audioManager) => _audioManager = audioManager;

    public void Initialize(ArmorView view)
    {
        _view = view;

        if (Application.platform == RuntimePlatform.Android)
        {
            var info = new DirectoryInfo(Application.persistentDataPath);
            var fileInfo = info.GetFiles("*.armor");
            byte[] jsonByte = null;
            foreach (FileInfo file in fileInfo)
            {
                jsonByte = File.ReadAllBytes(file.FullName);
                string jsonData = Encoding.UTF8.GetString(jsonByte);
                armor = JsonUtility.FromJson<SaveLoadArmor>(jsonData);
                _wounds = armor.wounds;
            }
        }
        else
        {
            var files = Directory.GetFiles($"{Application.dataPath}/StreamingAssets", "*.armor");
            foreach (string path in files)
            {
                string loadData = File.ReadAllText(path);
                armor = JsonUtility.FromJson<SaveLoadArmor>(loadData);
                _wounds = armor.wounds;
            }
        }
        Subscribe();
        _view.LoadArmor(armor);
    }

    public void ShowView() 
    {
        _audioManager.PlayClick();
        _view.gameObject.SetActive(true);
    } 

    public void TakeDamage(int damage)
    {
        _wounds -= damage;
        _view.SetWound(_wounds);
    }

    private void Subscribe()
    {
        _view.GoToDamagePanel += OpenPanelDamageDown;
        _view.PlusWound += PlusWound;
        _view.MinusWound += MinusWound;
        _view.SaveArmor += SaveArmor;
        _view.GoToArsenal += OpenArsenal;
    }

    private void Unscribe()
    {
        _view.GoToDamagePanel -= OpenPanelDamageDown;
        _view.PlusWound -= PlusWound;
        _view.MinusWound -= MinusWound;
        _view.SaveArmor -= SaveArmor;
        _view.GoToArsenal -= OpenArsenal;
    }

    private void OpenPanelDamageDown(SaveLoadArmor armor)
    {
        _audioManager.PlayClick();
        _view.gameObject.SetActive(false);
        armor.wounds = _wounds;
        GoToDamagePanel?.Invoke(armor);
    }

    private void OpenArsenal()
    {
        _audioManager.PlayClick();
        _view.gameObject.SetActive(false);
        GoToArsenal?.Invoke();
    }

    private void PlusWound()
    {
        _audioManager.PlayClick();
        _wounds++;
        _view.SetWound(_wounds);
    }

    private void MinusWound()
    {
        _audioManager.PlayClick();
        _wounds--;
        _view.SetWound(_wounds);
    }

    private void SaveArmor(SaveLoadArmor armor)
    {
        _audioManager.PlayDone();
        if (Application.platform == RuntimePlatform.Android)
        {
            string jsonDataString = JsonUtility.ToJson(armor, true);
            string path = Path.Combine(Application.persistentDataPath, $"armor.armor");
            byte[] jsonbytes = Encoding.UTF8.GetBytes(jsonDataString);
            File.WriteAllBytes(path, jsonbytes);
        }
        else
        {
            string jsonDataString = JsonUtility.ToJson(armor, true);
            string path = Path.Combine($"{Application.dataPath}/StreamingAssets", $"armor.armor");
            File.WriteAllText(path, jsonDataString);
        }
    }
}
