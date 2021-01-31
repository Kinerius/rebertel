using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class MeleeAI : MonoBehaviour
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
            if (player == null) return;
            MoveToPlayer();
        }
        private void MoveToPlayer()
        {
            entityTarget.SetMovementDirection((player.transform.position - gameObject.transform.position).normalized);
        }
    }
}