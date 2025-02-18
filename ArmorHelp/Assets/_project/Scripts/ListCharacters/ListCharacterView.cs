using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ArmorHelp
{
    public class ListCharacterView : MonoBehaviour
    {
        [SerializeField] Button _buttonLoadPrefab;
        [SerializeField] Transform listWithLoads;
        [SerializeField] Button _buttonClose;

        public event Action<string> OpenThisPath;
        public event Action Close;

        public void Initialize(string[] loads)
        {
            _buttonClose.onClick.AddListener(ClosePressed);

            foreach (string load in loads)
            {
                var dir = new FileInfo(load);
                Button button = Instantiate(_buttonLoadPrefab, listWithLoads);
                TextMeshProUGUI textMesh = button.GetComponentInChildren<TextMeshProUGUI>();
                textMesh.text = dir.Name;
                SetListenerToButton(button, dir.FullName);
                button.gameObject.SetActive(true);
            }
        }

        public void DestroyView() => Destroy(gameObject);

        private void SetListenerToButton(Button button, string value)
        {
            string path = value;
            button.onClick.AddListener(() => LoadCharacter(path));
        }

        private void LoadCharacter(string path) => OpenThisPath?.Invoke(path);

        private void ClosePressed() => Close?.Invoke();
    }
}


