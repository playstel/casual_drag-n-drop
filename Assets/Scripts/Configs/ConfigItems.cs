using System;
using System.Collections.Generic;
using Base.Models;
using Enums;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Config/Items")]
    public class ConfigItems : ScriptableObject
    {
        public List<BaseModels.ItemPrefab> itemPrefabList;
    }
}