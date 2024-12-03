using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ArsenalView : MonoBehaviour
{
    [SerializeField] Button _buttonAddWeapon, _buttonReturnToArmor, _buttonCalculateModifiersBallistic, _buttonCalculateModifiersWeapon, _buttonShop;
    [SerializeField] Transform _content;

    public event Action AddNewGun;
    public event Action ReturnToArmor;
    public event Action CalculateModifiersBallistic;
    public event Action CalculateModifiersWeapon;
    public event Action GoToShop;

    private void OnEnable()
    {
        _buttonAddWeapon.onClick.AddListener(AddGunPressed);
        _buttonReturnToArmor.onClick.AddListener(ReturnToArmorPressed);
        _buttonCalculateModifiersBallistic.onClick.AddListener(CalculateModifiersBallisticPressed);
        _buttonCalculateModifiersWeapon.onClick.AddListener(CalculateModifiersWeaponPressed);
        _buttonShop.onClick.AddListener(ShopPressed);
    }    

    private void OnDisable()
    {
        _buttonAddWeapon.onClick.RemoveAllListeners();
        _buttonReturnToArmor.onClick.RemoveAllListeners();
        _buttonCalculateModifiersBallistic.onClick.RemoveAllListeners();
        _buttonCalculateModifiersWeapon.onClick.RemoveAllListeners();
        _buttonShop.onClick.RemoveAllListeners();
    }

    public void AddGun(Gun gun) => gun.transform.SetParent(_content);

    private void ReturnToArmorPressed() => ReturnToArmor?.Invoke();

    private void CalculateModifiersBallisticPressed() => CalculateModifiersBallistic?.Invoke();
    private void CalculateModifiersWeaponPressed() => CalculateModifiersWeapon?.Invoke();

    private void AddGunPressed() => AddNewGun?.Invoke();
    private void ShopPressed() => GoToShop?.Invoke();
}
