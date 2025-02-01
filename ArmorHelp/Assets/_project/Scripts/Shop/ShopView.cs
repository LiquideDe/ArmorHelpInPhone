using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Zenject;

namespace ArmorHelp
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private Button _buttonClose;
        public event Action CloseShop;
        private ViewFactory _viewFactory;
        private AudioManager _audioManager;
        private string _data;
        public enum TypeEquipment
        {
            Thing, Melee, Range, Armor, Special, Grenade
        }
        [Inject]
        private void Construct(ViewFactory viewFactory, AudioManager audioManager)
        {
            _viewFactory = viewFactory;
            _audioManager = audioManager;
        }

        private void OnDisable()
        {
            _buttonClose.onClick.RemoveAllListeners();
        }

        public void Initialize(string url)
        {
            _buttonClose.onClick.AddListener(ClosePressed);
            StartCoroutine(Download(url));
        }

        IEnumerator Download(string url)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                _data = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                ReadData();
            }

            www.Dispose();
        }

        private void ReadData()
        {
            _audioManager.PlayDone();
            string[] data = _data.Split('\n');

            for (int i = 0; i < data.Length; i++)
            {
                JSONTypeReader typeReader = JsonUtility.FromJson<JSONTypeReader>(data[i]);

                if (data[i].Length < 1)
                    break;

                if (string.Compare(typeReader.typeEquipment, TypeEquipment.Thing.ToString(), true) == 0)
                    AddEquipment(data[i]);

                else if (string.Compare(typeReader.typeEquipment, TypeEquipment.Armor.ToString(), true) == 0)
                    AddArmor(data[i]);

                else if (string.Compare(typeReader.typeEquipment, TypeEquipment.Range.ToString(), true) == 0)
                    AddRange(data[i]);

                else if (string.Compare(typeReader.typeEquipment, TypeEquipment.Melee.ToString(), true) == 0)
                    AddMelee(data[i]);

                else
                    AddGrenade(data[i]);
            }
        }

        private void AddEquipment(string data)
        {
            JSONEquipmentReader reader = JsonUtility.FromJson<JSONEquipmentReader>(data);
            EquipmentFormInShop formInShop = _viewFactory.Get(TypeScene.EquipmentForm).GetComponent<EquipmentFormInShop>();
            formInShop.transform.SetParent(_content);
            formInShop.Initialize(reader);
        }

        private void AddArmor(string data)
        {
            JSONArmorReader reader = JsonUtility.FromJson<JSONArmorReader>(data);
            ArmorFormInShop formInShop = _viewFactory.Get(TypeScene.ArmorForm).GetComponent<ArmorFormInShop>();
            formInShop.transform.SetParent(_content);
            formInShop.Initialize(reader);
        }

        private void AddMelee(string data)
        {
            JSONMeleeReader reader = JsonUtility.FromJson<JSONMeleeReader>(data);
            WeaponFormInShop formInShop = _viewFactory.Get(TypeScene.WeaponForm).GetComponent<WeaponFormInShop>();
            formInShop.transform.SetParent(_content);
            formInShop.Initialize(reader);
        }

        private void AddRange(string data)
        {
            JSONRangeReader reader = JsonUtility.FromJson<JSONRangeReader>(data);
            WeaponFormInShop formInShop = _viewFactory.Get(TypeScene.WeaponForm).GetComponent<WeaponFormInShop>();
            formInShop.transform.SetParent(_content);
            formInShop.Initialize(reader);
        }

        private void AddGrenade(string data)
        {
            JSONGrenadeReader reader = JsonUtility.FromJson<JSONGrenadeReader>(data);
            WeaponFormInShop formInShop = _viewFactory.Get(TypeScene.WeaponForm).GetComponent<WeaponFormInShop>();
            formInShop.transform.SetParent(_content);
            formInShop.Initialize(reader);
        }

        private void ClosePressed()
        {
            _audioManager.PlayClick();
            CloseShop?.Invoke();
            Destroy(gameObject);
        }
    }
}


