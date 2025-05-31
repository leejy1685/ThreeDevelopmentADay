using System;
using _02._Scripts.Item;
using _02._Scripts.Item.DataAndTable;
using _02._Scripts.Managers;
using _02._Scripts.Managers.Destructable;
using _02._Scripts.Utils;
using UnityEngine;

namespace _02._Scripts.Character.Player
{
    public class PlayerEquipment : MonoBehaviour
    {
        [Header("Current Selected Item")] 
        [SerializeField] private KeyItem currentKeyItem;
        
        [Header("Equipment Pivot")]
        [SerializeField] private Transform equipmentPivot;

        // Fields
        private bool _isEquipped;
        private Player _player;
        
        // Properties
        public KeyItem CurrentKeyItem{ get => currentKeyItem; set=> currentKeyItem = value; }
        public bool IsEquipped => _isEquipped;

        private void Start()
        {
            _player = CharacterManager.Instance.Player;
            equipmentPivot = _player.EquipmentPivot;
        }

        public void EquipItem(ItemData data)
        {
            UnequipItem();
            var item = Instantiate(data.equipPrefab, equipmentPivot);
            currentKeyItem = Helper.GetComponent_Helper<KeyItem>(item);
            _isEquipped = true;
        }

        public void UnequipItem()
        {
            if (!currentKeyItem) return;
            _isEquipped = false;
            Destroy(currentKeyItem.gameObject);
            currentKeyItem = null;
        }
    }
}