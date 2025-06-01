using System;
using System.Collections.Generic;
using _02._Scripts.Objects.LaserMachine;

namespace _02._Scripts.Item.DataAndTable
{
    [Serializable] public class ItemSpawnData
    {
        // Properties
        public List<LASER_COLOR> itemsToSpawn;
        public List<int> itemSpawnCount;
    }
}