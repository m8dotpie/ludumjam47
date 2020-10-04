﻿// using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using Random = System.Random;

public class Character : MonoBehaviour
{
    #region Initialization
    [SerializeField] private SpriteRenderer spriteRenderer;
    public float MinMovingSpeed
    {
        get => minMiningSpeed;
        private set => minMiningSpeed = value;
    }
    [SerializeField] private float minMiningSpeed = 2f;
    [SerializeField] private float movingSpeed = 2.0f;

    public float curDamage;
    
    public Cell.Direction myDirection;
    public Cell.Direction baseDirection;
    public enum State
    {
        Moving,
        Starting,
        Mining,
        Waiting
    }

    public State myState;
    
    public Cell curCell;

    public bool immortal;
    #endregion
    
    private void onButtonEntered ()
    {

    }
    // Start is called before the first frame update
    private void Start()
    {
        curDamage = minMiningSpeed;
        movingSpeed = minMiningSpeed;
        myState = State.Waiting;
    }

    private void ValidateAndMoveToNextCell()
    {
        if (!curCell.IsExist(myDirection) || !curCell.IsAvailable(myDirection))
        {
            print("Die");
            Die();
        }
        
        if (curCell.myState == Cell.State.StartingPoint)
        {
            switch (baseDirection)
            {
                //change the BaseDirection
                case Cell.Direction.Up:
                    baseDirection = Cell.Direction.Down;
                    break;
                case Cell.Direction.Down:
                    baseDirection = Cell.Direction.Up;
                    break;
            }

            //turn back from borders
            if (!curCell.IsExist(Cell.Direction.Left))
            {
                myDirection = Cell.Direction.Right;
            }
            else if (!curCell.IsExist(Cell.Direction.Right))
            {
                myDirection = Cell.Direction.Left;
            }
        }//if it's not a starting point
        else
        {
            curCell.myState = Cell.State.Deadly;
        }

        if (curCell.IsExist(myDirection) && curCell.IsAvailable(myDirection))
        {
            curCell = curCell.NeighborCells[myDirection];
        }
    }

    private void Mine()
    {
        ValidateAndMoveToNextCell();
        while (curCell.myState != Cell.State.Transition && curCell.myState != Cell.State.StartingPoint)
        {
            curCell.myState = Cell.State.Transition;
            curCell.GetDamage(this);
        }
        
        // Start mining.
        myState = State.Moving;
        Move();
    }
    
    private void Move()
    {
        print(myDirection);
        transform.Translate(-1000, -1000, -1000);
        myState = State.Waiting;
        transform.position = Vector3.MoveTowards(transform.position, curCell.transform.position, Time.deltaTime*movingSpeed);
    }

    // Update is called once per frame
    private void Update()
    {
        switch (myState)
        {
            case State.Moving:
                break;
            case State.Starting:
                break;
            case State.Mining:
                break;
            case State.Waiting:
                myState = State.Mining;
                Mine();
                break;
        }
    }

    private void Die()
    {
        print("DIE");
        Destroy(this);
    }
}
