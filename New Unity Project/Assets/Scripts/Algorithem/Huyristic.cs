using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public static class Huyristic
{
    public static int Evaluation(int[,] i_gameBoard, int i_startingColl, int i_startingRow ,int pawnToCheck)
    {
        return GetMaxValue(i_gameBoard, i_startingColl, i_startingRow , pawnToCheck);
    }

    private static int GetMaxValue(int[,] i_gameBoard, int i_startingColl, int i_startingRow,int pawnToCheck)
    {
        int o_maxValue = 0;

        int rowValue = checkRowSeq(i_gameBoard, i_startingColl, i_startingRow, pawnToCheck);
        //Debug.Log("h_horizontal: " + rowValue);

        int colValue = checkCollSeq(i_gameBoard, i_startingColl, i_startingRow, pawnToCheck);
        //Debug.Log("h_vertical: " + colValue);

        int diagValue = checkDiag(i_gameBoard, i_startingColl, i_startingRow, pawnToCheck);
        //Debug.Log("h_diag: " + diagValue);

        o_maxValue = Math.Max(rowValue, colValue);
        o_maxValue = Math.Max(o_maxValue, diagValue);
        //Debug.Log("h_max: " + o_maxValue);
        return o_maxValue;
    }

    private static int CalcRowSequ(int[,] i_gameBoard, int i_CurrentIndex) //check up and down
    {
        //Check Vertical
        int o_value = 0;
        int curr_value = 0;
        bool rowStreak = false;
        /*for (int coll = 0; coll < 7; coll++) 
        {
            rowStreak = false;
            for (int row = 0; row < 6; row++)
            {
                if(i_gameBoard[coll,row] == 2) //com pawn
                {
                    if(rowStreak){
                        curr_value++;
                        if(o_value < curr_value){ 
                            o_value = curr_value;
                        }
                    }
                    else
                    {
                        if(o_value < curr_value){ 
                            o_value = curr_value;
                        }
                        rowStreak = true;
                        curr_value = 1; //reset
                    }
                }else{rowStreak = false;}
            }
            if(o_value < curr_value)
            {
                o_value = curr_value;
            }
        }*/
        return o_value;
    }

    private static int CalcColSequ(int[,] i_gameBoard, int i_CurrentIndex) //check left to right
    {
        int o_value = 0;
        int curr_value = 0;
        bool rowStreak = false;

        /*for (int i = i_CurrentIndex - 1, j = i_CurrentIndex + 1; i > i_CurrentIndex - 3; i--, j++)
        {
            try
            {
                if (i_gameBoard[i, i_CurrentIndex] == 2)
                {
                    o_value++;
                }
            }
            catch (ArgumentOutOfRangeException e) { }
            try
            {
                if (i_gameBoard[j, i_CurrentIndex] == 2)
                {
                    o_value++;
                }
            }
            catch (ArgumentOutOfRangeException e) { }
        }*/

         for (int row = 0; row < 6; row++)
        {
            rowStreak = false;
            for (int coll = 0; coll < 7; coll++)
            {
                if(i_gameBoard[coll,row] == 2) //com pawn
                {
                    if(rowStreak){
                        curr_value++;
                        if(o_value < curr_value){
                            o_value = curr_value;
                        }
                    }
                    else
                    {
                        if(o_value < curr_value){
                            o_value = curr_value;
                        }
                        rowStreak = true;
                        curr_value = 1; //reset
                    }
                }else{rowStreak = false;}
            }
        }

        return o_value;
    }

    private static int CalcDiagoSequ(int[,] i_gameBoard, int i_CurrentIndex)
    {
        int left2Right = 0;
        int right2Left = 0;
        int curr_value = 0;
        bool rowStreak = false;
        //Check Diagnel left to right
        for (int coll = 0; coll < 4; coll++)
        {
            rowStreak = false;
            int calc_row = 6;
            if(coll == 2){
                calc_row = 5;
            }else if(coll == 3){
                calc_row = 4;
            }
            int tempColl = coll;
            for (int row = 0 ;row < calc_row; row++)
            {
                if(i_gameBoard[tempColl,row] == 2) //com pawn
                {
                    if(rowStreak){
                        curr_value +=1;
                        //Debug.Log("Hit: " + tempColl +"," + row);
                        if(left2Right < curr_value){
                            left2Right = curr_value;
                        }
                    }
                    else
                    {
                        rowStreak = true;
                        //Debug.Log("Hit start count: " + tempColl +"," + row);
                        left2Right = 1; //reset
                        curr_value = 1;
                    }
                }
                else{rowStreak = false;}
                //Debug.Log("Checking: (" + tempColl + " , " + row + ") seq status is now: " + rowStreak);
                tempColl++;
            }  
        }
        rowStreak = false;
        curr_value = 0;
        //Check Diagnel right to left
        for (int coll = 6; coll >= 3; coll--)
        {
            rowStreak = false;
            int calc_row = 6;
            if(coll == 4){
                calc_row = 5;
            }else if(coll == 3){
                calc_row = 4;
            }
            int tempColl = coll;
            for (int row = 0;row< calc_row; row++)
            {
                if(i_gameBoard[tempColl,row] == 2) //com pawn
                {
                    if(rowStreak){
                        curr_value++;
                        //Debug.Log("Hit: " + tempColl +"," + row);
                        if(right2Left < curr_value){
                            right2Left = curr_value;
                        }
                    }
                    else
                    {
                        rowStreak = true;
                        //Debug.Log("Hit start count: " + tempColl +"," + row);
                        right2Left = 1; //reset
                        curr_value = 1;
                    }
                }else{rowStreak = false;}
               // Debug.Log("Checking: (" + tempColl + " , " + row + ") seq status is now: " + rowStreak);
                tempColl--;
            }
            
        }

        //Debug.Log("a"+right2Left+" b"+left2Right);
        return Math.Max(right2Left,left2Right);
    }

    private static int checkRowSeq(int[,] i_gameBoard, int i_startingColl , int i_startingRow , int pawnToCheck){ //take the position of the virtual pawn and return the longest sequnce it will be part of
        int longestSeq = 1;
        for (int i = i_startingColl + 1; i < 7; i++) //to the right
        {
            if(i_gameBoard[i,i_startingRow] == pawnToCheck)
            {
                longestSeq++;
            }
            else{
                break;
            }
        }

        for (int i = i_startingColl - 1; i >= 0; i--) //to the left
        {
            if(i_gameBoard[i,i_startingRow] == pawnToCheck)
            {
                longestSeq++;
            }
            else{
                break;
            }
        }

        return longestSeq;
    }

    private static int checkCollSeq(int[,] i_gameBoard, int i_startingColl , int i_startingRow, int pawnToCheck){ 
        int longestSeq = 1;
        for (int i = i_startingRow + 1; i < 6; i++) //uppward
        {
            if(i_gameBoard[i_startingColl,i] == pawnToCheck)
            {
                longestSeq++;
            }
            else{
                break;
            }
        }

        for (int i = i_startingRow - 1; i >= 0; i--) //downward
        {
            if(i_gameBoard[i_startingColl,i] == pawnToCheck)
            {
                longestSeq++;
            }
            else{
                break;
            }
        }

        return longestSeq;
    }

    private static int checkDiag(int[,] i_gameBoard, int i_startingColl, int i_startingRow, int pawnToCheck)
    {
        return Math.Max(checkDiagSeqLtoR(i_gameBoard, i_startingColl, i_startingRow, pawnToCheck), checkDiagSeqRtoL(i_gameBoard, i_startingColl, i_startingRow, pawnToCheck));
    }

    private static int checkDiagSeqLtoR(int[,] i_gameBoard, int i_startingColl , int i_startingRow, int pawnToCheck){ //Working
        int longestSeq = 1;
        int tempRow = i_startingRow;
        //upward
        for (int coll = i_startingColl + 1; coll < 7; coll++)
        {
            if (tempRow + 1 < 6)
            {
                if (i_gameBoard[coll, tempRow + 1] == pawnToCheck)
                {
                    longestSeq++;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
            tempRow++;
        }
        //downward
        for (int coll = i_startingColl - 1; coll >= 0; coll--)
        {
            if (tempRow - 1 >= 0)
            {
                if (i_gameBoard[coll, tempRow - 1] == pawnToCheck)
                {
                    longestSeq++;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
            tempRow--;
        }
        return longestSeq;
    }

    private static int checkDiagSeqRtoL(int[,] i_gameBoard, int i_startingColl, int i_startingRow, int pawnToCheck)
    { //Working
        int longestSeq = 1;
        int tempRow = i_startingRow;
        //upward
        for (int coll = i_startingColl - 1; coll >= 0; coll--)
        {
            if (tempRow + 1 < 6)
            {
                if (i_gameBoard[coll, tempRow + 1] == pawnToCheck)
                {
                    longestSeq++;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
            tempRow++;
        }
        //downward
        for (int coll = i_startingColl + 1; coll < 7; coll++)
        {
            if (tempRow - 1 >= 0)
            {
                if (i_gameBoard[coll, tempRow - 1] == pawnToCheck)
                {
                    longestSeq++;
                }
                else
                {
                    break;
                }
            }
            tempRow--;
        }
        return longestSeq;
    }
}