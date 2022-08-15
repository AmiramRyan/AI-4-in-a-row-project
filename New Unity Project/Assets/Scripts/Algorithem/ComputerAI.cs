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
        if (gameManager.boardManager.GetSpotState(rnd,5) != 0) //no more room in this coll
        {
            int counter = 0; //maximum 7 checks
            while(counter <= 7)
            {
                if(rnd == 5) //loopAround check
                {
                    rnd = 0;
                    if(gameManager.boardManager.GetSpotState(rnd, 5) != 0)
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
        //int coll = PickRandomColl();
        int coll = CreateAndPrintTrees();
        gameManager.SpawnManager.SetColl(coll); //spawn AI pawn on screen and on the logic board
        //let the player play
        gameManager.playersTurn = true;
        GameManager.ReadyUpSpawns?.Invoke();
    }

    /*private void createTree(int[,] i_gameBoard, Node i_node, int i_n) //i_n is what type of pawn we are putting in  1/2
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
    }*/

    private Node createTree(int[,] original_gBoard) //Projected moves tree
    {
        //initialize
        GameState current = new GameState(original_gBoard, 0);
        Node projectedR = new Node(current); //tree ill be stored here
        
        
        for (int i = 0; i < 7; i++) //Create parents
        {
            //Virtualy insert pawn in coll i
            int[,] original_gameBoard = new int[7,6];
            original_gameBoard = Copy2dArr(original_gameBoard, original_gBoard);
            Debug.Log("----Parent " + i + " ----");
            int[,] temp = new int[7,6];
            temp = setGameBoardByIndexCol(i, 2, original_gameBoard);
           
            //initialize node with parent board
            GameState parentGameState = new GameState(temp,i);
            Node parentNode = new Node(parentGameState);
            //Applying huristic value to base case (winner is desied with current move)
            parentNode.getGameState().calcHuyristicValue();
            PrintGameState(parentGameState);
            //connect to parent node
            projectedR.setNode(parentNode);
        }

        return projectedR;
    }

    public int[,] Copy2dArr(int[,] copyTo, int[,] copyFrom){
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                copyTo[i,j] = copyFrom[i,j];
            }
        }
        return copyTo;
    }

    private void printTestNode(Node root)
    {
        for (int i = 0; i < 7; i++) 
        {
            for (int j = 0; j < 7; j++) 
            {
                print("Parent node num: " + (i+1) + " Child node num: " + (j+1) + " ,Huyristic: " + root.getChildByIndex(j).getGameState().getHuyristicValue());
                //PrintGameState(root.getChildByIndex(j).getGameState());
            }
        }
        return;
    }

    private void PrintGameState(GameState state) //For Debuging
    {
        for (int i = 5; i >= 0; i--)
        {
            print("| " + state.getGameBoard()[0, i] + " | " + state.getGameBoard()[1, i] + " | " + state.getGameBoard()[2, i] + " | " + state.getGameBoard()[3, i] + " | " + state.getGameBoard()[4, i] + " | " + state.getGameBoard()[5, i] + " | " + state.getGameBoard()[6, i] + " |");
        }
        print("-------------------------------------");
    }

    private void PrintGameBoard(int[,] gameBoard){
        for (int i = 5; i >= 0; i--)
        {
            print("| " + gameBoard[0, i] + " | " + gameBoard[1, i] + " | " + gameBoard[2, i] + " | " + gameBoard[3, i] + " | " + gameBoard[4, i] + " | " + gameBoard[5, i] + " | " + gameBoard[6, i] + " |");
        }
        print("-------------------------------------");
    }

    public int[,] setGameBoardByIndexCol(int i_indexCol, int i_whichPlayer, int[,] gameBoard) //This function adds a pawn to the indexCol of type whichPlayer
    {
        int[,] virtualGameBoard = gameBoard;
        if (gameBoard[i_indexCol, 5] != 0) //Coll is full dont add 
        { 
            Debug.Log("Coll: " + i_indexCol + " is Full, not adding pawn");
            return virtualGameBoard;
        }

        for (int i = 0; i < 6; i++)
        {
            if (gameBoard[i_indexCol, i] == 0) //if its an empty spot
            {
                virtualGameBoard[i_indexCol, i] = i_whichPlayer; //add virtual pawn and end func
                return virtualGameBoard;
            }
        }
        return virtualGameBoard;
    }

    //Desied where to play
    public int CreateAndPrintTrees()
    {
        GameState temp = new GameState(gameManager.boardManager.gameBoard,0);
        Node root = new Node(temp);
        Node projectedRoot = createTree(root.getGameState().getGameBoard());

        //explore the child nodes and return the index of the best value
        int collToPlay = PickRandomColl();
        int curr_best_h_value = 999;
        for (int i = 0; i < 7; i++)
        {
            GameState projectedState = projectedRoot.getChildByIndex(i).getGameState();
            if(!projectedState.blocked)
            {
                int n_h_value = projectedState.getHuyristicValue();
                if((4 - n_h_value) == 0)//winning move
                {
                    curr_best_h_value = n_h_value;
                    collToPlay = projectedState.getIndex();
                    break;
                }
                
                else if((4 - n_h_value) < curr_best_h_value)
                {
                    if((4 - n_h_value) == 3) //trivial result
                    {
                        collToPlay = PickRandomColl();
                    }
                    if(gameManager.boardManager.GetNextAvailableRow(collToPlay) != 999) //if coll is full skip this check
                    {
                        curr_best_h_value = (4 - n_h_value);
                        collToPlay = projectedState.getIndex();
                    }
                }
            }
        }

        //print results
        Debug.Log("Choose to play: " + collToPlay + " For H Value of: " + curr_best_h_value);
        return collToPlay; 
    }

}
