using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text;

public class Game : MonoBehaviour
{
    [SerializeField] TMP_InputField inputHead, inputRightHand, inputLeftHand, inputBody, inputLeftLeg, inputRightLeg, inputBonusToughness, inputBonusWP;
    [SerializeField] TextMeshProUGUI textWound;
    [SerializeField] PanelDamage panelDamageExample;
    [SerializeField] PanelText panelText;
    int wounds = 5;
    SaveLoadArmor armor;

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
    }

    private void LoadArmor()
    {
        inputHead.text = armor.head.ToString();
        inputRightHand.text= armor.rightHand.ToString();
        inputLeftHand.text= armor.leftHand.ToString();
        inputBody.text= armor.body.ToString();
        inputRightLeg.text= armor.rightLeg.ToString();
        inputLeftLeg.text= armor.leftLeg.ToString();
        inputBonusToughness.text= armor.bToughness.ToString();
        inputBonusWP.text= armor.bWillPower.ToString();
        wounds = armor.wounds;
        textWound.text = $"{wounds}";
    }
    public void CalculateDamage()
    {
        //panelDamage.SetActive(true);    
        ParseInputs();
        var panelDamage = Instantiate(panelDamageExample, transform);
        panelDamage.SetParams(armor, TakeDamage);

    }

    private void TakeDamage(int damage, string text)
    {
        wounds -= damage;
        textWound.text = $"{wounds}";
        var pText = Instantiate(panelText, transform);
        pText.SetParams(text);
    }

    public void PlusWound()
    {
        wounds += 1;
        textWound.text = $"{wounds}";
    }

    public void MinusWound()
    {
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
        int.TryParse(inputHead.text, out armor.head);
        int.TryParse(inputRightHand.text, out armor.rightHand);
        int.TryParse(inputLeftHand.text, out armor.leftHand);
        int.TryParse(inputBody.text, out armor.body);
        int.TryParse(inputRightLeg.text, out armor.rightLeg);
        int.TryParse(inputLeftLeg.text, out armor.leftLeg);
        int.TryParse(inputBonusToughness.text, out armor.bToughness);
        int.TryParse(inputBonusWP.text, out armor.bWillPower);
        armor.wounds = wounds;
    }
    public void SaveArmor()
    {
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
}
