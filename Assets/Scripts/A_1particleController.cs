using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_1particleController : UltimateA_particleScript // this script is a child of the ulitimateA_particleScript
{
    public aParticle thisA_particle; // we declare an(this) a_particle using the class defined in the UltimateA_particleScript

    // Use this for initialization
    protected void Start () { // override the start function in UltimateA_particleScript
        thisA_particle = new aParticle(1/SearchForCollidingWormies(), 1, gameObject); // set the value from our component(this a_particle) of type aParticle 
    }
	
	// Update is called once per frame
	protected void Update () { // override the update function
        Move(); // use a method from the parent script to move the particle
        thisA_particle.pointsPerExplosion = SearchForCollidingWormies(); // calculate the amount of points according to the number of colliding wormies in each turn
    }
}
