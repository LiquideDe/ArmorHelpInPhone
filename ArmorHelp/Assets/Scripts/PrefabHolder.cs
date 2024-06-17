using UnityEngine;

[CreateAssetMenu(fileName = "PrefabHolder", menuName = "Holder/PrefabHolder")]
public class PrefabHolder : ScriptableObject
{
    [SerializeField] GameObject _armorPrefab, _arsenalPrefab, _damageParametersPanelPrefab, _panelTextDamage, _listWithGuns, _newGunPanel, _qrScanner;
    [SerializeField] GameObject _ballisticModifier, _weaponModifier;

    public GameObject Get(TypeScene typeScene)
    {
        switch (typeScene)
        {
            case TypeScene.Armor:
                return _armorPrefab;

            case TypeScene.DamageParametersPanel:
                return _damageParametersPanelPrefab;

            case TypeScene.TextDamage:
                return _panelTextDamage;

            case TypeScene.Arsenal:
                return _arsenalPrefab;

            case TypeScene.ListWithGuns:
                return _listWithGuns;

            case TypeScene.CreateGunPanel:
                return _newGunPanel;

            case TypeScene.QrScanner:
                return _qrScanner;

            case TypeScene.BallisticModifier:
                return _ballisticModifier;

            case TypeScene.WeaponModifier:
                return _weaponModifier;

            default:
                throw new System.Exception($"Нет такого типа сцены {typeScene}");
        }
    }
}
