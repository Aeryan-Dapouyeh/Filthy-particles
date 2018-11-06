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
    private void Update()
    {
        if(gameManagerScript.mode == 0) // if filthy
        {
            if(Time.time >= nextTimeToBurn)
            {
                Debug.Log("Time To burn!");
                gameManagerScript.allGOs = gameManagerScript.GetAllObjectsInScene();
                List<GameObject> allGOs = gameManagerScript.allGOs;
                List<GameObject> objectsToBurn = new List<GameObject>();
                Vector2[] wormiesAround = gameManagerScript.FindWormiesAround(gameObject);
                Vector2[] PositionsToLookAfter;
                foreach(Vector2 wormie in wormiesAround)
                {
                    for(int i = 0; i <= gameManagerScript.wormies.Count - 1; i++)
                    {
                        foreach(GameObject Wormie_Obj in gameManagerScript.wormies[i])
                        {
                            if((Vector2)Wormie_Obj.transform.position == wormie)
                            {
                                int lengthOfWormie = Wormie_Obj.GetComponent<WormieResistor>().lengthOfWormie;
                                int typeOfWormie = Wormie_Obj.GetComponent<WormieResistor>().thisWormie.rotationType; //if 0 then horizontal, if 1, vertical
                                if(typeOfWormie == 0)
                                {
                                    // TO CONTINUE FROM HERE
                                }
                                else if(typeOfWormie == 1)
                                {

                                }
                                Debug.Log("Wormie length: " + lengthOfWormie);
                            }
                        }
                    }
                    Debug.Log(wormie);
                }
                nextTimeToBurn = Time.time + burnEvery;
            }
        }
        else if(gameManagerScript.mode == 1)
        {            
            if(gameManagerScript.autoTurn != previousAutoTurn && gameManagerScript.turnInProgress == false)
            {
                int randomNumber = Random.Range(0, 100);
                if (randomNumber <= burnEveryAutoChance)
                {
                    Debug.Log("Time to burn!");
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
