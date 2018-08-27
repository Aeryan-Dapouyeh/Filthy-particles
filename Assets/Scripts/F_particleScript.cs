using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_particleScript : MonoBehaviour {
    public GameObject gameManagerObject;
    public GameObject player;
    public GameObject e_particlePlayerPrefab;
    public GameObject n_particleNonePlayerPrefab;
    public GameObject p_particleNonePlayerPrefab;
    public float f_particleEffect = 1.2f;

    private E_particle playerScript;
    private GameManager gameManagerScript;

    private void Awake()
    {
        if(gameManagerObject == null)
        {
            gameManagerObject = GameObject.FindGameObjectWithTag("GameController");
        }
        gameManagerScript = gameManagerObject.GetComponent<GameManager>();
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        playerScript = player.GetComponent<E_particle>();
    }
    private void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    private void BreakDownAtie(GameObject n_particle, GameObject p_particle, GameObject e_particle)
    {
        Destroy(gameObject);
        Instantiate(e_particle, transform.position, Quaternion.identity);
        Vector2 positionToInstantiate_P_particle;
        Vector2 positionToInstantiate_N_particle;
        List<Vector2> stoppingPointpositions = gameManagerScript.stoppingPointPositions;
        int luckyPosition1 = Random.Range(0, stoppingPointpositions.Count - 1);
        int luckyPosition2 = Random.Range(0, stoppingPointpositions.Count - 1);
        while(luckyPosition1 == luckyPosition2)
        {
            luckyPosition2 = Random.Range(0, stoppingPointpositions.Count - 1);
        }
        positionToInstantiate_P_particle = stoppingPointpositions[luckyPosition1];
        positionToInstantiate_N_particle = stoppingPointpositions[luckyPosition2];
        if(luckyPosition1 != luckyPosition2)
        {
            Instantiate(p_particle, positionToInstantiate_P_particle, Quaternion.identity); // it can sometimes instantiate mergable particles, yet I decided to keep it as a bonus mechanism for the player
            Instantiate(n_particle, positionToInstantiate_N_particle, Quaternion.identity);
        }        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(other.GetComponent<E_particle>().isAti)
            {
                BreakDownAtie(n_particleNonePlayerPrefab, p_particleNonePlayerPrefab, e_particlePlayerPrefab);
                Destroy(other.gameObject);
            }
            if(other.GetComponent<E_particle>().isAti == false)
            {
                gameManagerScript.initialMoveTimeValue = gameManagerScript.initialMoveTimeValue * f_particleEffect;
                playerScript.pointsForSingleCollision = playerScript.pointsForSingleCollision / f_particleEffect;
            }            
        }
        if(other.tag == "Player" && playerScript.isAti)
        {
            Destroy(other.gameObject);
        }
    }
}
