using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioClick;
    [SerializeField] AudioSource audioDone;
    [SerializeField] AudioSource audioCancel;
    [SerializeField] AudioSource audioWarning;
    [SerializeField] List<GunSounds> _gunSounds;

    public void PlayClick()
    {
        audioClick.Play();
    }

    public void PlayDone()
    {
        audioDone.Play();
    }

    public void PlayCancel()
    {
        audioCancel.Play();
    }

    public void PlayWarning()
    {
        audioWarning.Play();
    }

    public void PlayShoot(int id) => PlaySound(_gunSounds[id].Shoot);

    public void PlayReload(int id) => PlaySound(_gunSounds[id].Reload);

    public void PlayEmpty(int id) => PlaySound(_gunSounds[id].Empty);
    

    private void PlaySound(AudioClip clip)
    {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.clip = clip;
        audio.Play();
        Destroy(audio, 2f);
    }
}
