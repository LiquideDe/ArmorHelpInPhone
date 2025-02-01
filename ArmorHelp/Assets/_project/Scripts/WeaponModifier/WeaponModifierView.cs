using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace ArmorHelp
{
    public class WeaponModifierView : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _textTotalModifier, _textLightings, _textTypeAttack, _textTarget, _textSize, _textLandscape,
            _textSuperiority, _textQuality;

        [SerializeField]
        Button _buttonPrevLightings, _buttonPrevTypeAttack, _buttonPrevTarget, _buttonPrevSize, _buttonPrevLandscape,
            _buttonPrevSuperiority, _buttonPrevQuality;

        [SerializeField]
        Button _buttonNextLightings, _buttonNextTypeAttack, _buttonNextTarget, _buttonNextSize, _buttonNextLandscape,
            _buttonNextSuperiority, _buttonNextQuality;

        [SerializeField] Button _buttonExit;

        public event Action PrevLightings, PrevTypeAttack, PrevTarget, PrevSize, PrevLandscape, PrevSuperiority, PrevQuality;
        public event Action NextLightings, NextTypeAttack, NextTarget, NextSize, NextLandscape, NextSuperiority, NextQuality;
        public event Action Exit;

        private void OnEnable()
        {
            _buttonExit.onClick.AddListener(ExitPressed);

            _buttonPrevLightings.onClick.AddListener(PrevLightingsPressed);
            _buttonPrevTypeAttack.onClick.AddListener(PrevTypeAttackPressed);
            _buttonPrevTarget.onClick.AddListener(PrevTargetPressed);
            _buttonPrevSize.onClick.AddListener(PrevSizePressed);
            _buttonPrevLandscape.onClick.AddListener(PrevLandscapePressed);
            _buttonPrevSuperiority.onClick.AddListener(PrevSuperiorityPressed);
            _buttonPrevQuality.onClick.AddListener(PrevQualityPressed);

            _buttonNextLightings.onClick.AddListener(NextLightingsPressed);
            _buttonNextTypeAttack.onClick.AddListener(NextTypeAttackPressed);
            _buttonNextTarget.onClick.AddListener(NextTargetPressed);
            _buttonNextSize.onClick.AddListener(NextSizePressed);
            _buttonNextLandscape.onClick.AddListener(NextLandscapePressed);
            _buttonNextSuperiority.onClick.AddListener(NextSuperiorityPressed);
            _buttonNextQuality.onClick.AddListener(NextQualityPressed);
        }

        private void OnDisable()
        {
            _buttonExit.onClick.RemoveAllListeners();

            _buttonPrevLightings.onClick.RemoveAllListeners();
            _buttonPrevTypeAttack.onClick.RemoveAllListeners();
            _buttonPrevTarget.onClick.RemoveAllListeners();
            _buttonPrevSize.onClick.RemoveAllListeners();
            _buttonPrevLandscape.onClick.RemoveAllListeners();
            _buttonPrevSuperiority.onClick.RemoveAllListeners();
            _buttonPrevQuality.onClick.RemoveAllListeners();

            _buttonNextLightings.onClick.RemoveAllListeners();
            _buttonNextTypeAttack.onClick.RemoveAllListeners();
            _buttonNextTarget.onClick.RemoveAllListeners();
            _buttonNextSize.onClick.RemoveAllListeners();
            _buttonNextLandscape.onClick.RemoveAllListeners();
            _buttonNextSuperiority.onClick.RemoveAllListeners();
            _buttonNextQuality.onClick.RemoveAllListeners();
        }

        public void SetTotalModifierText(string value) => _textTotalModifier.text = value;
        public void SetLightingsText(string value) => _textLightings.text = value;
        public void SetTypeAttackText(string value) => _textTypeAttack.text = value;
        public void SetTargetText(string value) => _textTarget.text = value;
        public void SetSizeText(string value) => _textSize.text = value;
        public void SetLandscapeText(string value) => _textLandscape.text = value;
        public void SetSuperiorityText(string value) => _textSuperiority.text = value;
        public void SetQualityText(string value) => _textQuality.text = value;

        private void PrevLightingsPressed() => PrevLightings?.Invoke();
        private void PrevTypeAttackPressed() => PrevTypeAttack?.Invoke();
        private void PrevTargetPressed() => PrevTarget?.Invoke();
        private void PrevSizePressed() => PrevSize?.Invoke();
        private void PrevLandscapePressed() => PrevLandscape?.Invoke();
        private void PrevSuperiorityPressed() => PrevSuperiority?.Invoke();
        private void PrevQualityPressed() => PrevQuality?.Invoke();

        private void NextLightingsPressed() => NextLightings?.Invoke();
        private void NextTypeAttackPressed() => NextTypeAttack?.Invoke();
        private void NextTargetPressed() => NextTarget?.Invoke();
        private void NextSizePressed() => NextSize?.Invoke();
        private void NextLandscapePressed() => NextLandscape?.Invoke();
        private void NextSuperiorityPressed() => NextSuperiority?.Invoke();
        private void NextQualityPressed() => NextQuality?.Invoke();

        private void ExitPressed() => Exit?.Invoke();
    }
}

