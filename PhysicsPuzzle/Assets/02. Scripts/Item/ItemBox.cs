using System.Collections.Generic;
using _02._Scripts.Managers;
using _02._Scripts.Managers.Destructable.Item;
using _02._Scripts.Objects.LaserMachine;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02._Scripts.Item
{
    public class ItemBox : MonoBehaviour
    {
        [Header("Item Spawn Point")]
        [SerializeField] private Transform itemSpawnPoint;

        [Header("Spawn Settings")] 
        [SerializeField] private int id = 0;
        [SerializeField] private List<LASER_COLOR> itemsToSpawn = new();
        [SerializeField] private List<int> itemSpawnCount = new();
        [SerializeField] private Rect spawnArea;
        
        private ItemManager _itemManager;
        
        public void Init(int id, ItemSpawnData data)
        {
            _itemManager = ItemManager.Instance;
            this.id = id;
            itemsToSpawn = new List<LASER_COLOR>(data.itemsToSpawn);
            itemSpawnCount = new(data.itemSpawnCount);
        }

        public void SpawnItems()
        {
            for (var i = 0; i < itemsToSpawn.Count; i++)
            {
                for (var j = 0; j < itemSpawnCount[i]; j++)
                {
                    var spawnPoint = itemSpawnPoint.position + new Vector3(
                        Random.Range(spawnArea.x - spawnArea.width / 2f, spawnArea.x + spawnArea.width / 2f), 0, 
                        Random.Range(spawnArea.y - spawnArea.height / 2f, spawnArea.y + spawnArea.height / 2f));
                    Instantiate(_itemManager.ItemTable.GetItemByIndex((int)itemsToSpawn[i]).itemPrefab, spawnPoint, Quaternion.identity);
                }
            }
        }
        
        /// <summary>
        /// Draw Gizmos which are selected in a scene
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 center = new(spawnArea.x, itemSpawnPoint.position.y, spawnArea.y);
            Vector3 size = new(spawnArea.width, 0, spawnArea.height);

            Gizmos.DrawCube(center, size);
        }
    }
}