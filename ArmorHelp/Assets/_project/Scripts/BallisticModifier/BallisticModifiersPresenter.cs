using System;
using System.Collections.Generic;
using Zenject;

namespace ArmorHelp
{
    public class BallisticModifiersPresenter : IPresenter
    {
        public event Action CloseBallisticModifier;

        private List<string> _lightings = new List<string>() { "Светло", "Слабый свет/дождь", "Дым/Туман/Ливень", "Тьма" };
        private List<int> _lightingsModifiers = new List<int>() { 0, -10, -20, -30 };
        private int _idLightings = 0;

        private List<string> _distance = new List<string>() { "Рукопашная", "В упор", "Близкая дистанция", "Боевая дистанция", "Дальняя дистанция", "Сверхдальняя дистанция" };
        private List<int> _distanceModifiers = new List<int>() { -20, 30, 10, 0, -10, -30 };
        private int _idDistance = 3;

        private List<string> _shoot = new List<string>() { "Одиночный", "Короткая очередь", "Длинная очередь" };
        private List<int> _shootModifiers = new List<int>() { 10, 0, -10 };
        private int _idShoot = 0;

        private List<string> _aiming = new List<string>() { "От бедра", "Быстрое прицеливание", "Полное прицеливание" };
        private List<int> _aimingModifiers = new List<int>() { 0, 10, 20 };
        private int _idAiming = 0;

        private List<string> _target = new List<string>() { "В норме", "Лежит", "Бежит", "Оглушена", "Врасплох" };
        private List<int> _targetModifiers = new List<int>() { 0, -20, -20, 20, 30 };
        private int _idTarget = 0;

        private List<string> _weaponKit = new List<string>() { "Нет", "Лазерный прицел", "Прицел ночного видения", "Тепловизор", "Оптический", "Омни прицел" };
        private int _idWeaponKit = 0;

        private List<string> _handle = new List<string>() { "Нет", "На заказ" };
        private List<int> _handleModifiers = new List<int>() { 0, 5 };
        private int _idHandle = 0;

        private List<string> _navigateSystem = new List<string>() { "Нет", "Есть" };
        private int _idNavigate = 0;

        private List<string> _size = new List<string>() { "Крошечный(1)", "Маленький(2)", "Небольшой(3)", "Средний(4)", "Громадный(5)", "Огромный(6)", "Массивный(7)" };
        private List<int> _sizeModifiers = new List<int>() { -30, -20, -10, 0, 10, 20, 30 };
        private int _idSize = 3;

        private BallisticModifiersView _view;
        private AudioManager _audioManager;

        [Inject]
        private void Construct(AudioManager audioManager) => _audioManager = audioManager;

        public void Initialize(BallisticModifiersView view)
        {
            _view = view;
            Subscribe();
            CalculateResult();
            _view.SetLightText(_lightings[_idLightings]);
            _view.SetDistanceText(_distance[_idDistance]);
            _view.SetShootText(_shoot[_idShoot]);
            _view.SetAimingText(_aiming[_idAiming]);
            _view.SetTargetText(_target[_idTarget]);
            _view.SetWeaponKitText(_weaponKit[_idWeaponKit]);
            _view.SetHandleText(_handle[_idHandle]);
            _view.SetNavigateText(_navigateSystem[_idNavigate]);
            _view.SetSizeText(_size[_idSize]);
        }

        public void ShowView() => _view.gameObject.SetActive(true);

        private void Subscribe()
        {
            _view.NextAiming += NextAimingDown;
            _view.NextDistance += NextDistanceDown;
            _view.NextHandle += NextHandleDown;
            _view.NextLight += NextLightDown;
            _view.NextNavigate += NextNavigateDown;
            _view.NextShoot += NextShootDown;
            _view.NextTarget += NextTargetDown;
            _view.NextWeaponKit += NextWeaponKitDown;
            _view.NextSize += NextSizeDown;

            _view.PrevAiming += PrevAimingDown;
            _view.PrevDistance += PrevDistanceDown;
            _view.PrevHandle += PrevHandleDown;
            _view.PrevLight += PrevLightDown;
            _view.PrevNavigate += PrevNavigateDown;
            _view.PrevShoot += PrevShootDown;
            _view.PrevTarget += PrevTargetDown;
            _view.PrevWeaponKit += PrevWeaponKitDown;
            _view.PrevSize += PrevSizeDown;

            _view.Exit += ExitDown;
        }

