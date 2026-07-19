using System;
using UnityEngine;

namespace LostMonkey.Core
{
    /// <summary>
    /// Per-level coordinator (one per gameplay scene). Tracks banana collection
    /// and level completion, and exposes events the HUD and end screen subscribe
    /// to. Reachable within the scene via <see cref="Instance"/>.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Tooltip("1-based level number; used for progression/unlocking.")]
        [SerializeField] private int levelNumber = 1;

        /// <summary>(collected, total)</summary>
        public event Action<int, int> BananasChanged;
        /// <summary>(collected, total)</summary>
        public event Action<int, int> LevelCompleted;

        private BananaCounter _counter;
        private bool _completed;

        public int LevelNumber => levelNumber;
        public int BananasCollected => _counter?.Collected ?? 0;
        public int BananasTotal => _counter?.Total ?? 0;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            // Total = number of Collectibles placed in the scene.
            int total = FindObjectsOfType<Collectible>(includeInactive: true).Length;
            _counter = new BananaCounter(total);
            BananasChanged?.Invoke(_counter.Collected, _counter.Total);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void CollectBanana()
        {
            if (_counter != null && _counter.Collect())
            {
                BananasChanged?.Invoke(_counter.Collected, _counter.Total);
            }
        }

        public void CompleteLevel()
        {
            if (_completed)
            {
                return;
            }
            _completed = true;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.NotifyLevelCompleted(levelNumber, BananasCollected);
            }
            LevelCompleted?.Invoke(BananasCollected, BananasTotal);
        }
    }
}
