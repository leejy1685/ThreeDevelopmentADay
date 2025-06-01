using _02._Scripts.Character.Player.Interface;
using _02._Scripts.Item.DataAndTable;
using _02._Scripts.Managers;
using _02._Scripts.Managers.Destructable;
using _02._Scripts.Managers.Indestructable;
using UnityEngine;

namespace _02._Scripts.Item
{
    public class KeyItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemData itemData;
        [SerializeField] private AudioClip pickupSound;
        
        public void Init(ItemData data){ itemData = data; }
        public void OnInteract()
        {
            SoundManager.PlaySFX(pickupSound);
            InventoryManager.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}