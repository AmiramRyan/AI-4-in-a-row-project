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
        StartCoroutine(PlayTurnCo());
    }

    public int PickRandomColl()
    {
        return Random.Range(0, 6);
    }

    public IEnumerator PlayTurnCo()
    {
        yield return new WaitForSeconds(2);
        int coll = PickRandomColl();
        gameManager.SpawnManager.SetColl(coll); //spawn AI pawn on screen and on the logic board
        //let the player play
        gameManager.playersTurn = true;
        GameManager.ReadyUpSpawns?.Invoke();
    }
}
