using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Zenject;

public class EquipmentFormInShop : MonoBehaviour
{
    [SerializeField] Button _buttonNextPage, _buttonPrevPage;
    [SerializeField] TextMeshProUGUI _textName, _textWeightAndRarity, _textDescription, _textNumberPage;
    private int _page = 1;
    private AudioManager _audioManager;

    [Inject]
    private void Construct(AudioManager audioManager) => _audioManager = audioManager;

    private void OnEnable()
    {
        _buttonNextPage.onClick.AddListener(NextPagePressed);
        _buttonPrevPage.onClick.AddListener(PrevPagePressed);
    }    

    public virtual void Initialize(JSONEquipmentReader reader)
    {
        gameObject.SetActive(true);
        _textName.text = reader.name;
        _textWeightAndRarity.text = $"Вес {reader.weight}кг, Доступность: {reader.rarity}";
        _textDescription.text = reader.description;
        _textNumberPage.text = $"Страница 1/{_textDescription.textInfo.pageCount}";
    }

    private void PrevPagePressed()
    {
        if (_page > 1)
        {
            _page--;
            _textDescription.pageToDisplay = _page;
            _textNumberPage.text = $"Страница {_page}/{_textDescription.textInfo.pageCount}";
            _audioManager.PlayClick();
        }
        else
            _audioManager.PlayWarning();
    }

    private void NextPagePressed()
    {
        if(_page < _textDescription.textInfo.pageCount)
        {
            _page++;
            _textDescription.pageToDisplay = _page;
            _textNumberPage.text = $"Страница {_page}/{_textDescription.textInfo.pageCount}";
            _audioManager.PlayClick();
        }
        else
            _audioManager.PlayWarning();
    }
}
