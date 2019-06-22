using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIInputManager : MonoBehaviour
{
    private SignalBus signalBus;

    public void GenerateNewMap()
    {

    }

    public void SaveMap()
    {

    }

    public void LoadMap()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

    }
}