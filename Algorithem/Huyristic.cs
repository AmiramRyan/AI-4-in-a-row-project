using System.Collections;
using System.Collections.Generic;
using System;
public class Huyristic
{
    public static int Evaluation(int[,] i_gameBoard)
    {
        return getRandomNumber();
    }
    private static int getRandomNumber()
    {
        Random r = new Random();
        int month = r.Next(0, 5);

        return month;
    }
    /*
     * adding here PRIVATE STATIC function to calculate things we want
     */



}
