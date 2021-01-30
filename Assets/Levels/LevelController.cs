using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    GameObject playerSpawn;
    [SerializeField]
    GameObject[] enemySpawn;
    [SerializeField]
    GameObject enemyPrefab;

    private GameObject player;
    public void Initialize()
    {
        player = GameObject.Find("Player");
        if(player == null)
        {
            throw new Exception("No se encontro al Player");
        }
        player.transform.position = playerSpawn.transform.position;

        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        var avalibleSpawners = enemySpawn.Where(e => Vector3.Distance(e.transform.position, player.transform.position) > 5).ToList();
        var spawner = avalibleSpawners[UnityEngine.Random.Range(0, avalibleSpawners.Count())];
        Instantiate(enemyPrefab, spawner.transform.position,Quaternion.identity);
    }
}
