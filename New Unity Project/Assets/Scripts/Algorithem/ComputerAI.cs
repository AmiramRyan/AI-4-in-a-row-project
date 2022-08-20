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

    private Node createTree(int[,] original_gBoard) //Projected moves tree
    {
        //initialize
        GameState current = new GameState(original_gBoard, 0, 0 ,0);
        Node projectedR = new Node(current); //tree ill be stored here
        
        
        for (int i = 0; i < 7; i++) //Create parents
        {
            //Virtualy insert pawn in coll i
            int[,] original_gameBoard = new int[7,6];
            original_gameBoard = Copy2dArr(original_gameBoard, original_gBoard);
            //Debug.Log("----Parent " + i + " ----");
            int[,] temp = new int[7,6];
            temp = setGameBoardByIndexCol(i, 2, original_gameBoard);
            //Find the row
            int currRow = 0;
            for (int j = 5; j >= 0; j--)
            {
                if(temp[i,j] == 2){
                    currRow = j;
                    break;
                }
            }

            //initialize node with parent board
            GameState parentGameState = new GameState(temp,i, currRow , 2);
            Node parentNode = new Node(parentGameState);
            //Applying huristic value to base case (winner is desied with current move)
            parentNode.getGameState().calcHuyristicValue();
            //PrintGameState(parentGameState);
            //connect to parent node
            projectedR.setNode(parentNode);
            for (int j = 0; j < 7; j++)//Create child nodes
            {
                 int[,] child_gameBoard = new int[7,6];
                 child_gameBoard = Copy2dArr(child_gameBoard, parentNode.getGameState().getGameBoard());
                 //Debug.Log("------Child " + j + " Of Parent " + i + " ------");
                 int[,] tempC = new int[7,6];
                 tempC = setGameBoardByIndexCol(j, 1, child_gameBoard);
                 int currRowC = 0;
                for (int row = 5; row >= 0; row--)
                {
                    if(tempC[j,row] == 1){
                        currRowC = row;
                        break;
                    }
                }
                //initialize node with parent board
                GameState childGameState = new GameState(tempC,j, currRowC , 1);
                Node childNode = new Node(childGameState);
                //Change parent Huyristic value if that move can lead to a player win
                childNode.getGameState().calcHuyristicValue();
                //PrintGameState(childGameState);
                //connect to parent node
                parentNode.setNode(childNode);
                if(childNode.getGameState().getHuyristicValue() >= 4 & parentNode.getGameState().getHuyristicValue() != 4){ //Player win on next move and COM dosent
                    parentNode.getGameState().setHuyristicValue(-995); //Innsures the move that leads to player win wont be made (Blocking is a side effect here)
                }
            }
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
        GameState temp = new GameState(gameManager.boardManager.gameBoard,0 ,0 ,0);
        Node root = new Node(temp);
        Node projectedRoot = createTree(root.getGameState().getGameBoard()); //Create the tree

        //explore the child nodes and return the index of the best value
        int collToPlay = PickRandomColl();
        int curr_best_h_value = 999;
        int[,] potentialMoves = new int[7,1];
        //Register Huyristic results
        for (int i = 0; i < 7; i++) //Which coll is better to play?
        {
            GameState projectedState = projectedRoot.getChildByIndex(i).getGameState();
            int n_h_value = projectedState.getHuyristicValue();
            potentialMoves[i,0] = 4 - n_h_value; //(where to play, h_value of that play)
            if(potentialMoves[i,0] == 0){ //COM WIN no need to check 
                return i;
            }
        }

        //Get choosen Coll to play by choosing best Huyristic value
        int tempColl = 0;
        if(curr_best_h_value > potentialMoves[3,0])
        {
            collToPlay = 3;
            curr_best_h_value = potentialMoves[3,0];
        }

        tempColl = Compare2CollHu(2,4,potentialMoves);
        if(curr_best_h_value > potentialMoves[tempColl,0])
        {
            collToPlay =  tempColl;
            curr_best_h_value = potentialMoves[tempColl,0];
        }

        tempColl = Compare2CollHu(1,5,potentialMoves);
        if(curr_best_h_value > potentialMoves[tempColl,0])
        {
            collToPlay =  tempColl;
            curr_best_h_value = potentialMoves[tempColl,0];
        }

        tempColl = Compare2CollHu(0,6,potentialMoves);
        if(curr_best_h_value > potentialMoves[tempColl,0])
        {
            collToPlay =  tempColl;
            curr_best_h_value = potentialMoves[tempColl,0];
        }

        //print results
        Debug.Log("Choose to play: " + collToPlay + " For H Value of: " + curr_best_h_value);
        return collToPlay; 
    }
    
    private int Compare2CollHu(int coll1, int coll2 , int[,] potentialMoves)
    {
        int val1 =  potentialMoves[coll1,0];
        int val2 =  potentialMoves[coll2,0];
        if(val1 > val2){
            return coll2;
        }
        else{
            return coll1;
        }
        
    }
}
