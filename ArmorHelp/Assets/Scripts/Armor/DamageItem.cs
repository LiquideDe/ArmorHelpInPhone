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

    public int Place => place; 
    public int Damage => damage; 
    public int Penetration => penetration; 
    public bool IsWarp => isWarp; 
    public bool IsIgnoreArmor => isIgnoreArmor; 
    public bool IsIgnoreToughness => isIgnoreToughness; 
    public string PlaceText => textPlace.text; 

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
            textPlace.text = "В голову";
        }
        else if (this.place > 10 && this.place < 21)
        {
            textPlace.text = "В правую руку";
        }
        else if (this.place > 20 && this.place < 31)
        {
            textPlace.text = "В левую руку";
        }
        else if (this.place > 30 && this.place < 71)
        {
            textPlace.text = "В тело";
        }
        else if (this.place > 70 && this.place < 86)
        {
            textPlace.text = "В правую ногу";
        }
        else if (this.place > 85 && this.place < 101)
        {
            textPlace.text = "В левую ногу";
        }
        textDamage.text = damage;
        this.penetration = penetration;
        this.isIgnoreArmor = isIgnoreArmor;
        this.isIgnoreToughness = isIgnoreToughness;
        this.isWarp = isWarp;
    }
}
