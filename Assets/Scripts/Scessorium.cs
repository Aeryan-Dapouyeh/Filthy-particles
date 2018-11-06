using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scessorium : MonoBehaviour {
    public GameManager gameManagerScript;
    public int CutAWormieEvery = 5;
    [Range(0, 100)]
    public int cuttingChancePerTurn;
    public int atomicCurrencyPerCut = 5;
    public int explosionReward = 25;
    public GameObject currencyPrefab;
    public ParticleSystem ExplosionEffect;

    //private bool wormieCutInThisTurn = false;
    private int previousAutoTurn;
    private float nextTimeToCutAWormie = 0;
    private Vector2 eliminatedPosition;
    private Vector2[] neighbouringWormies;

    private void Awake()
    {
        if(gameManagerScript == null)
        {
            gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
    }
    private void Update()
    {
        neighbouringWormies = gameManagerScript.FindWormiesAround(gameObject);
        if(CheckIfAnyWormieExists(neighbouringWormies) == false)
        {
            Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            gameManagerScript.atomiccurreny += explosionReward;
        }
        CutWormies();
    }
    bool CheckIfAnyWormieExists(Vector2[] positionArray)
    {
        bool aWormieExists = false;
        foreach(Vector2 position in positionArray)
        {
            if(position != Vector2.zero)
            {
                aWormieExists = true;
            }
        }
        return aWormieExists;
    } 
    void CutWormies()
    {        
        if(gameManagerScript.mode == 0)
        {
            if(nextTimeToCutAWormie == 0)
            {
                nextTimeToCutAWormie = Time.time + CutAWormieEvery;
            }
            else
            {
                if(nextTimeToCutAWormie <= Time.time)
                {
                    CheckAndCut();
                }
            }
        }
        else if(gameManagerScript.mode == 1 && gameManagerScript.turnInProgress == false && previousAutoTurn != gameManagerScript.autoTurn)
        {
            if (nextTimeToCutAWormie == 0)
            {
                nextTimeToCutAWormie = Time.time + CutAWormieEvery;
            }
            else
            {
                int chanceToCut = Random.Range(0, 100);
                if(chanceToCut >= cuttingChancePerTurn && chanceToCut != 0)
                {
                    previousAutoTurn = gameManagerScript.autoTurn;
                    CheckAndCut();
                }
            }
        }
    }
    void CheckAndCut()
    {
        int randomWormieTocut = Random.Range(0, neighbouringWormies.Length);
        bool aNeighbouringWormieExists = false;
        foreach (Vector2 wormie in neighbouringWormies)
        {
            if (aNeighbouringWormieExists == false && wormie != Vector2.zero)
            {
                aNeighbouringWormieExists = true;
            }
        }
        bool aWormieChosenToCut = false;
        if (aNeighbouringWormieExists)
        {
            while (aWormieChosenToCut == false)
            {
                if (neighbouringWormies[randomWormieTocut] != Vector2.zero)
                {
                    aWormieChosenToCut = true;
                    break;
                }
                else
                {
                    randomWormieTocut = Random.Range(0, neighbouringWormies.Length);
                }
            }
        }
        for (int i = 0; i <= gameManagerScript.wormies.Count - 1; i++)
        {
            for (int j = 0; j <= gameManagerScript.wormies[i].Length - 1; j++)
            {
                if ((Vector2)gameManagerScript.wormies[i][j].transform.position == neighbouringWormies[randomWormieTocut])
                {
                    GameObject wormie = gameManagerScript.wormies[i][j];
                    eliminatedPosition = wormie.transform.position;
                    Destroy(wormie);
                    GameObject[] arrayReference = gameManagerScript.wormies[i];
                    for (int x = j + 1; x <= arrayReference.Length - 1; x++)
                    {
                        // Debug.Log("Element " + x + " to " + (x - 1) + " length " + arrayReference.Length + " to " + (arrayReference.Length - 1));
                        arrayReference[x - 1] = arrayReference[x];
                    }
                    System.Array.Resize(ref arrayReference, arrayReference.Length - 1);
                }
            }
        }
        gameManagerScript.atomiccurreny += atomicCurrencyPerCut;
        GameObject InsCurrency = Instantiate(currencyPrefab, eliminatedPosition, Quaternion.identity);
        InsCurrency.GetComponent<CurrencyMover>().target = gameObject.transform.position;
        nextTimeToCutAWormie = Time.time + CutAWormieEvery;
    }
}
