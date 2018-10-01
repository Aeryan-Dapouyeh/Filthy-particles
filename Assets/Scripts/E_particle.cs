using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class E_particle : MonoBehaviour {

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

    protected int[] decayingTurns; // an array of turns in which, the p_particles, will decay

    public bool isOnAPoint = false; // a valuable that ensures that our destination is located on a stoppingpoint
    public bool canMove = true; // a boolean that determines wether a particle is allowed to move or not
    public bool wormHoleTransitation = false; // a boolean that determines wether a wormhole transition of this particle is about to happen or not
    public bool isAti; // is this particle an ati?
    public bool isAnE_particle = false; // is this particle an e_particle?
    public bool isAnN_particle = false; // is this particle an n_particle?
    public bool isAnP_particle = false; // is this particle a p_particle?
    public bool isA_binaryBond = false; // is this particle a binary bond?
    public bool isA_tripleBond = false; // is this particle a triple bond?
    public bool isA_PE_Bond = false;    // is this particle a PE bond?
    public bool isA_NE_bond = false;    // is this particle a NE bond?
    public bool isA_NP_bond = false;    // is this particle a NP bond?
    public int horizontal = 0;          // the varriable containing the raw input of the moving buttons for the horizontal axis
    public int vertical = 0;            // the varriable containing the raw input of the moving buttons for the vertical axis
    public int movmentCount = 0;        // counts the number of moves the player has made from the begining of the game
    public int movingDistance = 10;
    public float pointsForSingleCollision = 5;
    public float moveTime;
    public Sprite[] A_particleSprites;  // references to different a_particle sprites that would help us calculate the points
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
    }
    protected virtual void Start () {        
        rb2d = GetComponent<Rigidbody2D>(); // the reference to the rigidbody
        _pos = GetComponent<Transform>().position; // the reference to the position
        if(isAnP_particle) // if this is an p_particle
        {
            decayingTurns = p_particleDecayingTurnFinder(); // set random decaying turns
            gameManagerScript.initialMoveTimeValue *= 3; // set a speed value for it 
        }
        if(isAnN_particle) // if the particle is of type N
        {
            gameManagerScript.initialMoveTimeValue /= 2; // set a speed for it
        }
        if(isA_PE_Bond) // if the particle is a pe_bond...
        {
            gameManagerScript.initialMoveTimeValue *= 2; // set a speed for it
        }
        if(isA_NE_bond) // if the particle is an ne bond
        {
            gameManagerScript.initialMoveTimeValue /= 1.5f; // set a speed for it
        }
        if(isAti) // if the particle is an ati
        {
            gameManagerScript.initialMoveTimeValue /= 4f; // set a speed for it
        }
    }
    protected virtual void FixedUpdate()
    {
        if(isAnN_particle) // if the particle is of type N
        {
            ModifyN_particleResistance(); // make the wormies you go through slower 
        }
    }
    protected virtual void Update () {
        if(wormHoleTransitation) // if a wormhole transition is happening...
        {
            targetPosition = wormHoleTarget; // change the targetposition to the wormhole position
        }

        if(isAnP_particle) // if this particle is of type p...
        {
            Decay(); // then decay
        }

        inverseMoveTime = 1.0f / moveTime; // set the value for inversemovetime
        horizontal = (int)Input.GetAxisRaw("Horizontal"); // assign the input value to horizontal
        vertical = (int)Input.GetAxisRaw("Vertical");     // assign the input value to vertical   
        Move(horizontal, vertical, targetPosition); // let the player move, if moving is allowed
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
    protected virtual void OnTriggerEnter2D(Collider2D other) // if this object is triggering another object
    {
        if(other.tag == "p_particle")
        {
            if(isAnE_particle)
            {
                GameObject PE_par = Instantiate(PE_particle, other.transform);
                PE_par.GetComponent<E_particle>().isA_PE_Bond = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }    
            if(isA_NE_bond)
            {
                GameObject ati = Instantiate(Ati_prefab, other.transform);
                ati.GetComponent<E_particle>().isAti = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
        if(other.tag == "N_particle")
        {
            if(isAnE_particle)
            {
                GameObject NE_par = Instantiate(NE_particle, other.transform);
                NE_par.GetComponent<E_particle>().isA_NE_bond = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            if(isA_PE_Bond)
            {
                GameObject ati = Instantiate(Ati_prefab, other.transform);
                ati.GetComponent<E_particle>().isAti = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            if(isAnP_particle)
            {
                GameObject NP_par = Instantiate(NP_particle, other.transform);
                NP_par.GetComponent<E_particle>().isA_NP_bond = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
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
            if (other.GetComponent<SpriteRenderer>().sprite == A_particleSprites[0])
            {
                points = other.GetComponent<A_normalParticleController>().thisA_particle.pointsPerExplosion;
            }
            if (other.GetComponent<SpriteRenderer>().sprite == A_particleSprites[1])
            {
                points = other.GetComponent<A_1particleController>().thisA_particle.pointsPerExplosion;
            }
            if (other.GetComponent<SpriteRenderer>().sprite == A_particleSprites[2])
            {
                points = other.GetComponent<A_2particleController>().thisA_particle.pointsPerExplosion;
            }
            if (other.GetComponent<SpriteRenderer>().sprite == A_particleSprites[3])
            {
                points = other.GetComponent<A_3particleController>().thisA_particle.pointsPerExplosion;
            }
            gameManagerScript.Points += pointsForSingleCollision * points;
            pointsUI.text = gameManagerScript.Points.ToString();

            Destroy(other.gameObject);
        }
        if (other.tag == "E_particle")
        {
            if(isA_NP_bond)
            {
                GameObject Ati_particle = Instantiate(Ati_prefab, other.transform);
                Ati_particle.GetComponent<E_particle>().isAti = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            if(isAnE_particle)
            {
                GameObject two_Es = Instantiate(binaryBond, other.transform);
                other.transform.GetChild(0).transform.parent = null;
                two_Es.GetComponent<E_particle>().isOnAPoint = true;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            if(isA_binaryBond)
            {
                GameObject three_Es = tripleBond;
                Instantiate(three_Es, other.transform);
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            if(isAnP_particle)
            {
                GameObject PE_par = Instantiate(PE_particle, other.transform);
                PE_par.GetComponent<E_particle>().isA_PE_Bond = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            if (isAnN_particle)
            {
                GameObject NE_par = Instantiate(NE_particle, other.transform);
                NE_par.GetComponent<E_particle>().isA_NE_bond = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
