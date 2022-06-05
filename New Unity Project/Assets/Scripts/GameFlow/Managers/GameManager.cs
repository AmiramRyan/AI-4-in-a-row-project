using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public enum Winner{
    player1,
    COM,
    noPlayer
}

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

    [Header("UI panels")]
    public GameObject EndGamePanel;
    public TextMeshProUGUI winnerText; 

    public Winner winner;
    public static bool stopGame;

    private void Start()
    {
        playersTurn = true;
        stopGame = false;
        winner = Winner.noPlayer;
        EndGamePanel.SetActive(false);
    }
}
