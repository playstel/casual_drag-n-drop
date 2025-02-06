using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UiInvisible : MonoBehaviour
{
    [Header("Status")]
    public bool invisibleStatus;
    
    [Header("Setup")]
    public bool invisibleAtStart;
    [SerializeField] private int delayAtStart;
    
    private float fadeSpeed = 0.25f;
    
    [HideInInspector]
    public CanvasGroup сanvasGroup;

    private async void OnEnable()
    {
        GetCanvasGroup();
        
        сanvasGroup.alpha = 0;

        if (delayAtStart > 0) await UniTask.Delay(delayAtStart);
        
        SetInvisible(invisibleAtStart);
    }

    private void GetCanvasGroup()
    {
        if (сanvasGroup) return;

        сanvasGroup = !GetComponent<CanvasGroup>() ? gameObject.AddComponent<CanvasGroup>() : GetComponent<CanvasGroup>();
    }
    
    public async UniTask SetInvisible(bool state)
    {
        if (сanvasGroup)
        {
            invisibleStatus = state;
            сanvasGroup.DOFade(state ? 0 : 1, fadeSpeed);
            BlocksRaycasts(!state);
            await UniTask.Delay((int)(fadeSpeed * 1000));
        }
    }
    
    public void BlocksRaycasts(bool state)
    {
        if(сanvasGroup) сanvasGroup.blocksRaycasts = state;
    }

}
