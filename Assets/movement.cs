using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public Rigidbody rg;
    float vertical;
    float horizontal;

    public float speed;
    
    void Start()
    {
        
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector3 finalMovement = new Vector3(horizontal * speed, 0, vertical * speed);
        finalMovement = Vector3.ClampMagnitude(finalMovement, 1f);
        rg.velocity = finalMovement * speed;
    }
}
