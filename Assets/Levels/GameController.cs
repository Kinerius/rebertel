using UnityEngine;

namespace Levels
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        LevelController[] levelControllers;

        private LevelController _currentLevel;
        private int _currentLevelIndex = 0;
        private void Start()
        {
            StartLevel(0);
        }

        private void StartLevel(int level)
        {
            _currentLevel = levelControllers[level];
            _currentLevel.Initialize();
            _currentLevel.gameObject.SetActive(true);
            _currentLevel.LevelCompleted += OnLevelComplete;
        }

        private void OnLevelComplete()
        {
            if (_currentLevelIndex == levelControllers.Length - 1)
            {
                Debug.Log("Ganaste papu");
                return;
            }
            
            _currentLevel.LevelCompleted -= OnLevelComplete;
            _currentLevel.gameObject.SetActive(false);

            _currentLevelIndex++;
            StartLevel(_currentLevelIndex);
        }
    }
}
