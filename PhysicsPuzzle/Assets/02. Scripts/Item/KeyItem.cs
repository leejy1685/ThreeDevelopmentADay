using _02._Scripts.Item.DataAndTable;
using _02._Scripts.Managers;
using _02._Scripts.Managers.Destructable;
using _02._Scripts.Utils.Interface;
using UnityEngine;

namespace _02._Scripts.Item
{
    public class KeyItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemData itemData;
        
        public void Init(ItemData data){ itemData = data; }
        public void OnInteract()
        {
            InventoryManager.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}