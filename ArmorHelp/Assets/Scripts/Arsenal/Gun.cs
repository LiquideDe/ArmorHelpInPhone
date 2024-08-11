using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;

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
    private AudioClip _shootSound;
    private AudioClip _reloadSound;
    private AudioClip _emptySound;
    private string _name;

    private List<MyToggle> rof = new List<MyToggle>();
    private bool isFiring;

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

    public void Initialize(SaveLoadGun loadGun, GunSounds gunSounds)
    {
        gameObject.SetActive(true);
        nameText.text = loadGun.name;
        _name = loadGun.name;
        _totalAmmo = loadGun.totalClips * loadGun.maxClip;
        _maxClip = loadGun.maxClip;
        _semiAutoFire = loadGun.semiAutoFire;
        _autoFire = loadGun.autoFire;
        _shootSound = gunSounds.Shoot;
        _reloadSound = gunSounds.Reload;
        _emptySound = gunSounds.Empty;
        _ammoClip = this._maxClip;
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
        if(_autoFire > 0)
        {
            rof.Add(Instantiate(_togglePrefab, toggleGroup.transform));
            rof[^1].Text.text = "Автомат";
            rof[^1].Id = _autoFire;
            rof[^1].gameObject.SetActive(true);
        }
        UpdateText();
        ChangeProperty?.Invoke(this);
    }

    public void Initialize(SaveLoadGunUsed loadGun, GunSounds gunSounds)
    {
        gameObject.SetActive(true);
        nameText.text = loadGun.name;
        _name = loadGun.name;
        _totalAmmo = loadGun.totalAmmo;
        _maxClip = loadGun.maxClip;
        _semiAutoFire = loadGun.semiAutoFire;
        _autoFire = loadGun.autoFire;
        _shootSound = gunSounds.Shoot;
        _reloadSound = gunSounds.Reload;
        _emptySound = gunSounds.Empty;
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

    public void DestroyView() => Destroy(gameObject);

    private void Reload()
    {
        if(_ammoClip == 0 && _maxClip <= _totalAmmo)
        {
            _totalAmmo -= _maxClip;
            _ammoClip = _maxClip;
            PlaySound(_reloadSound);
        }
        else if(_ammoClip > 0 && _maxClip <= _totalAmmo)
        {
            _totalAmmo -= (_maxClip - _ammoClip);
            _ammoClip = _maxClip;
            PlaySound(_reloadSound);
        }
        else if(_totalAmmo > 0 && _maxClip - _ammoClip <= _totalAmmo)
        {
            _totalAmmo -= (_maxClip - _ammoClip);
            _ammoClip = _maxClip;            
            PlaySound(_reloadSound);
        }
        else if(_totalAmmo > 0)
        {
            _ammoClip = _totalAmmo;
            _totalAmmo = 0;
            PlaySound(_reloadSound);
        }
        UpdateText();
        ChangeProperty?.Invoke(this);
    }

    private void Shoot()
    {
        if (!isFiring)
        {
            _arrayBullet = toggleGroup.ActiveToggles().FirstOrDefault().GetComponent<MyToggle>().Id;
            isFiring = true;
            if (_arrayBullet == _semiAutoFire)
            {
                InvokeRepeating("ShootSound", 0.1f, 0.16f);
            }
            else if (_arrayBullet == _autoFire)
            {
                InvokeRepeating("ShootSound", 0.1f, 0.13f);
            }
            else
            {
                InvokeRepeating("ShootSound", 0.1f, 0.16f);
            }
        }      
    }

    private void UpdateText()
    {
        clipText.text = $"{_ammoClip}";
        totalAmmoText.text = $"{_totalAmmo}";
    }

    private void ShootSound()
    {
        if (_ammoClip == 0 || _arrayBullet == 0)
        {
            CancelInvoke();
            if(_ammoClip == 0)
            {
                PlaySound(_emptySound);
            }
            Invoke("StopFiring", _shootSound.length);
            ChangeProperty?.Invoke(this);
        }
        else
        {
            PlaySound(_shootSound);
            _ammoClip--;
            _arrayBullet--;
            UpdateText();
        }
        
    }

    private void PlaySound(AudioClip clip)
    {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.clip = clip;
        audio.Play();
        Destroy(audio, 2f);
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

    private void StopFiring() => isFiring = false;
    
}
