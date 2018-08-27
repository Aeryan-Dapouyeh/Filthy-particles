using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_2particleController : UltimateA_particleScript
{
    public aParticle thisA_particle;

    // Use this for initialization
    protected override void Start () {
        thisA_particle = new aParticle(1/SearchForCollidingWormies(), 1, gameObject); // the collision point calculation is quit controversial, it's
        // not clear wether the decrease of the points would be better while several wormies collide or an increase of it
    }
	
	// Update is called once per frame
	protected override void Update () {
        vanish();
	}
}
