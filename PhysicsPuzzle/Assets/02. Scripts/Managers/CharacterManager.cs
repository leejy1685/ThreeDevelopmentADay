using _02._Scripts.Character.Player;
using UnityEngine;

namespace _02._Scripts.Managers
{
    public class CharacterManager : Singleton<CharacterManager>
    {
        // Manages player in every scene
        [Header("Player")] 
        [SerializeField] private Player player;
        
        // Properties
        public Player Player => player;
        
        protected override void Awake()
        {
            base.Awake();
            player = FindFirstObjectByType<Player>();
        }
    }
}