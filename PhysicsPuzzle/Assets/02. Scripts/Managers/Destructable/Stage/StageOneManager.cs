using System;
using System.Linq;
using _02._Scripts.Character.Player;
using _02._Scripts.Managers.Destructable.Item;
using _02._Scripts.Managers.Indestructable;
using UnityEngine;

namespace _02._Scripts.Managers.Destructable.Stage
{
    public class StageOneManager : StageManager
    {
        private ItemManager _itemManager;
        private ItemSpawnManager _itemSpawnManager;
        private bool _isItemSpawned;

        protected override void Start()
        {
            _itemManager = ItemManager.Instance;
            _itemSpawnManager = ItemSpawnManager.Instance;
            
            base.Start();
            _playerCondition.SetGravityAllow(false);
            
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