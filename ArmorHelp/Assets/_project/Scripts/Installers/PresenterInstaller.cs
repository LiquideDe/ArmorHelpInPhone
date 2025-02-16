using UnityEngine;
using Zenject;

namespace ArmorHelp
{
    public class PresenterInstaller : MonoInstaller
    {
        [SerializeField] PrefabHolder _prefabHolder;
        [SerializeField] GunHolder _gunFactory;

        public override void InstallBindings()
        {
            Container.Bind<PrefabHolder>().FromInstance(_prefabHolder).AsSingle();
            Container.Bind<GunHolder>().FromInstance(_gunFactory).AsSingle();
            Container.Bind<PresenterFactory>().AsSingle();
            Container.Bind<ViewFactory>().AsSingle();
            Container.Bind<SceneMediator>().AsSingle();
        }
    }
}

