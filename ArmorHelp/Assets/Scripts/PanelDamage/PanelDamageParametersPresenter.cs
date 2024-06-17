using System.Collections.Generic;
using System;
using Zenject;
using UnityEngine;

public class PanelDamageParametersPresenter : IPresenter
{
    public event Action<string, int> ReturnTextToArmor;
    public event Action Cancel;

    private SaveLoadArmor _armor;
    private AudioManager _audioManager;
    private PanelDamageParametersView _view;
    private int[] _placesWithDamage = new int[6];

    [Inject]
    private void Construct(AudioManager audioManager) => _audioManager = audioManager;

    public void Initialize(PanelDamageParametersView view, SaveLoadArmor armor)
    {
        _view = view;
        _armor = armor;
        Subscribe();
    }

    private void Subscribe()
    {
        _view.CalculateDamage += CalculateDamageDown;
        _view.Cancel += CancelDown;
    }

    private void Unscribe()
    {
        _view.CalculateDamage -= CalculateDamageDown;
        _view.Cancel -= CancelDown;
    }

    private void CalculateDamageDown(List<DamageItem> damageItems)
    {
        _audioManager.PlayDone();
        foreach (DamageItem item in damageItems)
        {
            if (item.Place < 10)
            {
                Damage(_armor.head, _armor.headArmor, item, 0); //0 - голова
            }
            else if (item.Place > 10 && item.Place < 21)
            {
                Damage(_armor.rightHand, _armor.rightHandArmor, item, 1); //1 - правая рука
            }
            else if (item.Place > 20 && item.Place < 31)
            {
                Damage(_armor.leftHand, _armor.leftHandArmor, item, 2); //2 - левая рука
            }
            else if (item.Place > 30 && item.Place < 71)
            {
                Damage(_armor.body, _armor.bodyArmor, item, 3);//3 - тело
            }
            else if (item.Place > 70 && item.Place < 86)
            {
                Damage(_armor.rightLeg, _armor.rightLegArmor, item, 4);//4 - правая нога
            }
            else if (item.Place > 85 && item.Place < 101)
            {
                Damage(_armor.leftLeg, _armor.leftLegArmor, item, 5);//5 - левая нога
            }
        }
        SetFinalText();
    }

    private void Damage(int totalDef, int armor, DamageItem item, int idPlace)
    {
        int damage = 0;
        int bToughness = totalDef - armor;
        if (!item.IsIgnoreArmor && !item.IsIgnoreToughness && !item.IsWarp)
        {

            if (armor < item.Penetration)
            {
                damage = item.Damage - bToughness;
            }
            else
            {
                damage = item.Damage - (armor - item.Penetration + bToughness);
            }
        }
        else if (item.IsIgnoreArmor && !item.IsIgnoreToughness && !item.IsWarp)
        {
            damage = item.Damage - bToughness;
        }
        else if (!item.IsIgnoreArmor && item.IsIgnoreToughness && !item.IsWarp)
        {
            if (armor >= item.Penetration)
                damage = item.Damage - (armor - item.Penetration);
            else
                damage = item.Damage;
        }
        else if (item.IsWarp)
        {
            damage = item.Damage - this._armor.bWillPower;
        }
        else if (item.IsIgnoreArmor && item.IsIgnoreToughness && !item.IsWarp)
        {
            damage = item.Damage;
        }

        if (damage > 0)
        {
            _placesWithDamage[idPlace] += damage;
        }
    }

    private void SetFinalText()
    {
        int totalDamage = 0;
        string textDamage = "";
        if (_placesWithDamage[0] > 0)
        {
            textDamage += $"Нанесено {_placesWithDamage[0]} урона в голову. \n";
            totalDamage += _placesWithDamage[0];
        }
        if (_placesWithDamage[1] > 0)
        {
            textDamage += $"Нанесено {_placesWithDamage[1]} урона в правую руку. \n";
            totalDamage += _placesWithDamage[1];
        }
        if (_placesWithDamage[2] > 0)
        {
            textDamage += $"Нанесено {_placesWithDamage[2]} урона в левую руку. \n";
            totalDamage += _placesWithDamage[2];
        }
        if (_placesWithDamage[3] > 0)
        {
            textDamage += $"Нанесено {_placesWithDamage[3]} урона в тело. \n";
            totalDamage += _placesWithDamage[3];
        }
        if (_placesWithDamage[4] > 0)
        {
            textDamage += $"Нанесено {_placesWithDamage[4]} урона в правую ногу. \n";
            totalDamage += _placesWithDamage[4];
        }
        if (_placesWithDamage[5] > 0)
        {
            textDamage += $"Нанесено {_placesWithDamage[5]} урона в левую ногу. \n";
            totalDamage += _placesWithDamage[5];
        }
        textDamage += $"Всего нанесено {totalDamage} урона";
        ReturnTextToArmor?.Invoke(textDamage, totalDamage);
        Unscribe();
        _view.DestroyView();
    }

    private void CancelDown()
    {
        _audioManager.PlayClick();
        Unscribe();
        _view.DestroyView();
        Cancel?.Invoke();
    }    

}
