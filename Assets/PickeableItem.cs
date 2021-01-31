using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class PickeableItem : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sentences;

    private Dialog _dialog;

    public bool upgradeWeapon = false;
    public bool upgradeShield = false;
    public bool upgradeDash = false;
    private EntityController _player;

    private void Start()
    {
        GameObject dialogController = GameObject.Find("DialogController");
        _dialog = dialogController.GetComponent<Dialog>();
        _player = GameObject.Find("Player").GetComponent<EntityController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            //Collisiono con el player, pikearlo y setear dialogo
            _dialog.setSentences(sentences);

            SetUpgrades();
            
            Destroy(gameObject);
        }
    }

    private void SetUpgrades()
    {
        _player.isBlasterUpgraded |= upgradeWeapon;
        _player.isDashUpgraded |= upgradeDash;
        _player.isShieldUpgraded |= upgradeShield;
    }
}
