using System;
using UnityEngine;

namespace Levels.LevelScripts
{
    [Serializable]
    public struct WaveData
    {
        public float when;
        public int count;
        public GameObject prefab;
        public float timeForEachSpawn;
    }
}