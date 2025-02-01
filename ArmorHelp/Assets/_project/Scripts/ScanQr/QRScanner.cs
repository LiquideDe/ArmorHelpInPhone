using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZXing;

namespace ArmorHelp
{
    public class QRScanner : MonoBehaviour
    {
        [SerializeField] RawImage _rawImage;
        [SerializeField] Button _buttonClose;
        [SerializeField] GameObject _textError;
        [SerializeField] TextMeshProUGUI _textErrorText;

        public event System.Action<string> GetScannedText;
        public event System.Action Cancel;

        public Coroutine _coroutine;

        WebCamTexture webcamTexture;
        string QrCode = string.Empty;

        private void OnEnable() => _buttonClose.onClick.AddListener(CancelPressed);

        public void StartQrReading()
        {
            var renderer = _rawImage;
            webcamTexture = new WebCamTexture(512, 512);
            renderer.texture = webcamTexture;
            StartCoroutine(GetQRCode());
        }

        public void DestroyView() => Destroy(gameObject);

        public void ShowError() => _textError.SetActive(true);

        IEnumerator GetQRCode()
        {
            IBarcodeReader barCodeReader = new BarcodeReader();
            webcamTexture.Play();
            var snap = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.ARGB32, false);
            while (string.IsNullOrEmpty(QrCode))
            {
                try
                {
                    snap.SetPixels32(webcamTexture.GetPixels32());
                    var Result = barCodeReader.Decode(snap.GetRawTextureData(), webcamTexture.width, webcamTexture.height, RGBLuminanceSource.BitmapFormat.ARGB32);
                    if (Result != null)
                    {
                        QrCode = Result.Text;
                        if (!string.IsNullOrEmpty(QrCode))
                        {
                            Debug.Log("DECODED TEXT FROM QR: " + QrCode);
                            GetScannedText?.Invoke(QrCode);
                            break;
                        }
                    }
                }
                catch (Exception ex) { Debug.LogWarning(ex.Message); }
                yield return null;
            }
            webcamTexture.Stop();
        }

        private void CancelPressed()
        {
            Cancel?.Invoke();
        }

    }
}


