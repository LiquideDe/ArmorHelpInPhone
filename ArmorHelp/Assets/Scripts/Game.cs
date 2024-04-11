using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text;

public class Game : MonoBehaviour
{
    [SerializeField] TMP_InputField inputBonusWP;
    [SerializeField] TMP_InputField[] inputArmors;
    [SerializeField] TMP_InputField[] inputTotals;
    [SerializeField] TextMeshProUGUI textWound;
    [SerializeField] PanelDamage panelDamageExample;
    [SerializeField] PanelText panelText;
    [SerializeField] AudioManager audioManager;
    [SerializeField] Image[] backgrounds;
    [SerializeField] Sprite nonActive;
    [SerializeField] Sprite ActiveSmall;
    [SerializeField] Sprite ActiveBig;
    int wounds = 5;
    SaveLoadArmor armor;
    List<TMP_InputField> inputs = new List<TMP_InputField>();

    //0 - Голова
    //1 - Правая рука
    //2 - Левая рука
    //3 - Тело
    //4 - Правая нога
    //5 - Левая нога

    private void Start()
    {
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
                LoadArmor();
            }
        }
        else
        {
            var files = Directory.GetFiles($"{Application.dataPath}/StreamingAssets", "*.armor");
            foreach (string path in files)
            {
                string loadData = File.ReadAllText(path);
                armor = JsonUtility.FromJson<SaveLoadArmor>(loadData);
                LoadArmor();
            }
        }

        inputs.Add(inputBonusWP);
        for(int i = 0; i < inputArmors.Length; i++)
        {
            inputs.Add(inputArmors[i]);
            inputs.Add(inputTotals[i]);
        }
    }

    private void LoadArmor()
    {

        inputTotals[0].text = armor.head.ToString();
        inputTotals[1].text = armor.rightHand.ToString();
        inputTotals[2].text = armor.leftHand.ToString();
        inputTotals[3].text = armor.body.ToString();
        inputTotals[4].text = armor.rightLeg.ToString();
        inputTotals[5].text = armor.leftLeg.ToString();
                            
        inputArmors[0].text = armor.headArmor.ToString();
        inputArmors[1].text = armor.rightHandArmor.ToString();
        inputArmors[2].text = armor.leftHandArmor.ToString();
        inputArmors[3].text = armor.bodyArmor.ToString();
        inputArmors[4].text = armor.rightLegArmor.ToString();
        inputArmors[5].text = armor.leftLegArmor.ToString();
        inputBonusWP.text= armor.bWillPower.ToString();
        wounds = armor.wounds;
        textWound.text = $"{wounds}";
    }
    public void CalculateDamage()
    {
        audioManager.PlayClick();
        ParseInputs();
        var panelDamage = Instantiate(panelDamageExample, transform);
        panelDamage.SetParams(armor, TakeDamage, audioManager);

    }

    private void TakeDamage(int damage, string text)
    {
        wounds -= damage;
        textWound.text = $"{wounds}";
        var pText = Instantiate(panelText, transform);
        pText.SetParams(text, audioManager);
    }

    public void PlusWound()
    {
        audioManager.PlayClick();
        wounds += 1;
        textWound.text = $"{wounds}";
    }

    public void MinusWound()
    {
        audioManager.PlayClick();
        wounds -= 1;
        textWound.text = $"{wounds}";
    }
    public void Exit()
    {
        Application.Quit();
    }
    private void ParseInputs()
    {
        if (armor == null)
        {
            armor = new SaveLoadArmor();
        }
        int.TryParse(inputTotals[0].text, out armor.head);
        int.TryParse(inputTotals[1].text, out armor.rightHand);
        int.TryParse(inputTotals[2].text, out armor.leftHand);
        int.TryParse(inputTotals[3].text, out armor.body);
        int.TryParse(inputTotals[4].text, out armor.rightLeg);
        int.TryParse(inputTotals[5].text, out armor.leftLeg);

        int.TryParse(inputArmors[0].text, out armor.headArmor);
        int.TryParse(inputArmors[1].text, out armor.rightHandArmor);
        int.TryParse(inputArmors[2].text, out armor.leftHandArmor);
        int.TryParse(inputArmors[3].text, out armor.bodyArmor);
        int.TryParse(inputArmors[4].text, out armor.rightLegArmor);
        int.TryParse(inputArmors[5].text, out armor.leftLegArmor);

        int.TryParse(inputBonusWP.text, out armor.bWillPower);
        armor.wounds = wounds;

    }
    public void SaveArmor()
    {
        audioManager.PlayDone();
        ParseInputs();
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

    public void NextInput(int id)
    {
        //id++;
        audioManager.PlayDone();
        if(id < 12)
        {            
            if (id % 2 != 0)
            {
                backgrounds[id].sprite = ActiveBig;
            }
            else
            {
                backgrounds[id].sprite = ActiveSmall;
                if (id > 1)
                {
                    backgrounds[id - 1].sprite = nonActive;
                }
            }

            inputs[id+1].Select();
        }
        else
        {
            backgrounds[11].sprite = nonActive;
        }
        
    }
}
