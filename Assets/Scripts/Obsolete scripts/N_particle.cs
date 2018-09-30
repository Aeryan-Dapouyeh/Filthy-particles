using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_particle : MovableObject {


    private bool canMove = true;
    private float inverseMoveTime;
    private GameManager gameManagerScript;
    private int horizontal = 0;
    private int vertical = 0;
    private Rigidbody2D rb2d;
    private Vector2 _pos;
    private Vector2 targetPosition;

    public int pointsPer_A_Collision = 3;

    protected override void Awake()
    {
        gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        base.Awake();
    }

    // Use this for initialization
    protected override void Start () {
        inverseMoveTime = 1.0f / moveTime;
        rb2d = GetComponent<Rigidbody2D>();
        _pos = GetComponent<Transform>().position;
    }
	
	// Update is called once per frame
	protected override void Update () {
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        Move(horizontal, vertical, targetPosition);
    }

    protected override void Move(int horizontalInput, int verticalInput, Vector2 target)
    {
        if (target == Vector2.zero)
        {
            target = _pos + new Vector2(horizontalInput, verticalInput) * movingDistance;
        }
        if (target != Vector2.zero)
        {
            Vector2 current = transform.position;
            if (current == target)
            {
                target = current + new Vector2(horizontalInput, verticalInput) * movingDistance;
                canMove = true;
            }
            if (current == Vector2.zero)
            {
                target = current + new Vector2(horizontalInput, verticalInput) * movingDistance;
                canMove = true;
            }
        }

        if (verticalInput != 0 || horizontalInput != 0)
        {
            if (canMove)
            {
                StartCoroutine(Smoothing(target));
                canMove = false;
            }
        }
        targetPosition = target;
    }

    private IEnumerator Smoothing(Vector3 end)
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
        if (other.tag == "a_particle")
        {
            Destroy(other.gameObject);
            gameManagerScript.Points += pointsPer_A_Collision; 
        }
        
    }
}
