using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        _armorPresenter.ScanArmorQr += ShowQrScannerArmor;
        _armorPresenter.Initialize(armorView);

        ArsenalView arsenalView = _viewFactory.Get(TypeScene.Arsenal).GetComponent<ArsenalView>();
        _arsenalPresenter = (ArsenalPresenter)_presenterFactory.Get(TypeScene.Arsenal);
        _arsenalPresenter.ReturnToArmorDown += ShowArmor;
        _arsenalPresenter.AddNewGunDown += ShowListGunPanel;
        _arsenalPresenter.CalculateBallisticModifiersDown += ShowBallisticModifiers;
        _arsenalPresenter.CalculateWeaponModifiersDown += ShowWeaponModifiers;
        _arsenalPresenter.GotToShop += ShowQrScannerJson;
        _arsenalPresenter.Initialize(arsenalView);
    }        

    private void ShowArmor() => _armorPresenter.ShowView();

    private void ShowArsenal() => _arsenalPresenter.ShowArsenal();

    private void ShowDamageParametersPanel(Character character)
    {
        PanelDamageParametersView parametersView = _viewFactory.Get(TypeScene.DamageParametersPanel).GetComponent<PanelDamageParametersView>();
        PanelDamageParametersPresenter parametersPresenter = (PanelDamageParametersPresenter)_presenterFactory.Get(TypeScene.DamageParametersPanel);
        parametersPresenter.Cancel += ShowArmor;
        parametersPresenter.ReturnTextToArmor += ShowDamageText;
        parametersPresenter.Initialize(parametersView, character);
    }

    private void ShowDamageText(string text, int damage)
    {
        PanelText panelText = _viewFactory.Get(TypeScene.TextDamage).GetComponent<PanelText>();
        panelText.Initialize(text);
        panelText.PanelClose += ShowArmor;
        _armorPresenter.TakeDamage();
    }

    private void ShowListGunPanel()
    {
        ListWithNewGunView listView = _viewFactory.Get(TypeScene.ListWithGuns).GetComponent<ListWithNewGunView>();
        ListWithNewGunsPresenter newGunsPresenter = (ListWithNewGunsPresenter)_presenterFactory.Get(TypeScene.ListWithGuns);
        newGunsPresenter.Close += ShowArsenal;
        newGunsPresenter.OpenCreationPanel += ShowCreationGunPanel;
        newGunsPresenter.OpenQr += ShowQrScannerGun;
        newGunsPresenter.SetThisGun += SetGun;
        newGunsPresenter.Initialize(listView);
    }

    private void SetGun(SaveLoadGun gun) => _arsenalPresenter.AddGun(gun);
    

    private void ShowCreationGunPanel()
    {
        NewGunPanel newGunPanel = _viewFactory.Get(TypeScene.CreateGunPanel).GetComponent<NewGunPanel>();        
        newGunPanel.ClosePanel += ShowArsenal;
        newGunPanel.ReturnNewGun += SetGun;
        newGunPanel.Initialize();
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

    private void ShowQrScannerGun()
    {
        QRScanner qRScanner = _viewFactory.Get(TypeScene.QrScanner).GetComponent<QRScanner>();
        QRScannerPresenter scannerPresenter = (QRScannerPresenter)_presenterFactory.Get(TypeScene.QrScanner);
        scannerPresenter.CloseQr += ShowArsenal;
        scannerPresenter.ReturnValue += TransferStringToGun;
        scannerPresenter.Initialize(qRScanner);
    }

    private void TransferStringToGun(string value)
    {
        byte[] bytes = Encoding.Default.GetBytes(value);
        value = Encoding.UTF8.GetString(bytes);
        SaveLoadGun gun = new SaveLoadGun();
        List<string> strings = new List<string>();
        strings = value.Split(new char[] { '/' }).ToList();
        Debug.Log(strings);
        if (string.Compare(strings[0], "W", true) == 0)
        {
            int.TryParse(strings[6], out int type);
            int.TryParse(strings[4], out int auto);
            int.TryParse(strings[3], out int semi);
            int.TryParse(strings[5], out int clip);
            gun.type = type;
            gun.name = strings[1];
            gun.autoFire = auto;
            gun.semiAutoFire = semi;
            if (strings[2] != "-")
                gun.singleFire = true;
            else
                gun.singleFire = false;

            gun.maxClip = clip;
            Debug.Log($"clip = {clip}");
            gun.totalClips = 3;
            SetGun(gun);
        }            
    }

    private void ShowQrScannerJson()
    {
        QRScanner qRScanner = _viewFactory.Get(TypeScene.QrScanner).GetComponent<QRScanner>();
        QRScannerPresenter scannerPresenter = (QRScannerPresenter)_presenterFactory.Get(TypeScene.QrScanner);
        scannerPresenter.CloseQr += ShowArsenal;
        scannerPresenter.ReturnValue += ShowShop;
        scannerPresenter.Initialize(qRScanner);
    }

    private void ShowShop(string data)
    {
        ShopView shopView = _viewFactory.Get(TypeScene.Shop).GetComponent<ShopView>();
        shopView.CloseShop += ShowArsenal;
        shopView.Initialize(data);
    }

    private void ShowQrScannerArmor()
    {
        QRScanner qRScanner = _viewFactory.Get(TypeScene.QrScanner).GetComponent<QRScanner>();
        QRScannerPresenter scannerPresenter = (QRScannerPresenter)_presenterFactory.Get(TypeScene.QrScanner);
        scannerPresenter.CloseQr += ShowArmor;
        scannerPresenter.ReturnValue += _armorPresenter.LoadDataFromQr;
        scannerPresenter.Initialize(qRScanner);
    }
}
