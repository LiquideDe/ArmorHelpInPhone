using System;
using Zenject;

namespace ArmorHelp
{
    public class QRScannerPresenter : IPresenter
    {
        public event Action<string> ReturnValue;
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
            _audioManager.PlayDone();
            ReturnValue?.Invoke(value);
            Unscribe();
            _qrScanner.DestroyView();
        }
    }
}

