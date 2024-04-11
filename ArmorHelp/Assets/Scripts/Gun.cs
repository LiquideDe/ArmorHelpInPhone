using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Gun : MonoBehaviour
{
    int ammoClip, maxClip, totalAmmo, semiAutoFire, autoFire, arrayBullet;
    bool singleFire;
    AudioClip shootSound;
    AudioClip reloadSound;
    AudioClip emptySound;
    [SerializeField] TextMeshProUGUI nameText, clipText, totalAmmoText;
    [SerializeField] AudioSource audioSource;
    [SerializeField] MyToggle exampleToggle;
    [SerializeField] ToggleGroup toggleGroup;
    [SerializeField] TMP_InputField inputFieldAmmo;
    [SerializeField] GameObject panelWithAmmo;
    List<MyToggle> rof = new List<MyToggle>();
    bool isFiring;

    public void SetGun(SaveLoadGun loadGun, AudioClip shootSound, AudioClip reloadSound, AudioClip emptySound)
    {
        gameObject.SetActive(true);
        nameText.text = loadGun.name;
        totalAmmo = loadGun.totalClips * loadGun.maxClip;
        maxClip = loadGun.maxClip;
        semiAutoFire = loadGun.semiAutoFire;
        autoFire = loadGun.autoFire;
        this.shootSound = shootSound;
        this.reloadSound = reloadSound;
        this.emptySound = emptySound;
        ammoClip = this.maxClip;
        UpdateText();
        singleFire = loadGun.singleFire;
        if (singleFire)
        {
            rof.Add(Instantiate(exampleToggle, toggleGroup.transform));
            rof[^1].Text.text = "Одиночный режим";
            rof[^1].Id = 1;
            rof[^1].gameObject.SetActive(true);
        }
        if (this.semiAutoFire > 0)
        {
            rof.Add(Instantiate(exampleToggle, toggleGroup.transform));
            rof[^1].Text.text = "Полу автомат";
            rof[^1].Id = this.semiAutoFire;
            rof[^1].gameObject.SetActive(true);
        }
        if(this.autoFire > 0)
        {
            rof.Add(Instantiate(exampleToggle, toggleGroup.transform));
            rof[^1].Text.text = "Автомат";
            rof[^1].Id = this.autoFire;
            rof[^1].gameObject.SetActive(true);
        }
    }

    public void Reload()
    {
        if(ammoClip == 0 && maxClip <= totalAmmo)
        {
            totalAmmo -= maxClip;
            ammoClip = maxClip;
            PlaySound(reloadSound);
        }
        else if(ammoClip > 0 && maxClip <= totalAmmo)
        {
            totalAmmo -= (maxClip - ammoClip);
            ammoClip = maxClip;
            PlaySound(reloadSound);
        }
        else if(totalAmmo > 0 && maxClip - ammoClip <= totalAmmo)
        {
            totalAmmo -= (maxClip - ammoClip);
            ammoClip = maxClip;            
            PlaySound(reloadSound);
        }
        else if(totalAmmo > 0)
        {
            ammoClip = totalAmmo;
            totalAmmo = 0;
            PlaySound(reloadSound);
        }
        UpdateText();

    }

    public void Shoot()
    {
        if (!isFiring)
        {
            arrayBullet = toggleGroup.ActiveToggles().FirstOrDefault().GetComponent<MyToggle>().Id;
            isFiring = true;
            if (arrayBullet == semiAutoFire)
            {
                InvokeRepeating("ShootSound", 0.1f, 0.16f);
            }
            else if (arrayBullet == autoFire)
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
        clipText.text = $"{ammoClip}";
        totalAmmoText.text = $"{totalAmmo}";
    }

    private void ShootSound()
    {
        if (ammoClip == 0 || arrayBullet == 0)
        {
            CancelInvoke();
            if(ammoClip == 0)
            {
                PlaySound(emptySound);
            }
            Invoke("StopFiring", shootSound.length);
        }
        else
        {
            PlaySound(shootSound);
            ammoClip--;
            arrayBullet--;
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
        totalAmmo += ammo;
        UpdateText();
        panelWithAmmo.SetActive(false);
        inputFieldAmmo.text = "";
    }

    private void StopFiring()
    {
        isFiring = false;
    }
}
