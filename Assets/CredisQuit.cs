using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CredisQuit : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        } 
    }
}
