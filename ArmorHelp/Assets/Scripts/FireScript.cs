using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Gun gun;
    int sum = 10;

    public void OnPointerClick(PointerEventData eventData)
    {
        InvokeRepeating("Shoot", 0.1f,0.16f);
    }

    private void Shoot()
    {
        sum--;
        gun.Shoot();
        if(sum == 0)
        {
            CancelInvoke();
        }
    }
}

