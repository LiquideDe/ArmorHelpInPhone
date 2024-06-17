using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Zenject;

public class PanelDamageParametersView : MonoBehaviour
{
    [SerializeField] TMP_InputField _inputPenitration, _inputPlace, _inputDamage;
    [SerializeField] Toggle _toggleIsWarp, _toggleIsIgnoreArmor, _toggleIsIgnoreToughness;
    [SerializeField] DamageItem _damageItemPrefab;
    [SerializeField] Transform _contentForDamageItems;
    [SerializeField] Button _buttonAddDamage, _buttonCancel, _buttonDone;

    public event Action<List<DamageItem>> CalculateDamage;
    public event Action Cancel;

    private List<DamageItem> _damageItems = new List<DamageItem>();
    private int[] placesTakeDamage = new int[6];
    private AudioManager _audioManager;

    private void OnEnable()
    {
        _buttonAddDamage.onClick.AddListener(AddDamagePressed);
        _buttonDone.onClick.AddListener(CalculateDamagePressed);
        _buttonCancel.onClick.AddListener(CancelPressed);
        _toggleIsIgnoreArmor.onValueChanged.AddListener(PlayClickWhenToggleChanged);
        _toggleIsWarp.onValueChanged.AddListener(PlayClickWhenToggleChanged);
        _toggleIsIgnoreToughness.onValueChanged.AddListener(PlayClickWhenToggleChanged);
    }

    private void OnDisable()
    {
        _buttonAddDamage.onClick.RemoveAllListeners();
        _buttonDone.onClick.RemoveAllListeners();
        _buttonCancel.onClick.RemoveAllListeners();
        _toggleIsIgnoreArmor.onValueChanged.RemoveAllListeners();
        _toggleIsWarp.onValueChanged.RemoveAllListeners();
        _toggleIsIgnoreToughness.onValueChanged.RemoveAllListeners();
    }

    [Inject]
    private void Construct(AudioManager audioManager) => _audioManager = audioManager;

    public void DestroyView() => Destroy(gameObject);

    private void AddDamagePressed()
    {
        if (_inputPlace.text.Length > 0 && _inputDamage.text.Length > 0)
        {
            _audioManager.PlayClick();
            SetNewDamage(_inputPlace.text, _inputDamage.text);
            _inputDamage.text = "";
            _inputDamage.Select();
        }
        else
            _audioManager.PlayWarning();
    }

    private void SetNewDamage(string placeText, string damageText)
    {
        int.TryParse(_inputPenitration.text, out int penetration);
        _damageItems.Add(Instantiate(_damageItemPrefab, _contentForDamageItems));
        _damageItems[^1].SetParams(placeText, damageText, penetration, _toggleIsWarp.isOn, _toggleIsIgnoreArmor.isOn, _toggleIsIgnoreToughness.isOn, DeleteItem);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_contentForDamageItems as RectTransform);
    }

    private void DeleteItem(DamageItem damageItem)
    {
        _audioManager.PlayCancel();
        _damageItems.Remove(damageItem);
        Destroy(damageItem.gameObject);
    }

    private void CalculateDamagePressed()
    {
        if (_inputPlace.text.Length > 0 && _inputDamage.text.Length > 0)
            AddDamagePressed();

        CalculateDamage?.Invoke(_damageItems);
    }

    private void CancelPressed() => Cancel?.Invoke();

    private void PlayClickWhenToggleChanged(bool value) => _audioManager.PlayClick();
}
