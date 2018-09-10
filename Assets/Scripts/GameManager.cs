using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public GameObject stoppingPointPrefab;
    public GameObject[] stoppingPoints;
    public GameObject[] wormies1R;
    public GameObject[] wormies2R;
    public GameObject[] wormies3R;
    public float Points;
    public float initialMoveTimeValue;
    public int moveCount = 0;
    public List<GameObject[]> wormies;
    public List<GameObject> existingStoppingPoints;
    public GameObject[] a_particles;
    public List<Vector2> stoppingPointPositions;
    public Vector2 aimedPosition = new Vector2(0,0);
    public Text pointsUI;

    private bool moveTime_toBeModified = false;
    private E_particle player_E_particle_Script;    
    private int playerHorizontalInput;
    private int playerVertical;    
    private List<Vector3> usedPositions = new List<Vector3>();
    private Vector3 playerPosition;

    private void Awake()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        a_particles = GameObject.FindGameObjectsWithTag("a_particle");
        player_E_particle_Script = player.GetComponent<E_particle>();
        wormies1R = GameObject.FindGameObjectsWithTag("Wormie(1r)");
        wormies2R = GameObject.FindGameObjectsWithTag("Wormie(2r)");
        wormies3R = GameObject.FindGameObjectsWithTag("Wormie(3r)");
        wormies = new List<GameObject[]> { wormies1R, wormies2R, wormies3R };

        GenerateStoppingPoints(wormies);
        stoppingPoints = GameObject.FindGameObjectsWithTag("StopPoint");
        DeleteTheOverLappingStoppingPoints();
        ListTheExistingStoppingPoints();
    }
    private void FixedUpdate()
    {
        a_particles = GameObject.FindGameObjectsWithTag("a_particle");
        if (a_particles.Length == 0)
        {            
            Debug.Log("Victory!");
        }
        if(player != null)
        {
            playerPosition = player.transform.position;
        }      
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player_E_particle_Script = player.GetComponent<E_particle>();
        }
        RegulateMovmentThroughWormies();
    }
    private void GenerateStoppingPoints(List<GameObject[]> generalArrayOfWormholes)
    {
        int wormholeVariationNumber = 1;
        foreach (GameObject[] wormHoleVariation in generalArrayOfWormholes)
        {
            foreach (GameObject wormie in wormHoleVariation)
            {
                if (wormie.transform.rotation.z == 0 || wormie.transform.rotation.z == 180)
                {
                    Vector3 firstSpawn = new Vector3(wormie.transform.position.x + 5 * wormholeVariationNumber, wormie.transform.position.y, 0);
                    Vector3 secondSpawn = new Vector3(wormie.transform.position.x - 5 * wormholeVariationNumber, wormie.transform.position.y, 0);
                    Instantiate(stoppingPointPrefab, firstSpawn, Quaternion.identity);
                    usedPositions.Add(firstSpawn);
                    Instantiate(stoppingPointPrefab, secondSpawn, Quaternion.identity);
                    usedPositions.Add(secondSpawn);
                }                
                else
                {
                        Vector3 firstSpawn = new Vector3(wormie.transform.position.x, wormie.transform.position.y + 5 * wormholeVariationNumber, 0);
                        Vector3 secondSpawn = new Vector3(wormie.transform.position.x, wormie.transform.position.y - 5 * wormholeVariationNumber, 0);
                        Instantiate(stoppingPointPrefab, firstSpawn, Quaternion.identity);
                        usedPositions.Add(firstSpawn);
                        Instantiate(stoppingPointPrefab, secondSpawn, Quaternion.identity);
                        usedPositions.Add(secondSpawn);
                }
            }
            wormholeVariationNumber += 1;
        }
    }
    void Start() {
        Points = 0;
        initialMoveTimeValue = player_E_particle_Script.moveTime;
    }
    private void ListTheExistingStoppingPoints()
    {
        List<Vector2> alreadyMentionedPositions = new List<Vector2>();
        foreach(GameObject stoppingPoint in stoppingPoints)
        {
            bool stoppingPointAlreadyExists = false;
            foreach(Vector2 position in alreadyMentionedPositions)
            {
                if((Vector2)stoppingPoint.transform.position == position)
                {
                    stoppingPointAlreadyExists = true;
                    break;
                }
            }
            if(stoppingPointAlreadyExists == false && stoppingPoint != null)
            {
                existingStoppingPoints.Add(stoppingPoint);
                alreadyMentionedPositions.Add(stoppingPoint.transform.position);
            }
        }
        stoppingPointPositions = alreadyMentionedPositions;
    }
    private void DeleteTheOverLappingStoppingPoints()
    {
        foreach (GameObject SubjectStoppingPoint in stoppingPoints)
        {
            for (int i = 0; i <= stoppingPoints.Length; i++)
            {
                if (SubjectStoppingPoint.transform.position == stoppingPoints[i].transform.position)
                {
                    if (SubjectStoppingPoint.gameObject == stoppingPoints[i].gameObject)
                    {
                        break;
                    }
                    else
                    {
                        List<GameObject> listToBe = new List<GameObject>(stoppingPoints);
                        Destroy(stoppingPoints[i]);
                        listToBe.Remove(stoppingPoints[i]);
                    }
                }
            }
        }
    }    
    private void RegulateMovmentThroughWormies ()
    {
        foreach (GameObject stoppingPoint in stoppingPoints)
        {
            player_E_particle_Script.canMove = false;
            if(stoppingPoint != null)
            {
                if (stoppingPoint.transform.position == playerPosition)
                {
                    if ((int)Input.GetAxisRaw("Horizontal") == 1)
                    {
                        for (int i = 0; i < 31; i++)
                        {
                            bool closestStopPointFound = false;
                            foreach (GameObject StoppingPoint in stoppingPoints)
                            {
                                float NewPosition = playerPosition.x + i;
                                if(StoppingPoint != null)
                                {
                                    if (StoppingPoint.transform.position.x == NewPosition && playerPosition.x != StoppingPoint.transform.position.x && StoppingPoint.transform.position.y == playerPosition.y)
                                    {
                                        //Debug.Log("A candidate Found at " + NewPosition + ".");

                                        player_E_particle_Script.movingDistance = (int)NewPosition - (int)playerPosition.x;
                                        float distance = NewPosition - playerPosition.x;
                                        float x = distance / 10;  
                                        if(aimedPosition != new Vector2(NewPosition, playerPosition.y))
                                        {
                                            aimedPosition = new Vector2(NewPosition, playerPosition.y);
                                        }
                                        if (distance <= 10)
                                        {
                                            float resistanceFactor = 0;
                                            foreach (GameObject wormie in wormies1R)
                                            {
                                                if (wormie.GetComponent<WormieResistor>().thisWormie.position == new Vector2((playerPosition.x + (wormie.GetComponent<WormieResistor>().thisWormie.length / 2)), playerPosition.y))
                                                {
                                                    resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor;                                                    
                                                }
                                            }
                                            player_E_particle_Script.moveTime = initialMoveTimeValue * x * resistanceFactor;
                                        }
                                        if (distance > 10)
                                        {
                                            float resistanceFactor = 0;                                            
                                            for(int f = 1; f <= wormies.Count - 1; f++)
                                            {
                                                foreach(GameObject wormie in wormies[f])
                                                {
                                                    if(wormie.GetComponent<WormieResistor>().thisWormie.length == distance && (Vector2)wormie.transform.position == new Vector2(playerPosition.x + ((f + 1) * 5), playerPosition.y))
                                                    {
                                                        resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor;                                                        
                                                    }
                                                }
                                            }
                                            //Debug.Log("resistanceFactor: " + resistanceFactor);
                                            player_E_particle_Script.moveTime = Mathf.Pow(2, (x - 1)) * initialMoveTimeValue * resistanceFactor;
                                        }                                     
                                        moveTime_toBeModified = true;
                                        player_E_particle_Script.canMove = true;
                                        closestStopPointFound = true;                                        
                                        break;
                                    }
                                }                                
                            }
                            if (closestStopPointFound)
                            {
                                break;
                            }
                        }
                    }
                    if ((int)Input.GetAxisRaw("Horizontal") == -1)
                    {
                        for (int i = 0; i > -31; i--)
                        {
                            bool closestStopPointFound = false;
                            foreach (GameObject StoppingPoint in stoppingPoints)
                            {
                                float NewPosition = playerPosition.x + i;
                                if (StoppingPoint != null)
                                {
                                    if (StoppingPoint.transform.position.x == NewPosition && playerPosition.x != StoppingPoint.transform.position.x && StoppingPoint.transform.position.y == playerPosition.y)
                                    {
                                        //Debug.Log("A candidate Found at " + NewPosition + ".");

                                        player_E_particle_Script.movingDistance = (int)playerPosition.x - (int)NewPosition;
                                        float distance = playerPosition.x - NewPosition;
                                        float x = distance / 10;
                                        if (aimedPosition != new Vector2(NewPosition, playerPosition.y))
                                        {
                                            aimedPosition = new Vector2(NewPosition, playerPosition.y);
                                        }
                                        if (distance <= 10)
                                        {
                                            float resistanceFactor = 0;
                                            foreach (GameObject wormie in wormies1R)
                                            {
                                                if (wormie.GetComponent<WormieResistor>().thisWormie.position == new Vector2((playerPosition.x +  (-(wormie.GetComponent<WormieResistor>().thisWormie.length / 2))), playerPosition.y))
                                                {
                                                    resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor;
                                                }
                                            }
                                            player_E_particle_Script.moveTime = initialMoveTimeValue * x * resistanceFactor;
                                        }
                                        if(distance > 10)
                                        {
                                            float resistanceFactor = 0;
                                            for (int f = 1; f <= wormies.Count - 1; f++)
                                            {
                                                foreach (GameObject wormie in wormies[f])
                                                {
                                                    if (wormie.GetComponent<WormieResistor>().thisWormie.length == distance && (Vector2)wormie.transform.position == new Vector2(playerPosition.x - ((f + 1) * 5), playerPosition.y))
                                                    {
                                                        resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor;
                                                    }
                                                }
                                            }
                                            player_E_particle_Script.moveTime = Mathf.Pow(2, (x - 1)) * initialMoveTimeValue * resistanceFactor;
                                        }
                                        moveTime_toBeModified = true;
                                        player_E_particle_Script.canMove = true;
                                        closestStopPointFound = true;                                        
                                        break;
                                    }
                                }                                
                            }
                            if (closestStopPointFound)
                            {
                                break;
                            }
                        }
                    }
                    if ((int)Input.GetAxisRaw("Vertical") == 1)
                    {
                        for (int i = 0; i < 31; i++)
                        {
                            bool closestStopPointFound = false;
                            foreach (GameObject StoppingPoint in stoppingPoints)
                            {
                                float NewPosition = playerPosition.y + i;
                                //Debug.Log("New position: (" + playerPosition.x + ", " + NewPosition);
                                //Debug.Log("Local position is equal to: " + StoppingPoint.transform.localPosition + " whereas position relative to world is equal to " + StoppingPoint.transform.position);
                                //Debug.Log("StoppingPoint: " + StoppingPoint.transform.position + " New position: (" + player.transform.position.x + ", " + NewPosition + " Player Position: " + player.transform.position);
                                if(StoppingPoint != null)
                                {
                                    if (StoppingPoint.transform.position.y == NewPosition && StoppingPoint.transform.position.y != playerPosition.y && StoppingPoint.transform.position.x == playerPosition.x)
                                    {
                                        //Debug.Log("A candidate Found at " + NewPosition + ".");
                                        player_E_particle_Script.movingDistance = (int)NewPosition - (int)playerPosition.y;
                                        float distance = NewPosition - playerPosition.y;
                                        float x = distance / 10;
                                        if (aimedPosition != new Vector2(playerPosition.x, NewPosition))
                                        {
                                            aimedPosition = new Vector2(playerPosition.x, NewPosition);
                                        }
                                        if (distance <= 10)
                                        {
                                            float resistanceFactor = 0;
                                            foreach (GameObject wormie in wormies1R)
                                            {
                                                if (wormie.GetComponent<WormieResistor>().thisWormie.position == new Vector2(playerPosition.x, playerPosition.y + (wormie.GetComponent<WormieResistor>().thisWormie.length / 2)))
                                                {
                                                    resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor;
                                                }
                                            }
                                            player_E_particle_Script.moveTime = initialMoveTimeValue * x * resistanceFactor;
                                        }
                                        if (distance > 10)
                                        {
                                            float resistanceFactor = 0;
                                            for (int f = 1; f <= wormies.Count - 1; f++)
                                            {
                                                foreach (GameObject wormie in wormies[f])
                                                {
                                                    if (wormie.GetComponent<WormieResistor>().thisWormie.length == distance && (Vector2)wormie.transform.position == new Vector2(playerPosition.x, playerPosition.y + ((f + 1) * 5)))
                                                    {
                                                        resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor;
                                                    }
                                                }
                                            }
                                            player_E_particle_Script.moveTime = Mathf.Pow(2, (x - 1)) * initialMoveTimeValue * resistanceFactor;
                                        }
                                        moveTime_toBeModified = true;
                                        player_E_particle_Script.canMove = true;
                                        closestStopPointFound = true;                                        
                                        break;
                                    }
                                }                                
                            }
                            if (closestStopPointFound)
                            {
                                break;
                            }
                        }
                    }
                    if ((int)Input.GetAxisRaw("Vertical") == -1)
                    {
                        for (int i = 0; i > -31; i--)
                        {
                            bool closestStopPointFound = false;
                            foreach (GameObject StoppingPoint in stoppingPoints)
                            {
                                float NewPosition = playerPosition.y + i;
                                if (StoppingPoint != null)
                                {
                                    if (StoppingPoint.transform.position.y == NewPosition && playerPosition.y != StoppingPoint.transform.position.y && StoppingPoint.transform.position.x == playerPosition.x)
                                    {
                                        //Debug.Log("A candidate Found at " + NewPosition + ".");

                                        player_E_particle_Script.movingDistance = (int)playerPosition.y - (int)NewPosition;
                                        float distance = playerPosition.y - NewPosition;
                                        float x = distance / 10;
                                        if (aimedPosition != new Vector2(playerPosition.x, NewPosition))
                                        {
                                            aimedPosition = new Vector2(playerPosition.x, NewPosition);
                                        }
                                        if (distance <= 10)
                                        {
                                            float resistanceFactor = 0;
                                            foreach (GameObject wormie in wormies1R)
                                            {
                                                if (wormie.GetComponent<WormieResistor>().thisWormie.position == new Vector2(playerPosition.x, playerPosition.y + (-(wormie.GetComponent<WormieResistor>().thisWormie.length / 2))))
                                                {
                                                    resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor;
                                                }
                                            }
                                            player_E_particle_Script.moveTime = initialMoveTimeValue * x * resistanceFactor;
                                        }
                                        if (distance > 10)
                                        {
                                            float resistanceFactor = 0;
                                            for (int f = 1; f <= wormies.Count - 1; f++)
                                            {
                                                foreach (GameObject wormie in wormies[f])
                                                {
                                                    if (wormie.GetComponent<WormieResistor>().thisWormie.length == distance && (Vector2)wormie.transform.position == new Vector2(playerPosition.x, playerPosition.y - ((f + 1) * 5)))
                                                    {
                                                        resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor;
                                                    }
                                                }
                                            }
                                            player_E_particle_Script.moveTime = Mathf.Pow(2, (x - 1)) * initialMoveTimeValue * resistanceFactor;
                                        }
                                        moveTime_toBeModified = true;
                                        player_E_particle_Script.canMove = true;
                                        closestStopPointFound = true;
                                        break;
                                    }
                                }                                
                            }
                            if (closestStopPointFound)
                            {
                                break;
                            }
                        }
                    }
                }
            }            
        }
    }
    void Update()
    {
        // to be coded   
    }
}
