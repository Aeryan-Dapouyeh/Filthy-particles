﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_normalParticleController : UltimateA_particleScript {

    public aParticle thisA_particle;
    protected void Start()
    {
        thisA_particle = new aParticle(1/SearchForCollidingWormies(), 0, gameObject);
        pointsForExplosion = thisA_particle.pointsPerExplosion;
    }      
}
