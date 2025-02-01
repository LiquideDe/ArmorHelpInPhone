using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmorHelp
{
    [CreateAssetMenu(fileName = "GunFactory", menuName = "Holder/GunFactory")]
    public class GunHolder : ScriptableObject
    {
        [SerializeField] private Gun _gunPrefab;

        public Gun Get(SaveLoadGun loadGun)
        {
            Gun gun = Instantiate(_gunPrefab);
            gun.Initialize(loadGun);
            return gun;
        }

        public Gun Get(SaveLoadGunUsed loadGunUsed)
        {
            Gun gun = Instantiate(_gunPrefab);
            gun.Initialize(loadGunUsed);
            return gun;
        }
    }
}


