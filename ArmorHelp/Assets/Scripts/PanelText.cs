using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDamage;
    AudioManager audioManager;

    public void SetParams(string text, AudioManager audioManager)
    {
        this.audioManager = audioManager;
        gameObject.SetActive(true);
        textDamage.text = text;
    }

    public void Cancel()
    {
        audioManager.PlayDone();
        Destroy(gameObject);
    }
}
