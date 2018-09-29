using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class E_particle : MovableObject {

    private bool hasChosenToDecrease = false;
    private bool isOnAPoint = false;
    private bool f_particleInstantiated = false;
    private bool isOn_a_stoppingPoint = false;
    private int Turn;
    private int[] decayingTurns;
    private int numberOf_F_particlesInstantiated = 0;
    private float inverseMoveTime;
    private float childCount;
    private List<bool> typeOfPlayer;
    private List<GameObject> instantiated_F_particles;
    private Rigidbody2D rb2d;
    private Sprite thisParticles_Sprite;
    private Vector2 _pos;
    private Vector2 preMovingPosition;
    private Vector2 targetPosition;
    private Vector2 wormHoleTarget;
    private Vector2 onPointPosition = new Vector2(0,0);
    private GameManager gameManagerScript;
    private GameObject gameManager;
    private GameObject wormholeObject;


    public bool canMove = true;
    public bool wormHoleTransitation = false;
    public bool isAti;
    public bool isAnE_particle = false;
    public bool isAnN_particle = false;
    public bool isAnP_particle = false;
    public bool isA_binaryBond = false;
    public bool isA_tripleBond = false;
    public bool isA_PE_Bond = false;
    public bool isA_NE_bond = false;
    public bool isA_NP_bond = false;
    public int horizontal = 0;
    public int vertical = 0;
    public int movmentCount = 0;
    public float pointsForSingleCollision = 5;
    public float pointsForDoubleCollision = 10;
    public float pointsForTripleCollision = 15;    
    public Sprite p_particleSprite;
    public Sprite n_particleSprite;
    public Sprite[] A_particleSprites;
    public GameObject Ati_prefab;
    public GameObject NE_particle;
    public GameObject NP_particle;
    public GameObject PE_particle;
    public GameObject f_particlePrefab;
    public GameObject binaryBond;
    public GameObject tripleBond;
    public Text pointsUI;

    protected override void Awake()
    {        
        gameManager = GameObject.FindWithTag("GameController");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        pointsUI = gameManagerScript.pointsUI;
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            thisParticles_Sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        }
        typeOfPlayer = new List<bool>();
        typeOfPlayer.Add(isAnE_particle); //1
        typeOfPlayer.Add(isAnN_particle); //2 
        typeOfPlayer.Add(isAnP_particle); //3
        typeOfPlayer.Add(isA_binaryBond); //4
        typeOfPlayer.Add(isA_tripleBond); //5
        typeOfPlayer.Add(isA_PE_Bond);    //6 
        typeOfPlayer.Add(isA_NE_bond);    //7

    }
    void resetTypeOfPlayer(List<bool> typeList)
    {
        for(int i = 0; i <= typeList.Count - 1; i++)
        {
            typeList[i] = false;
        }
    }
    protected override void Start () {        
        rb2d = GetComponent<Rigidbody2D>();
        _pos = GetComponent<Transform>().position;
        if (thisParticles_Sprite == p_particleSprite)
        {
            isAnP_particle = true;
            isAnE_particle = false;
            decayingTurns = p_particleDecayingTurnFinder();
            gameManagerScript.initialMoveTimeValue *= 3;
        }
        if(thisParticles_Sprite == n_particleSprite)
        {
            isAnN_particle = true;
            isAnE_particle = false;
            gameManagerScript.initialMoveTimeValue /= 2;
        }
        if(isA_PE_Bond)
        {
            gameManagerScript.initialMoveTimeValue *= 2;
        }
        if(isA_NE_bond)
        {
            gameManagerScript.initialMoveTimeValue /= 1.5f;
        }
        if(isAti)
        {
            gameManagerScript.initialMoveTimeValue /= 4f;
        }
    }
    void FixedUpdate()
    {
        if(isAnN_particle)
        {
            ModifyN_particleResistance();    
        }
        //Debug.Log("is on a point: " + isOnAPoint); // is on a point is false when created as a binary bond!
    }
    protected override void Update () {
        if(wormHoleTransitation)
        {
            targetPosition = wormHoleTarget;
            /* Debug.Log("Wormhole transition!");
            Debug.Log("Can move: " + canMove); */
        }

        Decay();
        inverseMoveTime = 1.0f / moveTime;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");        
        Move(horizontal, vertical, targetPosition);
        childCount = gameObject.transform.childCount;
    }
    void ModifyN_particleResistance()
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
    void Decay()
    {
        if (decayingTurns != null)
        {
            foreach (int turn in decayingTurns)
            {
                if (f_particleInstantiated && turn != Turn)
                {
                    f_particleInstantiated = false;
                }
                if (movmentCount == turn && f_particleInstantiated == false && turn != Turn)
                {
                    f_particleInstantiated = true;
                    Turn = turn;
                    Instantiate(f_particlePrefab, gameManagerScript.aimedPosition, Quaternion.identity);
                    numberOf_F_particlesInstantiated += 1;
                }
                if (numberOf_F_particlesInstantiated == 3)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    int[] p_particleDecayingTurnFinder()
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
    protected override void Move(int horizontalInput, int verticalInput, Vector2 target)
    {       
        if (target == Vector2.zero)
        {
            target = _pos + new Vector2(horizontalInput, verticalInput) * movingDistance;
        }
        if (target != Vector2.zero)
        {
            Vector2 current = transform.position;            
            if (current == target || isOnAPoint == false) // the reason isOnAPoint == false is one of the conditions is that, the player freezes if it doesn't reach the target otherwise
            {                
                target = current + new Vector2(horizontalInput, verticalInput) * movingDistance;
                canMove = true;
            }
            if (current == Vector2.zero || isOnAPoint == false)
            {                
                target = current + new Vector2(horizontalInput, verticalInput) * movingDistance;
                canMove = true;
            }            
        }               
        if(wormHoleTransitation && wormholeObject.GetComponent<TheWormHoleScript>().isActive)
        {
            target = wormHoleTarget;
            transform.position = new Vector2(target.x, target.y);
            //wormholeObject.GetComponent<TheWormHoleScript>().isActive = false;
        }
        //Debug.Log("Target: " + target + "Target position: " + targetPosition);
        if (verticalInput != 0 || horizontalInput != 0)
        {
            foreach (GameObject stoppingPoint in gameManagerScript.stoppingPoints)
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
            if(isOnAPoint == false)
            {
                canMove = true;
            }
            if(wormHoleTransitation)
            {
                canMove = false;
                //target = wormHoleTarget;
                if((Vector2)transform.position == target && (Vector2)transform.position == targetPosition)
                {
                    wormHoleTransitation = false;
                    canMove = true;
                }
                /* if(wormholeObject.GetComponent<TheWormHoleScript>().isActive == false)
                {
                    wormHoleTransitation = false;
                } */
            }
            if (canMove && isOnAPoint && wormHoleTransitation == false)
            {
                StartCoroutine(Smoothing(target)); // this function runs twice when exiting a wormHole
                movmentCount += 1;
                canMove = false;
                isOnAPoint = false;
            }
        }
        if(wormHoleTransitation == false)
        {
            targetPosition = target;
        }            
    }

    private IEnumerator Smoothing(Vector3 end)
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "p_particle")
        {
            if(isAnE_particle)
            {
                GameObject PE_par = Instantiate(PE_particle, other.transform);
                resetTypeOfPlayer(PE_par.GetComponent<E_particle>().typeOfPlayer);
                PE_par.GetComponent<E_particle>().isA_PE_Bond = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }    
            if(isA_NE_bond)
            {
                GameObject ati = Instantiate(Ati_prefab, other.transform);
                resetTypeOfPlayer(ati.GetComponent<E_particle>().typeOfPlayer);   //TO BE ACTIVATED LATER
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
                resetTypeOfPlayer(NE_par.GetComponent<E_particle>().typeOfPlayer);
                NE_par.GetComponent<E_particle>().isA_NE_bond = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            if(isA_PE_Bond)
            {
                GameObject ati = Instantiate(Ati_prefab, other.transform);
                resetTypeOfPlayer(ati.GetComponent<E_particle>().typeOfPlayer);
                ati.GetComponent<E_particle>().isAti = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            if(isAnP_particle)
            {
                GameObject NP_par = Instantiate(NP_particle, other.transform);
                resetTypeOfPlayer(NP_par.GetComponent<E_particle>().typeOfPlayer);
                NP_par.GetComponent<E_particle>().isA_NP_bond = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
        if(other.tag == "wormHole")
        {
            if(other.GetComponent<TheWormHoleScript>().isActive && other.GetComponent<TheWormHoleScript>().wormHoleBond.GetComponent<TheWormHoleScript>().isActive)
            {
                wormholeObject = other.gameObject;                
                other.GetComponent<TheWormHoleScript>().wormHoleBond.GetComponent<TheWormHoleScript>().isActive = false;
                wormHoleTransitation = true;
                Vector2 bondPosition = other.GetComponent<TheWormHoleScript>().wormHoleBond.transform.position;
                wormHoleTarget = bondPosition;
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
                resetTypeOfPlayer(Ati_particle.GetComponent<E_particle>().typeOfPlayer);
                Ati_particle.GetComponent<E_particle>().isAti = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            if(childCount == 0 && isAnE_particle)
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
            if(childCount == 0 && isAnP_particle)
            {
                GameObject PE_par = Instantiate(PE_particle, other.transform);
                resetTypeOfPlayer(PE_par.GetComponent<E_particle>().typeOfPlayer);
                PE_par.GetComponent<E_particle>().isA_PE_Bond = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            if (childCount == 0 && isAnN_particle)
            {
                GameObject NE_par = Instantiate(NE_particle, other.transform);
                resetTypeOfPlayer(NE_par.GetComponent<E_particle>().typeOfPlayer);
                NE_par.GetComponent<E_particle>().isA_NE_bond = true;
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
