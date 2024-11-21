using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Block")
        {
            Block block = collision.gameObject.GetComponent<Block>();
            if(block != null)
            {
                if(!block.IsBlockStopped())
                {
                    Destroy(collision.gameObject);
                    gameManager.GameOver();
                }
            }           
        }
    }
}
