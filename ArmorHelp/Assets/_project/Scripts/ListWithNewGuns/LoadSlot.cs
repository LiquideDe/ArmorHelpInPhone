using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

namespace ArmorHelp
{
    public class LoadSlot : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private TextMeshProUGUI _textName;
        [SerializeField] private Button _buttonDestroy;

        public event Action<SaveLoadGun> ReturnThisGun;
        public event Action<string> RemoveThisGun;

        private SaveLoadGun _loadGun;

        private void OnEnable() => _buttonDestroy.onClick.AddListener(Remove);

        private void OnDisable() => _buttonDestroy.onClick.RemoveAllListeners();

        public void OnPointerDown(PointerEventData eventData)
        {
            ReturnThisGun?.Invoke(_loadGun);
        }

        public void Initialize(SaveLoadGun loadGun)
        {
            gameObject.SetActive(true);
            _loadGun = loadGun;
            _textName.text = loadGun.name;
        }

        public void Remove()
        {
            RemoveThisGun?.Invoke(_loadGun.name);
            Destroy(gameObject);
        }
    }
}

