using System.Collections.Generic;
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
    }
}