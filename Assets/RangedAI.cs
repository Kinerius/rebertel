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
            entityTarget.OnCollidedWithSomething += RefreshMoveAround;
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
            int x = Random.Range(-90, 90);
            int z = Random.Range(-90, 90);
            if (x == 0 && z == 0)
                x = 1;
            var randomPoint = new Vector3(x, 0, z);
            Debug.Log(randomPoint.normalized);
            entityTarget.SetMovementDirection(randomPoint.normalized);
            _movingTimer = 2;
        }
        private void RefreshMoveAround()
        {
            _movingTimer = 0;
        }
        private void UpdateCooldowns()
        {
            _movingTimer = Mathf.Max(_movingTimer - Time.deltaTime, 0);
        }

    }
}
