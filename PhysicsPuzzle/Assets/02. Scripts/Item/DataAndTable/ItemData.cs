using UnityEngine;

namespace _02._Scripts.Item.DataAndTable
{
    public enum ITEM_TYPE
    {
        Prop,
        Barrel,
    }
    
    [CreateAssetMenu(fileName = "New ItemData", menuName = "Item/Create New ItemData", order = 0)]
    public class ItemData : ScriptableObject
    {
        [Header("Item Info.")] 
        public string itemName;
        public string description;
        public ITEM_TYPE itemType;
        public Sprite icon;
        public Color color;
        public GameObject itemPrefab;
        
        [Header("Max Stack Count")]
        public int maxStackCount;
    }
}