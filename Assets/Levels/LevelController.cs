using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Levels.LevelScripts;
using UI;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Levels
{
    public class LevelController : MonoBehaviour
    {
        public event Action LevelCompleted;
        
        [SerializeField] GameObject playerSpawn;
        [SerializeField] GameObject[] enemySpawners;
        [SerializeField] private WaveData[] waves;
        [SerializeField] GameObject nextLevelPortal;
        [SerializeField] public List<Sprite> sentences;
        
        [SerializeField] GameObject item;

        private EntityController _player;
        private List<Wave> _waves;
        private int enemyCount;
        private IDisposable _nextLevelTimer;
        private int SECONDS_UNTIL_NEXT_LEVEL = 3;
        

        public void Initialize()
        {
            nextLevelPortal.gameObject.SetActive(false);
            if (item != null)
            {
                item.gameObject.SetActive(false);
            }
            
            _player = GameObject.Find("Player").GetComponent<EntityController>();
            if(_player == null)
            {
                throw new Exception("No se encontro al Player");
            }
            _player.transform.position = playerSpawn.transform.position;

                        
            _player.OnNextLevelPortalEntered += OnNextLevelPortalEntered;
            _player.OnNextLevelPortalExited += OnNextLevelPortalExited;
            
            SetupWaves();
        }

        private void OnNextLevelPortalEntered()
        {
            InitializeNextLevelCountdown();
        }

        private void InitializeNextLevelCountdown()
        {
            _nextLevelTimer = Observable.Interval(TimeSpan.FromSeconds(1))
                .TakeWhile(interval => interval < SECONDS_UNTIL_NEXT_LEVEL)
                .DoOnSubscribe(() => UIController.Instance.SetCountdown(SECONDS_UNTIL_NEXT_LEVEL))
                .Do(t => UIController.Instance.SetCountdown(SECONDS_UNTIL_NEXT_LEVEL - (t+1)))
                .DoOnCompleted(OnNextLevelTimerComplete)
                .Subscribe();
        }

        private void OnNextLevelTimerComplete()
        {
            UIController.Instance.SetCountdown(-1);
            LevelCompleted?.Invoke();
            _player.Heal();
        }

        private void OnNextLevelPortalExited()
        {
            UIController.Instance.SetCountdown(-1);
            _nextLevelTimer?.Dispose();
        }

        private void Update()
        {
            if (_waves == null) return;
            for (int i = _waves.Count - 1; i >= 0; i--)
            {
                var wave = _waves[i];
                if (!wave.Update(Time.deltaTime)) continue;
                
                _waves.Remove(wave);
                OnWaveSpawn(wave.GetData());
            }
        }

        private void SetupWaves()
        {
            _waves = new List<Wave>();
            foreach (var waveData in waves)
            {
                var wave = new Wave(waveData);
                _waves.Add(wave);
            }
        }

        private void OnWaveSpawn(WaveData waveData)
        {
            enemyCount += waveData.count;
            
            for (int i = 0; i < waveData.count; i++)
            {
                Observable.Timer(TimeSpan.FromSeconds(i * Random.Range(waveData.timeForEachSpawn, waveData.timeForEachSpawn+0.5f )))
                    .Do(_ => SpawnEnemy(waveData.prefab))
                    .Subscribe();
            }
        }

        private void SpawnEnemy(GameObject enemy)
        {
            var avalableSpawners = enemySpawners.Where(e => Vector3.Distance(e.transform.position, _player.transform.position) > 3).ToList();
            var spawner = avalableSpawners[Random.Range(0, avalableSpawners.Count)];
            var spawnerPosition = spawner.transform.position;

            var spawnerBounds = spawner.GetComponent<BoxCollider>().bounds;
            var randX = spawnerBounds.extents.x * Random.Range(-1, 1);
            var randZ = spawnerBounds.extents.z * Random.Range(-1, 1);
           
            var inst = Instantiate(enemy, new Vector3(spawnerPosition.x + randX, spawnerPosition.y, spawnerPosition.z + randZ) ,Quaternion.identity);
            inst.GetComponent<EntityController>().OnDeathEvent += OnEnemyDestroyed;
        }

        private void OnEnemyDestroyed()
        {
            enemyCount--;
            if (enemyCount == 0)
            {
                nextLevelPortal.gameObject.SetActive(true);
                if (item != null)
                {
                    item.gameObject.SetActive(true);
                }
            }
        }
    }
}
