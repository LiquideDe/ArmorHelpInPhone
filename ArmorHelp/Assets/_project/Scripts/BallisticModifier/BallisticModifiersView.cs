using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace ArmorHelp
{
    public class BallisticModifiersView : MonoBehaviour
    {

        [SerializeField]
        Button _buttonPrevLight, _buttonPrevDistance, _buttonPrevShoot, _buttonPrevAiming, _buttonPrevTarget, _buttonPrevWeaponKit,
            _buttonPrevHandle, _buttonPrevNavigate, _buttonPrevSize;
        [SerializeField]
        Button _buttonNextLight, _buttonNextDistance, _buttonNextShoot, _buttonNextAiming, _buttonNextTarget, _buttonNextWeaponKit,
            _buttonNextHandle, _buttonNextNavigate, _buttonNextSize;
        [SerializeField] Button _buttonExit;
        [SerializeField] TextMeshProUGUI _textLight, _textDistance, _textShoot, _textAiming, _textTarget, _textWeaponKit, _textHandle, _textNavigate, _textTotalModifier, _textSize;

        public event Action PrevLight, PrevDistance, PrevShoot, PrevAiming, PrevTarget, PrevWeaponKit, PrevHandle, PrevNavigate, PrevSize;
        public event Action NextLight, NextDistance, NextShoot, NextAiming, NextTarget, NextWeaponKit, NextHandle, NextNavigate, NextSize;
        public event Action Exit;

        private void OnEnable()
        {
            _buttonPrevLight.onClick.AddListener(PrevLightPressed);
            _buttonPrevDistance.onClick.AddListener(PrevDistancePressed);
            _buttonPrevShoot.onClick.AddListener(PrevShootPressed);
            _buttonPrevAiming.onClick.AddListener(PrevAimingPressed);
            _buttonPrevTarget.onClick.AddListener(PrevTargetPressed);
            _buttonPrevWeaponKit.onClick.AddListener(PrevWeaponKitPressed);
            _buttonPrevHandle.onClick.AddListener(PrevHandlePressed);
            _buttonPrevNavigate.onClick.AddListener(PrevNavigatePressed);
            _buttonPrevSize.onClick.AddListener(PrevSizePressed);

            _buttonNextLight.onClick.AddListener(NextLightPressed);
            _buttonNextDistance.onClick.AddListener(NextDistancePressed);
            _buttonNextShoot.onClick.AddListener(NextShootPressed);
            _buttonNextAiming.onClick.AddListener(NextAimingPressed);
            _buttonNextTarget.onClick.AddListener(NextTargetPressed);
            _buttonNextWeaponKit.onClick.AddListener(NextWeaponKitPressed);
            _buttonNextHandle.onClick.AddListener(NextHandlePressed);
            _buttonNextNavigate.onClick.AddListener(NextNavigatePressed);
            _buttonNextSize.onClick.AddListener(NextSizePressed);

            _buttonExit.onClick.AddListener(ExitPressed);
        }

        private void OnDisable()
        {
            _buttonPrevLight.onClick.RemoveAllListeners();
            _buttonPrevDistance.onClick.RemoveAllListeners();
            _buttonPrevShoot.onClick.RemoveAllListeners();
            _buttonPrevAiming.onClick.RemoveAllListeners();
            _buttonPrevTarget.onClick.RemoveAllListeners();
            _buttonPrevWeaponKit.onClick.RemoveAllListeners();
            _buttonPrevHandle.onClick.RemoveAllListeners();
            _buttonPrevNavigate.onClick.RemoveAllListeners();
            _buttonPrevSize.onClick.RemoveAllListeners();

            _buttonNextLight.onClick.RemoveAllListeners();
            _buttonNextDistance.onClick.RemoveAllListeners();
            _buttonNextShoot.onClick.RemoveAllListeners();
            _buttonNextAiming.onClick.RemoveAllListeners();
            _buttonNextTarget.onClick.RemoveAllListeners();
            _buttonNextWeaponKit.onClick.RemoveAllListeners();
            _buttonNextHandle.onClick.RemoveAllListeners();
            _buttonNextNavigate.onClick.RemoveAllListeners();
            _buttonNextSize.onClick.RemoveAllListeners();

            _buttonExit.onClick.RemoveAllListeners();
        }

        public void SetLightText(string value) => _textLight.text = value;
        public void SetDistanceText(string value) => _textDistance.text = value;
        public void SetShootText(string value) => _textShoot.text = value;
        public void SetAimingText(string value) => _textAiming.text = value;
        public void SetTargetText(string value) => _textTarget.text = value;
        public void SetWeaponKitText(string value) => _textWeaponKit.text = value;
        public void SetHandleText(string value) => _textHandle.text = value;
        public void SetNavigateText(string value) => _textNavigate.text = value;
        public void SetSizeText(string value) => _textSize.text = value;
        public void SetTotalModifierText(string value) => _textTotalModifier.text = value;

        private void PrevLightPressed() => PrevLight?.Invoke();
        private void PrevDistancePressed() => PrevDistance?.Invoke();
        private void PrevShootPressed() => PrevShoot?.Invoke();
        private void PrevAimingPressed() => PrevAiming?.Invoke();
        private void PrevTargetPressed() => PrevTarget?.Invoke();
        private void PrevWeaponKitPressed() => PrevWeaponKit?.Invoke();
        private void PrevHandlePressed() => PrevHandle?.Invoke();
        private void PrevNavigatePressed() => PrevNavigate?.Invoke();
        private void PrevSizePressed() => PrevSize?.Invoke();

        private void NextLightPressed() => NextLight?.Invoke();
        private void NextDistancePressed() => NextDistance?.Invoke();
        private void NextShootPressed() => NextShoot?.Invoke();
        private void NextAimingPressed() => NextAiming?.Invoke();
        private void NextTargetPressed() => NextTarget?.Invoke();
        private void NextWeaponKitPressed() => NextWeaponKit?.Invoke();
        private void NextHandlePressed() => NextHandle?.Invoke();
        private void NextNavigatePressed() => NextNavigate?.Invoke();
        private void NextSizePressed() => NextSize?.Invoke();

        private void ExitPressed() => Exit?.Invoke();
    }
}

