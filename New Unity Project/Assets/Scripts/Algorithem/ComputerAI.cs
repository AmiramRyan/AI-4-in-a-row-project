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
        int coll = PickRandomColl();
        gameManager.SpawnManager.SetColl(coll); //spawn AI pawn on screen and on the logic board
        //let the player play
        gameManager.playersTurn = true;
        GameManager.ReadyUpSpawns?.Invoke();
    }

}
