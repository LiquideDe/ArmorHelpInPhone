using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{
    [SerializeField] TMP_InputField inputHead, inputRightHand, inputLeftHand, inputBody, inputLeftLeg, inputRightLeg, inputPlace, inputDamage, inputBonusToughness, inputPenitration;
    [SerializeField] GameObject panelDamage, panelWithText;
    [SerializeField] TextMeshProUGUI textDamage;
    string placeText;
    
    public void TakeDamage()
    {
        if(inputHead.text != "" && inputRightHand.text != "" && inputLeftHand.text != "" && inputBody.text != "" && inputLeftLeg.text != "" && inputRightLeg.text != "" && inputBonusToughness.text != "")
        {
            panelDamage.SetActive(true);
        }
    }

    public void ConfirmDamage()
    {
        if(inputPlace.text != "" && inputDamage.text != "" && inputPenitration.text != "")
        {
            panelDamage.SetActive(false);
            panelWithText.SetActive(true);
            int place = int.Parse(inputPlace.text);
            if (place < 10)
            {
                placeText = "голову.";
                Damage(int.Parse(inputHead.text) - int.Parse(inputPenitration.text), int.Parse(inputDamage.text));
            }
            else if (place > 10 && place < 21)
            {
                placeText = "правую руку.";
                Damage(int.Parse(inputRightHand.text) - int.Parse(inputPenitration.text), int.Parse(inputDamage.text));
            }
            else if (place > 20 && place < 31)
            {
                placeText = "левую руку.";
                Damage(int.Parse(inputLeftHand.text) - int.Parse(inputPenitration.text), int.Parse(inputDamage.text));
            }
            else if (place > 30 && place < 71)
            {
                placeText = "тело.";
                Damage(int.Parse(inputBody.text) - int.Parse(inputPenitration.text), int.Parse(inputDamage.text));
            }
            else if (place > 70 && place < 86)
            {
                placeText = "правую ногу.";
                Damage(int.Parse(inputRightLeg.text) - int.Parse(inputPenitration.text), int.Parse(inputDamage.text));
            }
            else if (place > 85 && place < 101)
            {
                placeText = "левую ногу.";
                Damage(int.Parse(inputLeftLeg.text) - int.Parse(inputPenitration.text), int.Parse(inputDamage.text));
            }
        }
    }

    private void Damage(int armor, int damage)
    {
        if (armor < 0)
            armor = 0;
        damage -= (armor + int.Parse(inputBonusToughness.text));
        if (damage < 0)
        {
            damage = 0;
        }
        textDamage.text = $"Вам нанесли {damage} урона в {placeText}";
        inputPlace.text = "";
        inputDamage.text = "";
        inputPenitration.text = "";
    }

    public void Cancel()
    {
        panelDamage.SetActive(false);
        panelWithText.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
