using Cache;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Installer
{
    public class MonoInstallerBoot : MonoInstaller
    {
        [Header("Bind")] 
        public CacheAudio CacheAudio;
        public CacheItems CacheItems;
        public CacheGame CacheGame;

        public override void InstallBindings()
        {
            Debug.Log("Install Boot Bindings");
            
            BindAsSingleton(this);
            BindAsSingleton(CacheAudio);
            BindAsSingleton(CacheItems);
            BindAsSingleton(CacheGame);

            LoadGameScene();
        }

        private static void LoadGameScene()
        {
            Debug.Log("Load Game Scene");
            SceneManager.LoadScene(1);
        }

        private void BindAsSingleton<T>(T instance)
        {
            // AsSingle() регистрирует как синглтон
            // NonLazy() для указания, что объект должен быть создан сразу
            Container.Bind<T>().FromInstance(instance).AsSingle().NonLazy();
        }
    }
}