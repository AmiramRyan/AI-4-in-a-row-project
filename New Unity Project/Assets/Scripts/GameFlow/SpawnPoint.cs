using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    //This class defines the actions a spawning point need to have

    private GameManager gameManager;
    private SpriteRenderer mySpriteRenderer;
    [SerializeField] private bool ready;
    [SerializeField] private int myColl;

    private void OnEnable()
    {
        GameManager.ReadyUpSpawns += ReadySpawn;
        GameManager.NotReadySpawns += NotReadySpawn;
    }

    private void OnDisable()
    {
        GameManager.ReadyUpSpawns -= ReadySpawn;
        GameManager.NotReadySpawns -= NotReadySpawn;
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
        mySpriteRenderer = this.GetComponent<SpriteRenderer>();
        ready = true;
    }

    private void OnMouseEnter() //when the mouse hovers above the spawn spot
    {
        if (!GameManager.stopGame)
        {
            //if its the player turn
            //if sprite not already active
            if (!mySpriteRenderer.enabled)
            {
                mySpriteRenderer.enabled = true;
            }
        }
    }

    private void OnMouseExit()
    {
        if (mySpriteRenderer.enabled)
        {
            mySpriteRenderer.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        if (!GameManager.stopGame)
        {
            if (ready) //make sure its not already clicked
            {
                GameManager.NotReadySpawns?.Invoke();
                gameManager.SpawnManager.SetColl(myColl);
                //let the AI play
                gameManager.playersTurn = false;
                gameManager.computerAI.PlayTurn();
            }
        }
    }

    private void ReadySpawn() { ready = true; } //Called when computer done his turn
    private void NotReadySpawn(){ ready = false; } //called when the player picks a spawn point, to make sure the others wont be active
}
