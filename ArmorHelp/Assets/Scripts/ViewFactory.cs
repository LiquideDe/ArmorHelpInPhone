using UnityEngine;
using Zenject;

public class ViewFactory
{
    private DiContainer _diContainer;
    private PrefabHolder _prefabHolder;

    public ViewFactory(DiContainer diContainer, PrefabHolder prefabHolder)
    {
        _diContainer = diContainer;
        _prefabHolder = prefabHolder;
    }

    public GameObject Get(TypeScene typeScene)
    {
        return _diContainer.InstantiatePrefab(_prefabHolder.Get(typeScene)); ;
    }
}
