using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ArmorHelp
{
    public class ListCharacterPresenter
    {
        public event Action<SaveLoadGun> CreateGun;
        public event Action<string> FillArmor;
        public event Action Close;
        private AudioManager _audioManager;
        private ListCharacterView _view;
        private List<string> _nameCharacters = new List<string>();

        public ListCharacterPresenter(AudioManager audioManager, ListCharacterView characterView)
        {
            _audioManager = audioManager;
            _view = characterView;
            Subscribe();
            LoadFilesCharacters();
        }

        public void LoadCharacter(string path)
        {
            string allText = File.ReadAllText(path);
            allText = allText.Replace(Environment.NewLine, string.Empty);

            string[] notFormatData = allText.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> data = new List<string>();


            for (int i = 0; i < notFormatData.Length; i++)
                if (notFormatData[i].Length > 5)
                    data.Add($"{{{notFormatData[i]}}}");

            SaveLoadCharacter loadCharacter = JsonUtility.FromJson<SaveLoadCharacter>(data[0]);

            List<SaveLoadCharacteristic> characteristics = new List<SaveLoadCharacteristic>();
            int min = 1;
            int max = 10 + min;
            for (int i = min; i < max; i++)
            {
                SaveLoadCharacteristic characteristic = JsonUtility.FromJson<SaveLoadCharacteristic>(data[i]);
                characteristics.Add(characteristic);
            }

            min = max;
            max = min + loadCharacter.amountSkills;

            if (loadCharacter.amountImplants > 0)
            {

                min = max;
                max = min + loadCharacter.amountImplants;
            }

            List<JSONEquipmentReader> equipments = new List<JSONEquipmentReader>();

            for (int i = max; i < data.Count; i++)
            {
                Debug.Log($"data = {data[i]}");
                JSONTypeReader typeReader = JsonUtility.FromJson<JSONTypeReader>(data[i]);
                if (string.Compare(typeReader.typeEquipment,"Range", true)==0)
                {
                    JSONRangeReader rangeReader = JsonUtility.FromJson<JSONRangeReader>(data[i]);
                    equipments.Add(rangeReader);                    
                }
                else if (string.Compare(typeReader.typeEquipment, "Armor", true)==0)
                {
                    JSONArmorReader armorReader = JsonUtility.FromJson<JSONArmorReader>(data[i]);
                    equipments.Add(armorReader);
                }
                else if (string.Compare(typeReader.typeEquipment, "Shield", true) == 0)
                {
                    JSONArmorReader armorReader = JsonUtility.FromJson<JSONArmorReader>(data[i]);
                    equipments.Add(armorReader);
                }
            }

            int bonusWP = 0;
            int bonusToughness= 0;
            int bestArmorHead = 0;
            int bestArmorBody = 0;
            int bestArmorLeftHand = 0;
            int bestArmorRightHand = 0;
            int bestArmorLeftLeg = 0;
            int bestArmorRightLeg = 0;

            JSONArmorReader bestShield = new JSONArmorReader();
            JSONArmorReader bestArmor = new JSONArmorReader();
            List<SaveLoadGun> guns = new List<SaveLoadGun>();

            foreach (var item in equipments)
            {
                if(string.Compare(item.typeEquipment, "Shield", true) == 0)
                {
                    JSONArmorReader shield = (JSONArmorReader)item;
                    if(shield.body > bestShield.body)
                        bestShield = shield;
                }

                if(string.Compare(item.typeEquipment, "Armor", true) == 0)
                {
                    JSONArmorReader armor = (JSONArmorReader)item;
                    if(armor.head > bestArmorHead)
                        bestArmorHead = armor.head;
                    if(armor.body > bestArmorBody)
                        bestArmorBody = armor.body;
                    if(armor.hands > bestArmorLeftHand || armor.hands > bestArmorRightHand)
                    {
                        bestArmorLeftHand = armor.hands;
                        bestArmorRightHand= armor.hands;
                    }
                    if(armor.legs > bestArmorLeftLeg || armor.legs > bestArmorRightLeg)
                    {
                        bestArmorLeftLeg = armor.legs;
                        bestArmorRightLeg= armor.legs;
                    }
                }

                if(string.Compare(item.typeEquipment, "Range", true) == 0)
                {
                    JSONRangeReader range = (JSONRangeReader)item;
                    guns.Add(new SaveLoadGun());
                    guns[^1].name = range.name;
                    guns[^1].type = range.typeSound;
                    guns[^1].maxClip = range.clip;
                    guns[^1].totalClips = 2;
                    List<string> rofs = range.rof.Split(new char[] { '/' }).ToList();
                    if ((string.Compare(rofs[0],"-", true) == 0) == false)
                    {
                        guns[^1].singleFire = true;
                    }
                    int.TryParse(rofs[1], out int semiAuto);
                    int.TryParse(rofs[2], out int auto);
                    guns[^1].semiAutoFire = semiAuto;
                    guns[^1].autoFire = auto;
                }
            }

            foreach (var item in characteristics)
            {
                if(string.Compare(item.name, "Выносливость", true) == 0)                
                    bonusToughness = item.amount/10;                

                if(string.Compare(item.name, "Сила Воли", true) == 0)
                    bonusWP = item.amount/10;
            }

            string endArmor = $"A/{bonusWP}/{bestArmorHead + bestShield.head}/{bestArmorHead + bonusToughness + bestShield.head}/" +
                $"{bestArmorBody + bestShield.body}/{bestArmorBody + bestShield.body + bonusToughness}/" +
                $"{bestArmorRightHand}/{bestArmorRightHand + bonusToughness}/" +
                $"{bestArmorLeftHand + bestShield.hands}/{bestArmorLeftHand + bestShield.hands + bonusToughness}/" +
                $"{bestArmorRightLeg + bestShield.legs}/{bestArmorRightLeg + bestShield.legs + bonusToughness}/" +
                $"{bestArmorLeftLeg + bestShield.legs}/{bestArmorLeftLeg + bestShield.legs + bonusToughness}/{loadCharacter.wounds}";


            FillArmor?.Invoke(endArmor);

            foreach (var item in guns)
            {
                CreateGun?.Invoke(item);
            }

            Unscribe();
            _view.DestroyView();
            Close?.Invoke();
        }

        private void Subscribe()
        {
            _view.OpenThisPath += LoadCharacter;
            _view.Close += CloseList;
        }

        private void Unscribe()
        {
            _view.OpenThisPath -= LoadCharacter;
            _view.Close -= CloseList;
        }

        private void CloseList()
        {
            _audioManager.PlayCancel();
            Unscribe();
            _view.DestroyView();
            Close?.Invoke();
        }

        private void LoadFilesCharacters()
        {
            string[] loads = Directory.GetFiles($"{Application.dataPath}/StreamingAssets/CharacterSheets");
            _view.Initialize(loads);
        }        
    }
}

