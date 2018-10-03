using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_particleScript : UltimatePlayerScript
{

	// Use this for initialization
	protected override void Start () {
        base.Start();
        gameManagerScript.initialMoveTimeValue /= 2; // set a speed for it        
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}

    protected override void FixedUpdate()
    {
        ModifyN_particleResistance(); // make the wormies you go through slower 
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if(other.tag == "E_particle")
        {
            GameObject NE_par = Instantiate(NE_particle, other.transform);
            other.transform.GetChild(0).transform.parent = null;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
