using Core;
using UnityEngine;
using Zenject;

namespace Installer
{
    public class MonoInstallerGame : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("Install Game Bindings");
        }
        
        private void BindAsSingleton<T>(T instance)
        {
            // AsSingle() регистрирует как синглтон
            // NonLazy() для указания, что объект должен быть создан сразу
            Container.Bind<T>().FromInstance(instance).AsSingle().NonLazy();
        }
    }
}