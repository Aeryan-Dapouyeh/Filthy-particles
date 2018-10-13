using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public bool turnInProgress = true;
    public int currentTurn = 0;
    public int mode = 0;
    public GameObject player; // a reference to the player
    public GameObject stoppingPointPrefab; // a prefab for the stoppingpoint generator to generate stopping points
    public GameObject[] stoppingPoints; // an array containing all stopping points
    public GameObject[] wormies1R; // an array containing all wormies with a length of 1 * standard length
    public GameObject[] wormies2R; // an array containing all wormies with a length of 2 * standard length
    public GameObject[] wormies3R; // an array containing all wormies with a length of 3 * standard length
    public float Points; // a variable containing the points earned by the player
    public float initialMoveTimeValue; // the value entered as move time in the editor
    public List<GameObject[]> wormies; // a list of arrays that contain all arrays containing a varitey of different types of wormholes
    public List<GameObject> existingStoppingPoints; // a list of existing stopping points
    public GameObject[] a_particles; // a list of all a_particles in the scene
    public List<Vector2> stoppingPointPositions; // a list of all stopping point positions
    public Vector2 aimedPosition = new Vector2(0,0); // a position we are aiming to move towards, this variable is modified int he player(e_particle) script.
    public Text pointsUI; // a reference to our points UI

    //private bool moveTime_toBeModified = false; // 
    private p_particleScript player_P_particle_Script; // a reference to the player script of type p
    private E_particleScript player_E_particle_Script; // a reference to the player script of type e
    private N_particleScript player_N_particle_Script; // a reference to the player script of type n
    private BondsParticleScript player_Bond_particle_Script; // a reference to the player script of type bond
    private Ati_particleScript player_Ati_particle_Script; // a reference to the player script of type ati
    private eBond_particleScript player_eBond_particle_Script; // a reference to the player script of type bond
    private UltimatePlayerScript Player_script; // the reference to the player script
    private List<Vector3> usedPositions = new List<Vector3>(); // a variable used to store the positions in which a stopping point has been created
    private Vector3 playerPosition; // a reference to the player's position

    private void Awake()
    {
        if(player == null) // if there is no player
        {
            player = GameObject.FindGameObjectWithTag("Player"); // find the player
        }
        a_particles = GameObject.FindGameObjectsWithTag("a_particle"); // find a_particles
        wormies1R = GameObject.FindGameObjectsWithTag("Wormie(1r)"); // find the wormies of type one
        wormies2R = GameObject.FindGameObjectsWithTag("Wormie(2r)"); // find the wormies of type two
        wormies3R = GameObject.FindGameObjectsWithTag("Wormie(3r)"); // find the wormies of type three
        wormies = new List<GameObject[]> { wormies1R, wormies2R, wormies3R }; // list all the arrays contaning wormies, MUST BE DONE MANUALLY IN CODE

        FindThePlayerScript(player); // find player's script
        GenerateStoppingPoints(wormies); // generate stopping points
        stoppingPoints = GameObject.FindGameObjectsWithTag("StopPoint"); // find all the stopping points
        DeleteTheOverLappingStoppingPoints(); // delete the extra stopping points
        ListTheExistingStoppingPoints(); // list all the stopping points that exist 
    }
    private void FixedUpdate()
    {
        a_particles = GameObject.FindGameObjectsWithTag("a_particle"); // every turn, find a_particles
        if (a_particles.Length == 0) // if there are no a_particles, decalre victory
        {            
            Debug.Log("Victory!");
        }
        if(player != null) // if the player exists
        {
            playerPosition = player.transform.position; // update the player's position every turn
        }      
        if(player == null) // if the player doesn't exist
        {
            player = GameObject.FindGameObjectWithTag("Player"); // find the player
            FindThePlayerScript(player); // and establish a reference to its script
        }
    }
    // every filthy_particle has a certain script designed only for it, yet all of them are children of the main player script, this functions determines which script is used and establishes a reference to it
    private void FindThePlayerScript(GameObject player) 
    {
        int type = player.GetComponent<UltimatePlayerScript>().typeOfParticle;
        if(type == 1)
        {
            player_E_particle_Script = player.GetComponent<E_particleScript>();
            Player_script = player_E_particle_Script;
        }
        if (type == 2)
        {
            player_P_particle_Script = player.GetComponent<p_particleScript>();
            Player_script = player_P_particle_Script;
        }
        if (type == 3)
        {
            player_N_particle_Script = player.GetComponent<N_particleScript>();
            Player_script = player_N_particle_Script;
        }
        if(type == 4)
        {
            player_eBond_particle_Script = player.GetComponent<eBond_particleScript>();
            Player_script = player_eBond_particle_Script;
        }
        if(type == 5)
        {
            player_Bond_particle_Script = player.GetComponent<BondsParticleScript>();
            Player_script = player_Bond_particle_Script;
        }
        if(type == 6)
        {
            player_Ati_particle_Script = player.GetComponent<Ati_particleScript>();
            Player_script = player_Ati_particle_Script;
        }
    }
    // this function generates stopping points at the beginning of the game
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
        Points = 0; // at the beginning, the player must have no points at all
        initialMoveTimeValue = Player_script.moveTime; // the value for moveTime that is entered in the editor wil be saved in this valuable
    }
    // this function makes a list of all existing points where a stopping point, can be found
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
    // this function, deletes all but one stopping points that are located in the same location
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
    /* private void RegulateMovmentThroughWormies ()
    {
        foreach (GameObject stoppingPoint in stoppingPoints) // for every stoppingpoint in the scene
        {
            Player_script.canMove = false; // first disable the player to move
            if(stoppingPoint != null) // if the stopping point actually exists
            {
                if (stoppingPoint.transform.position == playerPosition) // if the player is located on a stopping point
                {
                    if ((int)Input.GetAxisRaw("Horizontal") == 1) // if the player wishes to move eight
                    {
                        for (int i = 0; i < 31; i++) // Note: the reason i is equall to 31 is that, i checks all the 31 positions to the right of the player, if more positions are to be added, change the number
                        {
                            bool closestStopPointFound = false; // a boolean that determines wether a stopping point has been found or not
                            foreach (GameObject StoppingPoint in stoppingPoints) // for every stopping point in the scene
                            {
                                float NewPosition = playerPosition.x + i; // the new position is i units away
                                if(StoppingPoint != null)
                                {
                                    // if the stopping point has the same value as the new position and the player is not located on this stopping point and the stoppingpoint has the same y value as the player(aka. the suting stopping point has been found)
                                    if (StoppingPoint.transform.position.x == NewPosition && playerPosition.x != StoppingPoint.transform.position.x && StoppingPoint.transform.position.y == playerPosition.y)
                                    {
                                        Player_script.movingDistance = (int)NewPosition - (int)playerPosition.x; // set the moving distance
                                        float distance = NewPosition - playerPosition.x; // calculate the distance
                                        float x = distance / 10; // calculate the x movetime variable
                                        if(aimedPosition != new Vector2(NewPosition, playerPosition.y)) // if the position we are aiming for is not equal to the new position(aka. if it's not update it, update it!)
                                        {
                                            aimedPosition = new Vector2(NewPosition, playerPosition.y); // set it to that
                                        }
                                        if (distance <= 10) // if we are moving through a distance, that is less or equal than 10
                                        {
                                            float resistanceFactor = 0; // define a variable for the resistance factor
                                            foreach (GameObject wormie in wormies1R) // foreach wormie of type 1
                                            {
                                                // if the wormie is this wormie...
                                                if (wormie.GetComponent<WormieResistor>().thisWormie.position == new Vector2((playerPosition.x + (wormie.GetComponent<WormieResistor>().thisWormie.length / 2)), playerPosition.y))
                                                {
                                                    resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor; // store its resistance factor                                                     
                                                }
                                            }
                                            Player_script.moveTime = initialMoveTimeValue * x * resistanceFactor; // modify the movetime value
                                        }
                                        // if we are dealing with a distance that is more than 10
                                        if (distance > 10)
                                        {
                                            float resistanceFactor = 0; // define a variable for the resistance factor                                       
                                            // for every single wormie that is not of type 1
                                            for (int f = 1; f <= wormies.Count - 1; f++)
                                            {
                                                foreach(GameObject wormie in wormies[f])
                                                {
                                                    // if the wormie is this wormie
                                                    if(wormie.GetComponent<WormieResistor>().thisWormie.length == distance && (Vector2)wormie.transform.position == new Vector2(playerPosition.x + ((f + 1) * 5), playerPosition.y))
                                                    {
                                                        resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor; // store its resistance factor somewhere                                                   
                                                    }
                                                }
                                            }
                                            // modify the movetime according to all those factors
                                            Player_script.moveTime = Mathf.Pow(2, (x - 1)) * initialMoveTimeValue * resistanceFactor;
                                        }            
                                        // enable the player to move
                                        Player_script.canMove = true;
                                        // declare the closest point found
                                        closestStopPointFound = true;                                        
                                        break;
                                    }
                                }                                
                            }
                            if (closestStopPointFound) // if the closest stoppingpoint is found...
                            {
                                break; // no reason for wasting our precious computer power, break out of the loop
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

                                        Player_script.movingDistance = (int)playerPosition.x - (int)NewPosition;
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
                                            Player_script.moveTime = initialMoveTimeValue * x * resistanceFactor;
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
                                            Player_script.moveTime = Mathf.Pow(2, (x - 1)) * initialMoveTimeValue * resistanceFactor;
                                        }
                                        Player_script.canMove = true;
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
                                        Player_script.movingDistance = (int)NewPosition - (int)playerPosition.y;
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
                                            Player_script.moveTime = initialMoveTimeValue * x * resistanceFactor;
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
                                            Player_script.moveTime = Mathf.Pow(2, (x - 1)) * initialMoveTimeValue * resistanceFactor;
                                        }
                                        Player_script.canMove = true;
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

                                        Player_script.movingDistance = (int)playerPosition.y - (int)NewPosition;
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
                                            Player_script.moveTime = initialMoveTimeValue * x * resistanceFactor;
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
                                            Player_script.moveTime = Mathf.Pow(2, (x - 1)) * initialMoveTimeValue * resistanceFactor;
                                        }
                                        Player_script.canMove = true;
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
    }*/
    void Update()
    {
        // to be coded   
        if(Input.GetButtonDown("Jump"))
        {
            Debug.Log(mode);
            if(mode == 0)
            {
                mode = 1;
            }
            else if(mode == 1)
            {
                mode = 0;
            }
        }
    }
}
