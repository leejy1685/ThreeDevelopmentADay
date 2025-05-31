using System;
using System.Collections.Generic;
using _02._Scripts.Objects.LaserMachine;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02._Scripts.Item
{
    [Serializable] public class ItemSpawnData
    {
        // Properties
        public List<LASER_COLOR> itemsToSpawn;
        public List<int> itemSpawnCount;
    }
}