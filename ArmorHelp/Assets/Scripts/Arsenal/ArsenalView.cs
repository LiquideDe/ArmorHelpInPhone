using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ArsenalView : MonoBehaviour
{
    [SerializeField] Button _buttonAddWeapon, _buttonReturnToArmor, _buttonCalculateModifiersBallistic, _buttonCalculateModifiersWeapon;
    [SerializeField] Transform _content;

    public event Action AddNewGun;
    public event Action ReturnToArmor;
    public event Action CalculateModifiersBallistic;
    public event Action CalculateModifiersWeapon;

    private void OnEnable()
    {
        _buttonAddWeapon.onClick.AddListener(AddGunPressed);
        _buttonReturnToArmor.onClick.AddListener(ReturnToArmorPressed);
        _buttonCalculateModifiersBallistic.onClick.AddListener(CalculateModifiersBallisticPressed);
        _buttonCalculateModifiersWeapon.onClick.AddListener(CalculateModifiersWeaponPressed);
    }

    private void OnDisable()
    {
        _buttonAddWeapon.onClick.RemoveAllListeners();
        _buttonReturnToArmor.onClick.RemoveAllListeners();
        _buttonCalculateModifiersBallistic.onClick.RemoveAllListeners();
        _buttonCalculateModifiersWeapon.onClick.RemoveAllListeners();
    }

    public void AddGun(Gun gun) => gun.transform.SetParent(_content);

    private void ReturnToArmorPressed() => ReturnToArmor?.Invoke();

    private void CalculateModifiersBallisticPressed() => CalculateModifiersBallistic?.Invoke();
    private void CalculateModifiersWeaponPressed() => CalculateModifiersWeapon?.Invoke();

    private void AddGunPressed() => AddNewGun?.Invoke();
}
