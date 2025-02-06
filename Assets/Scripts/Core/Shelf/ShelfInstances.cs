using System.Collections.Generic;
using Base.Models;
using Cache;
using UnityEngine;
using Zenject;

namespace Core
{
    public class ShelfInstances
    {
        private List<ShelfTrigger> shelfTriggers;
        private List<ShelfItem> shelfItems;
        private Transform dragContainer;
        
        [Header("DI")]
        private CacheGame cacheGame;

        public ShelfInstances(CacheGame cacheGame, Transform dragContainer)
        {
            shelfTriggers = new List<ShelfTrigger>();
            shelfItems = new List<ShelfItem>();
            
            this.cacheGame = cacheGame;
            this.dragContainer = dragContainer;
        }

        public void BootShelves(List<BaseModels.ShelfElement> shelfSetup)
        {
            ClearShelves();

            foreach (var shelf in shelfSetup)
            {
                CreateShelfTriggers(shelf);
            }
        }

        private void CreateShelfTriggers(BaseModels.ShelfElement shelfSetup)
        {
            for (int i = 0; i < cacheGame.ConfigGame.itemsInShelf; i++)
            {
                var shelfInstance = Object.Instantiate(cacheGame.ConfigGame.ShelfTriggerPrefab, shelfSetup.transformShelf);

                if (shelfInstance.TryGetComponent(out ShelfTrigger shelfTrigger))
                {
                    shelfTriggers.Add(shelfTrigger);
                
                    var shelfItem = shelfTrigger.Init(shelfSetup.type, dragContainer);
                    
                    shelfItems.Add(shelfItem);
                }
            }
        }

        private void ClearShelves()
        {
            foreach (var trigger in shelfTriggers)
            {
                Object.Destroy(trigger.gameObject);
            }
            shelfTriggers.Clear();
            shelfItems.Clear();
        }

        public List<ShelfTrigger> GetShelfTriggers() => shelfTriggers;
        public List<ShelfItem> GetShelfItems() => shelfItems;
    }

}