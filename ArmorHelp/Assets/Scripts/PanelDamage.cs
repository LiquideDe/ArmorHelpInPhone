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
    public void SetParams(SaveLoadArmor armor, ReturnDamage returnDamage)
    {
        gameObject.SetActive(true);
        this.armor = armor;
        this.returnDamage = returnDamage;
    }

    public void ConfirmDamage()
    {       

        foreach (DamageItem item in damageItems)
        {
            if (item.Place < 10)
            {
                Damage(armor.head, item, 0); //0 - ������
            }
            else if (item.Place > 10 && item.Place < 21)
            {
                Damage(armor.rightHand, item, 1); //1 - ������ ����
            }
            else if (item.Place > 20 && item.Place < 31)
            {
                Damage(armor.leftHand, item, 2); //2 - ����� ����
            }
            else if (item.Place > 30 && item.Place < 71)
            {
                Damage(armor.body, item, 3);//3 - ����
            }
            else if (item.Place > 70 && item.Place < 86)
            {
                Damage(armor.rightLeg, item, 4);//4 - ������ ����
            }
            else if (item.Place > 85 && item.Place < 101)
            {
                Damage(armor.leftLeg, item, 5);//5 - ����� ����
            }
        }

        inputPenitration.text = "";
        SetFinalText();
    }

    private void Damage(int armor, DamageItem item, int idPlace)
    {
        int damage = 0;
        if (!item.IsIgnoreArmor && !item.IsIgnoreToughness && !item.IsWarp)
        {

            if (armor < item.Penetration)
            {
                damage = item.Damage - this.armor.bToughness;
            }
            else
            {
                damage = item.Damage - (armor - item.Penetration + this.armor.bToughness);
            }


        }
        else if (item.IsIgnoreArmor && !item.IsIgnoreToughness && !item.IsWarp)
        {
            damage = item.Damage - this.armor.bToughness;
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
            textDamage += $"�������� {placesTakeDamage[0]} ����� � ������. \n";
            totalDamage += placesTakeDamage[0];
        }
        if (placesTakeDamage[1] > 0)
        {
            textDamage += $"�������� {placesTakeDamage[1]} ����� � ������ ����. \n";
            totalDamage += placesTakeDamage[1];
        }
        if (placesTakeDamage[2] > 0)
        {
            textDamage += $"�������� {placesTakeDamage[2]} ����� � ����� ����. \n";
            totalDamage += placesTakeDamage[2];
        }
        if (placesTakeDamage[3] > 0)
        {
            textDamage += $"�������� {placesTakeDamage[3]} ����� � ����. \n";
            totalDamage += placesTakeDamage[3];
        }
        if (placesTakeDamage[4] > 0)
        {
            textDamage += $"�������� {placesTakeDamage[4]} ����� � ������ ����. \n";
            totalDamage += placesTakeDamage[4];
        }
        if (placesTakeDamage[5] > 0)
        {
            textDamage += $"�������� {placesTakeDamage[5]} ����� � ����� ����. \n";
            totalDamage += placesTakeDamage[5];
        }
        textDamage += $"����� �������� {totalDamage} �����";
        returnDamage?.Invoke(totalDamage, textDamage);
        Cancel();
    }
    public void AddMoreDamage()
    {
        if (inputPlace.text.Length > 0 && inputDamage.text.Length > 0)
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
        damageItems.Add(Instantiate(damageItemExample, contentForDamageItems));
        damageItems[^1].SetParams(placeText, damageText, penetration, toggleIsWarp.isOn, toggleIsIgnoreArmor.isOn, toggleIsIgnoreToughness.isOn, DeleteItem);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentForDamageItems as RectTransform);
    }

    private void DeleteItem(DamageItem damageItem)
    {
        damageItems.Remove(damageItem);
        Destroy(damageItem.gameObject);
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }
}
