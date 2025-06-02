using _02._Scripts.Managers.Destructable.Item;

namespace _02._Scripts.Managers.Destructable.Stage
{
    public class StageOneManager : StageManager
    {
        private ItemManager _itemManager;
        private ItemSpawnManager _itemSpawnManager;
        private bool _isItemSpawned;

        protected override void Start()
        {
            _itemManager = ItemManager.Instance;
            _itemSpawnManager = ItemSpawnManager.Instance;
            
            base.Start();
            _playerCondition.SetGravityAllow(false);
            
        }

        private void Update()
        {
            if (_isItemSpawned) return;
            if (_itemManager.IsItemTableLoaded && _itemSpawnManager.IsItemSpawnTableLoaded)
            {
                _itemSpawnManager.SpawnItemBoxes();
                _isItemSpawned = true;
            }
        }
    }
}