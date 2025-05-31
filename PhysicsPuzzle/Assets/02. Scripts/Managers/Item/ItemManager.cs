using System.Collections;
using _02._Scripts.Item.DataAndTable;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _02._Scripts.Managers.Item
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private ItemTable itemTable;
        [SerializeField] private AssetLabelReference itemTableLabel;
            
        // Singleton
        public static ItemManager Instance { get; private set; }
            
        // Properties
        public ItemTable ItemTable => itemTable;
        public bool IsItemTableLoaded => itemTable;
            
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                itemTableLabel = new AssetLabelReference { labelString = "InkTable" };
                StartCoroutine(LoadItemTable_Coroutine());
            } 
            else { if (Instance != this) Destroy(gameObject); }
        }

        private IEnumerator LoadItemTable_Coroutine()
        {
            var handle = Addressables.LoadAssetAsync<ItemTable>(itemTableLabel);
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                itemTable = handle.Result;
                Debug.Log("ItemTable Load Completed!");
            }
            else
            {
                Debug.LogError("ItemTable Load Failed!");
            }
        }
    }
}