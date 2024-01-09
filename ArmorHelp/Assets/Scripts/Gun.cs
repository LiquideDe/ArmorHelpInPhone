using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Gun : MonoBehaviour
{
    int ammoClip, maxClip, totalAmmo, shortFire, longFire, arrayBullet;
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


    public void SetGun(string name, string totalAmmo, string maxClip, bool singleFire, string shortFire, string longFire, AudioClip shootSound, AudioClip reloadSound, AudioClip emptySound)
    {
        gameObject.SetActive(true);
        nameText.text = name;
        int.TryParse(totalAmmo, out this.totalAmmo);
        int.TryParse(maxClip, out this.maxClip);
        int.TryParse(shortFire, out this.shortFire);
        int.TryParse(longFire, out this.longFire);
        this.shootSound = shootSound;
        this.reloadSound = reloadSound;
        this.emptySound = emptySound;
        ammoClip = this.maxClip;
        UpdateText();
        this.singleFire = singleFire;
        if (singleFire)
        {
            rof.Add(Instantiate(exampleToggle, toggleGroup.transform));
            rof[^1].Text.text = "Одиночный режим";
            rof[^1].Id = 1;
            rof[^1].gameObject.SetActive(true);
        }
        if (this.shortFire > 0)
        {
            rof.Add(Instantiate(exampleToggle, toggleGroup.transform));
            rof[^1].Text.text = "Полу автомат";
            rof[^1].Id = this.shortFire;
            rof[^1].gameObject.SetActive(true);
        }
        if(this.longFire > 0)
        {
            rof.Add(Instantiate(exampleToggle, toggleGroup.transform));
            rof[^1].Text.text = "Автомат";
            rof[^1].Id = this.longFire;
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
        arrayBullet = toggleGroup.ActiveToggles().FirstOrDefault().GetComponent<MyToggle>().Id;
        if(arrayBullet == shortFire)
        {
            InvokeRepeating("ShootSound", 0.1f, 0.16f);
        }
        else if(arrayBullet == longFire)
        {
            InvokeRepeating("ShootSound", 0.1f, 0.13f);
        }
        else
        {
            InvokeRepeating("ShootSound", 0.1f, 0.16f);
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
        //audioSource.clip = clip;
        //audioSource.Play();
    }

    public void AddAmmo()
    {
        int weapon = 0;
        int.TryParse(inputFieldAmmo.text, out weapon);
        totalAmmo += weapon;
        UpdateText();
        panelWithAmmo.SetActive(false);
        inputFieldAmmo.text = "";
    }
}
