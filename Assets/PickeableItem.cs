using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickeableItem : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sentences;

    private Dialog _dialog;
    private void Start()
    {
        GameObject dialogController = GameObject.Find("DialogController");
        _dialog = dialogController.GetComponent<Dialog>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            //Collisiono con el player, pikearlo y setear dialogo
            _dialog.setSentences(sentences);
            Destroy(gameObject);
        }
    }
}
