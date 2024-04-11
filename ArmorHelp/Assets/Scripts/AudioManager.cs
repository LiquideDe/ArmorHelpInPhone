using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioClick;
    [SerializeField] AudioSource audioDone;
    [SerializeField] AudioSource audioCancel;
    [SerializeField] AudioSource audioWarning;

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
}
