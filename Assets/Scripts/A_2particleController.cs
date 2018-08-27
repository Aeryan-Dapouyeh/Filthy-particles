using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_2particleController : UltimateA_particleScript
{
    public aParticle thisA_particle;

    // Use this for initialization
    protected override void Start () {
        thisA_particle = new aParticle(SearchForCollidingWormies(), 1, gameObject);
    }
	
	// Update is called once per frame
	protected override void Update () {
        int ex = SearchForCollidingWormies(); // the problem is that the game always returns an int as explosion value, when it actually should return a float in orther to devide it further, TO BE FIXED LATER
        Debug.Log("Points Per explosion: " + thisA_particle.pointsPerExplosion + " colliding wormies: " + ex);
        vanish();
	}
}
