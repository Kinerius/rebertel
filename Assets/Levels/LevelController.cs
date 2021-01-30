using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    GameObject playerSpawn;
    [SerializeField]
    GameObject[] enemySpawn;

    public void Initialize()
    {
        var player = GameObject.Find("Player");
        if(player == null)
        {
            throw new Exception("No se encontro al Player");
        }
        player.transform.position = playerSpawn.transform.position;
    }
}
