using System;
using Cache;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Core
{
    [RequireComponent(typeof(Button), typeof(BoxCollider2D), typeof(CanvasGroup))]
    public class ShelfItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Type")]
        public BaseEnums.ShelfType type;

        [Header("Mode")]
        public bool blended;

        [Header("Animation")] 
        [SerializeField] private Animation animationWrongTry;
        
        [Header("Debug Triggers")] 
        [HideInInspector] public ShelfTrigger dropTriggerConfirmed;
        [HideInInspector] public ShelfTrigger dropTriggerPending;
        
        [Header("Cache")] 
        private Transform _transform;
        private Vector2 _initialScale;
        private Vector2 _offset;
        private CanvasGroup _canvasGroup;

        [Header("DI")] 
        [Inject] private CacheGame _cacheGame;

        private void Awake()
        {
            _transform = transform;
            _initialScale = _transform.localScale;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetPendingTrigger(ShelfTrigger triggerTransform)
        {
            dropTriggerPending = triggerTransform;
        }
        
        public void SetConfirmedTrigger(ShelfTrigger triggerTransform)
        {
            dropTriggerConfirmed = triggerTransform;
        }

        public void SetBlendedStatus(bool state)
        {
            blended = state;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(!blended) return;
            
            if (!dropTriggerConfirmed)
            {
                Debug.LogError("Failed to find dropTriggerConfirmed");
                return;
            }
            
            if (dropTriggerConfirmed.shelfType == type)
            {
                Debug.Log("Item already valid");
                AnimationWrongTry();
                return;
            }
            
            ReleaseFromTrigger();

            SetOffset(eventData);
            
            ScaleOnDragStart();
        }
        
        public void ReleaseFromTrigger()
        {
            dropTriggerConfirmed = GetDropTrigger();
            
            if (dropTriggerConfirmed)
            {
                dropTriggerConfirmed.DetachItem();
            }
        }
        
        private ShelfTrigger GetDropTrigger()
        {
            return _transform.parent.TryGetComponent(out ShelfTrigger trigger) ? trigger : null;
        }

        private void SetOffset(PointerEventData eventData)
        {
            var pos = _transform.position;
            _offset = new Vector2(pos.x, pos.y) - eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(!blended) return;
            
            var newPos = eventData.position;
            _transform.position = newPos + _offset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(!blended) return;
            
            BlocksRaycasts(true);
            ScaleOnDragEnd();
            
            if (dropTriggerPending == null && dropTriggerConfirmed)
            {
                dropTriggerConfirmed.AttachItem(this);
            }
        }
        
        public void BlocksRaycasts(bool state)
        {
            _canvasGroup.blocksRaycasts = state;
        }
        
        private void ScaleOnDragStart()
        {
            var scaleFactor = 1 + (float)_cacheGame.ConfigGame.itemScalePercentOnDrag / 100;

            _transform.DOScale(_initialScale * scaleFactor, 0.1f).SetEase(Ease.InOutSine);
        }

        private void ScaleOnDragEnd()
        {
            _transform.DOScale(_initialScale, 0.1f).SetEase(Ease.InOutSine);
        }
        
        public async UniTask Move(Transform transformTarget, float moveSpeed = 0.2f)
        {
            if(!transformTarget) { Debug.Log("_transformTarget is null for " + name); return; }
            
            await _transform.DOMove(transformTarget.position, moveSpeed, true)
                .SetEase(Ease.InQuad)
                .AsyncWaitForCompletion();
        }
        
        public void AnimationWrongTry()
        {
            if (!animationWrongTry)
            {
                Debug.LogError("Failed to find animation");
                return;
            }
            
            animationWrongTry.Play();
        }
    }
}