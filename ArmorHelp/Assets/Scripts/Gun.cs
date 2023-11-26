using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    int ammo, maxAmmo;
    [SerializeField] AudioClip[] shootSounds;
    [SerializeField] TextMeshProUGUI nameText, ammoText;
    [SerializeField] AudioSource audioSource;
    int typeGunSound = 0;

    public void SetGun(string name, int type, int ammo)
    {
        typeGunSound = type;
        nameText.text = name;
        this.ammo = ammo;
        maxAmmo = ammo;
        ammoText.text = ammo.ToString();
    }

    public void Reload()
    {
        ammo = maxAmmo;
        ammoText.text = ammo.ToString();
    }

    public void Shoot()
    {
        if(ammo > 0)
        {
            //AudioSource audioSource = new AudioSource();
            audioSource.PlayOneShot(shootSounds[typeGunSound]);
            ammo--;
            ammoText.text = ammo.ToString();
        }
    }
}
