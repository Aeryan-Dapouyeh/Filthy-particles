using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p_particleScript : UltimatePlayerScript
{

	// Use this for initialization
	protected override void Start () {
        base.Start();
        decayingTurns = p_particleDecayingTurnFinder(); // set random decaying turns
        gameManagerScript.initialMoveTimeValue *= 3; // set a speed value for it 
    }
	
	// Update is called once per frame
	protected override void Update () {
        Decay();
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if(other.tag == "N_particle")
        {
            GameObject NP_par = Instantiate(NP_particle, other.transform);
            other.transform.GetChild(0).transform.parent = null;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (other.tag == "E_particle")
        {
            GameObject PE_par = Instantiate(PE_particle, other.transform);
            other.transform.GetChild(0).transform.parent = null;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
