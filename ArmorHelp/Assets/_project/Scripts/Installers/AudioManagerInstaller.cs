using UnityEngine;
using Zenject;

namespace ArmorHelp
{
    public class AudioManagerInstaller : MonoInstaller
    {
        [SerializeField] AudioManager _audioManager;
        public override void InstallBindings()
        {
            Container.Bind<AudioManager>().FromInstance(_audioManager).AsSingle();
        }
    }
}

