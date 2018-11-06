using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public bool turnInProgress = true; // a valuable describing the state of turn, if true player is playing, if false, the game is automatically running
    public int currentTurn = 0; // the number used to determine the number of movments by player in filthy mode
    public int autoTurn = 0; // the number used to determine the number of movments by player in auto mode
    public int mode = 0; // 1 for automized 0 for filthy
    public int turnDuration = 2;
    public int atomiccurreny = 0;
    public int currencyCoefficient = 10; // a coefficient used to determine the numeral value of every currency gained
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
    public List<GameObject> allGOs;
    public Vector2 aimedPosition = new Vector2(0,0); // a position we are aiming to move towards, this variable is modified int he player(e_particle) script.
    public Text pointsUI; // a reference to our points UI

    //private bool moveTime_toBeModified = false; // 
    private float NextTimeToMove;
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
        turnInProgress = true;
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
    void Update()
    {
        Debug.Log("Mode: " + mode);
        Debug.Log("Turn in progress: " + turnInProgress);
        // to be coded   
        if(Input.GetButtonDown("Change"))
        {
            if(mode == 0)
            {
                mode = 1;
            }
            else if(mode == 1)
            {
                mode = 0;
            }
        }
        if (mode == 1)
        {
            if (Input.GetButtonDown("Submit") && turnInProgress)
            {
                NextAutoTurn();
            }
        }
        if(Time.time >= NextTimeToMove && turnInProgress == false)
        {
            turnInProgress = true;
        }
        InCaseMouseClicked();        
    }
    public void NextAutoTurn()
    {
        turnInProgress = false;
        autoTurn += 1;
        NextTimeToMove = turnDuration + Time.time;
    }
    public Vector2[] FindWormiesAround(GameObject thisObject)
    {
        GameObject upperWormie = null;
        GameObject bottomWormie = null;
        GameObject rightWormie = null;
        GameObject leftWormie = null;
        Vector2[] neighbouringWormies = new Vector2[4];
        for (int i = 0; i <= wormies.Count - 1; i++)
        {
            foreach (GameObject wormie in wormies[i])
            {
                Vector2 _WormiePos = wormie.transform.position;
                Vector2 _pos = thisObject.transform.position;
                int rotationType = wormie.GetComponent<WormieResistor>().thisWormie.rotationType;
                if (upperWormie == null && _pos.y + (i + 1) * 5 == _WormiePos.y && _pos.x == _WormiePos.x && rotationType == 1)
                {
                    upperWormie = wormie;
                }
                if (bottomWormie == null && _pos.y - (i + 1) * 5 == _WormiePos.y && _pos.x == _WormiePos.x && rotationType == 1)
                {
                    bottomWormie = wormie;
                }
                if (rightWormie == null && _pos.x + (i + 1) * 5 == _WormiePos.x && _pos.y == _WormiePos.y && rotationType == 0)
                {
                    rightWormie = wormie;
                }
                if (leftWormie == null && _pos.x - (i + 1) * 5 == _WormiePos.x && _pos.y == _WormiePos.y && rotationType == 0)
                {
                    leftWormie = wormie;
                }
            }
        }
        if (upperWormie != null)
        {
            neighbouringWormies[0] = upperWormie.transform.position;
        }
        if (bottomWormie != null)
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
    void InCaseMouseClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); // this code can also be used for hovering
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit)
            {
                if (hit.transform.tag == "Batterium") // if dealing with a batterium
                {
                    GameObject[] batteriums = GameObject.FindGameObjectsWithTag("Batterium");
                    foreach (GameObject batterium in batteriums)
                    {
                        if (batterium.transform.position == hit.transform.position)
                        {
                            Batterium batteriumScript = batterium.GetComponent<Batterium>();
                            atomiccurreny += batteriumScript.exploitationValue + (batteriumScript.storedCash * currencyCoefficient);
                            Instantiate(batteriumScript.explosionEffect, batterium.transform.position, Quaternion.identity);
                            if(mode == 1)
                            {
                                NextAutoTurn();
                            }
                            Destroy(batterium);
                        }
                    }
                }
                if (hit.transform.tag == "OrbirRing_batterium") // if dealing with the batterium orbit
                {
                    GameObject[] batteriums = GameObject.FindGameObjectsWithTag("Batterium");
                    foreach (GameObject batterium in batteriums)
                    {
                        for (int i = 0; i <= batterium.transform.childCount - 1; i++)
                        {
                            Transform child_trans = batterium.transform.GetChild(i).transform;
                            if (child_trans.position == hit.transform.position && child_trans.tag == "OrbirRing_batterium")
                            {
                                if (batterium.GetComponent<Batterium>().storedCash != 0)
                                {
                                    Debug.Log(batterium.GetComponent<Batterium>().storedCash + " credits available!");
                                    atomiccurreny += (batterium.GetComponent<Batterium>().storedCash * currencyCoefficient);
                                    batterium.GetComponent<Batterium>().storedCash = 0;
                                    for (int j = 0; j <= child_trans.childCount - 1; j++)
                                    {
                                        child_trans.GetChild(j).gameObject.SetActive(false);
                                    }
                                    if (mode == 1)
                                    {
                                        NextAutoTurn();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    void GetAllChildren(Transform current, List<GameObject> arrayToFill)
    {
        arrayToFill.Add(current.gameObject);

        for (int i = 0; i < current.childCount; i++)
            GetAllChildren(current.GetChild(i), arrayToFill);
    }
    public List<GameObject> GetAllObjectsInScene()
    {
        List<GameObject> allGOs = new List<GameObject>();
        GameObject[] rootGOs = gameObject.scene.GetRootGameObjects();

        for (int i = 0; i < rootGOs.Length; i++)
            GetAllChildren(rootGOs[i].transform, allGOs);

        return allGOs;
    }
}
