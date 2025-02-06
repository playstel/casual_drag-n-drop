using System;
using System.Collections.Generic;
using Base.Models;
using Enums;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Config/Sounds")]
    public class ConfigSounds : ScriptableObject
    {
        public List<BaseModels.Sound> soundList;
    }
}