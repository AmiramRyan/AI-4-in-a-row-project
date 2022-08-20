using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    private readonly int M_ROW_SIZE = 6;

    private int[,] m_gameBoard;
    private int m_startingColl = 0;
    private int m_startingRow = 0;
    private int m_huyristicValue = 999;
    private int m_pawnToCheck = 0;
    public bool blocked = false; //if true disregard this state best h value (no chanse to increase the value from here)

    public GameState(int[,] i_gameBoard, int i_startingColl, int i_startingRow , int i_pawnToCheck)
    {
        m_gameBoard = i_gameBoard;
        m_startingColl = i_startingColl;
        m_startingRow = i_startingRow;
        m_pawnToCheck = i_pawnToCheck;
    }

    public int[,] getGameBoard()
    {
        return m_gameBoard;
    }

    public int getIndex(){
        return m_startingColl;
    }

    public int getHuyristicValue()
    {
        return m_huyristicValue;
    }

    public void setHuyristicValue(int i_huyristic)
    {
        m_huyristicValue = i_huyristic;
    }

    public void setGameBoard(int[,] newGameBoard)
    {
        m_gameBoard = newGameBoard;
    }

    public void calcHuyristicValue()
    {
        m_huyristicValue = Huyristic.Evaluation(m_gameBoard, m_startingColl, m_startingRow , m_pawnToCheck);
        if(m_huyristicValue != 4 && m_gameBoard[m_startingColl,5] != 0) //Not winning move and this coll is now full
        {
            m_huyristicValue = 1000;
        }
    }

}
