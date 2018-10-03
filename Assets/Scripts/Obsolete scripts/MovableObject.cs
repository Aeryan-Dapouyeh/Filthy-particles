using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableObject : MonoBehaviour {

    public float moveTime;
    public int movingDistance = 10;

    private bool canMove = true;
    private float inverseMoveTime;
    private Rigidbody2D rb2d;
    private Vector2 _pos;
    private Vector2 targetPosition;


    protected virtual void Awake()
    {
        _pos = GetComponent<Transform>().position;
    }

    protected virtual void Start () {        
        rb2d = GetComponent<Rigidbody2D>();        
	}
	
	protected virtual void Update () {
        inverseMoveTime = 1.0f / moveTime;
        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");


        /* if(targetPosition == Vector2.zero)
        {
            targetPosition = _transform + new Vector2(horizontal, vertical) * movingDistance;
        }
        if(targetPosition != Vector2.zero)
        {
            Vector2 current = transform.position;
            if(current == targetPosition)
            {
                targetPosition = current + new Vector2(horizontal, vertical) * movingDistance;
                canMove = true;
            }
            if(current == Vector2.zero)
            {
                targetPosition = current + new Vector2(horizontal, vertical) * movingDistance;
                canMove = true;
            }
        }

        if (vertical != 0 || horizontal != 0)
        {
            if(canMove)
            {
                Move(targetPosition);
                canMove = false;
            }
        } */
        Move(horizontal, vertical, targetPosition);
    }
    protected virtual void Move(int horizontalInput, int verticalInput, Vector2 target)
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

    IEnumerator Smoothing(Vector3 end)
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
}
