using System;
using Enums;
using UnityEngine;

namespace Base.Models
{
    public class BaseModels
    {
        [Serializable]
        public class Sound
        {
            public BaseEnums.Sounds soundType;
            public AudioClip soundClip;
        }

        [Serializable]
        public class ItemPrefab
        {
            public BaseEnums.ShelfType type;
            public GameObject prefabItem;
        }
        
        [Serializable]
        public class ShelfElement
        {
            public BaseEnums.ShelfType type;
            public Transform transformShelf;
        }
    }
}