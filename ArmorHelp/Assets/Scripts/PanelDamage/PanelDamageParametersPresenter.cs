using System.Collections.Generic;
using System;
using Zenject;
using UnityEngine;

public class PanelDamageParametersPresenter : IPresenter
{
    public event Action<string, int> ReturnTextToArmor;
    public event Action Cancel;

    private Character _character;
    private AudioManager _audioManager;
    private PanelDamageParametersView _view;
    private int[] _placesWithDamage = new int[6];

    [Inject]
    private void Construct(AudioManager audioManager) => _audioManager = audioManager;

    public void Initialize(PanelDamageParametersView view, Character character)
    {
        _view = view;
        _character = character;
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
            if (item.Place <= 10)
            {
                Damage(_character.HeadTotal, _character.ArmorHead, item, 0, _character.ShelterArmorPoint, _character.IsHeadSheltered); //0 - ������
            }
            else if (item.Place > 10 && item.Place < 21)
            {
                Damage(_character.RightHandTotal, _character.ArmorRightHand, item, 1, _character.ShelterArmorPoint, _character.IsRightHandSheltered); //1 - ������ ����
            }
            else if (item.Place > 20 && item.Place < 31)
            {
                Damage(_character.LeftHandTotal, _character.ArmorLeftHand, item, 2, _character.ShelterArmorPoint, _character.IsLeftHandSheltered); //2 - ����� ����
            }
            else if (item.Place > 30 && item.Place < 71)
            {
                Damage(_character.BodyTotal, _character.ArmorBody, item, 3, _character.ShelterArmorPoint, _character.IsBodySheltered);//3 - ����
            }
            else if (item.Place > 70 && item.Place < 86)
            {
                Damage(_character.RightLegTotal, _character.ArmorRightLeg, item, 4, _character.ShelterArmorPoint, _character.IsRightLegSheltered);//4 - ������ ����
            }
            else if (item.Place > 85 && item.Place < 101)
            {
                Damage(_character.LeftLegTotal, _character.ArmorLeftLeg, item, 5, _character.ShelterArmorPoint, _character.IsLeftLegSheltered);//5 - ����� ����
            }
        }
        SetFinalText();
    }

    private void Damage(int totalDef, int armor, DamageItem item, int idPlace, int shelterPoint, bool isTakeCover)
    {
        int damage = 0;
        int bToughness = totalDef - armor;
        if (isTakeCover)
            armor += shelterPoint;

        if (!item.IsIgnoreArmor && !item.IsIgnoreToughness && !item.IsWarp)
        {

            if (armor < item.Penetration)
            {
                damage = item.Damage - bToughness;
                if (isTakeCover)
                    _character.ShelterArmorPoint--;
            }
            else
            {
                damage = item.Damage - (armor - item.Penetration + bToughness);
                if (isTakeCover)
                {
                    if (item.Damage - (shelterPoint - item.Penetration) > 0)
                        _character.ShelterArmorPoint--;
                }
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

            if (item.Penetration > _character.ShelterArmorPoint || item.Damage - (_character.ShelterArmorPoint - item.Penetration) > 0)
                _character.ShelterArmorPoint--;
        }
        else if (item.IsWarp)
        {
            damage = item.Damage - _character.BWillpower;
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
            textDamage += $"�������� {_placesWithDamage[0]} ����� � ������. \n";
            totalDamage += _placesWithDamage[0];
        }
        if (_placesWithDamage[1] > 0)
        {
            textDamage += $"�������� {_placesWithDamage[1]} ����� � ������ ����. \n";
            totalDamage += _placesWithDamage[1];
        }
        if (_placesWithDamage[2] > 0)
        {
            textDamage += $"�������� {_placesWithDamage[2]} ����� � ����� ����. \n";
            totalDamage += _placesWithDamage[2];
        }
        if (_placesWithDamage[3] > 0)
        {
            textDamage += $"�������� {_placesWithDamage[3]} ����� � ����. \n";
            totalDamage += _placesWithDamage[3];
        }
        if (_placesWithDamage[4] > 0)
        {
            textDamage += $"�������� {_placesWithDamage[4]} ����� � ������ ����. \n";
            totalDamage += _placesWithDamage[4];
        }
        if (_placesWithDamage[5] > 0)
        {
            textDamage += $"�������� {_placesWithDamage[5]} ����� � ����� ����. \n";
            totalDamage += _placesWithDamage[5];
        }
        textDamage += $"����� �������� {totalDamage} �����";
        _character.Wounds -= totalDamage;
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
