
using UnityEngine;

namespace Character
{
    public class EntityController : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;

        public float speed;

        void Update()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            var horizontalSpeed = horizontal * Vector3.right;
            var verticalSpeed = vertical * Vector3.forward;

            var finalSpeed = Time.deltaTime * speed * (horizontalSpeed + verticalSpeed).normalized;
            
            characterController.Move( finalSpeed);
        }

    }
}
