using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyMover : MonoBehaviour {

    public float moveTime;
    public Vector3 target;

    private Rigidbody2D rb2d;
    private float inverseMoveTime;

    private void Awake()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        inverseMoveTime = 1.0f / moveTime;
        StartCoroutine(Smoothing(target));
        if(transform.position == target)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Smoothing(Vector3 end) // a magical function(like totally), that will let you move a path towards point smoothly
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime * Time.deltaTime);
            rb2d.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "OrbirRing_batterium")
        {
            Debug.Log("Runs!");
            other.GetComponentInParent<Batterium>().storedCash += 1;
            Destroy(gameObject);
        }
    }
}
