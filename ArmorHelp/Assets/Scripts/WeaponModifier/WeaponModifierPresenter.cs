using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

public class WeaponModifierPresenter : IPresenter
{
    public event Action CloseWeaponModifier;
    private WeaponModifierView _view;
    private AudioManager _audioManager;

    private List<string> _lightings = new List<string>() { "Светло", "Дым/Туман/Ливень" };
    private List<int> _lightingsModifiers = new List<int>() { 0,  -20 };
    private int _idLightings = 0;

    private List<string> _target = new List<string>() { "В норме", "Лежит", "Бежит", "Оглушена", "Врасплох" };
    private List<int> _targetModifiers = new List<int>() { 0, 10, 20, 20, 30 };
    private int _idTarget = 0;

    private List<string> _size = new List<string>() { "Крошечный(1)", "Маленький(2)", "Небольшой(3)", "Средний(4)", "Громадный(5)", "Огромный(6)", "Массивный(7)" };
    private List<int> _sizeModifiers = new List<int>() { -30, -20, -10, 0, 10, 20, 30 };
    private int _idSize = 3;

    private List<string> _typeAttack = new List<string>() {"Стандартная атака", "Быстрая атака", "Молниеносная атака", "Натиск", "Полная атака", "Осторожная атака" };
    private List<int> _typeAttackModifiers = new List<int>() { 10, 0, -10, 20, 30, -10 };
    private int _idTypeAttack = 0;

    private List<string> _landscape = new List<string>() {"Относительно ровная поверхность", "Трудный ландшафт", "Очень трудный ландшафт", "Глубокий снег/ болото"};
    private List<int> _landscapeModifiers = new List<int>() {0, -10, -20, -30 };
    private int _idLandscape = 0;

    private List<string> _superiority = new List<string>() {"Нет", "2 к 1", "3 к 1" };
    private List<int> _superiorityModifiers = new List<int>() {0, +10, +20 };
    private int _idSuperiority = 0;

    private List<string> _quality = new List<string>() { "Плохое", "Обычное", "Хорошее", "Лучшее" };
    private List<int> _qualityModifiers = new List<int>() { -10, 0, 5, 10 };
    private int _idQuality = 1;

    [Inject]
    private void Construct(AudioManager audioManager) => _audioManager = audioManager;

    public void Initialize(WeaponModifierView view)
    {
        _view = view;
        Subscribe();
        _view.SetLightingsText(_lightings[_idLightings]);
        _view.SetTargetText(_target[_idTarget]);
        _view.SetSizeText(_size[_idSize]);
        _view.SetTypeAttackText(_typeAttack[_idTypeAttack]);
        _view.SetLandscapeText(_landscape[_idLandscape]);
        _view.SetSuperiorityText(_superiority[_idSuperiority]);
        _view.SetQualityText(_quality[_idQuality]);
        CalculateResult();
    }

    public void ShowView() => _view.gameObject.SetActive(true);

    private void Subscribe()
    {
        _view.Exit += ExitDown;

        _view.NextLandscape += NextLandscapeDown;
        _view.NextLightings += NextLightingsDown;
        _view.NextQuality += NextQualityDown;
        _view.NextSize += NextSizeDown;
        _view.NextSuperiority += NextSuperiorityDown;
        _view.NextTarget += NextTargetDown;
        _view.NextTypeAttack += NextTypeAttackDown;

        _view.PrevLandscape += PrevLandscapeDown;
        _view.PrevLightings += PrevLightingsDown;
        _view.PrevQuality += PrevQualityDown;
        _view.PrevSize += PrevSizeDown;
        _view.PrevSuperiority += PrevSuperiorityDown;
        _view.PrevTarget += PrevTargetDown;
        _view.PrevTypeAttack += PrevTypeAttackDown;
    }

    private void PrevLightingsDown()
    {
        if (TryDecreaseId(ref _idLightings))
            _view.SetLightingsText(_lightings[_idLightings]);
    }
    private void PrevTargetDown()
    {
        if (TryDecreaseId(ref _idTarget))
            _view.SetTargetText(_target[_idTarget]);
    }
    private void PrevSizeDown()
    {
        if (TryDecreaseId(ref _idSize))
            _view.SetSizeText(_size[_idSize]);
    }
    private void PrevTypeAttackDown()
    {
        if (TryDecreaseId(ref _idTypeAttack))
            _view.SetTypeAttackText(_typeAttack[_idTypeAttack]);
    }
    private void PrevLandscapeDown()
    {
        if (TryDecreaseId(ref _idLandscape))
            _view.SetLandscapeText(_landscape[_idLandscape]);
    }
    private void PrevSuperiorityDown()
    {
        if (TryDecreaseId(ref _idSuperiority))
            _view.SetSuperiorityText(_superiority[_idSuperiority]);
    }
    private void PrevQualityDown()
    {
        if (TryDecreaseId(ref _idQuality))
            _view.SetQualityText(_quality[_idQuality]);
    }


    private void NextLightingsDown()
    {
        if (TryIncreaseId(ref _idLightings, _lightings.Count))
            _view.SetLightingsText(_lightings[_idLightings]);
    }
    private void NextTargetDown()
    {
        if (TryIncreaseId(ref _idTarget, _target.Count))
            _view.SetTargetText(_target[_idTarget]);
    }
    private void NextSizeDown()
    {
        if (TryIncreaseId(ref _idSize, _target.Count))
            _view.SetSizeText(_size[_idSize]);
    }
    private void NextTypeAttackDown()
    {
        if (TryIncreaseId(ref _idTypeAttack, _target.Count))
            _view.SetTypeAttackText(_typeAttack[_idTypeAttack]);
    }
    private void NextLandscapeDown()
    {
        if (TryIncreaseId(ref _idLandscape, _landscape.Count))
            _view.SetLandscapeText(_landscape[_idLandscape]);
    }
    private void NextSuperiorityDown()
    {
        if (TryIncreaseId(ref _idSuperiority, _superiority.Count))
            _view.SetSuperiorityText(_superiority[_idSuperiority]);
    }
    private void NextQualityDown()
    {
        if (TryIncreaseId(ref _idQuality, _quality.Count))
            _view.SetQualityText(_quality[_idQuality]);
    }

    private bool TryDecreaseId(ref int id)
    {
        if (id - 1 >= 0)
        {
            id--;
            _audioManager.PlayClick();
            CalculateResult();
            return true;
        }
        _audioManager.PlayWarning();
        return false;
    }

    private bool TryIncreaseId(ref int id, int maxValue)
    {
        if (id + 1 < maxValue)
        {
            id++;
            _audioManager.PlayClick();
            CalculateResult();
            return true;
        }

        _audioManager.PlayWarning();
        return false;
    }

    private void CalculateResult()
    {
        int totalModifier = 0;

        totalModifier += _lightingsModifiers[_idLightings];
        totalModifier += _targetModifiers[_idTarget];
        totalModifier += _sizeModifiers[_idSize];
        totalModifier += _typeAttackModifiers[_idTypeAttack];
        totalModifier += _landscapeModifiers[_idLandscape];
        totalModifier += _superiorityModifiers[_idSuperiority];
        totalModifier += _qualityModifiers[_idQuality];

        if(totalModifier > 0)
            _view.SetTotalModifierText($"+{totalModifier}");
        else
            _view.SetTotalModifierText($"{totalModifier}");
    }

    private void ExitDown()
    {
        _audioManager.PlayDone();
        _view.gameObject.SetActive(false);
        CloseWeaponModifier?.Invoke();
    }
}