        private void PrevLightDown()
        {
            if (TryDecreaseId(ref _idLightings))
                _view.SetLightText(_lightings[_idLightings]);
        }
        private void PrevDistanceDown()
        {
            if (TryDecreaseId(ref _idDistance))
                _view.SetDistanceText(_distance[_idDistance]);
        }
        private void PrevShootDown()
        {
            if (TryDecreaseId(ref _idShoot))
                _view.SetShootText(_shoot[_idShoot]);
        }
        private void PrevAimingDown()
        {
            if (TryDecreaseId(ref _idAiming))
                _view.SetAimingText(_aiming[_idAiming]);
        }
        private void PrevTargetDown()
        {
            if (TryDecreaseId(ref _idTarget))
                _view.SetTargetText(_target[_idTarget]);
        }
        private void PrevWeaponKitDown()
        {
            if (TryDecreaseId(ref _idWeaponKit))
                _view.SetWeaponKitText(_weaponKit[_idWeaponKit]);
        }
        private void PrevHandleDown()
        {
            if (TryDecreaseId(ref _idHandle))
                _view.SetHandleText(_handle[_idHandle]);
        }
        private void PrevNavigateDown()
        {
            if (TryDecreaseId(ref _idNavigate))
                _view.SetNavigateText(_navigateSystem[_idNavigate]);
        }
        private void PrevSizeDown()
        {
            if (TryDecreaseId(ref _idSize))
                _view.SetSizeText(_size[_idSize]);
        }

        private void NextLightDown()
        {
            if (TryIncreaseId(ref _idLightings, _lightings.Count))
                _view.SetLightText(_lightings[_idLightings]);
        }
        private void NextDistanceDown()
        {
            if (TryIncreaseId(ref _idDistance, _distance.Count))
                _view.SetDistanceText(_distance[_idDistance]);
        }
        private void NextShootDown()
        {
            if (TryIncreaseId(ref _idShoot, _shoot.Count))
                _view.SetShootText(_shoot[_idShoot]);
        }
        private void NextAimingDown()
        {
            if (TryIncreaseId(ref _idAiming, _aiming.Count))
                _view.SetAimingText(_aiming[_idAiming]);
        }
        private void NextTargetDown()
        {
            if (TryIncreaseId(ref _idTarget, _target.Count))
                _view.SetTargetText(_target[_idTarget]);
        }
        private void NextWeaponKitDown()
        {
            if (TryIncreaseId(ref _idWeaponKit, _weaponKit.Count))
                _view.SetWeaponKitText(_weaponKit[_idWeaponKit]);
        }
        private void NextHandleDown()
        {
            if (TryIncreaseId(ref _idHandle, _handle.Count))
                _view.SetHandleText(_handle[_idHandle]);
        }
        private void NextNavigateDown()
        {
            if (TryIncreaseId(ref _idNavigate, _navigateSystem.Count))
                _view.SetNavigateText(_navigateSystem[_idNavigate]);
        }
        private void NextSizeDown()
        {
            if (TryIncreaseId(ref _idSize, _size.Count))
                _view.SetSizeText(_size[_idSize]);
        }

        private bool TryDecreaseId(ref int id)
        {
            if (id - 1 >= 0)
            {
                id--;
                _audioManager.PlayClick();
                CalculateResult();
                return true;
            }
            _audioManager.PlayWarning();
            return false;
        }

        private bool TryIncreaseId(ref int id, int maxValue)
        {
            if (id + 1 < maxValue)
            {
                id++;
                _audioManager.PlayClick();
                CalculateResult();
                return true;
            }

            _audioManager.PlayWarning();
            return false;
        }

        private void CalculateResult()
        {
            int totalModifier = 0;

            totalModifier += _handleModifiers[_idHandle];
            totalModifier += _targetModifiers[_idTarget];
            totalModifier += _aimingModifiers[_idAiming];
            totalModifier += _sizeModifiers[_idSize];

            //При слабом освещении помогают Прицел ночного, тепловизор или омниприцел
            if (_idLightings == 1 || _idLightings == 2 || _idLightings == 3)
                if (_idWeaponKit != 2 && _idWeaponKit != 3 && _idWeaponKit != 5)
                    totalModifier += _lightingsModifiers[_idLightings];

            if ((_idDistance == 4 || _idDistance == 5) && _idWeaponKit != 4 && _idWeaponKit != 5)
                totalModifier += _distanceModifiers[_idDistance];
            else
                totalModifier += _distanceModifiers[_idDistance];

            if ((_idShoot == 1 || _idShoot == 2) && _idNavigate == 1)
                totalModifier += _shootModifiers[_idShoot] + 10;
            else if (_idShoot == 0 && (_idWeaponKit == 1 || _idWeaponKit == 5))
                totalModifier += _shootModifiers[_idShoot] + 10;
            else
                totalModifier += _shootModifiers[_idShoot];

            if (totalModifier > 0)
                _view.SetTotalModifierText($"+{totalModifier}");
            else
                _view.SetTotalModifierText($"{totalModifier}");
        }

        private void ExitDown()
        {
            _audioManager.PlayDone();
            _view.gameObject.SetActive(false);
            CloseBallisticModifier?.Invoke();
        }
    }
}

