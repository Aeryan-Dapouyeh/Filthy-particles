using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tardinium : MonoBehaviour {

    public GameManager gameManagerScript;
    public float teleportEvery = 5;
    [Range(0, 100)]
    public int teleportAutoChance;
    public int waitingTimeForMines = 2;
    public GameObject minePrefab;

    bool yetToReturn = false;
    private int previousAutoTurn;
    private float nextTimeTeleport = 0;
    private Vector2 initial_pos;

    private void Awake()
    {
        if (gameManagerScript == null)
        {
            gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
        if (nextTimeTeleport == 0 && gameManagerScript.mode == 0)
            nextTimeTeleport = teleportEvery;
        initial_pos = transform.position;
    }
    void Teleport()
    {
        Debug.Log("Time to teleport!");
        GameObject[] stoppingPoints = GameObject.FindGameObjectsWithTag("StopPoint");
        int random = Random.Range(0, stoppingPoints.Length - 1);
        gameManagerScript.allGOs = gameManagerScript.GetAllObjectsInScene();
        List<GameObject> allGOs = gameManagerScript.allGOs;
        bool positionEligible = true;
        bool pos_available = false;
        foreach (GameObject GO in allGOs)
        {
            if (stoppingPoints[random].transform.position == GO.transform.position && GO.tag != "StopPoint" && GO.activeSelf)
            {
                positionEligible = false;
            }
            if (GO.transform.position != stoppingPoints[random].transform.position && GO.activeSelf)
            {
                pos_available = true;
            }
        }

        // ensure that a position is chosen
        if (positionEligible == false && pos_available)
        {
            while (positionEligible == false && pos_available)
            {
                random = Random.Range(0, stoppingPoints.Length - 1);
                foreach (GameObject GO in allGOs)
                {
                    if (stoppingPoints[random].transform.position == GO.transform.position && GO.tag != "StopPoint" && GO.activeSelf)
                    {
                        positionEligible = false;
                    }
                    else
                    {
                        positionEligible = true;
                    }
                    if (GO.transform.position != stoppingPoints[random].transform.position && GO.activeSelf)
                    {
                        pos_available = true;
                    }
                }
                if (pos_available == false)
                {
                    break;
                }
            }
        }
        //transform.Translate(stoppingPoints[random].transform.position - transform.position);
        Vector2 target_pos = stoppingPoints[random].transform.position;
        if(gameManagerScript.mode == 0)
        {
            StartCoroutine(WaitAFewSeconds(initial_pos, target_pos));
        }
        else if(gameManagerScript.mode == 1 && yetToReturn == false)
        {
            transform.Translate(target_pos - (Vector2)transform.position);
            yetToReturn = true;
        }
        else if(gameManagerScript.mode == 1 && yetToReturn)
        {
            yetToReturn = false;
            transform.Translate(initial_pos - (Vector2)transform.position);
        }
    }
    private void Update()
    {
        if(gameManagerScript.mode == 0)
        {
            if(nextTimeTeleport <= Time.time)
            {
                Teleport();
                nextTimeTeleport = Time.time + teleportEvery;
            }
        }
        else if(gameManagerScript.mode == 1 && gameManagerScript.autoTurn != previousAutoTurn && gameManagerScript.turnInProgress == false)
        {
            int randomNumber = Random.Range(0, 100);
            previousAutoTurn = gameManagerScript.autoTurn;
            if (randomNumber <= teleportAutoChance)
            {
                Debug.Log("Time to teleport!");
                Teleport();
            }
        }
    }
    IEnumerator WaitAFewSeconds(Vector2 returning_pos, Vector2 target_pos)
    {
        transform.Translate(target_pos - (Vector2)transform.position);
        yield return new WaitForSeconds(waitingTimeForMines);
        Instantiate(minePrefab, transform.position, Quaternion.identity);
        transform.Translate(returning_pos - target_pos);
    }
}
