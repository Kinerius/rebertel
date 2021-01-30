using System;
using System.Collections.Generic;
using System.Linq;
using Levels.LevelScripts;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Levels
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] GameObject playerSpawn;
        [SerializeField] GameObject[] enemySpawners;

        [SerializeField] private WaveData[] waves;

        private GameObject _player;
        private List<Wave> _waves;
        
        public void Initialize()
        {
            _player = GameObject.Find("Player");
            if(_player == null)
            {
                throw new Exception("No se encontro al Player");
            }
            _player.transform.position = playerSpawn.transform.position;

            SetupWaves();
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
            for (int i = 0; i < waveData.count; i++)
            {
                Observable.Timer(TimeSpan.FromSeconds(i * Random.Range(waveData.timeForEachSpawn, waveData.timeForEachSpawn+0.5f )))
                    .Do(_ => SpawnEnemy(waveData.prefab))
                    .Subscribe();
            }
        }

        private void SpawnEnemy(GameObject enemy)
        {
            var avalableSpawners = enemySpawners.Where(e => Vector3.Distance(e.transform.position, _player.transform.position) > 5).ToList();
            var spawner = avalableSpawners[Random.Range(0, avalableSpawners.Count)];
            var spawnerPosition = spawner.transform.position;

            var spawnerBounds = spawner.GetComponent<BoxCollider>().bounds;
            var randX = spawnerBounds.extents.x * Random.Range(-1, 1);
            var randZ = spawnerBounds.extents.z * Random.Range(-1, 1);
           
            Instantiate(enemy, new Vector3(spawnerPosition.x + randX, spawnerPosition.y, spawnerPosition.z + randZ) ,Quaternion.identity);
        }
    }
}
