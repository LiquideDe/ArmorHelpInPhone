using UnityEngine;
using System;
using Zenject;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace ArmorHelp
{
    public class ArmorPresenter : IPresenter
    {
        public event Action GoToArsenal;
        public event Action<Character> GoToDamagePanel;
        public event Action ScanArmorQr;
        private ArmorView _view;
        private AudioManager _audioManager;
        private Character _character;

        [Inject]
        private void Construct(AudioManager audioManager) => _audioManager = audioManager;

        public void Initialize(ArmorView view)
        {
            _view = view;
            _character = new Character();
            LoadDataFromSave();
            Subscribe();
        }

        public void LoadDataFromQr(string data)
        {
            SaveLoadArmor armor = new SaveLoadArmor();
            List<string> strings = new List<string>();
            strings = data.Split(new char[] { '/' }).ToList();
            if (string.Compare(strings[0], "A", true) == 0)
            {
                _view.InputBonusWP.text = strings[1];
                _view.InputArmorHead.text = strings[2];
                _view.InputTotalHead.text = strings[3];

                _view.InputArmorBody.text = strings[4];
                _view.InputTotalBody.text = strings[5];

                _view.InputArmorRightHand.text = strings[6];
                _view.InputTotalRightHand.text = strings[7];

                _view.InputArmorLeftHand.text = strings[8];
                _view.InputTotalLeftHand.text = strings[9];

                _view.InputArmorRightLeg.text = strings[10];
                _view.InputTotalRightLeg.text = strings[11];

                _view.InputArmorLeftLeg.text = strings[12];
                _view.InputTotalLeftLeg.text = strings[13];
                if (strings.Count >= 15)
                {
                    int.TryParse(strings[14], out int wounds);
                    _view.SetWound(wounds);
                }
                ParseInputs();
            }
            else
                Debug.Log($"Не прочитал {data}");
        }

        public void ShowView()
        {
            _audioManager.PlayClick();
            _view.gameObject.SetActive(true);
        }

        public void TakeDamage()
        {
            _view.SetWound(_character.Wounds);
            _view.InputShelterPoint.text = $"{_character.ShelterArmorPoint}";
            TakeCover();
        }

        private void Subscribe()
        {
            _view.GoToDamagePanel += OpenPanelDamageDown;
            _view.PlusWound += PlusWound;
            _view.MinusWound += MinusWound;
            _view.SaveArmor += SaveArmor;
            _view.GoToArsenal += OpenArsenal;
            _view.Exit += Exit;
            _view.TakeCover += TakeCover;
            _view.ParseInputs += ParseInputs;
            _view.ScanQr += ScanQr;
        }

        private void Unscribe()
        {
            _view.GoToDamagePanel -= OpenPanelDamageDown;
            _view.PlusWound -= PlusWound;
            _view.MinusWound -= MinusWound;
            _view.SaveArmor -= SaveArmor;
            _view.GoToArsenal -= OpenArsenal;
            _view.Exit -= Exit;

        }

        private void LoadDataFromSave()
        {
            SaveLoadArmor armor = null;
            if (Application.platform == RuntimePlatform.Android)
            {
                var info = new DirectoryInfo(Application.persistentDataPath);
                var fileInfo = info.GetFiles("*.armor");
                byte[] jsonByte = null;
                foreach (FileInfo file in fileInfo)
                {
                    jsonByte = File.ReadAllBytes(file.FullName);
                    string jsonData = Encoding.UTF8.GetString(jsonByte);
                    armor = JsonUtility.FromJson<SaveLoadArmor>(jsonData);
                }
            }
            else
            {
                var files = Directory.GetFiles($"{Application.dataPath}/StreamingAssets", "*.armor");
                foreach (string path in files)
                {
                    string loadData = File.ReadAllText(path);
                    armor = JsonUtility.FromJson<SaveLoadArmor>(loadData);
                }
            }
            if (armor != null)
            {
                _view.LoadArmor(armor);
                LoadCharacter(armor);
            }
        }

        private void OpenPanelDamageDown()
        {
            _audioManager.PlayClick();
            _view.gameObject.SetActive(false);
            GoToDamagePanel?.Invoke(_character);
        }

        private void LoadCharacter(SaveLoadArmor armor)
        {
            _character.ArmorBody = armor.bodyArmor;
            _character.ArmorHead = armor.headArmor;
            _character.ArmorLeftHand = armor.leftHandArmor;
            _character.ArmorLeftLeg = armor.leftLegArmor;
            _character.ArmorRightHand = armor.rightHandArmor;
            _character.ArmorRightLeg = armor.rightLegArmor;
            _character.BodyTotal = armor.body;
            _character.HeadTotal = armor.head;
            _character.LeftHandTotal = armor.leftHand;
            _character.LeftLegTotal = armor.leftLeg;
            _character.RightHandTotal = armor.rightHand;
            _character.RightLegTotal = armor.rightLeg;
            _character.Wounds = armor.wounds;
            _character.BWillpower = armor.bWillPower;
        }

        private void OpenArsenal()
        {
            _audioManager.PlayClick();
            _view.gameObject.SetActive(false);
            GoToArsenal?.Invoke();
        }

        private void PlusWound()
        {
            _audioManager.PlayClick();
            _character.Wounds++;
            _view.SetWound(_character.Wounds);
        }

        private void MinusWound()
        {
            _audioManager.PlayClick();
            _character.Wounds--;
            _view.SetWound(_character.Wounds);
        }

        private void ParseInputs()
        {
            int.TryParse(_view.InputArmorBody.text, out int armorBody);
            int.TryParse(_view.InputArmorHead.text, out int armorHead);
            int.TryParse(_view.InputArmorLeftHand.text, out int armorLeftHand);
            int.TryParse(_view.InputArmorLeftLeg.text, out int armorLeftLeg);
            int.TryParse(_view.InputArmorRightHand.text, out int armorRightHand);
            int.TryParse(_view.InputArmorRightLeg.text, out int armorRightLeg);
            int.TryParse(_view.InputBonusWP.text, out int bonusW);
            int.TryParse(_view.InputShelterPoint.text, out int shelterPoint);
            int.TryParse(_view.InputTotalBody.text, out int bodyTotal);
            int.TryParse(_view.InputTotalHead.text, out int headTotal);
            int.TryParse(_view.InputTotalLeftHand.text, out int leftHandTotal);
            int.TryParse(_view.InputTotalLeftLeg.text, out int leftLegTotal);
            int.TryParse(_view.InputTotalRightHand.text, out int rightHandTotal);
            int.TryParse(_view.InputTotalRightLeg.text, out int rightLegTotal);

            if (_character.IsHeadSheltered)
            {
                armorHead -= _character.ShelterArmorPoint;
                headTotal -= _character.ShelterArmorPoint;
            }

            if (_character.IsRightHandSheltered)
            {
                rightHandTotal -= _character.ShelterArmorPoint;
                armorRightHand -= _character.ShelterArmorPoint;
            }

            if (_character.IsLeftHandSheltered)
            {
                leftHandTotal -= _character.ShelterArmorPoint;
                armorLeftHand -= _character.ShelterArmorPoint;
            }

            if (_character.IsBodySheltered)
            {
                armorBody -= _character.ShelterArmorPoint;
                bodyTotal -= _character.ShelterArmorPoint;
            }

            if (_character.IsRightLegSheltered)
            {
                armorRightLeg -= _character.ShelterArmorPoint;
                rightLegTotal -= _character.ShelterArmorPoint;
            }

            if (_character.IsLeftLegSheltered)
            {
                armorLeftLeg -= _character.ShelterArmorPoint;
                leftLegTotal -= _character.ShelterArmorPoint;
            }

            _character.ArmorBody = armorBody;
            _character.ArmorHead = armorHead;
            _character.ArmorLeftHand = armorLeftHand;
            _character.ArmorLeftLeg = armorLeftLeg;
            _character.ArmorRightHand = armorRightHand;
            _character.ArmorRightLeg = armorRightLeg;
            _character.BodyTotal = bodyTotal;
            _character.BWillpower = bonusW;
            _character.HeadTotal = headTotal;
            _character.LeftHandTotal = leftHandTotal;
            _character.LeftLegTotal = leftLegTotal;
            _character.RightHandTotal = rightHandTotal;
            _character.RightLegTotal = rightLegTotal;
            _character.ShelterArmorPoint = shelterPoint;
        }

        private void SaveArmor()
        {
            _audioManager.PlayDone();
            ParseInputs();
            SaveLoadArmor armor = new SaveLoadArmor();
            armor.bodyArmor = _character.ArmorBody;
            armor.headArmor = _character.ArmorHead;
            armor.leftHandArmor = _character.ArmorLeftHand;
            armor.leftLegArmor = _character.ArmorLeftLeg;
            armor.rightHandArmor = _character.ArmorRightHand;
            armor.rightLegArmor = _character.ArmorRightLeg;
            armor.body = _character.BodyTotal;
            armor.head = _character.HeadTotal;
            armor.leftHand = _character.LeftHandTotal;
            armor.leftLeg = _character.LeftLegTotal;
            armor.rightHand = _character.RightHandTotal;
            armor.rightLeg = _character.RightLegTotal;
            armor.wounds = _character.Wounds;
            armor.bWillPower = _character.BWillpower;

            if (Application.platform == RuntimePlatform.Android)
            {
                string jsonDataString = JsonUtility.ToJson(armor, true);
                string path = Path.Combine(Application.persistentDataPath, $"armor.armor");
                byte[] jsonbytes = Encoding.UTF8.GetBytes(jsonDataString);
                File.WriteAllBytes(path, jsonbytes);
            }
            else
            {
                string jsonDataString = JsonUtility.ToJson(armor, true);
                string path = Path.Combine($"{Application.dataPath}/StreamingAssets", $"armor.armor");
                File.WriteAllText(path, jsonDataString);
            }
        }
        private void TakeCover()
        {
            Color32 color = new Color32(0, 255, 255, 255);
            if (_view.ToggleShelterHead.isOn)
            {
                _character.IsHeadSheltered = true;
                _view.InputArmorHead.textComponent.color = color;
                _view.InputTotalHead.textComponent.color = color;

                _view.InputArmorHead.text = $"{_character.ArmorHead + _character.ShelterArmorPoint}";
                _view.InputTotalHead.text = $"{_character.HeadTotal + _character.ShelterArmorPoint}";
            }
            else
            {
                _character.IsHeadSheltered = false;
                _view.InputArmorHead.textComponent.color = Color.white;
                _view.InputTotalHead.textComponent.color = Color.white;

                _view.InputArmorHead.text = $"{_character.ArmorHead}";
                _view.InputTotalHead.text = $"{_character.HeadTotal}";
            }

            if (_view.ToggleShelterRightHand.isOn)
            {
                _character.IsRightHandSheltered = true;
                _view.InputArmorRightHand.textComponent.color = color;
                _view.InputTotalRightHand.textComponent.color = color;

                _view.InputArmorRightHand.text = $"{_character.ArmorRightHand + _character.ShelterArmorPoint}";
                _view.InputTotalRightHand.text = $"{_character.RightHandTotal + _character.ShelterArmorPoint}";
            }
            else
            {
                _character.IsRightHandSheltered = false;
                _view.InputArmorRightHand.textComponent.color = Color.white;
                _view.InputTotalRightHand.textComponent.color = Color.white;

                _view.InputArmorRightHand.text = $"{_character.ArmorRightHand}";
                _view.InputTotalRightHand.text = $"{_character.RightHandTotal}";
            }

            if (_view.ToggleShelterLeftHand.isOn)
            {
                _character.IsLeftHandSheltered = true;
                _view.InputArmorLeftHand.textComponent.color = color;
                _view.InputTotalLeftHand.textComponent.color = color;

                _view.InputArmorLeftHand.text = $"{_character.ArmorLeftHand + _character.ShelterArmorPoint}";
                _view.InputTotalLeftHand.text = $"{_character.LeftHandTotal + _character.ShelterArmorPoint}";
            }
            else
            {
                _character.IsLeftHandSheltered = false;
                _view.InputArmorLeftHand.textComponent.color = Color.white;
                _view.InputTotalLeftHand.textComponent.color = Color.white;

                _view.InputArmorLeftHand.text = $"{_character.ArmorLeftHand}";
                _view.InputTotalLeftHand.text = $"{_character.LeftHandTotal}";
            }

            if (_view.ToggleShelterBody.isOn)
            {
                _character.IsBodySheltered = true;
                _view.InputArmorBody.textComponent.color = color;
                _view.InputTotalBody.textComponent.color = color;

                _view.InputArmorBody.text = $"{_character.ArmorBody + _character.ShelterArmorPoint}";
                _view.InputTotalBody.text = $"{_character.BodyTotal + _character.ShelterArmorPoint}";
            }
            else
            {
                _character.IsBodySheltered = false;
                _view.InputArmorBody.textComponent.color = Color.white;
                _view.InputTotalBody.textComponent.color = Color.white;

                _view.InputArmorBody.text = $"{_character.ArmorBody}";
                _view.InputTotalBody.text = $"{_character.BodyTotal}";
            }

            if (_view.ToggleShelterRightLeg.isOn)
            {
                _character.IsRightLegSheltered = true;
                _view.InputArmorRightLeg.textComponent.color = color;
                _view.InputTotalRightLeg.textComponent.color = color;

                _view.InputArmorRightLeg.text = $"{_character.ArmorRightLeg + _character.ShelterArmorPoint}";
                _view.InputTotalRightLeg.text = $"{_character.RightLegTotal + _character.ShelterArmorPoint}";
            }
            else
            {
                _character.IsRightLegSheltered = false;
                _view.InputArmorRightLeg.textComponent.color = Color.white;
                _view.InputTotalRightLeg.textComponent.color = Color.white;

                _view.InputArmorRightLeg.text = $"{_character.ArmorRightLeg}";
                _view.InputTotalRightLeg.text = $"{_character.RightLegTotal}";
            }

            if (_view.ToggleShelterLeftLeg.isOn)
            {
                _character.IsLeftLegSheltered = true;
                _view.InputArmorLeftLeg.textComponent.color = color;
                _view.InputTotalLeftLeg.textComponent.color = color;

                _view.InputArmorLeftLeg.text = $"{_character.ArmorLeftLeg + _character.ShelterArmorPoint}";
                _view.InputTotalLeftLeg.text = $"{_character.LeftLegTotal + _character.ShelterArmorPoint}";
            }
            else
            {
                _character.IsLeftLegSheltered = false;
                _view.InputArmorLeftLeg.textComponent.color = Color.white;
                _view.InputTotalLeftLeg.textComponent.color = Color.white;

                _view.InputArmorLeftLeg.text = $"{_character.ArmorLeftLeg}";
                _view.InputTotalLeftLeg.text = $"{_character.LeftLegTotal}";
            }
        }
        private void Exit() => Application.Quit();

        private void ScanQr()
        {
            _audioManager.PlayClick();
            ScanArmorQr?.Invoke();
        }
    }

}
