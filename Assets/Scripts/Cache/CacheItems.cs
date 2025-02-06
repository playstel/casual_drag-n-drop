using System.Linq;
using Configs;
using Enums;
using UnityEngine;

namespace Cache
{
    public class CacheItems : MonoBehaviour
    {
        [Header("Config")]
        public ConfigItems ConfigItems;

        public GameObject CreateItem(BaseEnums.ShelfType type)
        {
            var item = ConfigItems.itemPrefabList.FirstOrDefault(i => i.type == type);
            
            if (item == default)
            {
                Debug.LogError($"Failed to find {type} in ConfigItems");
                return null;
            }
            
            if (!item.prefabItem)
            {
                Debug.LogError($"Prefab for {type} was not initialized");
                return null;
            }
            
            return Instantiate(item.prefabItem);
        }
    }
}