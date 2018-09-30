using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UltimateA_particleScript : MonoBehaviour { // this script is a child of monobehaviour, it's been made abstract in order for it to parent childScripts

    public int maxRandomPeriodValue = 10; // the maximum value for the random int moveevery
    public int minRandomPeriodValue = 2; //  the minimum value for the random int moveevery
    public int maxVanishValue = 30; //  the maximum value for the random int moveevery     
    public int minVanishValue = 10; //  the minimum value for the random int vanishevery
    public int maxVanishFor = 5; // the maximum amount of time a particle can vanish
    public int minVanishFor = 1; // the minimum amount of time a particle can vanish

    public float numberOfWormiesColliding = 0; // a value that determines how many wormies are colliding with this particle at any certain frame
    public GameManager gameManagerScript; // a reference to the gameManager script

    private SpriteRenderer _sr; // a reference to the sprite renderer
    private Collider2D _crCollider; // a reference to the circleColliderComponent at any a_particle

    private float accumilatedTime; // the sum of the very first moment a particle has moved plus the amount it will remain motionless
    private Transform _transform; // a reference to the transform component
    private int moveEvery; // a random int that will be used to determine how often a particle should move 
    private int vanishFor; // a random int determening how long a particle will remain vanished
    private float appearanceMoment; // the moment a particle should appear, if vanished
    private int vanishEvery; // a random int that will be used to determine how often a particle should vansih
    private int accumilatedVanishTime; // pretty much like accumilated time, the sum of the very first moment a particle appears plus the amount of time it will reamain appearing

    protected virtual void Awake()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>(); // find the gamemanager and assign its script to this valuable
        _transform = GetComponent<Transform>(); // assign the transform component
        _sr = GetComponent<SpriteRenderer>(); // assign the transform component
        _crCollider = GetComponent<CircleCollider2D>(); // assign the circleCollider2D
        moveEvery = Random.Range(minRandomPeriodValue, maxRandomPeriodValue); // find a random value for moveEvery
        vanishEvery = Random.Range(minVanishValue, maxVanishValue); // find a random value for vanishevery
        accumilatedTime += moveEvery; // set the value of accumilatedtime 
        accumilatedVanishTime += vanishEvery; // set the value of accumilatedvanishtime
    }

    public float SearchForCollidingWormies() // here we define a function that counts the number of colliding wormies at every given time
    {
        if (gameManagerScript == null) // if there is no gamemanagerscript
        {
            // find the gameManagerComponent and assign its script into our valuable
            gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
        if (numberOfWormiesColliding != 0) // if the number of colliding wormies is not equal to zero
        {
            numberOfWormiesColliding = 0; // set it to zero
            // Reason: the function is designed to find colliding wormies at every given time, so it's a good idea to start by 0, since we assume there are
            // no wormies
        }
        // for each type of wormies in gamemanagerscript...
        for (int i = 0; i <= gameManagerScript.wormies.Count - 1; i++)
        {         
            // for each wormie in any given type of wormies
            foreach (GameObject wormie in gameManagerScript.wormies[i])
            {
                Vector2 wormiePosition = wormie.transform.position; // take the position of the wormie
                // if the wormie is to your right
                if (wormiePosition.x + (5 * (i + 1)) == transform.position.x && wormiePosition.y == transform.position.y)
                {
                    int alreadyMentionedPosition = 0; // declare a variable called already mentioned positions and set its value to zero - Reason: If a wormie is already found, there isabsoulotly no reason to search for more wormies, aka. it's a waste of computing power 
                    // foreach stopping point in the scene
                    foreach (GameObject stoppingPoint in gameManagerScript.stoppingPoints)
                    {         
                        // if the stopping point, exists
                        if (stoppingPoint != null)
                        {
                            // if there is a wormie at the right of the stoppingpoint, the particle is located on, and no position is mentioned... 
                            if (stoppingPoint.transform.position.x == wormiePosition.x - (5 * (i + 1)) && stoppingPoint.transform.position.y == wormiePosition.y && alreadyMentionedPosition == 0)
                            {
                                numberOfWormiesColliding += 1; // there is certainly a wormie colliding
                                alreadyMentionedPosition += 1; // and there is certainly a mentioned position
                            }
                        }                        
                    }
                }
                // the rest of the code in the function, do the same for other wormies in other directions(left, up and down)
                if (wormiePosition.x - (5 * (i + 1)) == transform.position.x && wormiePosition.y == transform.position.y)
                {
                    int alreadyMentionedPosition = 0;
                    foreach (GameObject stoppingPoint in gameManagerScript.stoppingPoints)
                    {
                        if (stoppingPoint != null)
                        {
                            if (stoppingPoint.transform.position.x == wormiePosition.x + (5 * (i + 1)) && stoppingPoint.transform.position.y == wormiePosition.y && alreadyMentionedPosition == 0)
                            {
                                numberOfWormiesColliding += 1;
                                alreadyMentionedPosition += 1;
                            }
                        }                        
                    }
                }
                if(wormiePosition.x == transform.position.x && wormiePosition.y - (5 * (i + 1)) == transform.position.y)
                {
                    int alreadyMentionedPosition = 0;
                    foreach (GameObject stoppingPoint in gameManagerScript.stoppingPoints)
                    {
                        if (stoppingPoint != null)
                        {
                            if (stoppingPoint.transform.position.y == wormiePosition.y + (5 * (i + 1)) && stoppingPoint.transform.position.x == wormiePosition.x && alreadyMentionedPosition == 0)
                            {
                                numberOfWormiesColliding += 1;
                                alreadyMentionedPosition += 1;
                            }
                        }                        
                    }
                }
                if(wormiePosition.x == transform.position.x && wormiePosition.y + (5 * (i + 1)) == transform.position.y)
                {
                    int alreadyMentionedPosition = 0;
                    foreach (GameObject stoppingPoint in gameManagerScript.stoppingPoints)
                    {
                        if(stoppingPoint != null)
                        {
                            if (stoppingPoint.transform.position.y == wormiePosition.y - (5 * (i + 1)) && stoppingPoint.transform.position.x == wormiePosition.x && alreadyMentionedPosition == 0)
                            {
                                numberOfWormiesColliding += 1;
                                alreadyMentionedPosition += 1;
                            }
                        }                        
                    }
                }
            }
        }
        // at last return the number of wormies colliding with this particle
        return numberOfWormiesColliding;        
    }
    protected void Move() // here we define a function, that let's the a_partilcles move
    {
        if (Time.time == accumilatedTime || accumilatedTime < Time.time) // if we have reached the accumilated time...
        {
            moveEvery = Random.Range(minRandomPeriodValue, maxRandomPeriodValue); // find a new value for move every
            accumilatedTime += moveEvery; // assign the a new value to acummilated time
            Vector2 newPosition = ChooseARandomPosition(); // new position is assigned a random value
            transform.Translate(newPosition - (Vector2)transform.position); // move the particle to the chosen position 
        }
    }
    protected Vector2 ChooseARandomPosition() // this function chooses a random point that is not 1) on the player 2) around the player 3) on other a_particles
    {
        bool positionChosen = false;
        int random_StoppingPointPosition = Random.Range(0, gameManagerScript.stoppingPointPositions.Count - 1); // find a random stopping point position
        Vector2 chosenStoppingPoint_pos = gameManagerScript.stoppingPointPositions[random_StoppingPointPosition]; // assign the position to a vector 2
        Vector2 newPosition = new Vector2(chosenStoppingPoint_pos.x, chosenStoppingPoint_pos.y); // declare newposition according to the random stoppingpoint position        


        GameObject[] a_particlesInScene = gameManagerScript.a_particles;
        List<Vector2> forbiddenPositions = new List<Vector2>();
        for(int i = 0; i <= a_particlesInScene.Length - 1; i++)
        {
            forbiddenPositions.Add(a_particlesInScene[i].transform.position);
        }
        forbiddenPositions.Add(gameManagerScript.player.transform.position);



        bool right = false;
        bool left = false;
        bool up = false;
        bool down = false;
        foreach (Vector2 stoppingPoint in gameManagerScript.stoppingPointPositions)
        {
            for (int i = 0; i <= gameManagerScript.wormies.Count - 1; i++)
            {
                if (stoppingPoint.x == gameManagerScript.player.transform.position.x + ((i + 1) * 10) && stoppingPoint.y == gameManagerScript.player.transform.position.y && right == false)
                {
                    right = true;
                    forbiddenPositions.Add(stoppingPoint);                   
                }
                if (stoppingPoint.x == gameManagerScript.player.transform.position.x + ((i + 1) * -10) && stoppingPoint.y == gameManagerScript.player.transform.position.y && left == false)
                {
                    left = true;
                    forbiddenPositions.Add(stoppingPoint);
                }
                if (stoppingPoint.x == gameManagerScript.player.transform.position.x && stoppingPoint.y == gameManagerScript.player.transform.position.y + ((i + 1) * 10) &&  up == false)
                {
                    up = true;
                    forbiddenPositions.Add(stoppingPoint);
                }
                if (stoppingPoint.x == gameManagerScript.player.transform.position.x && stoppingPoint.y == gameManagerScript.player.transform.position.y + ((i + 1) * -10) && down == false)
                {
                    down = true;
                    forbiddenPositions.Add(stoppingPoint);
                }
            }            
        }



        while (positionChosen == false)
        {
            foreach (Vector2 position in forbiddenPositions)
            {
                if (newPosition == position)
                {
                    random_StoppingPointPosition = Random.Range(0, gameManagerScript.stoppingPointPositions.Count - 1); // find a random stopping point position
                    chosenStoppingPoint_pos = gameManagerScript.stoppingPointPositions[random_StoppingPointPosition]; // assign the position to a vector 2
                    newPosition = new Vector2(chosenStoppingPoint_pos.x, chosenStoppingPoint_pos.y);
                }
            }
            positionChosen = true;
        }
        return newPosition;
    }
    protected void vanish()
    {
        // if the accumilatedtime is reached...
        if (Time.time == accumilatedVanishTime || accumilatedVanishTime < Time.time) 
        {
            vanishEvery = Random.Range(minVanishValue, maxVanishValue); // choose a randomvalue for vanishevery
            accumilatedVanishTime += vanishEvery; // assign a new value to accumilatedvanishingtime according to vanishevery

            _sr.enabled = !enabled; // disable the spriterenderer
            _crCollider.enabled = !enabled; // disable the collider
            vanishFor = Random.Range(minVanishFor, maxVanishFor); // vanishfor a certain amount of time
            appearanceMoment = Time.time + vanishFor; // assign a value to the very moment it should appear
        }
        if (appearanceMoment == Time.time || appearanceMoment < Time.time) // if it's time to appear...
        {
            _sr.enabled = enabled; // enable every thing you disabled
            _crCollider.enabled = enabled;
        }
    }
    public struct aParticle // a struct for every a_particle is defined here
    {
        public float pointsPerExplosion; // there must be an amount of points per explosion
        public int type; // there must be a type for every aParticle

        public aParticle(float _pointsPerExplosion, int _type, GameObject thisGameObject)
        {
            pointsPerExplosion = _pointsPerExplosion;
            type = _type;
        }
    }
}
