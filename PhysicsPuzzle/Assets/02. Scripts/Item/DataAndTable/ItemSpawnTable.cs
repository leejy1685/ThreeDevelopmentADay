using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace _02._Scripts.Item.DataAndTable
{
    [CreateAssetMenu(fileName = "ItemSpawnTable", menuName = "Item/ItemSpawnTable", order = 0)]
    public class ItemSpawnTable : ScriptableObject
    {
        [SerializeField] private List<ItemSpawnData> itemSpawnData = new();
        [SerializeField] private SerializedDictionary<int, ItemSpawnData> itemSpawnDataDictionary = new();

        private void OnEnable()
        {
            var i = 0;
            foreach (var data in itemSpawnData) itemSpawnDataDictionary.TryAdd(i++, data);
        }
        
        public ItemSpawnData GetItemSpawnData(int index)
        {
            return !itemSpawnDataDictionary.TryGetValue(index, out var data) ? null : data;
        }
    }
}