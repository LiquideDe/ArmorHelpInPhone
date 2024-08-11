using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Zenject;
using System.Collections.Generic;

public class NewGunPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField inputName, inputTotalClip, inputClipAmmo, inputSemiAutoFire, inputAutoFire;
    [SerializeField] Toggle toggleSingle;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] Button _buttonDone, _buttonClose;
    public event Action<SaveLoadGun> ReturnNewGun;
    public event Action ClosePanel;
    private AudioManager _audioManager;
    private Dictionary<TypeGun, string> _translateTypeGun = new Dictionary<TypeGun, string>() 
    {
        {TypeGun.Bolt, "���������" },
        {TypeGun.Bullet, "�������" },
        {TypeGun.Fire, "��������" },
        {TypeGun.Laser, "��������" },
        {TypeGun.Revolver, "���������" },
        {TypeGun.Shotgun, "��������" }
    };

    [Inject]
    private void Construct(AudioManager audioManager) => _audioManager = audioManager;

    private void OnEnable()
    {
        List<string> options = new List<string>();
        foreach(TypeGun typeGun in Enum.GetValues(typeof(TypeGun)))
        {
            options.Add(_translateTypeGun[typeGun]);
        }

        dropdown.AddOptions(options);
        _buttonClose.onClick.AddListener(Cancel);
        _buttonDone.onClick.AddListener(GunIsDone);
    }

    private void OnDisable()
    {
        dropdown.ClearOptions();
        _buttonClose.onClick.RemoveAllListeners();
        _buttonDone.onClick.RemoveAllListeners();
    }

    public void GunIsDone()
    {
        if (inputName.text.Length > 0 && inputClipAmmo.text.Length > 0 && inputTotalClip.text.Length > 0)
        {
            _audioManager.PlayDone();
            SaveLoadGun gun = new SaveLoadGun();
            gun.name = inputName.text;
            int.TryParse(inputTotalClip.text, out gun.totalClips);
            int.TryParse(inputClipAmmo.text, out gun.maxClip);
            int.TryParse(inputSemiAutoFire.text, out gun.semiAutoFire);
            int.TryParse(inputAutoFire.text, out gun.autoFire);
            gun.singleFire = toggleSingle.isOn;
            gun.type = dropdown.value;
            new LoadGuns().SaveGun(gun);
            ReturnNewGun?.Invoke(gun);
            Destroy(gameObject);
        }
        else
            _audioManager.PlayWarning();
    }

    public void Cancel()
    {
        _audioManager.PlayCancel();
        ClosePanel?.Invoke();
        Destroy(gameObject);
    }
}
