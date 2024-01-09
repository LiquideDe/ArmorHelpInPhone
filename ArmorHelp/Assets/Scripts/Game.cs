using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game : MonoBehaviour
{
    [SerializeField] TMP_InputField inputHead, inputRightHand, inputLeftHand, inputBody, inputLeftLeg, inputRightLeg, inputBonusToughness, inputPenitration, inputBonusWP;
    [SerializeField] TMP_InputField inputPlace, inputDamage;
    [SerializeField] GameObject panelDamage, panelWithText, panelForDamageItem, contentForDamageItems;
    [SerializeField] TextMeshProUGUI textDamage, textWound;
    [SerializeField] Toggle toggleIsWarp, toggleIsIgnoreArmor, toggleIsIgnoreToughness;
    [SerializeField] DamageItem damageItemExample;
    int head, rightHand, leftHand, body, rightLeg, leftLeg, toughness, willPower, penetration;
    List<DamageItem> damageItems = new List<DamageItem>();
    int[] placesTakeDamage = new int[6];
    int wound = 5;

    
    public void TakeDamage()
    {            
        panelDamage.SetActive(true);
    }

    public void ConfirmDamage()
    {

        panelDamage.SetActive(false);
        panelWithText.SetActive(true);

        int.TryParse(inputHead.text, out head);
        int.TryParse(inputRightHand.text, out rightHand);
        int.TryParse(inputLeftHand.text, out leftHand);
        int.TryParse(inputBody.text, out body);
        int.TryParse(inputRightLeg.text, out rightLeg);
        int.TryParse(inputLeftLeg.text, out leftLeg);
        int.TryParse(inputBonusToughness.text, out toughness);
        int.TryParse(inputBonusWP.text, out willPower);

        foreach(DamageItem item in damageItems)
        {
            if (item.Place < 10)
            {
                Damage(head, item, 0); //0 - голова
            }
            else if (item.Place > 10 && item.Place < 21)
            {
                Damage(rightHand, item, 1); //1 - правая рука
            }
            else if (item.Place > 20 && item.Place < 31)
            {
                Damage(leftHand, item, 2); //2 - левая рука
            }
            else if (item.Place > 30 && item.Place < 71)
            {
                Damage(body, item, 3);//3 - тело
            }
            else if (item.Place > 70 && item.Place < 86)
            {
                Damage(rightLeg, item, 4);//4 - правая нога
            }
            else if (item.Place > 85 && item.Place < 101)
            {
                Damage(leftLeg, item, 5);//5 - левая нога
            }
        }

        inputPenitration.text = "";
        SetFinalText();
        CleanItems();
    }

    private void Damage(int armor, DamageItem item, int idPlace)
    {
        int damage = 0;
        if (!item.IsIgnoreArmor && !item.IsIgnoreToughness && !item.IsWarp)
        {
            
            if (armor < item.Penetration)
            {
                damage = item.Damage - toughness;
            }
            else
            {
                damage = item.Damage - (armor - item.Penetration + toughness);
            }

            
        }
        else if(item.IsIgnoreArmor && !item.IsIgnoreToughness && !item.IsWarp)
        {
            damage = item.Damage - toughness;
        }
        else if(!item.IsIgnoreArmor && item.IsIgnoreToughness && !item.IsWarp)
        {
            if (armor >= item.Penetration)
                damage = item.Damage - (armor - item.Penetration);
            else
                damage = item.Damage;
        }
        else if(item.IsWarp)
        {
            damage = item.Damage - willPower;
        }
        else if (item.IsIgnoreArmor && item.IsIgnoreToughness && !item.IsWarp)
        {
            damage = item.Damage;
        }

        if (damage > 0)
        {
            placesTakeDamage[idPlace] += damage;
        }
    }

    private void SetFinalText()
    {
        int totalDamage = 0;
        textDamage.text = "";
        if (placesTakeDamage[0] > 0)
        {
            textDamage.text += $"Нанесено {placesTakeDamage[0]} урона в голову. \n";
            totalDamage += placesTakeDamage[0];
        }
        if (placesTakeDamage[1] > 0)
        {
            textDamage.text += $"Нанесено {placesTakeDamage[1]} урона в правую руку. \n";
            totalDamage += placesTakeDamage[1];
        }
        if (placesTakeDamage[2] > 0)
        {
            textDamage.text += $"Нанесено {placesTakeDamage[2]} урона в левую руку. \n";
            totalDamage += placesTakeDamage[2];
        }
        if (placesTakeDamage[3] > 0)
        {
            textDamage.text += $"Нанесено {placesTakeDamage[3]} урона в тело. \n";
            totalDamage += placesTakeDamage[3];
        }
        if (placesTakeDamage[4] > 0)
        {
            textDamage.text += $"Нанесено {placesTakeDamage[4]} урона в правую ногу. \n";
            totalDamage += placesTakeDamage[4];
        }
        if (placesTakeDamage[5] > 0)
        {
            textDamage.text += $"Нанесено {placesTakeDamage[5]} урона в левую ногу. \n";
            totalDamage += placesTakeDamage[5];
        }
        textDamage.text += $"Всего нанесено {totalDamage} урона";

        for(int i = 0; i < placesTakeDamage.Length; i++)
        {
            placesTakeDamage[i] = 0;
        }

        wound -= totalDamage;
        textWound.text = $"{wound}";
    }

    private void CleanItems()
    {
        foreach(DamageItem item in damageItems)
        {
            Destroy(item.gameObject);
        }
        damageItems.Clear();
    }
    public void AddMoreDamage()
    {
        if(inputPlace.text.Length > 0 && inputDamage.text.Length > 0)
        {
            GetNewDamage(inputPlace.text, inputDamage.text);
            inputDamage.text = "";
            inputDamage.Select();
        }
        
    }

    public void FinishAddDamage()
    {
        if (inputPlace.text.Length > 0 && inputDamage.text.Length > 0)
        {
            GetNewDamage(inputPlace.text, inputDamage.text);
            inputDamage.text = "";
            inputPlace.text = "";
        }            
    }
    private void GetNewDamage(string placeText, string damageText)
    {

        int.TryParse(inputPenitration.text, out penetration);
        damageItems.Add(Instantiate(damageItemExample, contentForDamageItems.transform));
        damageItems[^1].SetParams(placeText, damageText, penetration, toggleIsWarp.isOn, toggleIsIgnoreArmor.isOn, toggleIsIgnoreToughness.isOn, DeleteItem);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentForDamageItems.transform as RectTransform);
    }

    private void DeleteItem(DamageItem damageItem)
    {
        damageItems.Remove(damageItem);
        Destroy(damageItem.gameObject);
    }

    public void PlusWound()
    {
        wound += 1;
        textWound.text = $"{wound}";
    }

    public void MinusWound()
    {
        wound -= 1;
        textWound.text = $"{wound}";
    }
    public void Cancel()
    {
        Debug.Log("What?");
        panelDamage.SetActive(false);
        panelWithText.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
