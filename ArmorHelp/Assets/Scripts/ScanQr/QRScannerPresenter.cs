using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

public class QRScannerPresenter : IPresenter
{
    public event Action<SaveLoadGun> ReturnGunFromQR;
    public event Action CloseQr;

    private AudioManager _audioManager;

    private QRScanner _qrScanner;

    [Inject]
    private void Construct(AudioManager audioManager) => _audioManager = audioManager;

    public void Initialize(QRScanner qrScanner)
    {
        _qrScanner = qrScanner;
        Subscribe();
        _qrScanner.StartQrReading();
    }

    private void Subscribe()
    {
        _qrScanner.Cancel += CancelDown;
        _qrScanner.GetScannedText += ReturnFromQrCode;
    }

    private void Unscribe()
    {
        _qrScanner.Cancel -= CancelDown;
        _qrScanner.GetScannedText -= ReturnFromQrCode;
    }

    private void CancelDown()
    {
        _audioManager.PlayCancel();
        Unscribe();
        CloseQr?.Invoke();
        _qrScanner.DestroyView();
    }

    private void ReturnFromQrCode(string value)
    {

        try
        {
            SaveLoadGun gun = JsonUtility.FromJson<SaveLoadGun>(value);
        }

        catch
        {
            _audioManager.PlayWarning();
            _qrScanner.ShowError();
            _qrScanner.StartQrReading();
        }

        finally
        {
            SaveLoadGun gun = JsonUtility.FromJson<SaveLoadGun>(value);
            _audioManager.PlayDone();
            Unscribe();
            _qrScanner.DestroyView();
            ReturnGunFromQR?.Invoke(gun);
        }      
    }

}
