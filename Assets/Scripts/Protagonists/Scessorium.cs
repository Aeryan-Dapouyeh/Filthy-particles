using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scessorium : MonoBehaviour {

    public GameManager gameManagerScript;
    public float cutEvery = 5;
    [Range(0, 100)]
    public int Auto_cuttingChance;

    private int previousAutoTurn;
    private float nextTimeTocut = 0;
    //private List<GameObject> sceneObjects;
    private void Awake()
    {
        if (gameManagerScript == null)
        {
            gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
        if (nextTimeTocut == 0)
        {
            nextTimeTocut = cutEvery;
        }
    }
    private void Update()
    {
        if(nextTimeTocut <= Time.time && gameManagerScript.mode == 0)
        {
            Debug.Log("Time to cut!");
            List<GameObject> neighbouringWormies = new List<GameObject>();
            Vector2[] _wormies = gameManagerScript.FindWormiesAround(gameObject);

            for(int i = 0; i <= gameManagerScript.wormies.Count - 1; i++)
                foreach(GameObject Wormie in gameManagerScript.wormies[i])
                    foreach(Vector2 _pos in _wormies)
                        if (_pos == (Vector2)Wormie.transform.position)
                            neighbouringWormies.Add(Wormie);

            int randomWormie = Random.Range(0, neighbouringWormies.Count - 1);
            GameObject wormieTo_cut = neighbouringWormies[randomWormie];
            WormieResistor wormieScript = wormieTo_cut.GetComponent<WormieResistor>();
            if(wormieScript.lengthOfWormie > 10)
            {
                Debug.Log("Length: " + wormieScript.GetComponent<WormieResistor>().lengthOfWormie); // TO BE CONTINUED
            }
            else
            {
                Debug.Log("Length ten!");
            }

            nextTimeTocut = Time.time + cutEvery;
        }
        else if(gameManagerScript.mode == 1 && gameManagerScript.autoTurn != previousAutoTurn && gameManagerScript.turnInProgress == false)
        {
            int random = Random.Range(0, 100);
            previousAutoTurn = gameManagerScript.autoTurn;
            if (random <= Auto_cuttingChance)
            {                
                Debug.Log("Time to cut!");
            }            
        }
    }
}
