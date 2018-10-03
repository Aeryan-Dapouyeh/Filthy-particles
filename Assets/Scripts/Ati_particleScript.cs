using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ati_particleScript : UltimatePlayerScript {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        gameManagerScript.initialMoveTimeValue /= 4f; // set a speed for it
    }
}
