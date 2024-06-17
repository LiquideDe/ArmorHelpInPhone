using UnityEngine;

[CreateAssetMenu(fileName = "GunSounds", menuName = "Holder/GunSounds")]
public class GunSounds : ScriptableObject
{
    [SerializeField] AudioClip _fireClip, _reloadClip, _emptyClip;
    [SerializeField] TypeGun _typeGun;

    public AudioClip Shoot => _fireClip;
    public AudioClip Reload => _reloadClip;
    public AudioClip Empty => _emptyClip;

    public TypeGun TypeGun => _typeGun;
}
