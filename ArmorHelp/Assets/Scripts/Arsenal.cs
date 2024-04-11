using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Arsenal : MonoBehaviour
{
    [SerializeField] Gun gun;
    [SerializeField] GameObject listGuns;    
    [SerializeField] AudioClip[] gunShots;
    [SerializeField] AudioClip[] gunReloads;
    [SerializeField] AudioClip[] gunEmpty;
    [SerializeField] NewGunPanel newGunPanel;
    [SerializeField] ListGuns listNewGuns;
    private List<Gun> guns = new List<Gun>();
    [SerializeField] AudioManager audioManager;
    

    public void ConfirmCreation(SaveLoadGun loadGun)
    {
        audioManager.PlayDone();
        guns.Add(Instantiate(gun, listGuns.transform));
        guns[^1].SetGun(loadGun, gunShots[loadGun.type], gunReloads[loadGun.type], gunEmpty[loadGun.type]);
    }    

    public void AddGun()
    {
        audioManager.PlayClick();
        ListGuns list = Instantiate(listNewGuns, transform);
        list.SetParams(ConfirmCreation, CreateGun);        
    }

    private void CreateGun()
    {
        audioManager.PlayClick();
        NewGunPanel newGun = Instantiate(newGunPanel, transform);
        newGun.SetParams(ConfirmCreation);        
    }
    
}
