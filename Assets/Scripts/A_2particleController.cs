using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_2particleController : UltimateA_particleScript // this script is a child of UltimateA_particleScript 
{
    public aParticle thisA_particle; // a variable to declare a particle(this a_particle) of type aParticle

    // Use this for initialization
    protected void Start () { // overriding the original start method
        thisA_particle = new aParticle(1/SearchForCollidingWormies(), 1, gameObject); // the collision point calculation is quit controversial, it's
        // not clear wether the decrease of the points would be better while several wormies collide or an increase of it
    }
	
	// Update is called once per frame
	protected void Update () { // overriding the update function
        vanish(); // using a method from our parentscript UltimateA_particleScript, to vanish every now and then
        pointsForExplosion = thisA_particle.pointsPerExplosion;
    }
}
