using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DamageItem : MonoBehaviour, IPointerDownHandler
{
    public delegate void TapThisItem(DamageItem damageItem);
    TapThisItem tapThisItem;
    [SerializeField] TextMeshProUGUI textPlace, textDamage;
    int place, damage, penetration;
    bool isWarp, isIgnoreArmor, isIgnoreToughness;

    public int Place { get => place; }
    public int Damage { get => damage; }
    public int Penetration { get => penetration; }
    public bool IsWarp { get => isWarp; }
    public bool IsIgnoreArmor { get => isIgnoreArmor; }
    public bool IsIgnoreToughness { get => isIgnoreToughness; }
    public string PlaceText { get => textPlace.text; }

    public void OnPointerDown(PointerEventData eventData)
    {
        tapThisItem?.Invoke(this);
    }

    public void SetParams(string place, string damage, int penetration, bool isWarp, bool isIgnoreArmor, bool isIgnoreToughness, TapThisItem tapThisItem)
    {
        gameObject.SetActive(true);
        this.tapThisItem = tapThisItem;
        int.TryParse(place, out this.place);
        int.TryParse(damage, out this.damage);
        if (this.place < 10)
        {
            textPlace.text = "� ������";
        }
        else if (this.place > 10 && this.place < 21)
        {
            textPlace.text = "� ������ ����";
        }
        else if (this.place > 20 && this.place < 31)
        {
            textPlace.text = "� ����� ����";
        }
        else if (this.place > 30 && this.place < 71)
        {
            textPlace.text = "� ����";
        }
        else if (this.place > 70 && this.place < 86)
        {
            textPlace.text = "� ������ ����";
        }
        else if (this.place > 85 && this.place < 101)
        {
            textPlace.text = "� ����� ����";
        }
        textDamage.text = damage;
        this.penetration = penetration;
        this.isIgnoreArmor = isIgnoreArmor;
        this.isIgnoreToughness = isIgnoreToughness;
        this.isWarp = isWarp;
    }
}
