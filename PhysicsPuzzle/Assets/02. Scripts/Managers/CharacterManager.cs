using _02._Scripts.Character.Player;
using _02._Scripts.Utils;
using UnityEngine;

namespace _02._Scripts.Managers
{
    public class CharacterManager : MonoBehaviour
    {
        // Fields
        [SerializeField] private Player player;
        
        // Properties
        public Player Player => player;
        
        // Singleton
        public static CharacterManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                if (!player) player = Helper.GetComponent_Helper<Player>(GameObject.FindWithTag("Player"));
            }
            else { if (Instance != this) Destroy(gameObject); }
        }
    }
}