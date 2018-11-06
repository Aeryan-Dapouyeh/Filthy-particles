using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStoppingPointScript : MonoBehaviour {

    public GameManager gameManagerScript;

    private bool aSorroundingWormieExists = true;
    private Vector2[] surroudningWormies;

    private void Awake()
    {
        if(gameManagerScript == null)
        {
            gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
    }
    private void Start()
    {
        surroudningWormies = gameManagerScript.FindWormiesAround(gameObject);
    }
    private void Update()
    {
        surroudningWormies = gameManagerScript.FindWormiesAround(gameObject);
        aSorroundingWormieExists = false;
        foreach (Vector2 wormie in surroudningWormies)
        {
            if(wormie != Vector2.zero)
            {
                aSorroundingWormieExists = true;
                break;
            }
        }
        if(aSorroundingWormieExists == false)
        {
            Destroy(gameObject);
        }
    }
}
