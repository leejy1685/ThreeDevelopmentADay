using System.Collections.Generic;
using System.Linq;
using _02._Scripts.Utils;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02._Scripts.Item.DataAndTable
{
    [CreateAssetMenu(fileName = "New ItemTable", menuName = "Item/Create New ItemTable", order = 0)]
    public class ItemTable : ScriptableObject
    {
        [SerializeField] private List<ItemData> data = new();
        [SerializeField] private SerializedDictionary<string, ItemData> dictionary = new();

        public string[] ItemNames => dictionary.Keys.ToArray();
        
        private void OnEnable()
        {
            dictionary.Clear();
            foreach (var val in data)
            {
                var item = Helper.GetComponent_Helper<KeyItem>(val.itemPrefab);
                item.Init(val);
                dictionary.Add(val.name, val);
            }
        }
        
        /// <summary>
        /// Get Item Data from Dictionary by name
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns>Returns ItemData referred to 'itemName'</returns>
        public ItemData GetItemByName(string itemName)
        {
            return dictionary.GetValueOrDefault(itemName);
        }

        /// <summary>
        /// Get Item Data from Dictionary by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ItemData GetItemByIndex(int index)
        {
            return dictionary[ItemNames[index]];
        }
        
        public int GetItemCount()
        {
            return data.Count;
        }
    }
}