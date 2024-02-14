using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class LoadSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] TextMeshProUGUI textName;
    SaveLoadGun loadGun;
    public delegate void ReturnGun(SaveLoadGun loadGun);
    ReturnGun returnGun;
    public delegate void RemoveThis(string name);
    RemoveThis removeThis;
    public void OnPointerDown(PointerEventData eventData)
    {
        returnGun?.Invoke(loadGun);
    }

    public void SetParams(SaveLoadGun loadGun, ReturnGun returnGun, RemoveThis removeThis)
    {
        gameObject.SetActive(true);
        this.loadGun = loadGun;
        this.returnGun = returnGun;
        this.removeThis = removeThis;
        textName.text = loadGun.name;
    }

    public void Remove()
    {
        removeThis?.Invoke(loadGun.name);
        Destroy(gameObject);
    }
}
