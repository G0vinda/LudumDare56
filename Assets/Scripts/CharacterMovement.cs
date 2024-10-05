using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //kinematic rb
    Rigidbody2D rb;
    private Vector2 extraVelocity;
    private Vector2 inputVelocity;
    float horizontalInput;
    public float movementSpeed;
    public Transform groundChecker;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public float jumpHeight;

    public float frictionStrenght;


    bool isOnAir = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        inputVelocity = new Vector2(horizontalInput*movementSpeed, 0);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                Jump();
            }
        }
    }



    public void Jump()
    {

        ApplyForce(Vector2.up*Mathf.Sqrt(-2*Physics2D.gravity.y*jumpHeight), false);
    }


    private bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(groundChecker.position, groundCheckRadius, groundLayer) != null)
        {
            return true;
        }

        return false;

    }


    private void FixedUpdate()
    {

        ApplyGravity();
     //   ApplyFriction();
        Vector2 movementAmount= (inputVelocity + extraVelocity)*Time.fixedDeltaTime;
        rb.MovePosition(rb.position+movementAmount);
    }

    private void ApplyGravity()
    {
        if (IsGrounded())
        {
            if (extraVelocity.y < 0)
            {
                extraVelocity = new Vector2(extraVelocity.x, 0);

            }

        }
        else
        {

            extraVelocity +=Vector2.up*Physics2D.gravity.y*rb.mass*Time.fixedDeltaTime;
            
        }
    }

    public void ApplyForce(Vector2 forceAmount, bool isImpulse)
    {
        float multiplier = isImpulse ? 50 : 1;
        extraVelocity += forceAmount * multiplier;
    }

    private void ApplyFriction()
    {
        extraVelocity = extraVelocity - extraVelocity.normalized * frictionStrenght;

        if (extraVelocity.magnitude < 0.05f)
        {
            extraVelocity = Vector2.zero;
        }


    }

}
