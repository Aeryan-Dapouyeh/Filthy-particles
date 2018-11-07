using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batterium : MonoBehaviour {

    [Range(0, 100)]
    public int chanceOfEarningPerTurn; // chance of earning a particle per/in a specific turn
    public int[] filthyTimeValues = new int[2]; // element 0 for minimum and 1 for max
    public int storageCapacity;
    public int storedCash = 0;
    public int exploitationValue = 70;
    public GameManager gameManagerScript;
    public GameObject currency;
    public GameObject orbit;
    public ParticleSystem explosionEffect;

    private float timeToSpawn = 0;    
    private int randomTime;
    private int previousAutomTurn;

    // Use this for initialization
    void Start () {
		if(gameManagerScript == null)
        {
            gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
	}
    private void Awake()
    {
        randomTime = Random.Range(filthyTimeValues[0], filthyTimeValues[1]);
        timeToSpawn = randomTime;
    }
    // Update is called once per frame
    void Update () {
        CashFlowPerTurn(storageCapacity, chanceOfEarningPerTurn, filthyTimeValues);
        if(storedCash != 0)
        {
            if(orbit.transform.GetChild(storedCash - 1).gameObject.activeSelf == false)
            {
                orbit.transform.GetChild(storedCash - 1).gameObject.SetActive(true);
            }
        }
    }
    // this function returns the positions of all neighbouring wormies, note that if there are no wormies at any given position, the value for vector 2 will be (0, 0)
    private Vector2[] FindWormiesAround() 
    {
        GameObject upperWormie = null;
        GameObject bottomWormie = null;
        GameObject rightWormie = null;
        GameObject leftWormie = null;
        Vector2[] neighbouringWormies = new Vector2[4];
        for(int i = 0; i <= gameManagerScript.wormies.Count - 1 ; i++)
        {
            foreach(GameObject wormie in gameManagerScript.wormies[i])
            {
                Vector2 _WormiePos = wormie.transform.position;
                Vector2 _pos = gameObject.transform.position;
                int rotationType = wormie.GetComponent<WormieResistor>().thisWormie.rotationType;
                if(upperWormie == null && _pos.y + (i + 1) * 5 == _WormiePos.y && _pos.x == _WormiePos.x && rotationType == 1)
                {
                    upperWormie = wormie;
                }
                if(bottomWormie == null && _pos.y - (i + 1) * 5 == _WormiePos.y && _pos.x == _WormiePos.x && rotationType == 1)
                {
                    bottomWormie = wormie;
                }
                if(rightWormie == null && _pos.x + (i + 1) * 5 == _WormiePos.x && _pos.y == _WormiePos.y && rotationType == 0)
                {
                    rightWormie = wormie;
                }
                if (leftWormie == null && _pos.x - (i + 1) * 5 == _WormiePos.x && _pos.y == _WormiePos.y && rotationType == 0)
                {
                    leftWormie = wormie;
                }
            }            
        }  
        if(upperWormie != null)
        {
            neighbouringWormies[0] = upperWormie.transform.position;
        }
        if(bottomWormie != null)
        {
            neighbouringWormies[1] = bottomWormie.transform.position;
        }
        if (rightWormie != null)
        {
            neighbouringWormies[2] = rightWormie.transform.position;
        }
        if (leftWormie != null)
        {
            neighbouringWormies[3] = leftWormie.transform.position;
        }
        return neighbouringWormies;
    }
    private void SpawnMoney(Vector2[] possibleSpawnPositions)
    {
        int randomPosition = Random.Range(0, possibleSpawnPositions.Length);
        if(possibleSpawnPositions[randomPosition] != Vector2.zero && possibleSpawnPositions[randomPosition] != null)
        {
            GameObject InsCurrency = Instantiate(currency, possibleSpawnPositions[randomPosition], Quaternion.identity);
            InsCurrency.GetComponent<CurrencyMover>().target = gameObject.transform.position;
        }
    }
    private void CashFlowPerTurn(int storageCapacity, int chance, int[] filthyChance)
    {
        if(storedCash < storageCapacity)
        {
            //Debug.Log("Last time: " + timeToSpawn + "Random time: " + randomTime + "Time: " + Time.time);
            if(gameManagerScript.mode == 0) // if fithy mode
            {                
                if(Time.time - timeToSpawn == 0 || Time.time - timeToSpawn > 0)
                {
                    SpawnMoney(FindWormiesAround());
                    randomTime = Random.Range(filthyChance[0], filthyChance[1]);
                    timeToSpawn = Time.time + randomTime;
                }
            }
            if(gameManagerScript.mode == 1 && gameManagerScript.turnInProgress == false && previousAutomTurn != gameManagerScript.autoTurn) // if automized
            {
                int randomNumber = Random.Range(0, 100);
                if (randomNumber <= chance && chance != 0)
                {
                    Debug.Log("Cash earned!");
                    previousAutomTurn = gameManagerScript.autoTurn;
                    SpawnMoney(FindWormiesAround());
                }
                else
                {
                    previousAutomTurn = gameManagerScript.autoTurn;
                }
            }            
        }
    }
}
