using System.Linq;
using _02._Scripts.Managers.Destructable.Item;
using UnityEngine;

namespace _02._Scripts.Managers.Destructable.Stage
{
    public class StageOneManager : Singleton<StageOneManager>
    {
        [Header("Stage Clear Settings")]
        [SerializeField] private bool isStageCleared;

        [Header("Room Clear Settings")] 
        [SerializeField] private int currentRoomCount;
        [SerializeField] private bool[] isRoomCleared;
        
        private ItemManager _itemManager;
        private ItemSpawnManager _itemSpawnManager;
        private GameManager _gameManager;
        private bool _isItemSpawned;

        private void Start()
        {
            _itemManager = ItemManager.Instance;
            _itemSpawnManager = ItemSpawnManager.Instance;
            _gameManager = GameManager.Instance;
            
            isRoomCleared = new bool[currentRoomCount];
        }

        private void Update()
        {
            if (!_isItemSpawned)
            {
                if (_itemManager.IsItemTableLoaded && _itemSpawnManager.IsItemSpawnTableLoaded)
                {
                    _itemSpawnManager.SpawnItemBoxes();
                    _isItemSpawned = true;
                }
            }
        }

        public void RoomCleared(int roomId)
        {
            isRoomCleared[roomId] = true;
            if (isRoomCleared.Any(isCleared => !isCleared)) return;
            isStageCleared = true;
            _gameManager.StageClear();
        }
    }
}