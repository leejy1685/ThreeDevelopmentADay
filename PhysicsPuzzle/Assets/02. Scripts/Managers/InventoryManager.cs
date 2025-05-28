using System.Collections.Generic;
using System.Linq;
using _02._Scripts.Character.Player;
using _02._Scripts.Item;
using _02._Scripts.Item.DataAndTable;
using _02._Scripts.Objects.LaserMachine;
using _02._Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace _02._Scripts.Managers
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Inventory Slots")] 
        [SerializeField] private List<ItemSlot> itemSlots;
        [SerializeField] private int maxDataCount = 5;
        [SerializeField] private GameObject itemSlotPrefab;
        
        [Header("Item Info.")]
        [CanBeNull] [SerializeField] private ItemData selectedItem;
        [SerializeField] private int selectedItemIndex;
        
        [Header("Item Throw Position")]
        [SerializeField] private Transform itemThrowPivot;
        
        // Fields
        private CharacterManager _characterManager;
        private UIManager _uiManager;
        private PlayerEquipment _playerEquipment;
        private PlayerInteraction _playerInteraction;
        private PlayerCondition _playerCondition;
        private Player _player;
        
        // Properties
        public ItemData SelectedItem => selectedItem;
        
        // Singleton
        public static InventoryManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance) { Instance = this; }
            else { if(Instance != this) Destroy(gameObject); }
        }

        private void Start()
        {
            _characterManager = CharacterManager.Instance;
            _playerEquipment = _characterManager.Player.PlayerEquipment;
            _playerInteraction = _characterManager.Player.PlayerInteraction;
            _playerCondition = _characterManager.Player.PlayerCondition;
            _player = _characterManager.Player;
            _uiManager = UIManager.Instance;
            itemThrowPivot = _player.ItemThrowPivot;
            
            itemSlots = new List<ItemSlot> { Capacity = maxDataCount };

            for (var i = 0; i < maxDataCount; i++)
            {
                var go = Instantiate(itemSlotPrefab, _uiManager.GameUI.InventoryUITransform);
                itemSlots.Add(Helper.GetComponent_Helper<ItemSlot>(go));
                itemSlots[i].Index = i;
            }
            UpdateSlots();
            SelectItem(0);
        }
        
        public void AddItem(ItemData data)
        {
            if (!data) return;

            var slot = GetItemInStack(data);
            if (slot)
            {
                slot.Quantity++;
                UpdateSlots();
                SelectItem(itemSlots.IndexOf(slot));
                return;
            }
            
            // If Item is not stackable or reached to maxStackCount, find Empty Slot
            var emptySlot = GetEmptySlot();
            if(emptySlot)
            {
                emptySlot.ItemData = data;
                emptySlot.Quantity = 1;
                UpdateSlots();
                SelectItem(itemSlots.IndexOf(emptySlot));
                return;
            }
            
            // If there is no slot left to store, throw Picked Item
            ThrowItem(data);
        }

        public void UseItem()
        {
            if (!selectedItem || !_playerCondition.IsPlayerCharacterHasControl) return;
            if (_playerInteraction.Interactable is not LaserMachine laserMachine) return;
            laserMachine.SetLineColor(selectedItem.color);
            RemoveSelectedItem();
        }

        public void DropItem()
        {
            if (!selectedItem || !_playerCondition.IsPlayerCharacterHasControl) return;
            ThrowItem(selectedItem);
            RemoveSelectedItem();
        }
        
        private void UpdateSlots()
        {
            foreach (var slot in itemSlots)
            {
                if (slot.ItemData) slot.Set();
                else slot.Clear();
            }
        }
        
        private ItemSlot GetItemInStack(ItemData data)
        {
            return itemSlots.FirstOrDefault(slot => slot.ItemData == data && slot.Quantity < data.maxStackCount);
        }

        private ItemSlot GetEmptySlot()
        {
            return itemSlots.FirstOrDefault(slot => !slot.ItemData);
        }

        public void SelectNextItem()
        {
            // Set ItemSlot Outline
            itemSlots[selectedItemIndex].SetOutline(false);
            selectedItemIndex = (selectedItemIndex + 1) % maxDataCount;
            itemSlots[selectedItemIndex].SetOutline(true);
            
            SelectItem(selectedItemIndex);
        }

        public void SelectPrevItem()
        {
            // Set ItemSlot Outline
            itemSlots[selectedItemIndex].SetOutline(false);
            if (selectedItemIndex <= 0) selectedItemIndex = maxDataCount - 1;
            else selectedItemIndex = (selectedItemIndex - 1) % maxDataCount;
            itemSlots[selectedItemIndex].SetOutline(true);
            
            SelectItem(selectedItemIndex);
        }
        
        private void SelectItem(int index)
        {
            if (!itemSlots[index].ItemData) 
            { 
                _playerEquipment.UnequipItem();
                selectedItem = null; 
                return;
            }
            
            selectedItem = itemSlots[index].ItemData;
            selectedItemIndex = index;
            _playerEquipment.EquipItem(selectedItem);
        }
        
        private void RemoveSelectedItem()
        {
            itemSlots[selectedItemIndex].Quantity--;
            if (itemSlots[selectedItemIndex].Quantity <= 0)
            {
                selectedItem = null;
                itemSlots[selectedItemIndex].ItemData = null;
                _playerEquipment.UnequipItem();
            }
            UpdateSlots();
        }
        
        private void ThrowItem(ItemData item)
        {
            Instantiate(item.itemPrefab, itemThrowPivot.position, itemThrowPivot.rotation);
        }
    }
}