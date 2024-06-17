using System;
using System.Collections.Generic;
using Zenject;

public class ArsenalPresenter : IPresenter
{
    public event Action ReturnToArmorDown;
    public event Action AddNewGunDown;
    public event Action CalculateBallisticModifiersDown;
    public event Action CalculateWeaponModifiersDown;
    private ArsenalView _view;
    private AudioManager _audioManager;
    private GunHolder _gunFactory;
    

    [Inject]
    private void Construct(AudioManager audioManager, GunHolder gunFactory)
    {
        _audioManager = audioManager;
        _gunFactory = gunFactory;
    }

    public void Initialize(ArsenalView view)
    {
        _view = view;
        _view.gameObject.SetActive(false);
        Subscribe();
    }

    public void ShowArsenal() => _view.gameObject.SetActive(true);

    public void AddGun(SaveLoadGun loadGun)
    {
        ShowArsenal();
        Gun gun= _gunFactory.Get(loadGun);
        _view.AddGun(gun);
    }

    private void Subscribe()
    {
        _view.AddNewGun += AddNewGun;
        _view.CalculateModifiersBallistic += CalculateBallisticModifiers;
        _view.ReturnToArmor += ReturnToArmor;
        _view.CalculateModifiersWeapon += CalculateWeaponModifiers;
    }

    private void Unscribe()
    {
        _view.AddNewGun -= AddNewGun;
        _view.CalculateModifiersBallistic -= CalculateBallisticModifiers;
        _view.ReturnToArmor -= ReturnToArmor;
        _view.CalculateModifiersWeapon -= CalculateWeaponModifiers;
    }

    private void AddNewGun()
    {
        _audioManager.PlayClick();
        AddNewGunDown?.Invoke();
        _view.gameObject.SetActive(false);
    }

    private void ReturnToArmor()
    {
        _audioManager.PlayCancel();
        ReturnToArmorDown?.Invoke();
        _view.gameObject.SetActive(false);
    }

    private void CalculateBallisticModifiers()
    {
        _audioManager.PlayClick();
        CalculateBallisticModifiersDown?.Invoke();
        _view.gameObject.SetActive(false);
    }
    private void CalculateWeaponModifiers()
    {
        _audioManager.PlayClick();
        CalculateWeaponModifiersDown?.Invoke();
        _view.gameObject.SetActive(false);
    }
}
