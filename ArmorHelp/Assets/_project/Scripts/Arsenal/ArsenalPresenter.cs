using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Zenject;

namespace ArmorHelp
{
    public class ArsenalPresenter : IPresenter
    {
        public event Action ReturnToArmorDown;
        public event Action AddNewGunDown;
        public event Action CalculateBallisticModifiersDown;
        public event Action CalculateWeaponModifiersDown;
        public event Action GotToShop;
        private ArsenalView _view;
        private AudioManager _audioManager;
        private GunHolder _gunFactory;
        private List<Gun> _guns = new List<Gun>();


        [Inject]
        private void Construct(AudioManager audioManager, GunHolder gunFactory)
        {
            _audioManager = audioManager;
            _gunFactory = gunFactory;
        }

        public void Initialize(ArsenalView view)
        {
            _view = view;
            _view.gameObject.SetActive(false);
            CheckAndAddUsedGun();
            Subscribe();
        }

        public void ShowArsenal() => _view.gameObject.SetActive(true);

        public void AddGun(SaveLoadGun loadGun)
        {
            ShowArsenal();
            Gun gun = _gunFactory.Get(loadGun);
            gun.SetAudioManager(_audioManager);
            gun.ChangeProperty += SaveGunWithChanges;
            gun.RemoveThisGun += RemoveThisGun;
            _view.AddGun(gun);
            _guns.Add(gun);
        }

        private void CheckAndAddUsedGun()
        {
            if (_guns.Count > 0)
                return;

            List<SaveLoadGunUsed> guns = new List<SaveLoadGunUsed>();
            if (Application.platform == RuntimePlatform.Android)
            {
                var info = new DirectoryInfo(Application.persistentDataPath);
                var fileInfo = info.GetFiles("*.gunUsed");
                byte[] jsonByte = null;
                foreach (FileInfo file in fileInfo)
                {
                    jsonByte = File.ReadAllBytes(file.FullName);
                    string jsonData = Encoding.UTF8.GetString(jsonByte);
                    guns.Add(JsonUtility.FromJson<SaveLoadGunUsed>(jsonData));
                }
            }
            else
            {
                var files = Directory.GetFiles($"{Application.dataPath}/StreamingAssets", "*.gunUsed");
                foreach (string path in files)
                {
                    string loadData = File.ReadAllText(path);
                    guns.Add(JsonUtility.FromJson<SaveLoadGunUsed>(loadData));
                }
            }

            foreach (SaveLoadGunUsed gunUsed in guns)
            {
                Gun gun = _gunFactory.Get(gunUsed);
                gun.SetAudioManager(_audioManager);
                gun.ChangeProperty += SaveGunWithChanges;
                gun.RemoveThisGun += RemoveThisGun;
                _view.AddGun(gun);
                _guns.Add(gun);
            }
        }

        private void RemoveThisGun(Gun gun)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                var info = new DirectoryInfo(Application.persistentDataPath);
                var fileInfo = info.GetFiles($"{gun.Name}.gunUsed");
                File.Delete(fileInfo[0].FullName);
            }
            else
            {
                string filePath = Path.Combine(Application.dataPath, "StreamingAssets", gun.Name + ".gunUsed");
                File.Delete(filePath);
            }
            _guns.Remove(gun);
            gun.DestroyView();
        }

        private void SaveGunWithChanges(Gun gun)
        {
            SaveLoadGunUsed saveLoad = new SaveLoadGunUsed();
            saveLoad.name = gun.Name;
            saveLoad.autoFire = gun.AutoFire;
            saveLoad.clip = gun.AmmoClip;
            saveLoad.maxClip = gun.MaxClip;
            saveLoad.semiAutoFire = gun.SemiAutoFire;
            saveLoad.singleFire = gun.SingleFire;
            saveLoad.totalAmmo = gun.TotalAmmo;
            saveLoad.type = gun.IdTypeSound;

            if (Application.platform == RuntimePlatform.Android)
            {
                string jsonDataString = JsonUtility.ToJson(saveLoad, true);
                string path = Path.Combine(Application.persistentDataPath, $"{saveLoad.name}.gunUsed");
                byte[] jsonbytes = Encoding.UTF8.GetBytes(jsonDataString);
                File.WriteAllBytes(path, jsonbytes);
            }
            else
            {
                string jsonDataString = JsonUtility.ToJson(saveLoad, true);
                string path = Path.Combine($"{Application.dataPath}/StreamingAssets", $"{saveLoad.name}.gunUsed");
                File.WriteAllText(path, jsonDataString);
            }
        }

        private void Subscribe()
        {
            _view.AddNewGun += AddNewGun;
            _view.CalculateModifiersBallistic += CalculateBallisticModifiers;
            _view.ReturnToArmor += ReturnToArmor;
            _view.CalculateModifiersWeapon += CalculateWeaponModifiers;
            _view.GoToShop += Shop;
        }

        private void Shop()
        {
            _audioManager.PlayClick();
            _view.gameObject.SetActive(false);
            GotToShop?.Invoke();
        }

        private void Unscribe()
        {
            _view.AddNewGun -= AddNewGun;
            _view.CalculateModifiersBallistic -= CalculateBallisticModifiers;
            _view.ReturnToArmor -= ReturnToArmor;
            _view.CalculateModifiersWeapon -= CalculateWeaponModifiers;
        }

        private void AddNewGun()
        {
            _audioManager.PlayClick();
            AddNewGunDown?.Invoke();
            _view.gameObject.SetActive(false);
        }

        private void ReturnToArmor()
        {
            _audioManager.PlayCancel();
            ReturnToArmorDown?.Invoke();
            _view.gameObject.SetActive(false);
        }

        private void CalculateBallisticModifiers()
        {
            _audioManager.PlayClick();
            CalculateBallisticModifiersDown?.Invoke();
            _view.gameObject.SetActive(false);
        }
        private void CalculateWeaponModifiers()
        {
            _audioManager.PlayClick();
            CalculateWeaponModifiersDown?.Invoke();
            _view.gameObject.SetActive(false);
        }
    }
}


