using UnityEngine;
using TMPro;

public class WeaponFormInShop : EquipmentFormInShop
{
    [SerializeField] TextMeshProUGUI _textType, _textRangeRofDamagePen, _textClipReload, _textProperties;

    public void Initialize(JSONMeleeReader reader)
    {
        SendParametersToBase(reader.name, reader.rarity, reader.weight, reader.description);
        SetTextMeleeAndGrenade(reader.weaponClass, reader.damage, reader.penetration, reader.properties);
    }

    public void Initialize(JSONGrenadeReader reader)
    {
        SendParametersToBase(reader.name, reader.rarity, reader.weight, reader.description);
        SetTextMeleeAndGrenade(reader.weaponClass, reader.damage, reader.penetration, reader.properties);
    }

    public void Initialize(JSONRangeReader reader)
    {
        SendParametersToBase(reader.name, reader.rarity, reader.weight, reader.description);
        _textType.text = $"�����: {reader.weaponClass}";
        _textRangeRofDamagePen.text = $"���������: {reader.range}�, RoF {reader.rof}, ���� {reader.damage}, ���� {reader.penetration}";
        _textClipReload.text = $"�������: {reader.clip}, �����������: {reader.reload}";
        _textProperties.text = $"�����������: {reader.properties}";
    }

    private void SendParametersToBase(string name, string rarity, float weight, string description)
    {
        JSONEquipmentReader equipmentReader = new JSONEquipmentReader();
        equipmentReader.description = description;
        equipmentReader.name = name;
        equipmentReader.rarity = rarity;
        equipmentReader.weight = weight;
        base.Initialize(equipmentReader);
    }

    private void SetTextMeleeAndGrenade(string weaponClass, string damage, int penetration, string properties)
    {
        _textType.text = $"�����: {weaponClass}";
        _textRangeRofDamagePen.text = $"���� {damage}, ���� {penetration}";
        _textClipReload.text = "";
        _textProperties.text = $"�����������: reader.properties";
    }
}
