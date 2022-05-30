using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardManager : MonoBehaviour
{
    //This class will represent the logic representation of the game board

    public List<GameObject> activePawnsList; //Pawns on the screen

    public int[,] gameBoard; //6X7 6rows 7 collumns
    /*Example:
     * 0 0 0 0 0 0 0
     * 0 0 0 0 0 0 0
     * 0 0 0 0 0 1 2
     * 0 0 0 1 0 1 1
     * 2 2 2 1 0 1 2
     * 2 1 1 2 2 1 1
     * Where 0 = Empty , 1 = Player(Red) , 2 = Computer(Yellow)
     */

    void Start()
    {
        gameBoard = new int[7, 6];
        //ResetBoard
        // ResetGameBoard();
    }

    private void ResetGameBoard() 
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                gameBoard[i,j]= 0;
            }
        }
    }

    public void AddPawn(bool isPlayerPawn, int coll /*, GameObject thisPawn*/) //Add the spawned pawn to the logic array and ActivePawnsList
    {
        //activePawnsList.Add(thisPawn);
        int rowToSpawnIn = GetNextAvailableRow(coll);
        if(isPlayerPawn && rowToSpawnIn != 999) //Spawn player Pawn
        {
            gameBoard[coll, rowToSpawnIn] = 1;
        }
        else if(!isPlayerPawn && rowToSpawnIn != 999) //Spawn Computer Pawn
        {
            gameBoard[coll, rowToSpawnIn] = 2;
        }
        PrintArr();
    }

    private int GetNextAvailableRow(int coll)
    {
        for (int i = 0; i < 6; i++)
        {
            if(gameBoard[coll,i] == 0) //empty spot
            {
                return i;
            }
        }
        Debug.LogError("This Coll is full!");
        return 999;
    }

    private void PrintArr() //For Debuging
    {
        for (int i = 5; i >= 0; i--)
        {
            print("| " + gameBoard[0, i] + " | " + gameBoard[1, i] + " | " + gameBoard[2, i] + " | " + gameBoard[3, i] + " | " + gameBoard[4, i] + " | " + gameBoard[5, i] + " | " + gameBoard[6, i] + " |");
        }
        print("-------------------------------------");
    }
}
