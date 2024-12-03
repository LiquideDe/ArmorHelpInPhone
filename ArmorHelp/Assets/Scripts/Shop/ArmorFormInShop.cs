using UnityEngine;
using TMPro;

public class ArmorFormInShop : EquipmentFormInShop
{
    [SerializeField] private TextMeshProUGUI _textHeadAndHands, _textBodyAndLegs, _textAgility;
    public void Initialize(JSONArmorReader reader)
    {
        SendParametersToBase(reader.name, reader.rarity, reader.weight, reader.description);
        _textAgility.text = $"������������ ��������: {reader.maxAgility}";
        _textHeadAndHands.text = $"������ ������: {reader.head}, ������ ���: {reader.hands}";
        _textBodyAndLegs.text = $"������ ����: {reader.body}, ������ ���: {reader.legs}";
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
}
