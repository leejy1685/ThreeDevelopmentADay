using _02._Scripts.Item;
using _02._Scripts.Item.DataAndTable;
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
        
        // Properties
        public KeyItem CurrentKeyItem{ get => currentKeyItem; set=> currentKeyItem = value; }
        public Transform EquipmentPivot => equipmentPivot;
        public bool IsEquipped => _isEquipped;

        public void EquipItem(ItemData data)
        {
            UnequipItem();
            var item = Instantiate(data.itemPrefab, equipmentPivot);
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