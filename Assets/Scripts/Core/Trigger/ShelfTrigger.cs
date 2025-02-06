using System;
using Cache;
using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Core
{
    public class ShelfTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        [Header("Status")]
        public ShelfItem currentShelfItem;
        public event Action OnStatusChanged;
        
        [Header("Setup")]
        public BaseEnums.ShelfType shelfType;

        [Header("Cache")]
        private Transform _transform;
        private Transform _shelfItemContainer;
        
        [Header("DI")]
        [Inject] private CacheAudio _cacheAudio;
        [Inject] private CacheItems _cacheItems;

        private void Awake()
        {
            _transform = transform;
        }

        public ShelfItem Init(BaseEnums.ShelfType shelfType, Transform shelfItemContainer)
        {
            this.shelfType = shelfType;
            
            _shelfItemContainer = shelfItemContainer;

            name = $"{shelfType}_shelf";
            
            if (currentShelfItem)
            {
                Debug.Log("Destroy last shelf item with type " + shelfType);
                Destroy(currentShelfItem.gameObject);
                currentShelfItem = null;
            }
            
            if (_cacheItems)
            {
                var shelfItem = _cacheItems.CreateItem(shelfType);

                if (shelfItem.TryGetComponent(out ShelfItem item))
                {
                    currentShelfItem = item;
                    BindSlotAsChild(currentShelfItem);
                
                    return item;
                }
            }
            else
            {
                Debug.LogError("Failed to find Cache Items in the project context");
            }

            return null;
        }

        public async UniTask AttachItem(ShelfItem shelfItem)
        {
            if (shelfItem == null)
            {
                Debug.LogError("Failed to find item to attach");
                return;
            }
            
            AttachToDragContainer(shelfItem);

            await MoveSlotToTrigger(shelfItem);
            
            currentShelfItem = shelfItem;

            BindSlotAsChild(shelfItem);

            shelfItem.BlocksRaycasts(currentShelfItem.type != shelfType);
            
            OnStatusChanged?.Invoke();
        }
        
        public void DetachItem()
        {
            if (currentShelfItem)
            {
                AttachToDragContainer(currentShelfItem);
                currentShelfItem.BlocksRaycasts(false);
                currentShelfItem = null;
                
                _cacheAudio.Play(BaseEnums.Sounds.Get);
            }
        }

        public void AttachToDragContainer(ShelfItem shelfItem)
        {
            shelfItem?.transform.SetParent(_shelfItemContainer);
        }
        
        private void BindSlotAsChild(ShelfItem shelfItem)
        {
            if (shelfItem)
            {
                shelfItem.transform.SetParent(transform);
                
                shelfItem.transform.position = new Vector3(0, 0, 0);
                shelfItem.transform.localPosition = new Vector3(0, 0, 0);
                shelfItem.transform.localScale = new Vector3(1, 1, 1);
            
                shelfItem.SetConfirmedTrigger(this);
                shelfItem.SetPendingTrigger(null);
            }
        }
        
        private async UniTask MoveSlotToTrigger(ShelfItem slot)
        {
            await slot.Move(_transform);
        }
        
        public bool CheckValidity()
        {
            return currentShelfItem.type == shelfType && currentShelfItem.blended;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            
            var item = eventData.pointerDrag.GetComponent<ShelfItem>();
            
            if (item && item.type == shelfType)
            {           
                item.SetPendingTrigger(this);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            
            var item = eventData.pointerDrag.GetComponent<ShelfItem>();
            
            if (item && item.type == shelfType)
            {         
                item.SetPendingTrigger(null);
            }
        }

        public async void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            
            var item = eventData.pointerDrag.GetComponent<ShelfItem>();

            if (!item) return;
            
            if (item.type == shelfType)
            {
                if (currentShelfItem)
                {
                    if (item.dropTriggerConfirmed)
                    {
                        item.dropTriggerConfirmed.AttachItem(currentShelfItem).Forget();
                    }
                    else
                    {
                        Debug.Log("Failed to find dropTriggerConfirmed");
                    }
                        
                    if (currentShelfItem.type != item.type)
                    {
                        _cacheAudio.Play(BaseEnums.Sounds.Right);
                    }
                }

                await AttachItem(item);
            }
            else
            {
                if (item.dropTriggerConfirmed == this)
                {
                    return;
                }

                item.AnimationWrongTry();
                    
                _cacheAudio.Play(BaseEnums.Sounds.Wrong);
                    
                Debug.Log($"Replace error! Your item type: {item.type}; expected item type: {shelfType}");
            }
        }
    }
}