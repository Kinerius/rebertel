using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKey)
        {
            changeScene();
        }
    }
    private void changeScene()
    {
        SceneManager.LoadScene("Opening");
    }
}
