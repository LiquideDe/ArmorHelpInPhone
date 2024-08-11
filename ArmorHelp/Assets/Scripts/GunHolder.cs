using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GunFactory", menuName = "Holder/GunFactory")]
public class GunHolder : ScriptableObject
{
    [SerializeField] private List<GunSounds> gunSounds;
    [SerializeField] private Gun _gunPrefab;
    /*
     * 0 - револьвер
     * 1 - bullet
     * 2 - shotgun
     * 3 - laser
     * 4 - bolt
     * 5 - fire
    */
    private Dictionary<int, TypeGun> _idToType = new Dictionary<int, TypeGun>()
    {
        {0, TypeGun.Revolver },
        {1, TypeGun.Bullet },
        {2, TypeGun.Shotgun },
        {3, TypeGun.Laser },
        {4, TypeGun.Bolt },
        {5, TypeGun.Fire }
    };

    public Gun Get(SaveLoadGun loadGun)
    {
        Gun gun = Instantiate(_gunPrefab);
        Debug.Log($"номер {loadGun.type} имя {_idToType[loadGun.type]}");
        gun.Initialize(loadGun, GetSound(_idToType[loadGun.type]));
        return gun;
    }

    public Gun Get(SaveLoadGunUsed loadGunUsed)
    {
        Gun gun = Instantiate(_gunPrefab);
        gun.Initialize(loadGunUsed, GetSound(_idToType[loadGunUsed.type]));
        return gun;
    }

    private GunSounds GetSound(TypeGun typeGun)
    {
        foreach(GunSounds gunSound in gunSounds)
        {
            if (gunSound.TypeGun == typeGun)
            {
                Debug.Log($"Нашли type {typeGun}");
                return gunSound;
            }
                
        }

        throw new System.Exception($"Не найден звук для {typeGun}");
    }
}
