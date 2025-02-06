using System;
using System.Collections.Generic;
using System.Linq;
using Cache;
using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Core
{
    public class ShelfValidator
    {
        private ShelfInstances shelfInstances;
        private UiInvisible panelRestart;
        public event Action onFinishValidation;
        
        [Header("DI")] 
        private CacheAudio cacheAudio;

        public ShelfValidator(ShelfInstances shelfInstances, CacheAudio cacheAudio)
        {
            this.shelfInstances = shelfInstances;
            this.cacheAudio = cacheAudio;
        }

        public void AddTriggersValidation()
        {
            foreach (var shelfTrigger in shelfInstances.GetShelfTriggers())
            {
                shelfTrigger.OnStatusChanged += ValidateShelves;
            }
        }

        private void ValidateShelves()
        {
            if (AllShelvesAreReady())
            {
                onFinishValidation?.Invoke();
                cacheAudio.Play(BaseEnums.Sounds.Final, pitch: 0);
            }
        }

        private bool AllShelvesAreReady()
        {
            var shelfTriggers = shelfInstances.GetShelfTriggers();
            
            if (shelfTriggers.Count == 0) return false;
            
            return shelfTriggers.All(trigger => trigger.CheckValidity());
        }
    }

}