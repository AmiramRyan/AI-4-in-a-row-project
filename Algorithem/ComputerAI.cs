using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerAI : MonoBehaviour
{
    private GameManager gameManager;

    private void OnEnable()
    {
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
    }
    //for debuging

    public void PlayTurn()
    {
        if (!GameManager.stopGame)
        {
            StartCoroutine(PlayTurnCo());
        }
    }

    public int PickRandomColl()
    {
        int rnd = Random.Range(0, 5); //this is the coll number
        if (gameManager.boardManager.GetSpotState(rnd, 5) != 0) //no more room in this coll
        {
            int counter = 0; //maximum 7 checks
            while (counter <= 7)
            {
                if (rnd == 5) //loopAround check
                {
                    rnd = 0;
                    if (gameManager.boardManager.GetSpotState(rnd, 5) != 0)
                    {
                        rnd++;
                    }
                    else
                    {
                        return rnd;
                    }
                }
                else
                {
                    if (gameManager.boardManager.GetSpotState(rnd, 5) != 0)
                    {
                        rnd++;
                    }
                    else
                    {
                        return rnd;
                    }
                }
                counter++;
            }
        }
        return rnd;
    }

    public IEnumerator PlayTurnCo()
    {
        yield return new WaitForSeconds(2);
        int coll = PickRandomColl();

        // -- testing logic here


       // print("Value of the cell" + gameManager.boardManager.gameBoard[0, 0]);
        //Node node = new Node(createGameState(gameManager.boardManager.gameBoard, 0));
        //createTree(gameManager.boardManager.gameBoard, node, 2);

       

    
        // -- end testing logic here

        gameManager.SpawnManager.SetColl(coll); //spawn AI pawn on screen and on the logic board
        //let the player play
        gameManager.playersTurn = true;
        GameManager.ReadyUpSpawns?.Invoke();
    }

    // -- testing function

    private void createTree(int[,] i_gameBoard, Node i_node, int i_n)
    {
        if (i_n == 1)
            return;

        for (int i = 0; i < 2; i++) // deep x2
        {
            for (int j = 0; j < 7; j++) // for each node
            {


                i_node.setNode(createGameState(i_gameBoard, j));


                createTree(i_gameBoard, i_node.getChildByIndex(j), i_n - 1);
            }
        }
        return;
    }

    // creating by even or odd - temp
    private GameState createGameState(int[,] i_gameBoard, int i_n)
    {
        GameState gameState = new GameState(i_gameBoard);

        if(i_n % 2 == 0) // for player, otherwise for Bot
        {
            gameState.setGameBoardByIndexCol(i_n, 1);
        }
        else
        {
            gameState.setGameBoardByIndexCol(i_n, 2);
        }
        gameState.calcHuyristicValue();

        return gameState;
    }
 
        
    private void printTestNode(Node i_node, int i_n)
    {
        if (i_n == 0)
            return;

        for (int i = 0; i < 2; i++) // deep x2
        {
            for (int j = 0; j < 7; j++) // for each node
            {
                print("i : " + i + " j : " + j + " huyristic" + i_node.getGameState().getHuyristicValue());
                printTestNode(i_node.getChildByIndex(j), i_n - 1);
            }
        }
        return;
    }
    // -- end testing function 
}
