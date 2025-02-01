using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Zenject;
using System.Collections.Generic;

namespace ArmorHelp
{
    public class NewGunPanel : MonoBehaviour
    {
        [SerializeField] TMP_InputField inputName, inputTotalClip, inputClipAmmo, inputSemiAutoFire, inputAutoFire;
        [SerializeField] Toggle toggleSingle;
        [SerializeField] TMP_Dropdown dropdown;
        [SerializeField] Button _buttonDone, _buttonClose;
        public event Action<SaveLoadGun> ReturnNewGun;
        public event Action ClosePanel;
        private AudioManager _audioManager;
        private Dictionary<TypeGun, string> _translateTypeGun = new Dictionary<TypeGun, string>()
    {
        {TypeGun.Bolt, "Болтерное" },
        {TypeGun.Bullet, "Пулевое" },
        {TypeGun.Fire, "Огненное" },
        {TypeGun.Laser, "Лазерное" },
        {TypeGun.Revolver, "Револьвер" },
        {TypeGun.Shotgun, "Дробовик" },
        {TypeGun.Pistol, "Пистолет" },
        {TypeGun.Plasma, "Плазма" },
        {TypeGun.Rad, "Рад" },
        {TypeGun.Melta, "Мельта" },
        {TypeGun.Grav, "Грав" },
        {TypeGun.Electro, "Электро" }
    };

        [Inject]
        private void Construct(AudioManager audioManager) => _audioManager = audioManager;

        private void OnDisable()
        {
            dropdown.ClearOptions();
            _buttonClose.onClick.RemoveAllListeners();
            _buttonDone.onClick.RemoveAllListeners();
        }

        public void Initialize()
        {
            List<string> options = new List<string>();
            foreach (TypeGun typeGun in Enum.GetValues(typeof(TypeGun)))
            {
                options.Add(_translateTypeGun[typeGun]);
            }

            dropdown.AddOptions(options);
            _buttonClose.onClick.AddListener(Cancel);
            _buttonDone.onClick.AddListener(GunIsDone);
        }

        private void GunIsDone()
        {
            if (inputName.text.Length > 0 && inputClipAmmo.text.Length > 0 && inputTotalClip.text.Length > 0)
            {
                _audioManager.PlayDone();
                SaveLoadGun gun = new SaveLoadGun();
                gun.name = inputName.text;
                int.TryParse(inputTotalClip.text, out gun.totalClips);
                int.TryParse(inputClipAmmo.text, out gun.maxClip);
                int.TryParse(inputSemiAutoFire.text, out gun.semiAutoFire);
                int.TryParse(inputAutoFire.text, out gun.autoFire);
                gun.singleFire = toggleSingle.isOn;
                gun.type = dropdown.value;
                new LoadGuns().SaveGun(gun);
                ReturnNewGun?.Invoke(gun);
                Destroy(gameObject);
            }
            else
                _audioManager.PlayWarning();
        }

        private void Cancel()
        {
            _audioManager.PlayCancel();
            ClosePanel?.Invoke();
            Destroy(gameObject);
        }
    }
}

