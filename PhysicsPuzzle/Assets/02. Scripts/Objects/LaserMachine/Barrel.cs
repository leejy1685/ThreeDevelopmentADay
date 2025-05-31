using System;
using _02._Scripts.Item.DataAndTable;
using _02._Scripts.Managers;
using _02._Scripts.Managers.Destructable.Item;
using _02._Scripts.Utils.Interface;
using UnityEngine;

namespace _02._Scripts.Objects.LaserMachine
{
    public class Barrel : MonoBehaviour, IInteractable
    {
        [Header("Item Spawn Point")]
        [SerializeField] private Transform itemSpawnPoint;
        
        private LaserMachine _laserMachine;
        private ItemManager _itemManager;
        private ItemTable _itemTable;
        
        public Transform ItemSpawnPoint => itemSpawnPoint;

        public void Init(LaserMachine laserMachine)
        {
            _itemManager = ItemManager.Instance;
            _laserMachine = laserMachine;
        }
        
        public void OnInteract()
        {
            if (_laserMachine.laserColor == LASER_COLOR.White) return;
            
            _itemTable = _itemManager.ItemTable;
            Instantiate(_itemTable.GetItemByIndex((int)_laserMachine.laserColor).itemPrefab, itemSpawnPoint.position, Quaternion.identity);
            _laserMachine.SetColorOfMachine(LASER_COLOR.White);
        }
    }
}