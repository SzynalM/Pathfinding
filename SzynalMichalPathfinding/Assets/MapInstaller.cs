using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MapInstaller : MonoInstaller<MapInstaller>
{
    [SerializeField]
    private NodeFactory nodeFactory;
    
    public override void InstallBindings()
    {
        Container.Bind<IFactory<GameObject, Transform, Vector2, INode>>().FromInstance(nodeFactory.GetComponent<IFactory<GameObject, Transform, Vector2, INode>>());
        Container.Bind<MapGenerator>().AsSingle();
    }
}
