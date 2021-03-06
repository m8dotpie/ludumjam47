﻿// using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using Random = System.Random;

public class Character : MonoBehaviour
{
    #region Initialization
    [SerializeField] private SpriteRenderer spriteRenderer;
    public float MinDamage
    {
        get => minDamage;
        private set => minDamage = value;
    }
    [SerializeField] private float minDamage = 100.0f;
    [SerializeField] private float movingSpeed = 2f;

    public float curDamage = 100.0f;
    private bool iFixit = false;
    
    
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
    

    public void Init(Cell.Direction baseDirection, Cell.Direction myDirection, Cell curCell)
    {
        this.baseDirection = baseDirection;
        this.myDirection = myDirection;
        this.curCell = curCell;
        transform.position = Vector3.MoveTowards(transform.position, curCell.reachMe, Time.deltaTime * movingSpeed);
        curDamage = minDamage;
        myState = State.Waiting;
    }

    private bool _isValidated = false;
    private float _lastDamage = 0f;
    private float DamageDeltaTime = 1f;

    public Character(bool isValidated)
    {
        _isValidated = isValidated;
    }

    #endregion
    
    // Start is called before the first frame update
    void OnEnable()
    {
        _lastDamage = 0f;
        curDamage = minDamage;
        myState = State.Waiting;
    }

    private void ValidateAndMoveToNextCell()
    {
        var newCell = curCell.NeighborCells[myDirection];
        if (!curCell.IsExist(myDirection) || !curCell.IsAvailable(myDirection))
        {
            Die();
            return;
        }

        if (newCell.myState == Cell.State.StartingPoint)
        {
            if (!newCell.IsExist(myDirection))
            {
                bool left = true;
                {
                    float rand = Random.Range(-1.0f, 1.0f);
                    if (rand > 0)
                    {
                        left = false;
                    }
                }
                if (left)
                {
                    myDirection = Cell.Direction.Left;
                }
                else
                {
                    myDirection = Cell.Direction.Right;
                }
            }
            if (!newCell.IsExist(Cell.Direction.Up) && baseDirection != Cell.Direction.Down)
            {
                baseDirection = Cell.Direction.Down;
                this.transform.Rotate(new Vector3(0, 0, 1), 180);
            }
            else
            {
                if (myDirection != Cell.Direction.Up)
                {
                    baseDirection = Cell.Direction.Up;
                    this.transform.Rotate(new Vector3(0, 0, 1), 180);
                }
            }

            if (!newCell.IsExist(Cell.Direction.Right))
            {
                if (myDirection == Cell.Direction.Right)
                {
                    myDirection = Cell.Direction.Left;
                }
            } else if (!newCell.IsExist(Cell.Direction.Left))
            {
                if (myDirection == Cell.Direction.Left)
                {
                    myDirection = Cell.Direction.Right;
                }
            }
        }

        _isValidated = true;
        curCell = newCell;
        myState = State.Mining;
    }

    private void Move()
    {
        if (curCell.myState != Cell.State.Transition && curCell.myState != Cell.State.StartingPoint)
        {
            return;
        }
        //print(myDirection);
        transform.position = Vector3.MoveTowards(transform.position, curCell.reachMe, Time.deltaTime * movingSpeed);
    }

    // Update is called once per frame
    private void Update()
    {
        switch (myState)
        {
            case State.Moving:
                if (Vector3.Distance(curCell.reachMe, transform.position) > 1e-3)
                {
                    Move();
                }
                else
                {
                    myState = State.Waiting;
                    _isValidated = false;
                }
                break;
            case State.Starting:
                break;
            case State.Mining:
                if (!_isValidated)
                {
                    ValidateAndMoveToNextCell();
                }

                if (curCell.myState != Cell.State.Transition && curCell.myState != Cell.State.StartingPoint)
                {
                    print("Time: " + Time.deltaTime + " damage: " + curDamage + " real damage: " + curDamage * Time.deltaTime);
                    curCell.GetDamage(this, curDamage * Time.deltaTime);
                }
                else if(curCell.myState == Cell.State.Transition || curCell.myState == Cell.State.StartingPoint)
                {
                    myState = State.Moving;
                }
                break;
            case State.Waiting:
                myState = State.Mining;
                break;
        }
    }

    public void Die()
    {
        print("DIE   "  + this);
        Time.timeScale = 0;
        
        //Destroy(this.gameObject);
    }
}
