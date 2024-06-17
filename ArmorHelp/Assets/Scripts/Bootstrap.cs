using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    private SceneMediator _sceneMediator;

    private void Start()
    {
        _sceneMediator.Initialize();
    }

    [Inject]
    private void Construct(SceneMediator sceneMediator)
    {
        _sceneMediator = sceneMediator;
    }


}
