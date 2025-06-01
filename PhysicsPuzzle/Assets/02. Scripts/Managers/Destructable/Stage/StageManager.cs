using System.Linq;
using _02._Scripts.Character.Player;
using _02._Scripts.Managers.Destructable.Item;
using _02._Scripts.Managers.Indestructable;
using UnityEngine;

namespace _02._Scripts.Managers.Destructable.Stage
{
    public class StageManager : Singleton<StageManager>
    {
        [Header("Stage Clear Settings")]
        [SerializeField] protected bool isStageCleared;

        [Header("Room Clear Settings")] 
        [SerializeField] protected int currentRoomCount;
        [SerializeField] protected bool[] isRoomCleared;
        
        protected GameManager _gameManager;
        protected CharacterManager _characterManager;
        protected PlayerCondition _playerCondition;

        [SerializeField] private AudioClip StageBGM;
        
        protected virtual void Start()
        {
            _gameManager = GameManager.Instance;
            _characterManager = CharacterManager.Instance;
            _playerCondition = _characterManager.Player.PlayerCondition;
            
            isRoomCleared = new bool[currentRoomCount];
            
            //스테이지 시작
            GameManager.Instance.StageStart();
            SoundManager.Instance.ChangeBGM(StageBGM);
        }

        public void RoomCleared(int roomId)
        {
            isRoomCleared[roomId] = true;
            _gameManager.SaveData(roomId);
            if (isRoomCleared.Any(isCleared => !isCleared)) return;
            isStageCleared = true;
            _gameManager.StageClear();
        }
    }
}