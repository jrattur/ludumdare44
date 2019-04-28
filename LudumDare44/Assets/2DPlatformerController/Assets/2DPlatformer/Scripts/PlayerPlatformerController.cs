using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    public GameObject gameController;

    [SerializeField]
    public int arms = 2, legs = 2;

    public GameObject armOrLegDialogue;
    public GameObject armButton, legButton;

    [SerializeField] private float ladderMoveSpeed = 0.2f;

    [SerializeField]
    private bool touchingWallLeft = false;
    [SerializeField]
    private bool touchingWallRight = false;
    [SerializeField]
    private bool monkeyBars = false;

    [SerializeField] private bool climbingLadder = false;

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private CapsuleCollider2D capsuleCollider2D;
    private Vector2 capsuleCollider2DOriginalOffset, capsuleCollider2DOriginalSize;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        capsuleCollider2DOriginalOffset = capsuleCollider2D.offset;
        capsuleCollider2DOriginalSize = capsuleCollider2D.size;
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        RaycastHit2D raycastHit2D = Physics2D.Raycast(spriteRenderer.bounds.center, Vector2.left, 0.3f, LayerMask.GetMask("Wall"));
        touchingWallLeft = raycastHit2D.collider != null ? true : false;

        raycastHit2D = Physics2D.Raycast(spriteRenderer.bounds.center, Vector2.right, 0.3f, LayerMask.GetMask("Wall"));
        touchingWallRight = raycastHit2D.collider != null ? true : false;

        raycastHit2D = Physics2D.Raycast(spriteRenderer.bounds.center, Vector2.up, 0.5f, LayerMask.GetMask("MonkeyBar"));
        monkeyBars = raycastHit2D.collider != null ? true : false;

        raycastHit2D = Physics2D.Raycast(spriteRenderer.bounds.center, Vector2.up, 0.01f, LayerMask.GetMask("Ladder"));
        climbingLadder = raycastHit2D.collider != null && arms >= 1 && legs >=1 ? true : false;

        if (
            arms == 2 && 
            legs == 2 &&
            (
                (touchingWallLeft && Input.GetAxis("Horizontal") < 0f) ||
                (touchingWallRight && Input.GetAxis("Horizontal") > 0f) ||
                (monkeyBars && Input.GetAxis("Vertical") > 0f)
            ) ||
            climbingLadder)
        { gravityModifier = 0f; } else { gravityModifier = 1f; }

        if (climbingLadder && Mathf.Abs(Input.GetAxis("Vertical")) > 0f)
        {
            rb2d.position += Vector2.up * Mathf.Sign(Input.GetAxis("Vertical")) * ladderMoveSpeed;
        } 

        if (Input.GetButtonDown("Jump") && 
            legs == 2 &&
            (grounded ||
            (touchingWallRight && Input.GetAxis("Horizontal") < 0f && arms == 2) ||
            (touchingWallLeft && Input.GetAxis("Horizontal") > 0f && arms == 2) ||
             climbingLadder))
        { velocity.y = jumpTakeOffSpeed; }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        if (Input.GetAxis("Vertical") < 0f || legs == 0)
        {
            animator.SetBool("crouching", true);
            capsuleCollider2D.offset = new Vector2(capsuleCollider2DOriginalOffset.x, 0.3f);
            capsuleCollider2D.size = new Vector2(capsuleCollider2DOriginalSize.x, 0.6f);
        }
        else
        {
            animator.SetBool("crouching", false);
            capsuleCollider2D.offset = capsuleCollider2DOriginalOffset;
            capsuleCollider2D.size = capsuleCollider2DOriginalSize;
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Trigger")
        {
            triggeredObject = collider2D.gameObject;
            gameController.GetComponent<InGameMenu>().PauseGameToggle();
            armOrLegDialogue.SetActive(true);
            if (arms == 0) { armButton.SetActive(false); }
            if (legs == 0) { legButton.SetActive(false); }
        }
    }

    private GameObject triggeredObject;

    public void loseLimb(int limb) {
        switch (limb) {
            case 0: arms--; break;
            case 1: legs--; break;
            default: break;
        }
        Debug.Assert(arms > -1, "Something's wrong with arms");
        Debug.Assert(legs > -1, "Something's wrong with legs");
        armOrLegDialogue.SetActive(false);
        Destroy(triggeredObject);
        gameController.GetComponent<InGameMenu>().PauseGameToggle();
    }

    public void OnGUI()
    {
    }
}