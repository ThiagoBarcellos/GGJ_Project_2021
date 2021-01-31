using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    Rigidbody2D rb;
    float moveSpeed, moveSpeedInitial;
    Vector3 lastPosition;

    void Start()
    {
        moveSpeedInitial = Shooting.bulletForce;
        moveSpeed = moveSpeedInitial;

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        lastPosition = rb.velocity;
    }

     IEnumerator test(){
        while(moveSpeed > 0){
            yield return new WaitForSeconds(0.2f);
            moveSpeed -= 0.75f;
        }

        rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D other){
        
        Vector2 _wallNormal = other.contacts[0].normal;
        Vector3 direction = Vector3.Reflect(lastPosition, _wallNormal).normalized;

        rb.velocity = direction * moveSpeed;

        if(moveSpeed == moveSpeedInitial/2){
            moveSpeed /= 2;
            StartCoroutine(test());
        }
        else{
            moveSpeed /= 2;
        }
    }
}

