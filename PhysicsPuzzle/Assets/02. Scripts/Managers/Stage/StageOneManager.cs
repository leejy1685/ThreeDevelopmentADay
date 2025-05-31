using System;
using _02._Scripts.Managers.Item;
using UnityEngine;

namespace _02._Scripts.Managers.Stage
{
    public class StageOneManager : MonoBehaviour
    {
        private ItemManager _itemManager;
        private ItemSpawnManager _itemSpawnManager;
        private bool _isItemSpawned;
        
        public static StageOneManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            } else{ if(Instance != this) Destroy(gameObject); }
        }

        private void Start()
        {
            _itemManager = ItemManager.Instance;
            _itemSpawnManager = ItemSpawnManager.Instance;
        }

        private void Update()
        {
            if (!_isItemSpawned)
            {
                if (_itemManager.IsItemTableLoaded && _itemSpawnManager.IsItemSpawnTableLoaded)
                {
                    _itemSpawnManager.SpawnItemBoxes();
                    _isItemSpawned = true;
                }
            }
        }
    }
}