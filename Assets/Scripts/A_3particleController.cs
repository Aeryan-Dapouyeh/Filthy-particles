using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_3particleController : UltimateA_particleScript
{
    public aParticle thisA_particle;

    // Use this for initialization
    protected override void Start () {
        thisA_particle = new aParticle(1/SearchForCollidingWormies(), 1, gameObject);
    }
	
	// Update is called once per frame
	protected override void Update () {
        Move();
        thisA_particle.pointsPerExplosion = SearchForCollidingWormies();
        vanish();
	}
}
