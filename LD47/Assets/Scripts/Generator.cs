﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private float cellSize = 1f;
    public GlobalController globalController;

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject characterPrefab;
    
    public int rows, columns;
    public Sprite startCellSprite;
    public Character[] characters;
    public Cell[,] CellMatrix;

    private void OnEnable()
    {
        GenerateMatrix();
        SpawnChar();
        globalController.char1 = characters[0];
        globalController.char2 = characters[1];
    }

    void GenerateMatrix()
    {
        CellMatrix = new Cell[rows + 2, columns];
        //just for starting el-s
        for (int j = 0; j < columns; j++)
        {
            var position = this.transform.position;
            Vector3 spawnPoint = position + new Vector3(j * cellSize, 0, 0);
            CellMatrix[0, j] = Instantiate(cellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            CellMatrix[0, j].MyState = Cell.State.StartingPoint;
            //Debug.Log("" + 0 + " " + j + " " + CellMatrix[0, j]);

            spawnPoint = position + new Vector3(j * cellSize, -cellSize * (rows + 1), 0);
            CellMatrix[rows + 1, j] = Instantiate(cellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
            CellMatrix[rows + 1, j].MyState = Cell.State.StartingPoint;
            //Debug.Log("" + (rows + 1) + " " + j + " " + CellMatrix[rows + 1, j]);

            CellMatrix[rows + 1, j].GetComponent<SpriteRenderer>().sprite = startCellSprite;
            CellMatrix[0, j].GetComponent<SpriteRenderer>().sprite = startCellSprite;
        }
        
        for (int i = 1; i < rows + 1; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 spawnPoint = this.transform.position + new Vector3(j * cellSize, -i * cellSize, 0);
                CellMatrix[i, j] = Instantiate(cellPrefab, spawnPoint, Quaternion.identity).GetComponent<Cell>();
                CellMatrix[i, j].MyState = Cell.State.Surface;
                //Debug.Log("" + i + " " + j + " " + CellMatrix[i, j]);
            }
        }

        for (int i_1 = 0; i_1 < rows + 2; i_1++) // writing nighbours for each cell
            for (int i_2 = 0; i_2 < columns; i_2++)
            {
                //Debug.Log("Writing neighbours" + i_1 + " " + i_2 + " " + CellMatrix[i_1, i_2]);
                if (i_1 > 0)
                    CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.Up] = CellMatrix[i_1 - 1, i_2];
                if (i_2 + 1 < columns)
                    CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.Right] = CellMatrix[i_1, i_2 + 1];
                if (i_1 + 1 < rows)
                    CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.Down] = CellMatrix[i_1 + 1, i_2];
                if (i_2 > 0)
                    CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.Left] = CellMatrix[i_1, i_2 - 1];

                if (i_1 > 0 && i_2 > 0)
                    CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.UpLeft] = CellMatrix[i_1 - 1, i_2 - 1];
                if (i_1 > 0 && i_2 + 1 < columns)
                    CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.UpRight] = CellMatrix[i_1 - 1, i_2 + 1];
                if (i_1 + 1 < rows && i_2 + 1 < columns)
                    CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.DownRight] = CellMatrix[i_1 + 1, i_2 + 1];
                if (i_1 + 1 < rows && i_2 > 0)
                    CellMatrix[i_1, i_2].NeighborCells[Cell.Direction.DownLeft] = CellMatrix[i_1 + 1, i_2 - 1];
                //Debug.Log(CellMatrix[i_1, i_2].NeighborCells);
            }
    }
    
    void SpawnChar()
    {
        characters = new Character[2];
        int pointA = UnityEngine.Random.Range(0, columns);
        int pointB = UnityEngine.Random.Range(0, columns);
        Vector3 spawnA = this.transform.position + new Vector3(pointA * cellSize, 0, 0);
        Vector3 spawnB = this.transform.position +  new Vector3(pointB *cellSize, -cellSize * (rows + 1),0) ; 
        characters[0] = Instantiate(characterPrefab,spawnA , Quaternion.identity).GetComponent<Character>();

        characters[1] = Instantiate(characterPrefab,spawnB , Quaternion.identity).GetComponent<Character>();
        characters[1].BaseDirection = Cell.Direction.Up;
        characters[1].MyDirection = Cell.Direction.Up;
        characters[1].curCell = CellMatrix[rows + 1, pointB];
        
    }
}