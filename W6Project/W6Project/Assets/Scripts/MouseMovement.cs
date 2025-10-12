using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    //movement speed and player orientation
    public float moveSpeed;
    public Transform orientation;

    //basic movement variables
    float horizontalInput;
    float verticalInput;

    //groundchecks and drag variables
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundDistance = 0.4f;
    public float groundDrag;
    bool grounded;

    //jump variables
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public KeyCode jumpKey = KeyCode.Space;
    private int jumpsLeft = 1; // change to 1 for double jump

    //slope movement stuff
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    public float playerHeight;

    //CamToggle boolean
    public bool isMouseCam = true;

    Vector3 moveDirection;

    Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        //If player switches to scientistcam
        if (Input.GetKeyDown(KeyCode.C))
        {
            isMouseCam = !isMouseCam;
        }

        if (isMouseCam)
        {
            moveSpeed = 4;
        }

        else
        {
            moveSpeed = 0;
        }

    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        // in air 
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);


            if (Input.GetKey(jumpKey))
            {
                if (jumpsLeft > 0)
                {
                    // Jump(); Uncomment out for double jump
                }
            }
        }

        //on slope
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            if (rb.linearVelocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //disables gravity while on slope
        rb.useGravity = !OnSlope();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        //ground check
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

        MyInput();

        //handle drag
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }

        else
        {
            rb.linearDamping = 0;
        }
    }

    void SpeedControl() //caps max speed
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f * rb.linearVelocity.z);

        //limit velocity when needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //reset Y velocity to maintain uniform jump heihgt
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        jumpsLeft = jumpsLeft - 1;
    }

    private void ResetJump()
    {
        readyToJump = true;
        jumpsLeft = 1;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle > maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10) //what is ground layer
        {

        }
    }
}
