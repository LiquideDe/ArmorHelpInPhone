using System;

namespace ArmorHelp
{
    [Serializable]
    public class SaveLoadGunUsed
    {
        public string name;
        public int maxClip, semiAutoFire, autoFire, type, totalAmmo, clip;
        public bool singleFire;
    }
}


