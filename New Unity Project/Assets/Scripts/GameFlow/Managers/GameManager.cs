using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Managers Reffrance")]
    public SpawnManager SpawnManager;
    public BoardManager boardManager;
    public ComputerAI computerAI;

    [Header("Actions")]
    public static Action ReadyUpSpawns;
    public static Action NotReadySpawns;

    [Header("Turns Managment")]
    public bool playersTurn; //when false its the computer turn

    private void Start()
    {
        playersTurn = true;
    }
}
