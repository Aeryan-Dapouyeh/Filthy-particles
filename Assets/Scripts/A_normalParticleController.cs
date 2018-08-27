using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_normalParticleController : UltimateA_particleScript {

    public aParticle thisA_particle;
    protected override void Start()
    {
        thisA_particle = new aParticle(SearchForCollidingWormies(), 0, gameObject);
    }      
}
