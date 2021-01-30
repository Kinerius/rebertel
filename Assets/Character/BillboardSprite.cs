using System;
using UnityEngine;

namespace Character
{
    public class BillboardSprite : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var lookNormal = _camera.transform.position - sprite.transform.position;
            sprite.transform.rotation = Quaternion.LookRotation(lookNormal.normalized);
        }
    }
}
