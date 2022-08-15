using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    private readonly int M_ROW_SIZE = 6;

    private int[,] m_gameBoard;
    private int m_currentIndex = 0;
    private int m_huyristicValue = 999;
    public bool blocked = false; //if true disregard this state best h value (no chanse to increase the value from here)

    public GameState(int[,] i_gameBoard, int i_currentIndex)
    {
        m_gameBoard = i_gameBoard;
        m_currentIndex = i_currentIndex;
    }

    public int[,] getGameBoard()
    {
        return m_gameBoard;
    }

    public int getIndex(){
        return m_currentIndex;
    }

    public int getHuyristicValue()
    {
        return m_huyristicValue;
    }

    public void setHuyristicValue(int i_huyristic)
    {
        m_huyristicValue = i_huyristic;
    }
    /*public int[,] setGameBoardByIndexCol(int i_indexCol, int i_whichPlayer, int[,] gameBoard) //This function adds a pawn to the indexCol of type whichPlayer
    {
        int[,] virtualGameBoard = gameBoard;
        if (gameBoard[i_indexCol, 5] != 0) //Coll is full dont add 
        { 
            Debug.Log("Coll: " + i_indexCol + " is Full, not adding pawn");
            return virtualGameBoard;
        }

        for (int i = 0; i < M_ROW_SIZE; i++)
        {
            if (gameBoard[i_indexCol, i] == 0) //if its an empty spot
            {
                virtualGameBoard[i_indexCol, i] = i_whichPlayer; //add virtual pawn and end func
                return virtualGameBoard;
            }
        }
        return virtualGameBoard;
    }*/

    public void setGameBoard(int[,] newGameBoard)
    {
        m_gameBoard = newGameBoard;
    }

    public void calcHuyristicValue()
    {
        m_huyristicValue = Huyristic.Evaluation(m_gameBoard, m_currentIndex);
    }

}
