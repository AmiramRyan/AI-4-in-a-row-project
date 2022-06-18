using System.Collections;
using System.Collections.Generic;
using System;
public class Huyristic
{
    public static int Evaluation(int[,] i_gameBoard, int i_CurrentIndex)
    {
        return GetMaxValue(i_gameBoard, i_CurrentIndex)
    }
    
    private static int GetMaxValue(int[,] i_gameBoard, int i_CurrentIndex)
    {
        int o_maxValue = 0;
        int rowValue = CalcRowSequ(i_gameBoard, i_CurrentIndex);
        int colValue = CalcColSequ(i_gameBoard, i_CurrentIndex);
        int diagoValue = CalcDiagoSequ(i_gameBoard, i_CurrentIndex);

        o_maxValue = Math.Max(rowValue, colValue);
        o_maxValue = Math.Max(o_maxValue, diagoValue);

        return o_maxValue;
    }

    private static int CalcRowSequ(int[,] i_gameBoard, int i_CurrentIndex)
    {
        int o_value = 0;

        for(int i = i_CurrentIndex - 1, j = i_CurrentIndex + 1; i > i_CurrentIndex - 3; i--, j++)
        {
            try
            {
                if(i_gameBoard[0, i] == 2)
                {
                    o_value++;
                }
            }
            catch(ArgumentOutOfRangeException e) { }

            try
            {
                if(i_gameBoard[0, j] == 2)
                {
                    o_value++;
                }
            }
            catch (ArgumentOutOfRangeException e) { }
        }

        return o_value;
    }

    private static int CalcColSequ(int[,] i_gameBoard, int i_CurrentIndex)
    {
        int o_value = 0;

        for(int i = i_CurrentIndex - 1, j = i_CurrentIndex + 1; i > i_CurrentIndex - 3; i--, j++)
        {
            try
            {
                if(i_gameBoard[i, i_CurrentIndex] == 2)
                {
                    o_value++;
                }

            }
            catch (ArgumentOutOfRangeException e) { }

            try
            {
                if(i_gameBoard[j, i_CurrentIndex] == 2)
                {
                    o_value++;
                }
            }
            catch (ArgumentOutOfRangeException e) { }
        }

        return o_value;
    }

    private static int CalcDiagoSequ(int[,] i_gameBoard, int i_CurrentIndex)
    {
        int o_value = 0;

        for(int i = i_CurrentIndex - 1, j = i_CurrentIndex + 1; i > i_CurrentIndex - 3; i--, j++)
        {
            try
            {
                if (i_gameBoard[i, i] == 2)
                {
                    o_value++;
                }
            }
            catch (ArgumentOutOfRangeException e) { }

            try
            {
                if(i_gameBoard[j,j] == 2)
                {
                    o_value++;
                }
            }
            catch(ArgumentOutOfRangeException e) { }
        }

        return o_value;
    }
   
}
