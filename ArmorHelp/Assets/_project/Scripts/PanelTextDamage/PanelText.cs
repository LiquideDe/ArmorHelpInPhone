using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace ArmorHelp
{
    public class PanelText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textDamage;
        [SerializeField] Button _buttonClose;

        public event Action PanelClose;

        private void OnEnable() => _buttonClose.onClick.AddListener(Cancel);

        private void OnDisable() => _buttonClose.onClick.RemoveAllListeners();

        public void Initialize(string text)
        {
            textDamage.text = text;
        }

        public void Cancel()
        {
            PanelClose?.Invoke();
            Destroy(gameObject);
        }
    }
}

