using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEyes : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public MovementGhosts movement { get; private set; }
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.movement = GetComponentInParent<MovementGhosts>();
    }

    private void Update()
    {
        if (this.movement.direction == Vector2.up)
        {
            this.spriteRenderer.sprite = this.up;
        }
        if (this.movement.direction == Vector2.down)
        {
            this.spriteRenderer.sprite = this.down;
        }
        if (this.movement.direction == Vector2.left)
        {
            this.spriteRenderer.sprite = this.left;
        }
        if (this.movement.direction == Vector2.right)
        {
            this.spriteRenderer.sprite = this.right;
        }
    }
}
