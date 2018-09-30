using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_3particleController : UltimateA_particleScript // this script is a child of UltimateA_particleScript 
{
    public aParticle thisA_particle; // an a_particle(this a_particle) is declared of type aParticle

    // Use this for initialization
    protected void Start () { // overriding the start method
        thisA_particle = new aParticle(1/SearchForCollidingWormies(), 1, gameObject); // giving value to the particle that we created earlier
    }
	
	// Update is called once per frame
	protected void Update () { // overriding the update function
        Move(); // using a method from our parent script to move the particle
        thisA_particle.pointsPerExplosion = SearchForCollidingWormies(); // calculating the points earned from destroying this particle from the number of wormies surrounding it
        vanish(); // using a method from out parentscript to make the particle vansih every now and then
	}
}
