using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveLoadGun
{
    public string name;
    public int maxClip, semiAutoFire, autoFire, type, totalClips;
    public bool singleFire;
}
