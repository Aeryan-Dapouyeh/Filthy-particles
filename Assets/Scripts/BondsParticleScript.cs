using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondsParticleScript : UltimatePlayerScript {
    // consists of pe, pn, ne
    public bool pe;
    public bool ne;
    public bool pn;
    
    protected override void Start () {
        base.Start();
        if (pe)
        {
            gameManagerScript.initialMoveTimeValue *= 2; // set a speed for it
        }
        if(ne)
        {
            gameManagerScript.initialMoveTimeValue /= 1.5f; // set a speed for it
        }
	}

    // WE LACK THE UPDATE FUNCTION, IF ANY PROBLEM IS TO ARISE, ADD IT HERE

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if(other.tag == "p_particle")
        {
            if(ne)
            {
                GameObject ati = Instantiate(Ati_prefab, other.transform);
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
        if(other.tag == "N_particle")
        {
            if(pe)
            {
                GameObject ati = Instantiate(Ati_prefab, other.transform);
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
        if(other.tag == "E_particle")
        {
            if(pn)
            {
                GameObject Ati_particle = Instantiate(Ati_prefab, other.transform);
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
