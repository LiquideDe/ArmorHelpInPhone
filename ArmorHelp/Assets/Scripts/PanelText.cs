using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDamage;

    public void SetParams(string text)
    {
        gameObject.SetActive(true);
        textDamage.text = text;
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }
}
