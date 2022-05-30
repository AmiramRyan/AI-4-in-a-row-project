using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //This class will handel spawning of game pawns on the board
    [Header("SpawnPoints")]
    [SerializeField] private Transform shadow1;
    [SerializeField] private Transform shadow2;
    [SerializeField] private Transform shadow3;
    [SerializeField] private Transform shadow4;
    [SerializeField] private Transform shadow5;
    [SerializeField] private Transform shadow6;
    [SerializeField] private Transform shadow7;

    [Header("Prefabs and Containers")]
    [SerializeField] private GameObject pawnPreFabYellow;
    [SerializeField] private GameObject pawnPreFabRed;
    [SerializeField] private GameObject pawnsParent;

    private GameManager gameManager;
    private int choosenRow;

    private void OnEnable()
    {
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
    }

    public void SpawnPawnAt(bool isPlayerTurn) //Gets the spawning row and indicator for whos turn it is and drop a pawn in that row in the correct color
    {
        GameObject pawnPrefab;
        if (isPlayerTurn)
        {
            pawnPrefab = pawnPreFabRed;
        }
        else
        {
            pawnPrefab = pawnPreFabYellow;
        }

        gameManager.boardManager.AddPawn(isPlayerTurn, choosenRow);

        switch (choosenRow)
        {
            case 0:
                SpawnAtTransform(shadow1, pawnPrefab);
                break;
            case 1:
                SpawnAtTransform(shadow2, pawnPrefab);
                break;
            case 2:
                SpawnAtTransform(shadow3, pawnPrefab);
                break;
            case 3:
                SpawnAtTransform(shadow4, pawnPrefab);
                break;
            case 4:
                SpawnAtTransform(shadow5, pawnPrefab);
                break;
            case 5:
                SpawnAtTransform(shadow6, pawnPrefab);
                break;
            case 6:
                SpawnAtTransform(shadow7, pawnPrefab);
                break;
            default:
                Debug.LogError("No such spawn place!");
                break;
        }
    }

    private void SpawnAtTransform(Transform spawnHere , GameObject prefab) //Actual spawning proccese, called from SpawnPawnAt
    {
        Instantiate(prefab, spawnHere.position, Quaternion.identity).transform.SetParent(pawnsParent.transform); 
    } 

    #region Get/Set

    public int GetColl() { return choosenRow; }
    public void SetColl(int coll)
    {
        choosenRow = coll;
        SpawnPawnAt(gameManager.playersTurn); 
    }

    #endregion
}
