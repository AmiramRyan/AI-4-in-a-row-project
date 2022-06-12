using System.Collections;
using System.Collections.Generic;

public class GameState 
{
    private readonly int M_ROW_SIZE = 6;

    private int[,] m_gameBoard;
    private int m_huyristicValue = -1;

    public GameState(int[,] i_gameBoard)
    {
        m_gameBoard = i_gameBoard;
    }

    public int[,] getGameBoard()
    {
        return m_gameBoard;
    }

    public int getHuyristicValue()
    {
        return m_huyristicValue;
    }
    public void setGameBoardByIndexCol(int i_indexCol, int i_whichPlayer)
    {
        if (m_gameBoard[0, i_indexCol] != 0)
            return;

        for(int i = 0; i < M_ROW_SIZE; i++)
        {
            if(m_gameBoard[i, i_indexCol] != 0)
            {
                m_gameBoard[i - 1, i_indexCol] = i_whichPlayer;
                return;
            }
        }

        m_gameBoard[M_ROW_SIZE, i_indexCol] = i_whichPlayer;
    }

    public void calcHuyristicValue()
    {
        m_huyristicValue = Huyristic.Evaluation(m_gameBoard);
    }
}
