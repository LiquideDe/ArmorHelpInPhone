using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelDamage : MonoBehaviour
{
    [SerializeField] TMP_InputField inputPenitration, inputPlace, inputDamage;
    [SerializeField] Toggle toggleIsWarp, toggleIsIgnoreArmor, toggleIsIgnoreToughness;
    [SerializeField] DamageItem damageItemExample;
    [SerializeField] Transform contentForDamageItems;
    int penetration;
    public delegate void ReturnDamage(int damage, string text);
    ReturnDamage returnDamage;
    List<DamageItem> damageItems = new List<DamageItem>();
    int[] placesTakeDamage = new int[6];
    SaveLoadArmor armor;
    AudioManager audioManager;
    public void SetParams(SaveLoadArmor armor, ReturnDamage returnDamage, AudioManager audioManager)
    {
        gameObject.SetActive(true);
        this.armor = armor;
        this.returnDamage = returnDamage;
        this.audioManager = audioManager;
    }
    public void SoundClick()
    {
        audioManager.PlayClick();
    }
    public void ConfirmDamage()
    {
        audioManager.PlayDone();
        foreach (DamageItem item in damageItems)
        {
            if (item.Place < 10)
            {
                Damage(armor.head, armor.headArmor, item, 0); //0 - голова
            }
            else if (item.Place > 10 && item.Place < 21)
            {
                Damage(armor.rightHand, armor.rightHandArmor, item, 1); //1 - правая рука
            }
            else if (item.Place > 20 && item.Place < 31)
            {
                Damage(armor.leftHand,armor.leftHandArmor, item, 2); //2 - левая рука
            }
            else if (item.Place > 30 && item.Place < 71)
            {
                Damage(armor.body,armor.bodyArmor, item, 3);//3 - тело
            }
            else if (item.Place > 70 && item.Place < 86)
            {
                Damage(armor.rightLeg,armor.rightLegArmor, item, 4);//4 - правая нога
            }
            else if (item.Place > 85 && item.Place < 101)
            {
                Damage(armor.leftLeg,armor.leftLegArmor, item, 5);//5 - левая нога
            }
        }

        inputPenitration.text = "";
        SetFinalText();
    }

    private void Damage(int totalDef, int armor, DamageItem item, int idPlace)
    {
        int damage = 0;
        int bToughness = totalDef - armor;
        if (!item.IsIgnoreArmor && !item.IsIgnoreToughness && !item.IsWarp)
        {

            if (armor < item.Penetration)
            {
                damage = item.Damage - bToughness;
            }
            else
            {
                damage = item.Damage - (armor - item.Penetration + bToughness);
            }
        }
        else if (item.IsIgnoreArmor && !item.IsIgnoreToughness && !item.IsWarp)
        {
            damage = item.Damage - bToughness;
        }
        else if (!item.IsIgnoreArmor && item.IsIgnoreToughness && !item.IsWarp)
        {
            if (armor >= item.Penetration)
                damage = item.Damage - (armor - item.Penetration);
            else
                damage = item.Damage;
        }
        else if (item.IsWarp)
        {
            damage = item.Damage - this.armor.bWillPower;
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
        string textDamage = "";
        if (placesTakeDamage[0] > 0)
        {
            textDamage += $"Нанесено {placesTakeDamage[0]} урона в голову. \n";
            totalDamage += placesTakeDamage[0];
        }
        if (placesTakeDamage[1] > 0)
        {
            textDamage += $"Нанесено {placesTakeDamage[1]} урона в правую руку. \n";
            totalDamage += placesTakeDamage[1];
        }
        if (placesTakeDamage[2] > 0)
        {
            textDamage += $"Нанесено {placesTakeDamage[2]} урона в левую руку. \n";
            totalDamage += placesTakeDamage[2];
        }
        if (placesTakeDamage[3] > 0)
        {
            textDamage += $"Нанесено {placesTakeDamage[3]} урона в тело. \n";
            totalDamage += placesTakeDamage[3];
        }
        if (placesTakeDamage[4] > 0)
        {
            textDamage += $"Нанесено {placesTakeDamage[4]} урона в правую ногу. \n";
            totalDamage += placesTakeDamage[4];
        }
        if (placesTakeDamage[5] > 0)
        {
            textDamage += $"Нанесено {placesTakeDamage[5]} урона в левую ногу. \n";
            totalDamage += placesTakeDamage[5];
        }
        textDamage += $"Всего нанесено {totalDamage} урона";
        returnDamage?.Invoke(totalDamage, textDamage);
        Destroy(gameObject);
    }
    public void AddMoreDamage()
    {        
        if (inputPlace.text.Length > 0 && inputDamage.text.Length > 0)
        {
            audioManager.PlayClick();
            GetNewDamage(inputPlace.text, inputDamage.text);
            inputDamage.text = "";
            inputDamage.Select();
        }
        else
        {
            audioManager.PlayWarning();
        }

    }

    public void FinishAddDamage()
    {        
        if (inputPlace.text.Length > 0 && inputDamage.text.Length > 0)
        {
            audioManager.PlayDone();
            GetNewDamage(inputPlace.text, inputDamage.text);
            inputDamage.text = "";
            inputPlace.text = "";
        }
        else
        {
            audioManager.PlayWarning();
        }
    }
    private void GetNewDamage(string placeText, string damageText)
    {
        int.TryParse(inputPenitration.text, out penetration);
        damageItems.Add(Instantiate(damageItemExample, contentForDamageItems));
        damageItems[^1].SetParams(placeText, damageText, penetration, toggleIsWarp.isOn, toggleIsIgnoreArmor.isOn, toggleIsIgnoreToughness.isOn, DeleteItem);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentForDamageItems as RectTransform);
    }

    private void DeleteItem(DamageItem damageItem)
    {
        audioManager.PlayCancel();
        damageItems.Remove(damageItem);
        Destroy(damageItem.gameObject);
    }

    public void Cancel()
    {
        audioManager.PlayCancel();
        Destroy(gameObject);
    }
}
