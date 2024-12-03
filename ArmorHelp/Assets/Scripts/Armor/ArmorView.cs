using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class ArmorView : MonoBehaviour
{
    [SerializeField] TMP_InputField _inputBonusWP;
    [SerializeField] TMP_InputField _inputArmorHead, _inputArmorLeftHand, _inputArmorRightHand, _inputArmorBody, _inputArmorRightLeg, _inputArmorLeftLeg;
    [SerializeField] TMP_InputField _inputTotalHead, _inputTotalLeftHand, _inputTotalRightHand, _inputTotalBody, _inputTotalRightLeg, _inputTotalLeftLeg, _inputShelterPoint;
    [SerializeField] TextMeshProUGUI _textWound;
    [SerializeField] Image _backgroundHead, _backgroundBody, _backgroundRightHand, _backgroundLeftHand, _backgroundRightLeg, _backgroundLeftLeg;
    [SerializeField] Sprite _nonActive;
    [SerializeField] Sprite _activeSmall;
    [SerializeField] Sprite _activeBig;
    [SerializeField] Button _buttonOpenDamagePanel, _buttonExit, _buttonPlusWound, _buttonMinusWound, _buttonSave, _buttonArsenal, _buttonScanQrArmor;
    [SerializeField] Toggle _toggleShelterHead, _toggleShelterRightHand, _toggleShelterLeftHand, _toggleShelterBody, _toggleShelterRightLeg, _toggleShelterLeftLeg;

    public event Action GoToDamagePanel;
    public event Action Exit;
    public event Action PlusWound;
    public event Action MinusWound;
    public event Action SaveArmor;
    public event Action GoToArsenal;
    public event Action TakeCover;
    public event Action ParseInputs;
    public event Action ScanQr;
    
    private bool _isDeselectFromCode;

    public TMP_InputField InputBonusWP => _inputBonusWP;
    public TMP_InputField InputArmorHead => _inputArmorHead;
    public TMP_InputField InputArmorLeftHand => _inputArmorLeftHand;
    public TMP_InputField InputArmorRightHand => _inputArmorRightHand;
    public TMP_InputField InputArmorBody => _inputArmorBody;
    public TMP_InputField InputArmorRightLeg => _inputArmorRightLeg;
    public TMP_InputField InputArmorLeftLeg => _inputArmorLeftLeg;
    public TMP_InputField InputTotalHead => _inputTotalHead;
    public TMP_InputField InputTotalLeftHand => _inputTotalLeftHand;
    public TMP_InputField InputTotalRightHand => _inputTotalRightHand;
    public TMP_InputField InputTotalBody => _inputTotalBody;
    public TMP_InputField InputTotalRightLeg => _inputTotalRightLeg;
    public TMP_InputField InputTotalLeftLeg => _inputTotalLeftLeg;
    public TMP_InputField InputShelterPoint => _inputShelterPoint;
    public Toggle ToggleShelterHead => _toggleShelterHead;
    public Toggle ToggleShelterRightHand => _toggleShelterRightHand; 
    public Toggle ToggleShelterLeftHand => _toggleShelterLeftHand; 
    public Toggle ToggleShelterBody => _toggleShelterBody;
    public Toggle ToggleShelterRightLeg => _toggleShelterRightLeg;
    public Toggle ToggleShelterLeftLeg => _toggleShelterLeftLeg;

    private void Start()
    {
        _buttonArsenal.onClick.AddListener(ArsenalPressed);
        _buttonOpenDamagePanel.onClick.AddListener(OpenDamagePanelPressed);
        _buttonExit.onClick.AddListener(ExitPressed);
        _buttonPlusWound.onClick.AddListener(PlusWoundPressed);
        _buttonMinusWound.onClick.AddListener(MinusWoundPressed);
        _buttonSave.onClick.AddListener(SaveArmorPressed);
        _buttonScanQrArmor.onClick.AddListener(ScanQrArmorPressed);

        _inputBonusWP.onSubmit.AddListener(SetWillPower);
        _inputBonusWP.onDeselect.AddListener(CancelSelect);
        _inputBonusWP.onDeselect.AddListener(ParseInputsPressed);


        _inputArmorHead.onSelect.AddListener(ArmorHeadSelect);
        _inputArmorHead.onSubmit.AddListener(SetArmorHead);        
        _inputArmorHead.onDeselect.AddListener(CancelSelect);
        _inputArmorHead.onDeselect.AddListener(ReturnNormalImageHead);
        _inputArmorHead.onDeselect.AddListener(ParseInputsPressed);


        _inputArmorLeftHand.onSelect.AddListener(ArmorLeftHandSelect);
        _inputArmorLeftHand.onSubmit.AddListener(SetArmorLeftHand);
        _inputArmorLeftHand.onDeselect.AddListener(CancelSelect);
        _inputArmorLeftHand.onDeselect.AddListener(ReturnNormalImageLeftHand);
        _inputArmorLeftHand.onDeselect.AddListener(ParseInputsPressed);


        _inputArmorRightHand.onSelect.AddListener(ArmorRightHandSelect);
        _inputArmorRightHand.onSubmit.AddListener(SetArmorRightHand);
        _inputArmorRightHand.onDeselect.AddListener(CancelSelect);
        _inputArmorRightHand.onDeselect.AddListener(ReturnNormalImageRightHand);
        _inputArmorRightHand.onDeselect.AddListener(ParseInputsPressed);

        _inputArmorBody.onSelect.AddListener(ArmorBodySelect);
        _inputArmorBody.onSubmit.AddListener(SetArmorBody);
        _inputArmorBody.onDeselect.AddListener(CancelSelect);
        _inputArmorBody.onDeselect.AddListener(ReturnNormalImageBody);
        _inputArmorBody.onDeselect.AddListener(ParseInputsPressed);

        _inputArmorRightLeg.onSelect.AddListener(ArmorRightLegSelect);
        _inputArmorRightLeg.onSubmit.AddListener(SetArmorRightLeg);
        _inputArmorRightLeg.onDeselect.AddListener(CancelSelect);
        _inputArmorRightLeg.onDeselect.AddListener(ReturnNormalImageRightLeg);
        _inputArmorRightLeg.onDeselect.AddListener(ParseInputsPressed);

        _inputArmorLeftLeg.onSelect.AddListener(ArmorLeftLegSelect);
        _inputArmorLeftLeg.onSubmit.AddListener(SetArmorLeftLeg);
        _inputArmorLeftLeg.onDeselect.AddListener(CancelSelect);
        _inputArmorLeftLeg.onDeselect.AddListener(ReturnNormalImageLeftLeg);
        _inputArmorLeftLeg.onDeselect.AddListener(ParseInputsPressed);


        _inputTotalHead.onSelect.AddListener(HeadSelect);
        _inputTotalHead.onSubmit.AddListener(SetHeadTotal);
        _inputTotalHead.onDeselect.AddListener(CancelSelect);
        _inputTotalHead.onDeselect.AddListener(ReturnNormalImageHead);
        _inputTotalHead.onDeselect.AddListener(ParseInputsPressed);

        _inputTotalRightHand.onSelect.AddListener(RightHandSelect);
        _inputTotalRightHand.onSubmit.AddListener(SetRightHandTotal);
        _inputTotalRightHand.onDeselect.AddListener(CancelSelect);
        _inputTotalRightHand.onDeselect.AddListener(ReturnNormalImageRightHand);
        _inputTotalRightHand.onDeselect.AddListener(ParseInputsPressed);

        _inputTotalLeftHand.onSelect.AddListener(LeftHandSelect);
        _inputTotalLeftHand.onSubmit.AddListener(SetLeftHandTotal);
        _inputTotalLeftHand.onDeselect.AddListener(CancelSelect);
        _inputTotalLeftHand.onDeselect.AddListener(ReturnNormalImageLeftHand);
        _inputTotalLeftHand.onDeselect.AddListener(ParseInputsPressed);


        _inputTotalBody.onSelect.AddListener(BodySelect);
        _inputTotalBody.onSubmit.AddListener(SetBodyTotal);
        _inputTotalBody.onDeselect.AddListener(CancelSelect);
        _inputTotalBody.onDeselect.AddListener(ReturnNormalImageBody);
        _inputTotalBody.onDeselect.AddListener(ParseInputsPressed);


        _inputTotalRightLeg.onSelect.AddListener(RightLegSelect);
        _inputTotalRightLeg.onSubmit.AddListener(SetRightLegTotal);
        _inputTotalRightLeg.onDeselect.AddListener(CancelSelect);
        _inputTotalRightLeg.onDeselect.AddListener(ReturnNormalImageRightLeg);
        _inputTotalRightLeg.onDeselect.AddListener(ParseInputsPressed);

        _inputTotalLeftLeg.onSelect.AddListener(LeftLegSelect);
        _inputTotalLeftLeg.onSubmit.AddListener(SetLeftLegTotal);
        _inputTotalLeftLeg.onDeselect.AddListener(CancelSelect);
        _inputTotalLeftLeg.onDeselect.AddListener(ReturnNormalImageLeftLeg);
        _inputTotalLeftLeg.onDeselect.AddListener(ParseInputsPressed);

        _inputShelterPoint.onDeselect.AddListener(ParseInputsPressed);
        _inputShelterPoint.onDeselect.AddListener(UpdateShelters);

        _toggleShelterBody.onValueChanged.AddListener(TakeCoverPressed);
        _toggleShelterHead.onValueChanged.AddListener(TakeCoverPressed);
        _toggleShelterLeftHand.onValueChanged.AddListener(TakeCoverPressed);
        _toggleShelterLeftLeg.onValueChanged.AddListener(TakeCoverPressed);
        _toggleShelterRightHand.onValueChanged.AddListener(TakeCoverPressed);
        _toggleShelterRightLeg.onValueChanged.AddListener(TakeCoverPressed);
    }    

    public void LoadArmor(SaveLoadArmor armor)
    {
        _inputTotalHead.text = armor.head.ToString();
        _inputTotalRightHand.text = armor.rightHand.ToString();
        _inputTotalLeftHand.text = armor.leftHand.ToString();
        _inputTotalBody.text = armor.body.ToString();
        _inputTotalRightLeg.text = armor.rightLeg.ToString();
        _inputTotalLeftLeg.text = armor.leftLeg.ToString();

        _inputArmorHead.text = armor.headArmor.ToString();
        _inputArmorRightHand.text = armor.rightHandArmor.ToString();
        _inputArmorLeftHand.text = armor.leftHandArmor.ToString();
        _inputArmorBody.text = armor.bodyArmor.ToString();
        _inputArmorRightLeg.text = armor.rightLegArmor.ToString();
        _inputArmorLeftLeg.text = armor.leftLegArmor.ToString();
        _inputBonusWP.text = armor.bWillPower.ToString();
        _textWound.text = $"{armor.wounds}";
    }

    public void SetWound(int wound) => _textWound.text = $"{wound}";

    private void MinusWoundPressed() => MinusWound?.Invoke();
    private void PlusWoundPressed() => PlusWound?.Invoke();
    private void OpenDamagePanelPressed() => GoToDamagePanel?.Invoke();
    private void SaveArmorPressed() => SaveArmor?.Invoke();
    private void ExitPressed() => Exit?.Invoke();
    private void ArsenalPressed() => GoToArsenal?.Invoke();

    private void CancelSelect(string value)
    {
        var eventSystem = EventSystem.current;
        if (!eventSystem.alreadySelecting) eventSystem.SetSelectedGameObject(null);
    }

    private void SetWillPower(string value)
    {
        _isDeselectFromCode = true;
        _inputArmorHead.Select();
    }

    private void SetArmorHead(string value)
    {
        _isDeselectFromCode = true;
        _inputTotalHead.Select();
    }

    private void SetHeadTotal(string value)
    {
        _backgroundHead.sprite = _nonActive;
        _isDeselectFromCode = true;
        _inputArmorRightHand.Select();
    }

    private void SetArmorRightHand(string value)
    {
        _isDeselectFromCode = true;
        _inputTotalRightHand.Select();
    }

    private void SetRightHandTotal(string value)
    {
        _backgroundRightHand.sprite = _nonActive;
        _isDeselectFromCode = true;
        _inputArmorLeftHand.Select();
    }

    private void SetArmorLeftHand(string value)
    {
        _isDeselectFromCode = true;
        _inputTotalLeftHand.Select();
    }

    private void SetLeftHandTotal(string value)
    {
        _backgroundLeftHand.sprite = _nonActive;
        _isDeselectFromCode = true;
        _inputArmorBody.Select();
    }

    private void SetArmorBody(string value)
    {
        _isDeselectFromCode = true;
        _inputTotalBody.Select();
    }

    private void SetBodyTotal(string value)
    {
        _backgroundBody.sprite = _nonActive;
        _isDeselectFromCode = true;
        _inputArmorRightLeg.Select();
    }

    private void SetArmorRightLeg(string value)
    {
        _isDeselectFromCode = true;
        _inputTotalRightLeg.Select();
    }

    private void SetRightLegTotal(string value)
    {
        _backgroundRightLeg.sprite = _nonActive;
        _isDeselectFromCode = true;
        _inputArmorLeftLeg.Select();
    }

    private void SetArmorLeftLeg(string value)
    {
        _isDeselectFromCode = true;
        _inputTotalLeftLeg.Select();
    }

    private void SetLeftLegTotal(string value)
    {
        _backgroundLeftLeg.sprite = _nonActive;
        CancelSelect("");
    }

    private void ReturnNormalImageHead(string value)
    {
        if (!_isDeselectFromCode)
            _backgroundHead.sprite = _nonActive;

        _isDeselectFromCode = false;
    }

    private void ReturnNormalImageRightHand(string value)
    {
        if (!_isDeselectFromCode)
            _backgroundRightHand.sprite = _nonActive;

        _isDeselectFromCode = false;
    }

    private void ReturnNormalImageLeftHand(string value)
    {
        if (!_isDeselectFromCode)
            _backgroundLeftHand.sprite = _nonActive;

        _isDeselectFromCode = false;
    }

    private void ReturnNormalImageBody(string value)
    {
        if (!_isDeselectFromCode)
            _backgroundBody.sprite = _nonActive;

        _isDeselectFromCode = false;
    }

    private void ReturnNormalImageLeftLeg(string value)
    {
        if (!_isDeselectFromCode)
            _backgroundLeftLeg.sprite = _nonActive;

        _isDeselectFromCode = false;
    }

    private void ReturnNormalImageRightLeg(string value)
    {
        if (!_isDeselectFromCode)
            _backgroundRightLeg.sprite = _nonActive;

        _isDeselectFromCode = false;
    }

    private void ArmorHeadSelect(string value) => _backgroundHead.sprite = _activeSmall;
    private void ArmorRightHandSelect(string value) => _backgroundRightHand.sprite = _activeSmall;
    private void ArmorLeftHandSelect(string value) => _backgroundLeftHand.sprite = _activeSmall;
    private void ArmorBodySelect(string value) => _backgroundBody.sprite = _activeSmall;
    private void ArmorRightLegSelect(string value) => _backgroundRightLeg.sprite = _activeSmall;
    private void ArmorLeftLegSelect(string value) => _backgroundLeftLeg.sprite = _activeSmall;

    private void HeadSelect(string value) => _backgroundHead.sprite = _activeBig;
    private void RightHandSelect(string value) => _backgroundRightHand.sprite = _activeBig;
    private void LeftHandSelect(string value) => _backgroundLeftHand.sprite = _activeBig;
    private void BodySelect(string value) => _backgroundBody.sprite = _activeBig;
    private void RightLegSelect(string value) => _backgroundRightLeg.sprite = _activeBig;
    private void LeftLegSelect(string value) => _backgroundLeftLeg.sprite = _activeBig;

    private void TakeCoverPressed(bool isCover) => TakeCover?.Invoke();

    private void UpdateShelters(string value) => TakeCover?.Invoke();

    private void ParseInputsPressed(string value) => ParseInputs?.Invoke();

    private void ScanQrArmorPressed() => ScanQr?.Invoke();
}
