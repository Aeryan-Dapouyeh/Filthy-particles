using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_1particleController : UltimateA_particleScript
{

    public aParticle thisA_particle;

    protected override void Awake()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    // Use this for initialization
    protected override void Start () {
        thisA_particle = new aParticle(SearchForCollidingWormies(), 1, gameObject);
    }
	
	// Update is called once per frame
	protected override void Update () {
        Move();
        thisA_particle.pointsPerExplosion = SearchForCollidingWormies();
	}
}
