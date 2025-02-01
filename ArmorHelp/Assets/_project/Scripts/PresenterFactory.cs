using Zenject;

namespace ArmorHelp
{
    public class PresenterFactory
    {
        DiContainer _diContainer;

        public PresenterFactory(DiContainer diContainer) => _diContainer = diContainer;


        public IPresenter Get(TypeScene typeScene)
        {
            switch (typeScene)
            {
                case TypeScene.Armor:
                    return _diContainer.Instantiate<ArmorPresenter>();

                case TypeScene.DamageParametersPanel:
                    return _diContainer.Instantiate<PanelDamageParametersPresenter>();

                case TypeScene.Arsenal:
                    return _diContainer.Instantiate<ArsenalPresenter>();

                case TypeScene.ListWithGuns:
                    return _diContainer.Instantiate<ListWithNewGunsPresenter>();

                case TypeScene.QrScanner:
                    return _diContainer.Instantiate<QRScannerPresenter>();

                case TypeScene.BallisticModifier:
                    return _diContainer.Instantiate<BallisticModifiersPresenter>();

                case TypeScene.WeaponModifier:
                    return _diContainer.Instantiate<WeaponModifierPresenter>();

                default:
                    throw new System.Exception($"Нет такой сцены {typeScene}");
            }
        }

    }
}


