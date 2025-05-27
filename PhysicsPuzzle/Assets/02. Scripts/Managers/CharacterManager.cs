using _02._Scripts.Character.Player;
using _02._Scripts.Utils;
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
            player = Helper.GetComponent_Helper<Player>(GameObject.FindWithTag("Player"));
        }
    }
}