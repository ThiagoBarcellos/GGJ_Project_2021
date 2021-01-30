using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    Rigidbody2D rb;
    float moveSpeed, moveSpeedInitial;
    Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeedInitial = Shooting.bulletForce;
        moveSpeed = moveSpeedInitial;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lastPosition = rb.velocity;
    }

     IEnumerator test(){
        while(moveSpeed > 0){
            yield return new WaitForSeconds(1f);
            moveSpeed -= 0.75f;
            Debug.Log(moveSpeed);
        }

        rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D other){
        
        Vector2 _wallNormal = other.contacts[0].normal;
        Vector3 direction = Vector3.Reflect(lastPosition, _wallNormal).normalized;

        rb.velocity = direction * moveSpeed;

        if(moveSpeed == moveSpeedInitial/4){
            moveSpeed /= 2;
            StartCoroutine(test());
            Debug.Log(moveSpeed);

        }
        else{
            moveSpeed /= 2;
            Debug.Log(moveSpeed);

        }
    }
}

