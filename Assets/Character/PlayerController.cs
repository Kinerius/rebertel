using System;
using UnityEngine;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private EntityController targetEntity;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            HandleMovement();

            if (Input.GetMouseButton(0))
            {
                ShootAtMousePosition();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Dash();
            }
        }

        private void Dash()
        {
            targetEntity.Dash();
        }

        private void ShootAtMousePosition()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
             var plane = new Plane(Vector3.up, 0);

             if (plane.Raycast(ray, out var enter))
             {
                 Vector3 hitPoint = ray.GetPoint(enter);
                 targetEntity.ShootAt(hitPoint);
             }
        }

        private void HandleMovement()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            var horizontalSpeed = horizontal * Vector3.right;
            var verticalSpeed = vertical * Vector3.forward;

            targetEntity.SetMovementDirection((horizontalSpeed + verticalSpeed).normalized);
        }
    }
}