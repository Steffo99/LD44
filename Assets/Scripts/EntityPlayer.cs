﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlMode {
    Move,
    Attack
}

public class EntityPlayer : Entity
{
    public Dictionary<InventoryItems, int> inventory; 
    public InventoryItems selectedItem;
    protected ControlMode controlMode;
    protected Animator animator;

    protected override void Start() {
        base.Start();
        animator = GetComponent<Animator>();
        controlMode = ControlMode.Move;
        //Init inventory
        inventory.Add(InventoryItems.HealthPotion, 0);
    }

    protected void Update()
    {
        CheckForControlModeChange();
        CheckForInventoryInput();
        if(controlMode == ControlMode.Move) CheckForMovementInput();
        if(controlMode == ControlMode.Attack) CheckForAttackInput();    
    }

    protected void CheckForInventoryInput() {
        if(Input.GetKeyDown(KeyCode.X)) {
            if(selectedItem == InventoryItems.Empty) {
                selectedItem = 0;
            }
            else {
                selectedItem += 1;
            }
        }
    }

    protected void CheckForControlModeChange() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            controlMode = ControlMode.Move;
            messageBar.Write("Control mode: Move", Color.cyan);
            animator.SetBool("IsWalking", true);
        }
        if(Input.GetKeyDown(KeyCode.A)) {
            controlMode = ControlMode.Attack;
            messageBar.Write("Control mode: Attack", Color.cyan);
            animator.SetBool("IsWalking", false);
        }
    }

    protected void CheckForAttackInput() {
        bool hasAttacked = false;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            hasAttacked = GetComponent<PlayerAttack>().Attack(MapPosition + Vector2Int.left);
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            hasAttacked = GetComponent<PlayerAttack>().Attack(MapPosition + Vector2Int.right);
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            hasAttacked = GetComponent<PlayerAttack>().Attack(MapPosition + Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            hasAttacked = GetComponent<PlayerAttack>().Attack(MapPosition + Vector2Int.down);
        }
        if(hasAttacked) {
            //Turn happens!
            turnHandler.OnTurn();
        }
    }

    protected void CheckForMovementInput()
    {
        bool hasMoved = false;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (map.CanMoveTo(MapPosition + Vector2Int.left)) {
                transform.Translate(Vector3.left);
                hasMoved = true;
                spriteRenderer.flipX = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (map.CanMoveTo(MapPosition + Vector2Int.right)) {
                transform.Translate(Vector3.right);
                hasMoved = true;
                spriteRenderer.flipX = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (map.CanMoveTo(MapPosition + Vector2Int.up)) {
                transform.Translate(Vector3.up);
                hasMoved = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (map.CanMoveTo(MapPosition + Vector2Int.down)) {
                transform.Translate(Vector3.down);
                hasMoved = true;
            }
        }
        if(hasMoved) {
            //Check for pickuppable items
            List<Entity> entities = turnHandler.GetEntitiesAtPosition(MapPosition);
            foreach(Entity entity in entities) {
                if(entity is EntityItem) {
                    EntityItem item = entity as EntityItem;
                    item.OnPickup(this);
                }
            }
            //Turn happens!
            turnHandler.OnTurn();
        }
    }
}