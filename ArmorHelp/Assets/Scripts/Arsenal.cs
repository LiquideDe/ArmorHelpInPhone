using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Arsenal : MonoBehaviour
{
    [SerializeField] Gun gun;
    [SerializeField] GameObject listGuns;
    [SerializeField] TMP_InputField inputName, inputTotalAmmo, inputClipAmmo, inputShort, inputLong;
    [SerializeField] Toggle toggleSingle;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] AudioClip[] gunShots;
    [SerializeField] AudioClip[] gunReloads;
    [SerializeField] AudioClip[] gunEmpty;
    private List<Gun> guns = new List<Gun>();

    public void ConfirmCreation()
    {
        if(inputName.text.Length > 0 && inputTotalAmmo.text.Length > 0 && inputClipAmmo.text.Length > 0)
        {
            guns.Add(Instantiate(gun, listGuns.transform));
            guns[^1].SetGun(inputName.text, inputTotalAmmo.text, inputClipAmmo.text, toggleSingle.isOn, inputShort.text, inputLong.text,
                gunShots[dropdown.value], gunReloads[dropdown.value], gunEmpty[dropdown.value]);
            inputName.text = "";
            inputTotalAmmo.text = "";
            inputClipAmmo.text = "";
            inputShort.text = "";
            inputLong.text = "";
        }
    }

    
    
}
