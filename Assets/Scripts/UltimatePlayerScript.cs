using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class UltimatePlayerScript : MonoBehaviour {

    private bool hasChosenToDecrease = false;      
    private bool f_particleInstantiated = false; 
    private bool isOn_a_stoppingPoint = false;
    private int Turn;     
    private int numberOf_F_particlesInstantiated = 0; // a variable that tracks the number of e_particles instantiated
    private float inverseMoveTime; 
    private List<GameObject> instantiated_F_particles;
    private Rigidbody2D rb2d;
    private Vector2 _pos;
    private Vector2 targetPosition; // the target we are moving towards
    private Vector2 wormHoleTarget; // the wormhole target we are transforming to
    private Vector2 onPointPosition = new Vector2(0,0);    
    private GameObject wormholeObject;
    private GameObject[] stoppingPoints;

    protected int[] decayingTurns; // an array of turns in which, the p_particles, will decay

    public bool isOnAPoint = false; // a valuable that ensures that our destination is located on a stoppingpoint
    public bool canMove = true; // a boolean that determines wether a particle is allowed to move or not
    public bool wormHoleTransitation = false; // a boolean that determines wether a wormhole transition of this particle is about to happen or not
    public int horizontal = 0;          // the varriable containing the raw input of the moving buttons for the horizontal axis
    [Tooltip("E: 1 --- P:2 --- N:3 --- E_bonds: 4 --- Bonds: 5 --- Ati: 6")]
    public int typeOfParticle;
    public int vertical = 0;            // the varriable containing the raw input of the moving buttons for the vertical axis
    public int movmentCount = 0;        // counts the number of moves the player has made from the begining of the game
    public int movingDistance = 10;
    public float pointsForSingleCollision = 5;
    public float initialMoveTimeValue;
    public float moveTime;
    public GameObject Ati_prefab;       // a reference to the ati prefab
    public GameObject NE_particle;      // a reference to the NE_particle prefab
    public GameObject NP_particle;      // a reference to the NP_particle prefab
    public GameObject PE_particle;      // a reference to the PE_particle prefab
    public GameObject f_particlePrefab; // a reference to the F_particle prefab
    public GameObject binaryBond;       // a reference to the binary bonds prefab
    public GameObject tripleBond;       // a reference to the triple bonds prefab
    public GameManager gameManagerScript; // a referene to our gamemangerscript
    public Text pointsUI;               // a reference to the points UI

    void Awake()
    {        
        if(gameManagerScript == null) // if there are no references to the gamemanager
        {
            gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>(); // find the gamemanager and assign its script to this valuable
        }
        if(pointsUI == null) // if points UI doesn't exist ...
        {
            pointsUI = gameManagerScript.pointsUI; // set the value of points UI to the same thing as in gameManagerScript
        }
        initialMoveTimeValue = moveTime;
    }
    protected virtual void Start () {
        stoppingPoints = gameManagerScript.stoppingPoints;
        rb2d = GetComponent<Rigidbody2D>(); // the reference to the rigidbody
        _pos = GetComponent<Transform>().position; // the reference to the position
    }
    protected virtual void FixedUpdate()
    {
        if(gameManagerScript.turnInProgress && gameManagerScript.mode == 0)
        {
            RegulateMovmentThroughWormies();
        }
        if (wormHoleTransitation) // if a wormhole transition is happening...
        {
            targetPosition = wormHoleTarget; // change the targetposition to the wormhole position
        }
        if (gameManagerScript.turnInProgress && gameManagerScript.mode == 0)
        {
            inverseMoveTime = 1.0f / moveTime; // set the value for inversemovetime
            horizontal = (int)Input.GetAxisRaw("Horizontal"); // assign the input value to horizontal
            vertical = (int)Input.GetAxisRaw("Vertical");     // assign the input value to vertical   
            Move(horizontal, vertical, targetPosition); // let the player move, if moving is allowed
        }        
    }
    protected virtual void Update () {
        
    }
    protected void ModifyN_particleResistance() // this functions makes every wormie an n_particle goes through, more resistant
    {
        foreach (Vector2 position in gameManagerScript.stoppingPointPositions)
        {
            if (position == (Vector2)transform.position)
            {
                isOn_a_stoppingPoint = true;
                onPointPosition = position;
            }
        }
        Vector2 differenceVector = gameManagerScript.aimedPosition - onPointPosition;
        float chosenPointX = 0;
        float chosenPointY = 0;
        if (differenceVector.x == 0)
        {
            chosenPointY = differenceVector.y / 2;
            chosenPointX = 0;
        }
        if (differenceVector.y == 0)
        {
            chosenPointX = differenceVector.x / 2;
            chosenPointY = 0;
        }
        if (hasChosenToDecrease == true && onPointPosition == gameManagerScript.aimedPosition)
        {
            hasChosenToDecrease = false;
        }
        if (isOn_a_stoppingPoint == false)
        {
            for (int i = 0; i <= gameManagerScript.wormies.Count - 1; i++)
            {
                foreach (GameObject wormie in gameManagerScript.wormies[i])
                {
                    if (((Vector2)wormie.transform.position == onPointPosition + new Vector2(chosenPointX, chosenPointY)) && hasChosenToDecrease == false)
                    {
                        hasChosenToDecrease = true;
                        wormie.GetComponent<WormieResistor>().resistanceFactor *= 2;
                        wormie.GetComponent<WormieResistor>().thisWormie.resistanceValue *= 2;
                    }
                }
            }
        }
        isOn_a_stoppingPoint = false;
    }
    protected void Decay()
    {
        if (decayingTurns != null) // if the particle is indeed of type p and there is set a decayingturn...
        {
            foreach (int turn in decayingTurns) // foreach turn the particle should decay
            {
                if (f_particleInstantiated && turn != Turn) // if the f_particle is instantiated and the time is not right to instantiate
                {
                    f_particleInstantiated = false; // no further particle is instantiated at this turn
                }
                if (movmentCount == turn && f_particleInstantiated == false && turn != Turn) // if it's the right time to decay and there are no particles instantiated and this turn is not the last instantiation turn
                {
                    f_particleInstantiated = true; // an f_particle isinstantiated
                    Turn = turn; // this turn is the instantiation turn
                    Instantiate(f_particlePrefab, gameManagerScript.aimedPosition, Quaternion.identity); // instantiate the f_particle at the position the p_particle is moving towards without turning it
                    numberOf_F_particlesInstantiated += 1; // increase the number of the instantiated particles by one 
                }
                if (numberOf_F_particlesInstantiated == 3) // if there are enough f_particles instantiated 
                {
                    Destroy(gameObject); // destroy this gameobject
                }
            }
        }
    }
    protected int[] p_particleDecayingTurnFinder() // a function to randomly choose a bunch of turns for p_particles to decay in
    {
        int[] decayingTurns = new int[3];
        for (int i = 0; i <= 2; i++)
        {
            int turnValuable = Random.Range((i + 1) * (i + 1),(i + 5) * (i + 1));
            decayingTurns[i] = turnValuable;
        }
        foreach(int turn in decayingTurns)
        {
            //Debug.Log("Turn: " + turn);
        }
        return decayingTurns;
    }

    void Move(int horizontalInput, int verticalInput, Vector2 target) // here we define the most important function in the script, moving, that uses the verticalinput/horizontalinput to move twards the target
    {       
        if (target == Vector2.zero) // if we stand still and don't move towards anything...
        {
            target = _pos + new Vector2(horizontalInput, verticalInput) * movingDistance; // then define target as our very first destionation upon pressing down the unput buttons
        }
        if (target != Vector2.zero) // if the target, is not equal zero(aka. we either have moved or we are moving towards anything but (0,0))
        {
            Vector2 current = transform.position;      // note our current position      

            // the reason isOnAPoint == false is one of the conditions, is that the player freezes if it doesn't reach the target
            if (current == target || isOnAPoint == false) // if we have reached the target or we are moving towards a point that is not on a stoppingpoint
            {                
                target = current + new Vector2(horizontalInput, verticalInput) * movingDistance; // wait for some input and a new target
                canMove = true; // and allow the player to move
            }
            if (current == Vector2.zero || isOnAPoint == false) // if we are located on(0,0) or we are moving towards a point that is not on a stopping point
            {                
                target = current + new Vector2(horizontalInput, verticalInput) * movingDistance; // wait for some input and a new target
                canMove = true; // let the player move
            }            
        }               
        if(wormHoleTransitation && wormholeObject.GetComponent<TheWormHoleScript>().isActive) // if we are at the middle of a wormholetransition and the wormhole is active
        {
            target = wormHoleTarget; // our target will be the other side of the wormhole
            transform.position = new Vector2(target.x, target.y); // and we will magically move towards the other side of the wormhole
        }

        if (verticalInput != 0 || horizontalInput != 0) // if either the vertical or the horizontal buttons are pressed
        {
            foreach (GameObject stoppingPoint in gameManagerScript.stoppingPoints) // check if the point the player is asking us to move towards, is located on a stopping point
            {
                if (stoppingPoint != null)
                {
                    //Vector2 target = new Vector2(transform.position.x, transform.position.y) + new Vector2(horizontal, vertical) * movingDistance;
                    if (stoppingPoint.transform.position == new Vector3(target.x, target.y, 0))
                    {
                        isOnAPoint = true;
                    }
                }
            }
            if(isOnAPoint == false) // and if it's(the point we should move towards) not(located on a stopping point)...
            {
                canMove = true; // the player can move again and choose another direction
            }
            if(wormHoleTransitation) // if we are at the middle of a wormhole transition
            {
                canMove = false; // we can not move
                if((Vector2)transform.position == target && (Vector2)transform.position == targetPosition) // if we are located on the other side of the wormhole...
                {
                    wormHoleTransitation = false; // wormhole transtion is over
                    canMove = true; // we will be able to move
                }
            }
            if (canMove && isOnAPoint && wormHoleTransitation == false) // if we can move, and we are moving towards a stoppingpoint and we are not at the middle of a wormhole transtion
            {
                // smoothly move towards our target
                StartCoroutine(Smoothing(target)); // this function runs twice when exiting a wormHole
                movmentCount += 1; // track the number of turns we have moved
                gameManagerScript.currentTurn += 1;
                canMove = false; // make sure that we are not allowed to move(until we have reached the destination)
                isOnAPoint = false;// ensure that we accidentaly don't choose a moving point that is located on a stopping point
            }
        }
        if(wormHoleTransitation == false) // if there are no wormhole transitions going on...
        {
            targetPosition = target; // update our outer valuable for our chosen destination
            // this value will update every turn, until we have reached the destionation
        }            
    }

    private IEnumerator Smoothing(Vector3 end) // a magical function(like totally), that will let you move a path towards point smoothly
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon && wormHoleTransitation == false)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime * Time.deltaTime);
            rb2d.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }
    private void RegulateMovmentThroughWormies()
    {
        Vector2 playerPosition = transform.position;
        foreach (GameObject stoppingPoint in stoppingPoints) // for every stoppingpoint in the scene
        {
            canMove = false; // first disable the player to move
            if (stoppingPoint != null) // if the stopping point actually exists
            {
                if ((Vector2)stoppingPoint.transform.position == playerPosition) // if the player is located on a stopping point
                {
                    if ((int)Input.GetAxisRaw("Horizontal") == 1) // if the player wishes to move eight
                    {
                        for (int i = 0; i < 31; i++) // Note: the reason i is equall to 31 is that, i checks all the 31 positions to the right of the player, if more positions are to be added, change the number
                        {
                            bool closestStopPointFound = false; // a boolean that determines wether a stopping point has been found or not
                            foreach (GameObject StoppingPoint in stoppingPoints) // for every stopping point in the scene
                            {
                                float NewPosition = playerPosition.x + i; // the new position is i units away
                                if (StoppingPoint != null)
                                {
                                    // if the stopping point has the same value as the new position and the player is not located on this stopping point and the stoppingpoint has the same y value as the player(aka. the suting stopping point has been found)
                                    if (StoppingPoint.transform.position.x == NewPosition && playerPosition.x != StoppingPoint.transform.position.x && StoppingPoint.transform.position.y == playerPosition.y)
                                    {
                                        movingDistance = (int)NewPosition - (int)playerPosition.x; // set the moving distance
                                        float distance = NewPosition - playerPosition.x; // calculate the distance
                                        float x = distance / 10; // calculate the x movetime variable
                                        if (gameManagerScript.aimedPosition != new Vector2(NewPosition, playerPosition.y)) // if the position we are aiming for is not equal to the new position(aka. if it's not update it, update it!)
                                        {
                                            gameManagerScript.aimedPosition = new Vector2(NewPosition, playerPosition.y); // set it to that
                                        }
                                        if (distance <= 10) // if we are moving through a distance, that is less or equal than 10
                                        {
                                            float resistanceFactor = 0; // define a variable for the resistance factor
                                            foreach (GameObject wormie in gameManagerScript.wormies1R) // foreach wormie of type 1
                                            {
                                                // if the wormie is this wormie...
                                                if (wormie.GetComponent<WormieResistor>().thisWormie.position == new Vector2((playerPosition.x + (wormie.GetComponent<WormieResistor>().thisWormie.length / 2)), playerPosition.y))
                                                {
                                                    resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor; // store its resistance factor                                                     
                                                }
                                            }
                                            moveTime = initialMoveTimeValue * x * resistanceFactor; // modify the movetime value
                                        }
                                        // if we are dealing with a distance that is more than 10
                                        if (distance > 10)
                                        {
                                            float resistanceFactor = 0; // define a variable for the resistance factor                                       
                                            // for every single wormie that is not of type 1
                                            for (int f = 1; f <= gameManagerScript.wormies.Count - 1; f++)
                                            {
                                                foreach (GameObject wormie in gameManagerScript.wormies[f])
                                                {
                                                    // if the wormie is this wormie
                                                    if (wormie.GetComponent<WormieResistor>().thisWormie.length == distance && (Vector2)wormie.transform.position == new Vector2(playerPosition.x + ((f + 1) * 5), playerPosition.y))
                                                    {
                                                        resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor; // store its resistance factor somewhere                                                   
                                                    }
                                                }
                                            }
                                            // modify the movetime according to all those factors
                                            moveTime = Mathf.Pow(2, (x - 1)) * initialMoveTimeValue * resistanceFactor;
                                        }
                                        // enable the player to move
                                        canMove = true;
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

                                        movingDistance = (int)playerPosition.x - (int)NewPosition;
                                        float distance = playerPosition.x - NewPosition;
                                        float x = distance / 10;
                                        if (gameManagerScript.aimedPosition != new Vector2(NewPosition, playerPosition.y))
                                        {
                                            gameManagerScript.aimedPosition = new Vector2(NewPosition, playerPosition.y);
                                        }
                                        if (distance <= 10)
                                        {
                                            float resistanceFactor = 0;
                                            foreach (GameObject wormie in gameManagerScript.wormies1R)
                                            {
                                                if (wormie.GetComponent<WormieResistor>().thisWormie.position == new Vector2((playerPosition.x + (-(wormie.GetComponent<WormieResistor>().thisWormie.length / 2))), playerPosition.y))
                                                {
                                                    resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor;
                                                }
                                            }
                                            moveTime = initialMoveTimeValue * x * resistanceFactor;
                                        }
                                        if (distance > 10)
                                        {
                                            float resistanceFactor = 0;
                                            for (int f = 1; f <= gameManagerScript.wormies.Count - 1; f++)
                                            {
                                                foreach (GameObject wormie in gameManagerScript.wormies[f])
                                                {
                                                    if (wormie.GetComponent<WormieResistor>().thisWormie.length == distance && (Vector2)wormie.transform.position == new Vector2(playerPosition.x - ((f + 1) * 5), playerPosition.y))
                                                    {
                                                        resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor;
                                                    }
                                                }
                                            }
                                            moveTime = Mathf.Pow(2, (x - 1)) * initialMoveTimeValue * resistanceFactor;
                                        }
                                        canMove = true;
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
                                if (StoppingPoint != null)
                                {
                                    if (StoppingPoint.transform.position.y == NewPosition && StoppingPoint.transform.position.y != playerPosition.y && StoppingPoint.transform.position.x == playerPosition.x)
                                    {
                                        //Debug.Log("A candidate Found at " + NewPosition + ".");
                                        movingDistance = (int)NewPosition - (int)playerPosition.y;
                                        float distance = NewPosition - playerPosition.y;
                                        float x = distance / 10;
                                        if (gameManagerScript.aimedPosition != new Vector2(playerPosition.x, NewPosition))
                                        {
                                            gameManagerScript.aimedPosition = new Vector2(playerPosition.x, NewPosition);
                                        }
                                        if (distance <= 10)
                                        {
                                            float resistanceFactor = 0;
                                            foreach (GameObject wormie in gameManagerScript.wormies1R)
                                            {
                                                if (wormie.GetComponent<WormieResistor>().thisWormie.position == new Vector2(playerPosition.x, playerPosition.y + (wormie.GetComponent<WormieResistor>().thisWormie.length / 2)))
                                                {
                                                    resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor;
                                                }
                                            }
                                            moveTime = initialMoveTimeValue * x * resistanceFactor;
                                        }
                                        if (distance > 10)
                                        {
                                            float resistanceFactor = 0;
                                            for (int f = 1; f <= gameManagerScript.wormies.Count - 1; f++)
                                            {
                                                foreach (GameObject wormie in gameManagerScript.wormies[f])
                                                {
                                                    if (wormie.GetComponent<WormieResistor>().thisWormie.length == distance && (Vector2)wormie.transform.position == new Vector2(playerPosition.x, playerPosition.y + ((f + 1) * 5)))
                                                    {
                                                        resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor;
                                                    }
                                                }
                                            }
                                            moveTime = Mathf.Pow(2, (x - 1)) * initialMoveTimeValue * resistanceFactor;
                                        }
                                        canMove = true;
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

                                        movingDistance = (int)playerPosition.y - (int)NewPosition;
                                        float distance = playerPosition.y - NewPosition;
                                        float x = distance / 10;
                                        if (gameManagerScript.aimedPosition != new Vector2(playerPosition.x, NewPosition))
                                        {
                                            gameManagerScript.aimedPosition = new Vector2(playerPosition.x, NewPosition);
                                        }
                                        if (distance <= 10)
                                        {
                                            float resistanceFactor = 0;
                                            foreach (GameObject wormie in gameManagerScript.wormies1R)
                                            {
                                                if (wormie.GetComponent<WormieResistor>().thisWormie.position == new Vector2(playerPosition.x, playerPosition.y + (-(wormie.GetComponent<WormieResistor>().thisWormie.length / 2))))
                                                {
                                                    resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor;
                                                }
                                            }
                                            moveTime = initialMoveTimeValue * x * resistanceFactor;
                                        }
                                        if (distance > 10)
                                        {
                                            float resistanceFactor = 0;
                                            for (int f = 1; f <= gameManagerScript.wormies.Count - 1; f++)
                                            {
                                                foreach (GameObject wormie in gameManagerScript.wormies[f])
                                                {
                                                    if (wormie.GetComponent<WormieResistor>().thisWormie.length == distance && (Vector2)wormie.transform.position == new Vector2(playerPosition.x, playerPosition.y - ((f + 1) * 5)))
                                                    {
                                                        resistanceFactor = wormie.GetComponent<WormieResistor>().resistanceFactor;
                                                    }
                                                }
                                            }
                                            moveTime = Mathf.Pow(2, (x - 1)) * initialMoveTimeValue * resistanceFactor;
                                        }
                                        canMove = true;
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
    protected virtual void OnTriggerEnter2D(Collider2D other) // if this object is triggering another object
    {
        if(other.tag == "wormHole")
        {
            if(other.GetComponent<TheWormHoleScript>().isActive && other.GetComponent<TheWormHoleScript>().wormHoleBond.GetComponent<TheWormHoleScript>().isActive) // if both a wormhole and its pair are active...
            {
                wormholeObject = other.gameObject;   // define the wormhole object as the triggering wormhole             
                other.GetComponent<TheWormHoleScript>().wormHoleBond.GetComponent<TheWormHoleScript>().isActive = false; // deactivate the wormhole's bond
                wormHoleTransitation = true; // a wormhole transition is going on
                Vector2 bondPosition = other.GetComponent<TheWormHoleScript>().wormHoleBond.transform.position; // note the position of the wormhole
                wormHoleTarget = bondPosition; // the target of the gameobject, will be the wormhole
            }
        }
        if (other.tag == "a_particle")
        {
            float points = new float();
            points = other.GetComponent<UltimateA_particleScript>().pointsForExplosion;
            gameManagerScript.Points += pointsForSingleCollision * points;
            pointsUI.text = gameManagerScript.Points.ToString();
            Destroy(other.gameObject);
        }
    }
}
