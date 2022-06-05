using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardManager : MonoBehaviour
{
    //This class will represent the logic representation of the game board

    public List<GameObject> activePawnsList; //Pawns on the screen
    private GameManager gameManager;

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

    private void OnEnable()
    {
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
    }

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
        IsWinnerDesided(gameBoard);
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

    public int GetSpotState(int coll , int row) //returns the current pawn at the spot
    {
        return gameBoard[coll, row];
    }

    #region Winner Checkers 
    public void IsWinnerDesided(int[,] physicalBoard)
    {
        //Check Vertical
        for (int coll = 0; coll < 7; coll++) 
        {
            for (int row = 0; row < 4; row++)
            {
                if(physicalBoard[coll,row] == 0) // no need to keep checking this path
                {
                    break;
                }
                Winner winner = CheckWinnerVerticly(coll, row, physicalBoard);
                if (winner != Winner.noPlayer)
                {
                    TriggerEndGame(winner);
                    return;
                }
            }
            
        }

        //Check Horizontaly
        for (int row = 0; row < 6; row++)
        {
            for (int coll = 0; coll < 4; coll++)
            {
                if (physicalBoard[coll, row] == 0) // no need to keep checking this path
                {
                    //nothing
                }
                else
                {
                    Winner winner = CheckWinnerHorizontaly(coll, row, physicalBoard);
                    if (winner != Winner.noPlayer)
                    {
                        TriggerEndGame(winner);
                        return;
                    }
                }
            }

        }

        //Check Diagnel left to right
        for (int coll = 0; coll < 4; coll++)
        {
            for (int row = 0; row < 3; row++)
            {
                if (physicalBoard[coll, row] == 0) // no need to keep checking this path
                {
                    //nothing
                }
                else
                {
                    Winner winner = CheckWinnerDiagonalLR(coll, row, physicalBoard);
                    if (winner != Winner.noPlayer)
                    {
                        TriggerEndGame(winner);
                        return;
                    }
                }
            }
        }


        //Check Diagnel right to left
        for (int coll = 6; coll >= 3; coll--)
        {
            for (int row = 0; row < 3; row++)
            {
                if (physicalBoard[coll, row] == 0) // no need to keep checking this path
                {
                    //nothing
                }
                else
                {
                    Winner winner = CheckWinnerDiagonalRL(coll, row, physicalBoard);
                    if (winner != Winner.noPlayer)
                    {
                        TriggerEndGame(winner);
                        return;
                    }
                }
            }
        }
        return;
    }

    //Gets a pawn on the board and check if he is part of a winning row
    private Winner CheckWinnerVerticly(int coll, int row, int[,] physicalBoard) 
    {
        int thisPawn = physicalBoard[coll,row]; //1 - player  2 - COM
        for (int i = row; i < row + 4; i++) //Check Verticaly 4 up
        {
            if(thisPawn != physicalBoard[coll, i]) //no match no use to do more checks
            {
                return Winner.noPlayer;
            }
        }

        Debug.Log("Winner from verticle");
        if(thisPawn == 1)
        {
            return Winner.player1;
        }
        else
        {
            return Winner.COM;
        }
    }

    private Winner CheckWinnerHorizontaly(int coll, int row, int[,] physicalBoard) 
    {
        int thisPawn = physicalBoard[coll, row]; //1 - player  2 - COM
        for (int i = coll; i < coll + 4; i++) //Check Horizontaly 4 to the right
        {
            if (thisPawn != physicalBoard[i, row]) //no match no use to do more checks
            {
                return Winner.noPlayer;
            }
        }

        Debug.Log("Winner from horizontal");
        if (thisPawn == 1)
        {
            return Winner.player1;
        }
        else
        {
            return Winner.COM;
        }
    }

    private Winner CheckWinnerDiagonalLR(int coll, int row, int[,] physicalBoard) 
    {
        int thisPawn = physicalBoard[coll, row]; //1 - player  2 - COM
        for (int i = coll; i < coll + 4; i++) //Check Left to Right diagonal 
        {
            //Debug.Log("Checking (" + i + "," + row + ")");
            if (thisPawn != physicalBoard[i, row]) //no match no use to do more checks
            {
                return Winner.noPlayer;
            }

            row++;
        }


        Debug.Log("Winner from DiagonalLR");
        if (thisPawn == 1)
        {
            return Winner.player1;
        }
        else
        {
            return Winner.COM;
        }
    }

    private Winner CheckWinnerDiagonalRL(int coll, int row, int[,] physicalBoard)
    {
        int thisPawn = physicalBoard[coll, row]; //1 - player  2 - COM
        for (int i = coll; i > coll - 4; i--) //Check Right to Left diagonal 
        {
            //Debug.Log("Checking (" + i + "," + row + ")");
            if (thisPawn != physicalBoard[i, row]) //no match no use to do more checks
            {
                return Winner.noPlayer;
            }

            row++;
        }
        Debug.Log("Winner from DiagonalRL");
        if (thisPawn == 1)
        {
            return Winner.player1;
        }
        else
        {
            return Winner.COM;
        }
    }

    private void TriggerEndGame(Winner winner)
    {
        if(winner == Winner.player1)
        {
            Debug.Log("Player Wins!!!");
        }
        else if (winner == Winner.COM)
        {
            Debug.Log("COM Wins!!!");
        }
        gameManager.EndGamePanel.SetActive(true);
        gameManager.winnerText.text = "Winner is: " + winner;
        GameManager.stopGame = true; //stop all interactions 
    }
    #endregion


    /* //Check Horizontal
        for (int i = 0; i < 6; i++) //for every coll
        {
            int currentPawnToCount = 0;
            int fourInRowCounter = 0;

            for (int j = 0; j < 3; j++) //for every winnable combo
            {
                int thisPawn = physicalBoard[i, j];
                if (thisPawn != 0) //not empty
                {
                    if (fourInRowCounter == 0) //Will trigger when its the first pawn checked
                    {
                        fourInRowCounter = 1;
                        currentPawnToCount = thisPawn; //We wanna see more of this pawn (1 , 2)
                    }

                    else
                    {
                        if (currentPawnToCount == thisPawn) //match the last one
                        {
                            fourInRowCounter++;
                            if (fourInRowCounter == 4)
                            {
                                //set winner and exit
                                if (currentPawnToCount == 1)
                                {
                                    Debug.Log("Winner player1");
                                    return Winner.player1;
                                }
                                else
                                {
                                    Debug.Log("Winner AI");
                                    return Winner.COM;
                                }
                            }
                        }

                        else // This will trigger when thisPawn broke the row
                        {
                            fourInRowCounter = 1;
                            currentPawnToCount = thisPawn; //We wanna see more of this pawn (1 , 2)
                        }
                    }
                }
                else //If spot is empty no use to checking the ones above
                {
                    return Winner.noPlayer;
                }
            }
        }*/
}
