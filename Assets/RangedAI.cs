using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class RangedAI : MonoBehaviour
    {
        [SerializeField]
        private EntityController entityTarget;

        private GameObject player;

        private void Start()
        {
            player = GameObject.Find("Player");
        }
        private void Update()
        {
            ShootAtPlayer();
        }
        private void ShootAtPlayer()
        {
            entityTarget.ShootAt(player.transform.position);
        }
    }
}
