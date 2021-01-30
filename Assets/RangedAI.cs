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

        private float _movingTimer;
        private void Start()
        {
            player = GameObject.Find("Player");
        }
        private void Update()
        {
            ShootAtPlayer();
            MoveAround();
            UpdateCooldowns();
        }
        private void ShootAtPlayer()
        {
            entityTarget.ShootAt(player.transform.position);
            
        }

        private void MoveAround()
        {
            if (_movingTimer > 0) return;
            var randomPoint = new Vector3(Random.Range(-2,2),0, Random.Range(-2,2));
            Debug.Log(randomPoint);
            entityTarget.SetMovementDirection(randomPoint);
            _movingTimer = 5;
        }
        private void UpdateCooldowns()
        {
            _movingTimer = Mathf.Max(_movingTimer - Time.deltaTime, 0);
        }

    }
}
