using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Config/Game")]
    public class ConfigGame : ScriptableObject
    {
        [Header("Prefab")]
        public GameObject ShelfTriggerPrefab;
        
        [Header("Complexity")]
        [Range(2,6)]
        public int itemsInShelf = 4;
        
        [Header("Item Drag Scale, %")]
        [Range(0,100)]
        public int itemScalePercentOnDrag = 1;
        
        [Header("Delay, msec")]
        [Range(1,3)]
        public int delayBeforeStart = 1;
        
        [Range(1,3)]
        public int delayBeforeEnd = 1;
    }
}