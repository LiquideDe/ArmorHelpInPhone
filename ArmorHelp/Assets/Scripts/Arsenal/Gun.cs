using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Gun : MonoBehaviour
{
    int _ammoClip, _maxClip, _totalAmmo, _semiAutoFire, _autoFire, _arrayBullet;
    bool singleFire;
    AudioClip _shootSound;
    AudioClip _reloadSound;
    AudioClip _emptySound;
    [SerializeField] TextMeshProUGUI nameText, clipText, totalAmmoText;
    [SerializeField] MyToggle _togglePrefab;
    [SerializeField] ToggleGroup toggleGroup;
    [SerializeField] TMP_InputField inputFieldAmmo;
    [SerializeField] GameObject panelWithAmmo;
    List<MyToggle> rof = new List<MyToggle>();
    bool isFiring;

    public void Initialize(SaveLoadGun loadGun, GunSounds gunSounds)
    {
        gameObject.SetActive(true);
        nameText.text = loadGun.name;
        _totalAmmo = loadGun.totalClips * loadGun.maxClip;
        _maxClip = loadGun.maxClip;
        _semiAutoFire = loadGun.semiAutoFire;
        _autoFire = loadGun.autoFire;
        _shootSound = gunSounds.Shoot;
        _reloadSound = gunSounds.Reload;
        _emptySound = gunSounds.Empty;
        _ammoClip = this._maxClip;
        UpdateText();
        singleFire = loadGun.singleFire;
        if (singleFire)
        {
            rof.Add(Instantiate(_togglePrefab, toggleGroup.transform));
            rof[^1].Text.text = "Одиночный режим";
            rof[^1].Id = 1;
            rof[^1].gameObject.SetActive(true);
        }
        if (this._semiAutoFire > 0)
        {
            rof.Add(Instantiate(_togglePrefab, toggleGroup.transform));
            rof[^1].Text.text = "Полу автомат";
            rof[^1].Id = this._semiAutoFire;
            rof[^1].gameObject.SetActive(true);
        }
        if(this._autoFire > 0)
        {
            rof.Add(Instantiate(_togglePrefab, toggleGroup.transform));
            rof[^1].Text.text = "Автомат";
            rof[^1].Id = this._autoFire;
            rof[^1].gameObject.SetActive(true);
        }
    }

    public void Reload()
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

    }

    public void Shoot()
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

    public void AddAmmo()
    {
        int.TryParse(inputFieldAmmo.text, out int ammo);
        _totalAmmo += ammo;
        UpdateText();
        panelWithAmmo.SetActive(false);
        inputFieldAmmo.text = "";
    }

    private void StopFiring() => isFiring = false;
    
}
