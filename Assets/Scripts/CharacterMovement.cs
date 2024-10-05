using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //kinematic rb
    Rigidbody2D rb;

    public Vector2 extraVelocity;



    private void FixedUpdate()
    {
        ApplyFriction();

        rb.MovePosition(rb.position + extraVelocity);
    }

    public void ApplyForce(Vector2 forceAmount,bool isImpulse)
    {
        float multiplier = isImpulse ? 50 :1;
        extraVelocity += forceAmount * multiplier;
    }

    private void ApplyFriction()
    {

    }

}
