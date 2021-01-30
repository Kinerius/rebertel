using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Levels;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    LevelController[] levelControllers;

    private void Start()
    {
        LevelController levelController = levelControllers.First();
        levelController.Initialize();
        levelController.gameObject.SetActive(true);
    }
}
