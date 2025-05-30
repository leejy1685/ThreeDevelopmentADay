using System.Collections.Generic;
using _02._Scripts.Item;
using _02._Scripts.Item.DataAndTable;
using _02._Scripts.Utils;
using UnityEngine;

namespace _02._Scripts.Managers.Item
{
    public class ItemSpawnManager : MonoBehaviour
    {
        [Header("ItemBox Spawn Settings")]
        [SerializeField] private List<Transform> itemBoxSpawnPoints;
        [SerializeField] private GameObject itemBoxPrefab;
        
        [Header("Item Spawn Data Settings")]
        [SerializeField] private ItemSpawnTable itemSpawnTable;
        
        // Singleton
        public static ItemSpawnManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            } else { if (Instance != this) Destroy(gameObject); }
        }
        
        public void SpawnItemBoxes()
        {
            var i = 0;
            foreach (var itemBoxSpawnPoint in itemBoxSpawnPoints)
            {
                var go = Instantiate(itemBoxPrefab, itemBoxSpawnPoint.position, Quaternion.identity);
                var itemBox = Helper.GetComponent_Helper<ItemBox>(go);
                itemBox.Init(i, itemSpawnTable.GetItemSpawnData(i++));
                itemBox.SpawnItems();
            }
        }
    }
}