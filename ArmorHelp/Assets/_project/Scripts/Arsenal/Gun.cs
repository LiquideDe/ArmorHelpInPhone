using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;

namespace ArmorHelp
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText, clipText, totalAmmoText;
        [SerializeField] private MyToggle _togglePrefab;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private TMP_InputField inputFieldAmmo;
        [SerializeField] private GameObject panelWithAmmo;
        [SerializeField] private Button _buttonRemove, _buttonReload, _buttonFire, _buttonAddAmmo, _buttonCloseAddAmmo, _buttonConfirmAddAmmo;

        public event Action<Gun> ChangeProperty, RemoveThisGun;
        private int _ammoClip, _maxClip, _totalAmmo, _semiAutoFire, _autoFire, _arrayBullet, _idTypeSound;
        private bool singleFire;
        private string _name;

        private List<MyToggle> rof = new List<MyToggle>();
        private bool _isFiring;
        private AudioManager _audioManager;

        public string Name => _name;

        public int AmmoClip => _ammoClip;
        public int MaxClip => _maxClip;
        public int TotalAmmo => _totalAmmo;
        public int SemiAutoFire => _semiAutoFire;
        public int AutoFire => _autoFire;
        public int ArrayBullet => _arrayBullet;
        public bool SingleFire => singleFire;
        public int IdTypeSound => _idTypeSound;

        private void OnEnable()
        {
            _buttonRemove.onClick.AddListener(RemovePressed);
            _buttonReload.onClick.AddListener(Reload);
            _buttonFire.onClick.AddListener(Shoot);
            _buttonAddAmmo.onClick.AddListener(ShowAddAmmo);
            _buttonCloseAddAmmo.onClick.AddListener(CloseAddAmmo);
            _buttonConfirmAddAmmo.onClick.AddListener(ConfirmAddAmmo);
        }

        private void OnDisable()
        {
            _buttonRemove.onClick.RemoveAllListeners();
            _buttonReload.onClick.RemoveAllListeners();
            _buttonFire.onClick.RemoveAllListeners();
            _buttonAddAmmo.onClick.RemoveAllListeners();
            _buttonCloseAddAmmo.onClick.RemoveAllListeners();
            _buttonConfirmAddAmmo.onClick.RemoveAllListeners();
        }

        public void Initialize(SaveLoadGun loadGun)
        {
            gameObject.SetActive(true);
            nameText.text = loadGun.name;
            _name = loadGun.name;
            _totalAmmo = loadGun.totalClips * loadGun.maxClip;
            _maxClip = loadGun.maxClip;
            _semiAutoFire = loadGun.semiAutoFire;
            _autoFire = loadGun.autoFire;
            _ammoClip = _maxClip;
            _idTypeSound = loadGun.type;
            singleFire = loadGun.singleFire;
            if (singleFire)
            {
                rof.Add(Instantiate(_togglePrefab, toggleGroup.transform));
                rof[^1].Text.text = "Одиночный режим";
                rof[^1].Id = 1;
                rof[^1].gameObject.SetActive(true);
            }
            if (_semiAutoFire > 0)
            {
                rof.Add(Instantiate(_togglePrefab, toggleGroup.transform));
                rof[^1].Text.text = "Полу автомат";
                rof[^1].Id = _semiAutoFire;
                rof[^1].gameObject.SetActive(true);
            }
            if (_autoFire > 0)
            {
                rof.Add(Instantiate(_togglePrefab, toggleGroup.transform));
                rof[^1].Text.text = "Автомат";
                rof[^1].Id = _autoFire;
                rof[^1].gameObject.SetActive(true);
            }
            if (_maxClip == 0)
            {
                _buttonReload.gameObject.SetActive(false);
                _buttonAddAmmo.gameObject.SetActive(false);
            }
            UpdateText();
            ChangeProperty?.Invoke(this);
        }

        public void Initialize(SaveLoadGunUsed loadGun)
        {
            gameObject.SetActive(true);
            nameText.text = loadGun.name;
            _name = loadGun.name;
            _totalAmmo = loadGun.totalAmmo;
            _maxClip = loadGun.maxClip;
            _semiAutoFire = loadGun.semiAutoFire;
            _autoFire = loadGun.autoFire;
            _ammoClip = loadGun.clip;

            singleFire = loadGun.singleFire;
            _idTypeSound = loadGun.type;
            if (singleFire)
            {
                rof.Add(Instantiate(_togglePrefab, toggleGroup.transform));
                rof[^1].Text.text = "Одиночный режим";
                rof[^1].Id = 1;
                rof[^1].gameObject.SetActive(true);
            }
            if (_semiAutoFire > 0)
            {
                rof.Add(Instantiate(_togglePrefab, toggleGroup.transform));
                rof[^1].Text.text = "Полу автомат";
                rof[^1].Id = _semiAutoFire;
                rof[^1].gameObject.SetActive(true);
            }
            if (_autoFire > 0)
            {
                rof.Add(Instantiate(_togglePrefab, toggleGroup.transform));
                rof[^1].Text.text = "Автомат";
                rof[^1].Id = _autoFire;
                rof[^1].gameObject.SetActive(true);
            }
            UpdateText();
        }

        public void SetAudioManager(AudioManager audioManager) => _audioManager = audioManager;

        public void DestroyView() => Destroy(gameObject);

        private void Reload()
        {
            if (_ammoClip == 0 && _maxClip <= _totalAmmo)
            {
                _totalAmmo -= _maxClip;
                _ammoClip = _maxClip;
                _audioManager.PlayReload(_idTypeSound);
            }
            else if (_ammoClip > 0 && _maxClip <= _totalAmmo)
            {
                _totalAmmo -= (_maxClip - _ammoClip);
                _ammoClip = _maxClip;
                _audioManager.PlayReload(_idTypeSound);
            }
            else if (_totalAmmo > 0 && _maxClip - _ammoClip <= _totalAmmo)
            {
                _totalAmmo -= (_maxClip - _ammoClip);
                _ammoClip = _maxClip;
                _audioManager.PlayReload(_idTypeSound);
            }
            else if (_totalAmmo > 0)
            {
                _ammoClip = _totalAmmo;
                _totalAmmo = 0;
                _audioManager.PlayReload(_idTypeSound);
            }
            UpdateText();
            ChangeProperty?.Invoke(this);
        }

        private void Shoot()
        {
            if (!_isFiring)
            {
                _arrayBullet = toggleGroup.ActiveToggles().FirstOrDefault().GetComponent<MyToggle>().Id;
                _isFiring = true;
                StartCoroutine(Shooting(_arrayBullet));
            }
        }

        private void UpdateText()
        {
            if (_maxClip == 0)
            {
                clipText.text = $"Беск.";
                totalAmmoText.text = $"Беск.";
            }
            else
            {
                clipText.text = $"{_ammoClip}";
                totalAmmoText.text = $"{_totalAmmo}";
            }

        }

        IEnumerator Shooting(int bulletsToShoot)
        {
            float timeDelay = 1.5f / bulletsToShoot;
            for (int i = 0; i < bulletsToShoot; i++)
            {
                if (_ammoClip == 0 && _maxClip == 0)
                {
                    _audioManager.PlayShoot(_idTypeSound);
                }
                else if (_ammoClip == 0)
                {
                    _audioManager.PlayEmpty(_idTypeSound);
                    break;
                }
                else
                {
                    _ammoClip--;
                    UpdateText();
                    _audioManager.PlayShoot(_idTypeSound);
                }
                yield return new WaitForSeconds(timeDelay);
            }

            ChangeProperty?.Invoke(this);
            _isFiring = false;
        }

        private void ShowAddAmmo() => panelWithAmmo.SetActive(true);

        private void CloseAddAmmo()
        {
            panelWithAmmo.SetActive(false);
            inputFieldAmmo.text = "";
        }

        private void ConfirmAddAmmo()
        {
            int.TryParse(inputFieldAmmo.text, out int ammo);
            _totalAmmo += ammo;
            UpdateText();
            CloseAddAmmo();
            ChangeProperty?.Invoke(this);
        }

        private void RemovePressed() => RemoveThisGun?.Invoke(this);

    }
}

