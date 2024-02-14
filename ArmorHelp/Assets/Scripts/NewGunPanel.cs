using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewGunPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField inputName, inputTotalAmmo, inputClipAmmo, inputSemiAutoFire, inputAutoFire;
    [SerializeField] Toggle toggleSingle;
    [SerializeField] TMP_Dropdown dropdown;

    public delegate void ReturnNewGun(SaveLoadGun loadGun);
    ReturnNewGun returnNewGun;

    public void SetParams(ReturnNewGun returnNewGun)
    {
        gameObject.SetActive(true);
        this.returnNewGun = returnNewGun;
    }

    public void GunIsDone()
    {
        if(inputName.text.Length > 0 && inputClipAmmo.text.Length > 0 && inputTotalAmmo.text.Length > 0)
        {
            SaveLoadGun gun = new SaveLoadGun();
            gun.name = inputName.text;
            int.TryParse(inputTotalAmmo.text, out gun.totalAmmo);
            int.TryParse(inputClipAmmo.text, out gun.maxClip);
            int.TryParse(inputSemiAutoFire.text, out gun.semiAutoFire);
            int.TryParse(inputAutoFire.text, out gun.autoFire);
            gun.singleFire = toggleSingle.isOn;
            gun.type = dropdown.value;
            new LoadGuns().SetGun(gun);
            returnNewGun?.Invoke(gun);
            Cancel();
        }
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }
}
