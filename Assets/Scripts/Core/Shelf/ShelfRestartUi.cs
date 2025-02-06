using System;
using Cache;
using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core
{
    public class ShelfRestartUi
    {
        private UiInvisible panelRestart;

        public event Action onRestart;
        
        [Header("DI")] 
        private CacheGame cacheGame;
        private CacheAudio cacheAudio;

        public ShelfRestartUi(UiInvisible panelRestart, Button buttonRestart, CacheGame cacheGame, CacheAudio cacheAudio)
        {
            this.panelRestart = panelRestart;
            this.cacheGame = cacheGame;
            this.cacheAudio = cacheAudio;
            
            buttonRestart.onClick.AddListener(RestartButton);
        }

        public async void RestartPanel()
        {
            await UniTask.Delay(cacheGame.ConfigGame.delayBeforeEnd * 1000);
            RestartPanel(false);
        }

        private void RestartButton()
        {
            cacheAudio.Play(BaseEnums.Sounds.Get, pitch: 0);
            
            RestartPanel(true);

            onRestart?.Invoke();
        }

        private void RestartPanel(bool state)
        {
            panelRestart?.SetInvisible(state).Forget();
        }
    }

}