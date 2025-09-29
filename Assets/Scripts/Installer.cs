using System;
using UnityEngine;
using Zenject;

public class Installer : MonoInstaller
{
    private void Awake()
    {
        InstallBindings();
    }

    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<MainMenuManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        // Container.Bind<AudioController>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
