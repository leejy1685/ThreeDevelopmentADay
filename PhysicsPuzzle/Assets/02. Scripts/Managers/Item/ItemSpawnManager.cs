using System;
using System.Collections;
using System.Collections.Generic;
using _02._Scripts.Item;
using _02._Scripts.Item.DataAndTable;
using _02._Scripts.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _02._Scripts.Managers.Item
{
    public class ItemSpawnManager : MonoBehaviour
    {
        [Header("ItemBox Spawn Settings")]
        [SerializeField] private List<Transform> itemBoxSpawnPoints;
        [SerializeField] private GameObject itemBoxPrefab;
        
        [Header("Item Spawn Data Settings")]
        [SerializeField] private AssetLabelReference itemSpawnTableLabel;
        [SerializeField] private ItemSpawnTable itemSpawnTable;
        
        public bool IsItemSpawnTableLoaded => itemSpawnTable;
        
        // Singleton
        public static ItemSpawnManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                itemSpawnTableLabel = new AssetLabelReference { labelString = "ItemSpawnTable" };
                StartCoroutine(LoadItemSpawnTable_Coroutine());
            } else { if (Instance != this) Destroy(gameObject); }
        }

        private IEnumerator LoadItemSpawnTable_Coroutine()
        {
            var handle = Addressables.LoadAssetAsync<ItemSpawnTable>(itemSpawnTableLabel);
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                itemSpawnTable = handle.Result;
                Debug.Log("ItemTable Load Completed!");
            }
            else
            {
                Debug.LogError("ItemTable Load Failed!");
            }
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