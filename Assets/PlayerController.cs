using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed, jumpForce, gravityScale;
    public LayerMask groundMask; //A mask for ground collisions
    public List<Transform> groundPositions; //These transforms will be used to raycast.
    public Transform displayTransform; //This transform holds the player's sprites.

    private bool FacingRight = true;
    private CharacterController theCharacterController;
    private bool isGrounded = false;
    private Vector2 moveInput; //Player's controller input
    private float timeSinceLastJump = 100;
    private float jumpCooldown = .4f;
    private Vector3 moveVelocity = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        theCharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastJump += Time.deltaTime;
        //Get Player input
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        //Test to see if player is on ground
        isGrounded = testGrounded(.5f);

        if (Input.GetButtonDown("Jump") && canJump())
        {
            moveVelocity.y = jumpForce;
            timeSinceLastJump = 0f;
        }
        //Apply gravity
        moveVelocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;
        //Apply player input
        moveVelocity.x = moveInput.x * moveSpeed;
        moveVelocity.z = moveInput.y * moveSpeed;

        theCharacterController.Move(moveVelocity * Time.deltaTime);

        if (moveInput.x < 0)
        {
            FacingRight = false;
        }
        else if (moveInput.x > 0)
        {
            FacingRight = true;
        }
        //If player is grounded set the moveVelocity.y to zero, that way the player doesnt build up gravity while standing on ground
        if (testGrounded(.1f) && canJump())
        {
            moveVelocity.y = 0f;
        }

        handleAnimation();
    }

    void handleAnimation()
    {
        //This code handles the 'flip' effect as the player changes direction horizontally
        if (FacingRight)
        {
            displayTransform.localScale = Vector3.MoveTowards(displayTransform.localScale, new Vector3(1f, 1f, 1f), 10f * Time.deltaTime);
        }
        else
        {
            displayTransform.localScale = Vector3.MoveTowards(displayTransform.localScale, new Vector3(-1f, 1f, 1f), 10f * Time.deltaTime);
        }

        //Todo: Use Unity's built in Animator to switch animations
    }

    //Cast a ray downwards from various positions. If one of the rays hits a collider with a "Ground" tag, then return true.
    bool testGrounded(float distance)
    {
        foreach (Transform item in groundPositions)
        {
            Debug.DrawRay(item.position, Vector3.down * distance, Color.red);
            if (Physics.Raycast(item.position, Vector3.down, distance, groundMask))
            {
                return true;
            }
        }

        return false;
    }

    //If the player is grounded and some time has passed since their last jump, return true.
    bool canJump()
    {
        if (isGrounded && timeSinceLastJump > jumpCooldown)
        {
            return true;
        }
        return false;
    }
}
