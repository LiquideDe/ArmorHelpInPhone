using UnityEngine;
using Zenject;

public class AudioManagerInstaller : MonoInstaller
{
    [SerializeField] AudioManager _audioManager;
    public override void InstallBindings()
    {
        Container.Bind<AudioManager>().FromInstance(_audioManager).AsSingle();
    }
}
