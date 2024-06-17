using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMediator
{
    private PresenterFactory _presenterFactory;
    private ViewFactory _viewFactory;
    private ArmorPresenter _armorPresenter;
    private ArsenalPresenter _arsenalPresenter;
    private BallisticModifiersPresenter _ballisticPresenter;
    private WeaponModifierPresenter _weaponPresenter;

    public SceneMediator(PresenterFactory presenterFactory, ViewFactory viewFactory)
    {
        _presenterFactory = presenterFactory;
        _viewFactory = viewFactory;
    }

    public void Initialize()
    {
        ArmorView armorView = _viewFactory.Get(TypeScene.Armor).GetComponent<ArmorView>();
        _armorPresenter = (ArmorPresenter)_presenterFactory.Get(TypeScene.Armor);
        _armorPresenter.GoToArsenal += ShowArsenal;
        _armorPresenter.GoToDamagePanel += ShowDamageParametersPanel;
        _armorPresenter.Initialize(armorView);

        ArsenalView arsenalView = _viewFactory.Get(TypeScene.Arsenal).GetComponent<ArsenalView>();
        _arsenalPresenter = (ArsenalPresenter)_presenterFactory.Get(TypeScene.Arsenal);
        _arsenalPresenter.ReturnToArmorDown += ShowArmor;
        _arsenalPresenter.AddNewGunDown += ShowListGunPanel;
        _arsenalPresenter.CalculateBallisticModifiersDown += ShowBallisticModifiers;
        _arsenalPresenter.CalculateWeaponModifiersDown += ShowWeaponModifiers;
        _arsenalPresenter.Initialize(arsenalView);
    }    

    private void ShowArmor() => _armorPresenter.ShowView();

    private void ShowArsenal() => _arsenalPresenter.ShowArsenal();

    private void ShowDamageParametersPanel(SaveLoadArmor armor)
    {
        PanelDamageParametersView parametersView = _viewFactory.Get(TypeScene.DamageParametersPanel).GetComponent<PanelDamageParametersView>();
        PanelDamageParametersPresenter parametersPresenter = (PanelDamageParametersPresenter)_presenterFactory.Get(TypeScene.DamageParametersPanel);
        parametersPresenter.Cancel += ShowArmor;
        parametersPresenter.ReturnTextToArmor += ShowDamageText;
        parametersPresenter.Initialize(parametersView, armor);
    }

    private void ShowDamageText(string text, int damage)
    {
        PanelText panelText = _viewFactory.Get(TypeScene.TextDamage).GetComponent<PanelText>();
        panelText.Initialize(text);
        panelText.PanelClose += ShowArmor;
        _armorPresenter.TakeDamage(damage);
    }

    private void ShowListGunPanel()
    {
        ListWithNewGunView listView = _viewFactory.Get(TypeScene.ListWithGuns).GetComponent<ListWithNewGunView>();
        ListWithNewGunsPresenter newGunsPresenter = (ListWithNewGunsPresenter)_presenterFactory.Get(TypeScene.ListWithGuns);
        newGunsPresenter.Close += ShowArsenal;
        newGunsPresenter.OpenCreationPanel += ShowCreationGunPanel;
        newGunsPresenter.OpenQr += ShowQrScanner;
        newGunsPresenter.SetThisGun += SetGun;
        newGunsPresenter.Initialize(listView);
    }

    private void SetGun(SaveLoadGun gun) => _arsenalPresenter.AddGun(gun);
    

    private void ShowCreationGunPanel()
    {
        NewGunPanel newGunPanel = _viewFactory.Get(TypeScene.CreateGunPanel).GetComponent<NewGunPanel>();
        newGunPanel.ClosePanel += ShowArsenal;
        newGunPanel.ReturnNewGun += SetGun;
    }

    private void ShowBallisticModifiers()
    {
        if (_ballisticPresenter == null)
        {
            BallisticModifiersView ballisticView = _viewFactory.Get(TypeScene.BallisticModifier).GetComponent<BallisticModifiersView>();
            _ballisticPresenter = (BallisticModifiersPresenter)_presenterFactory.Get(TypeScene.BallisticModifier);
            _ballisticPresenter.CloseBallisticModifier += ShowArsenal;
            _ballisticPresenter.Initialize(ballisticView);
        }
        else
            _ballisticPresenter.ShowView();
    }

    private void ShowWeaponModifiers()
    {
        if (_weaponPresenter == null)
        {
            WeaponModifierView weaponView = _viewFactory.Get(TypeScene.WeaponModifier).GetComponent<WeaponModifierView>();
            _weaponPresenter = (WeaponModifierPresenter)_presenterFactory.Get(TypeScene.WeaponModifier);
            _weaponPresenter.CloseWeaponModifier += ShowArsenal;
            _weaponPresenter.Initialize(weaponView);
        }
        else
            _weaponPresenter.ShowView();
    }

    private void ShowQrScanner()
    {
        QRScanner qRScanner = _viewFactory.Get(TypeScene.QrScanner).GetComponent<QRScanner>();
        QRScannerPresenter scannerPresenter = (QRScannerPresenter)_presenterFactory.Get(TypeScene.QrScanner);
        scannerPresenter.CloseQr += ShowArsenal;
        scannerPresenter.ReturnGunFromQR += SetGun;
        scannerPresenter.Initialize(qRScanner);
    }
}
