using System;
using System.Collections;
using System.Collections.Generic;

public class Node
{
    private readonly int M_BOARD_COLUMS = 7;

    private List<Node> m_Node;
    private GameState m_gameState;

    public Node(GameState i_gameState)
    {
        m_Node = new List<Node>();
        m_gameState = i_gameState;
    }

    public GameState getGameState()
    {
        return m_gameState;
    }

    public Node getChildByIndex(int i_index)
    {
        if (i_index > M_BOARD_COLUMS)
            throw new Exception("index node out or range");

        return m_Node[i_index]; 
    }

    public bool isEmptyChilds()
    {
        return m_Node.Count == 0 ? true : false;
    }

    public void setNode(GameState i_gameState)
    {
        m_Node.Add(new Node(i_gameState));
    }
}
