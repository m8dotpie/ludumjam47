﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]private GlobalController globalController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {       

            if (globalController.char1.baseDirection == Cell.Direction.Up)
            {
                globalController.char1.myDirection = Cell.Direction.Up;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {    
            globalController.char1.myDirection = Cell.Direction.Left;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {    print("S OUTER IF");
            if (globalController.char1.baseDirection == Cell.Direction.Down)
            {
                print("s(Inner IF)");
                globalController.char1.myDirection = Cell.Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {    
            globalController.char1.myDirection = Cell.Direction.Right;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {    
            if (globalController.char1.baseDirection == Cell.Direction.Up)
            {
                globalController.char1.myDirection = Cell.Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {    
            if (globalController.char1.baseDirection == Cell.Direction.Down)
            {
                globalController.char1.myDirection = Cell.Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {    
            globalController.char1.myDirection = Cell.Direction.Left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {    
            globalController.char1.myDirection = Cell.Direction.Right;
        }
    }
}
