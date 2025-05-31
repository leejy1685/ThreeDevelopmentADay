using System.Linq;
using _02._Scripts.Character.Player;
using _02._Scripts.Managers.Destructable.Item;
using _02._Scripts.Managers.Indestructable;
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
        private CharacterManager _characterManager;
        private PlayerCondition _playerCondition;
        private bool _isItemSpawned;

        private void Start()
        {
            _itemManager = ItemManager.Instance;
            _itemSpawnManager = ItemSpawnManager.Instance;
            _gameManager = GameManager.Instance;
            _characterManager = CharacterManager.Instance;
            _playerCondition = _characterManager.Player.PlayerCondition;
            
            isRoomCleared = new bool[currentRoomCount];
            _playerCondition.SetGravityAllow(false);
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