using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Arsenal : MonoBehaviour
{
    [SerializeField] Gun gun;
    [SerializeField] GameObject panelGun;
    [SerializeField] TMP_InputField inputName, inputAmmo;
    [SerializeField] TMP_Dropdown dropdown;
    private List<Gun> guns = new List<Gun>();

    public void CreateGun()
    {
        panelGun.SetActive(true);
    }

    public void ConfirmCreation()
    {
        if(inputName.text != "" && inputAmmo.text != "")
        {
            guns.Add(Instantiate(gun, transform));
            guns[^1].SetGun(inputName.text, dropdown.value, int.Parse(inputAmmo.text));
            guns[^1].gameObject.SetActive(true);
            panelGun.SetActive(false);
            inputName.text = "";
            inputAmmo.text = "";
        }
    }

    public void Cancel()
    {
        panelGun.SetActive(false);
    }

    
    
}
