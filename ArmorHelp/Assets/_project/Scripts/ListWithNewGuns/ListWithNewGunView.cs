using UnityEngine;
using UnityEngine.UI;
using System;

namespace ArmorHelp
{
    public class ListWithNewGunView : MonoBehaviour
    {
        [SerializeField] Button _buttonClose, _buttonCreateGun, _buttonScanQr;
        [SerializeField] Transform _content;
        [SerializeField] LoadSlot _loadSLotPrefab;

        public event Action<string> RemoveGun;
        public event Action ClosePanel;
        public event Action OpenQr;
        public event Action<SaveLoadGun> UseThisGun;
        public event Action OpenCreationPanel;

        private void OnEnable()
        {
            _buttonClose.onClick.AddListener(ClosePanelPressed);
            _buttonCreateGun.onClick.AddListener(OpenCreationPanelPressed);
            _buttonScanQr.onClick.AddListener(OpenQrPressed);
        }

        private void OnDisable()
        {
            _buttonClose.onClick.RemoveAllListeners();
            _buttonCreateGun.onClick.RemoveAllListeners();
            _buttonScanQr.onClick.RemoveAllListeners();
        }

        public void AddGunToList(SaveLoadGun gun)
        {
            LoadSlot loadSlot = Instantiate(_loadSLotPrefab, _content);
            loadSlot.Initialize(gun);
            loadSlot.RemoveThisGun += RemoveGunPressed;
            loadSlot.ReturnThisGun += UseThisGunPressed;
        }

        public void DestroyView() => Destroy(gameObject);

        private void RemoveGunPressed(string name) => RemoveGun?.Invoke(name);

        private void ClosePanelPressed() => ClosePanel?.Invoke();

        private void OpenQrPressed() => OpenQr?.Invoke();

        private void UseThisGunPressed(SaveLoadGun gun) => UseThisGun?.Invoke(gun);

        private void OpenCreationPanelPressed() => OpenCreationPanel?.Invoke();
    }
}

