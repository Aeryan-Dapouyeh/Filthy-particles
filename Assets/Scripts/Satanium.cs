using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Satanium : MonoBehaviour {

    public GameManager gameManagerScript;
    public float evaporateEvery = 5;
    [Range(0, 100)]
    public int chanceOfEvaporation;

    private int previousAutoTurn;
    private float nextTimeToEvaporate = 0;
    private int EvaporationDirection; // a random value between zero and one, if 0 == HORIZONTAL else if 1 == VERTICAL 
    private List<GameObject> sceneObjects;
    private void Awake()
    {
        if(gameManagerScript == null)
        {
            gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
        if(nextTimeToEvaporate == 0)
        {
            nextTimeToEvaporate = evaporateEvery;
        }
    }
    private void Burn()
    {
        EvaporationDirection = Random.Range(0, 2);
        List<GameObject> objectsToBurn = new List<GameObject>();
        nextTimeToEvaporate = Time.time + evaporateEvery;
        gameManagerScript.allGOs = gameManagerScript.GetAllObjectsInScene();
        foreach (GameObject thing in gameManagerScript.allGOs)
        {
            if (thing.transform.parent == null && thing.tag != "MainCamera" && thing.tag != "StopPoint" && thing.tag != "wormHole" && thing.tag != "MainCamera" && thing.tag != "Untagged" && thing.tag != "GameController")
            {
                if (thing.activeSelf)
                {
                    if (EvaporationDirection == 0 && thing.transform.position.x == transform.position.x)
                    {
                        //Debug.Log(thing.name + "Horizontal indeed!");
                        objectsToBurn.Add(thing);
                    }
                    else if (EvaporationDirection == 1 && thing.transform.position.y == transform.position.y)
                    {
                        //Debug.Log(thing.name + "Vertical indeed!");
                        objectsToBurn.Add(thing);
                    }
                }
            }
        }
        for (int i = 0; i <= objectsToBurn.Count - 1; i++)
        {
            Destroy(objectsToBurn[i]);
        }
    }
    private void Update()
    {
        if (Time.time >= nextTimeToEvaporate && gameManagerScript.mode == 0)
        {
            Burn();
        }
        int chance = Random.Range(0, 100);        
        if (gameManagerScript.mode == 1 && gameManagerScript.turnInProgress == false && previousAutoTurn != gameManagerScript.autoTurn && chance != 0)
        {
            if (chance <= chanceOfEvaporation)
            {
                Burn();
                previousAutoTurn = gameManagerScript.autoTurn;
            }
            else
            {
                previousAutoTurn = gameManagerScript.autoTurn;
            }
        }
    }
}
