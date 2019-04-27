using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    [SerializeField]
    private bool touchingWallLeft = false;
    [SerializeField]
    private bool touchingWallRight = false;

    [SerializeField]
    private bool monketBars = false;

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");
        RaycastHit2D raycastHit2D = Physics2D.Raycast(spriteRenderer.bounds.center, Vector2.left, 0.3f, LayerMask.GetMask("Wall"));
        if (raycastHit2D.collider != null) {
            touchingWallLeft = true;
            }
        else {
            touchingWallLeft = false;
            raycastHit2D = Physics2D.Raycast(spriteRenderer.bounds.center, Vector2.right, 0.3f, LayerMask.GetMask("Wall"));
            if (raycastHit2D.collider != null)
            {
                touchingWallRight = true;
            }
            else { touchingWallRight = false; }
        }

        raycastHit2D = Physics2D.Raycast(spriteRenderer.bounds.center, Vector2.up, 0.5f, LayerMask.GetMask("MonkeyBar"));
        if (raycastHit2D.collider != null)
        {
            monketBars = true;
        }
        else { monketBars = false; }


        if (
            (touchingWallLeft && Input.GetAxis("Horizontal") < 0f) ||
            (touchingWallRight && Input.GetAxis("Horizontal") > 0f) ||
            (monketBars && Input.GetAxis("Vertical") > 0f))
        {
            gravityModifier = 0f;
        }
        else
        {
            gravityModifier = 1f;
        }

        if (
            Input.GetButtonDown("Jump") && 
            (grounded ||
            (touchingWallRight && Input.GetAxis("Horizontal") < 0f) ||
            (touchingWallLeft && Input.GetAxis("Horizontal") > 0f)))
        {
            velocity.y = jumpTakeOffSpeed;
        }
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

        if (Input.GetAxis("Vertical") < 0f)
        {
            animator.SetBool("crouching", true);
        }
        else { animator.SetBool("crouching", false); }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }
}