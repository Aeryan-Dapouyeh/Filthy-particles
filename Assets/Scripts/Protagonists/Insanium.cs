using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insanium : MonoBehaviour {

    public GameManager gameManagerScript;
    public float burnEvery = 5;
    [Range(0, 100)]
    public int burnEveryAutoChance;
    
    private int previousAutoTurn;
    private float nextTimeToBurn = 0;
    private void Awake()
    {
        if (gameManagerScript == null)
        {
            gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
        if (nextTimeToBurn == 0 && gameManagerScript.mode == 0)
            nextTimeToBurn = burnEvery;
    }
    void BurnThemAll()
    {
        gameManagerScript.allGOs = gameManagerScript.GetAllObjectsInScene();
        List<GameObject> allGOs = gameManagerScript.allGOs;
        List<GameObject> objectsToBurn = new List<GameObject>();
        Vector2[] wormiesAround = gameManagerScript.FindWormiesAround(gameObject);
        List<Vector2> PositionsToLookAfter = new List<Vector2>();
        // find the wormies around and determine the possible positions of particles to burn
        foreach (Vector2 wormie in wormiesAround)
        {
            for (int i = 0; i <= gameManagerScript.wormies.Count - 1; i++)
            {
                foreach (GameObject Wormie_Obj in gameManagerScript.wormies[i])
                {
                    if ((Vector2)Wormie_Obj.transform.position == wormie)
                    {
                        objectsToBurn.Add(Wormie_Obj);
                        int lengthOfWormie = Wormie_Obj.GetComponent<WormieResistor>().lengthOfWormie;
                        int typeOfWormie = Wormie_Obj.GetComponent<WormieResistor>().thisWormie.rotationType; //if 0 then horizontal, if 1, vertical
                        if (typeOfWormie == 0)
                        {
                            if (Wormie_Obj.transform.position.x > transform.position.x)
                            {
                                float positionX = transform.position.x + lengthOfWormie;
                                Vector2 position = new Vector2(positionX, transform.position.y);
                                PositionsToLookAfter.Add(position);
                            }
                            else
                            {
                                float positionX = transform.position.x - lengthOfWormie;
                                Vector2 position = new Vector2(positionX, transform.position.y);
                                PositionsToLookAfter.Add(position);
                            }
                        }
                        else if (typeOfWormie == 1)
                        {
                            if (Wormie_Obj.transform.position.y > transform.position.y)
                            {
                                float positionY = transform.position.y + lengthOfWormie;
                                Vector2 position = new Vector2(transform.position.x, positionY);
                                PositionsToLookAfter.Add(position);
                            }
                            else
                            {
                                float positionY = transform.position.y - lengthOfWormie;
                                Vector2 position = new Vector2(transform.position.x, positionY);
                                PositionsToLookAfter.Add(position);
                            }
                        }
                    }
                }
            }
        }
        foreach (Vector2 position in PositionsToLookAfter)
        {
            Debug.Log(position + " to look after!");
        }
        foreach (GameObject GO in allGOs)
        {
            foreach (Vector2 _pos in PositionsToLookAfter)
            {
                if (_pos == (Vector2)GO.transform.position && GO.transform.parent == null && GO.activeSelf && GO.tag != "Untagged" && GO.tag != "GameController" && GO.tag != "MainCamera" && GO.tag != "StopPoint")
                {
                    //Debug.Log(GO.name + " at: " + GO.transform.position + " _pos: " + _pos);
                    objectsToBurn.Add(GO);
                }
            }
        }
        foreach (GameObject unfortunateParticle in objectsToBurn)
        {
            Destroy(unfortunateParticle);
            Destroy(gameObject);
        }
        nextTimeToBurn = Time.time + burnEvery;
    }
    private void Update()
    {
        if(gameManagerScript.mode == 0) // if filthy
        {
            if(Time.time >= nextTimeToBurn)
            {
                BurnThemAll();   
            }
        }
        else if(gameManagerScript.mode == 1) // if auto
        {            
            if(gameManagerScript.autoTurn != previousAutoTurn && gameManagerScript.turnInProgress == false)
            {
                int randomNumber = Random.Range(0, 100);
                if (randomNumber <= burnEveryAutoChance)
                {
                    BurnThemAll();
                    previousAutoTurn = gameManagerScript.autoTurn;
                }   
                else
                {
                    previousAutoTurn = gameManagerScript.autoTurn;
                }
            }
        }
    }
}
