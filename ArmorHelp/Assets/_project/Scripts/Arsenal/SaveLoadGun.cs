using System;

namespace ArmorHelp
{
    [Serializable]
    public class SaveLoadGun
    {
        public string name;
        public int maxClip, semiAutoFire, autoFire, type, totalClips;
        public bool singleFire;
    }
}


