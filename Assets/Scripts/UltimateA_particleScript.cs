using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UltimateA_particleScript : MonoBehaviour {

    public int maxRandomPeriodValue = 10;
    public int minRandomPeriodValue = 2;
    public int maxVanishValue = 30;
    public int minVanishValue = 10;
    public int maxVanishFor = 5;
    public int minVanishFor = 1;

    public float numberOfWormiesColliding = 0;
    public GameManager gameManagerScript;

    private SpriteRenderer _sr;
    private Collider2D _crCollider;

    private float accumilatedTime = 0;
    private Transform _transform;
    private int moveEvery;
    private int vanishFor;
    private float appearanceMoment;
    private int vanishEvery;
    private int accumilatedVanishTime;

    protected virtual void Awake()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        _transform = GetComponent<Transform>();
        _sr = GetComponent<SpriteRenderer>();
        _crCollider = GetComponent<CircleCollider2D>();
        moveEvery = Random.Range(minRandomPeriodValue, maxRandomPeriodValue);
        vanishEvery = Random.Range(minVanishValue, maxVanishValue);

        accumilatedTime += moveEvery;
        accumilatedVanishTime += vanishEvery;
    }
    protected virtual void Start () {
        
    }
	protected virtual void Update () {

    }

    public float SearchForCollidingWormies()
    {
        if (gameManagerScript == null)
        {
            GameObject gameManagerGameObject = GameObject.FindGameObjectWithTag("GameController");
            gameManagerScript = gameManagerGameObject.GetComponent<GameManager>();
        }
        if (numberOfWormiesColliding != 0)
        {
            numberOfWormiesColliding = 0;
        }
        for (int i = 0; i <= gameManagerScript.wormies.Count - 1; i++)
        {            
            foreach (GameObject wormie in gameManagerScript.wormies[i])
            {
                Vector2 wormiePosition = wormie.transform.position;                
                if (wormiePosition.x + (5 * (i + 1)) == transform.position.x && wormiePosition.y == transform.position.y)
                {
                    int alreadyMentionedPosition = 0;
                    foreach (GameObject stoppingPoint in gameManagerScript.stoppingPoints)
                    {           
                        if (stoppingPoint != null)
                        {
                            if (stoppingPoint.transform.position.x == wormiePosition.x - (5 * (i + 1)) && stoppingPoint.transform.position.y == wormiePosition.y && alreadyMentionedPosition == 0)
                            {
                                numberOfWormiesColliding += 1;
                                alreadyMentionedPosition += 1;
                            }
                        }                        
                    }
                }
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
        return numberOfWormiesColliding;        
    }
    protected void Move()
    {
        if (Time.time == accumilatedTime || accumilatedTime < Time.time)
        {
            moveEvery = Random.Range(minRandomPeriodValue, maxRandomPeriodValue);
            accumilatedTime += moveEvery;
            int random_StoppingPointPosition = Random.Range(0, gameManagerScript.stoppingPointPositions.Count - 1);
            Vector2 chosenStoppingPoint_pos = gameManagerScript.stoppingPointPositions[random_StoppingPointPosition];
            //Vector2 newPosition = new Vector2(Random.Range(minX_pos, maxX_pos) - _transform.position.x, Random.Range(minY_pos, maxY_pos) - _transform.position.y);
            Vector2 newPosition = new Vector2(chosenStoppingPoint_pos.x, chosenStoppingPoint_pos.y);
            transform.Translate(newPosition - (Vector2)transform.position);           
        }
    }
    protected void vanish()
    {
        if (Time.time == accumilatedVanishTime || accumilatedVanishTime < Time.time)
        {
            vanishEvery = Random.Range(minVanishValue, maxVanishValue);
            accumilatedVanishTime += vanishEvery;

            _sr.enabled = !enabled;
            _crCollider.enabled = !enabled;
            vanishFor = Random.Range(minVanishFor, maxVanishFor);
            appearanceMoment = Time.time + vanishFor;
        }
        if (appearanceMoment == Time.time || appearanceMoment < Time.time)
        {
            _sr.enabled = enabled;
            _crCollider.enabled = enabled;
            appearanceMoment = 0;
        }
    }
    public struct aParticle
    {
        public float pointsPerExplosion;
        public int type;
        public Vector2 position;

        public aParticle(float _pointsPerExplosion, int _type, GameObject thisGameObject)
        {
            pointsPerExplosion = _pointsPerExplosion;
            type = _type;
            position = thisGameObject.transform.position;
        }
    }
}
