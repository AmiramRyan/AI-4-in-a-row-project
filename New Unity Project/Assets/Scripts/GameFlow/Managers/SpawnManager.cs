using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //This class will handel spawning of game pawns on the board
    [Header("SpawnPoints")] //Point above each collumn to be used as spawning points to the new pawns
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
    private int choosenRow; //the current row being inserted

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
                shadow1.GetComponent<SpawnPoint>().amountInColl++;
                SpawnAtTransform(shadow1, pawnPrefab);
                if (shadow1.GetComponent<SpawnPoint>().amountInColl == 6)
                {
                    shadow1.gameObject.SetActive(false); //turn yourself off
                }
                break;
            case 1:
                shadow2.GetComponent<SpawnPoint>().amountInColl++;
                SpawnAtTransform(shadow2, pawnPrefab);
                if (shadow2.GetComponent<SpawnPoint>().amountInColl == 6)
                {
                    shadow2.gameObject.SetActive(false); //turn yourself off
                }
                break;
            case 2:
                shadow3.GetComponent<SpawnPoint>().amountInColl++;
                SpawnAtTransform(shadow3, pawnPrefab);
                if (shadow3.GetComponent<SpawnPoint>().amountInColl == 6)
                {
                    shadow3.gameObject.SetActive(false); //turn yourself off
                }
                break;
            case 3:
                shadow4.GetComponent<SpawnPoint>().amountInColl++;
                SpawnAtTransform(shadow4, pawnPrefab);
                if (shadow4.GetComponent<SpawnPoint>().amountInColl == 6)
                {
                    shadow4.gameObject.SetActive(false); //turn yourself off;
                }
                break;
            case 4:
                shadow5.GetComponent<SpawnPoint>().amountInColl++;
                if (shadow5.GetComponent<SpawnPoint>().amountInColl == 6)
                {
                    shadow5.gameObject.SetActive(false); //turn yourself off
                }
                SpawnAtTransform(shadow5, pawnPrefab);
                break;
            case 5:
                shadow6.GetComponent<SpawnPoint>().amountInColl++;
                if (shadow6.GetComponent<SpawnPoint>().amountInColl == 6)
                {
                    shadow6.gameObject.SetActive(false); //turn yourself off
                }
                SpawnAtTransform(shadow6, pawnPrefab);
                break;
            case 6:
                shadow7.GetComponent<SpawnPoint>().amountInColl++;
                if (shadow7.GetComponent<SpawnPoint>().amountInColl == 6)
                {
                    shadow7.gameObject.SetActive(false); //turn yourself off
                }
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

    public int GetColl() 
    { 
        return choosenRow; 
    }
    public void SetColl(int coll)
    {
        choosenRow = coll;
        SpawnPawnAt(gameManager.playersTurn); 
    }

    public void DisableShadows()
    {
        shadow1.gameObject.SetActive(false);
        shadow2.gameObject.SetActive(false);
        shadow3.gameObject.SetActive(false);
        shadow4.gameObject.SetActive(false);
        shadow5.gameObject.SetActive(false);
        shadow6.gameObject.SetActive(false);
        shadow7.gameObject.SetActive(false);
    }

    #endregion
}
