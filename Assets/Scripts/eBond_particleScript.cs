using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eBond_particleScript : UltimatePlayerScript {
    [Tooltip("Displays the number of e_particles in an e_bond.")]
    public int numberOfE_particles = 2;

    // THE UPDATE AND START FUNCTIONS ARE DELETED, SHOULD ANY PROBLEM ARISE, ADD THEM HERE
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if(other.tag == "E_particle")
        {
            if(numberOfE_particles == 2)
            {
                GameObject three_Es = tripleBond;
                Instantiate(three_Es, other.transform);
                other.transform.GetChild(0).transform.parent = null;
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
