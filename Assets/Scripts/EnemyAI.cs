using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private GameObject Player;
    public bool isTouching = false;
    public bool attacked = false;
    public bool onSpawn = true;
    public float movementidth = 5f;
    public GameObject startingPosition;
    public float speed = 0.8f;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        //if (onSpawn)
           // IdleMovement();

        if (isTouching && !attacked)
            MoveTo(Player);
        else if (!isTouching && !attacked)
            MoveTo(startingPosition);
        else if (attacked)
            MoveTo(startingPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && attacked == false)
        {
            isTouching = true;
            onSpawn = false;
        }
        if (collision.gameObject.CompareTag("Spawn") && attacked == false)
        {
            onSpawn = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && attacked == false)
        {
            isTouching = false;
            attacked = false;
            GameObject.FindGameObjectWithTag("DashLimit").GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    void MoveTo(GameObject target)
    {
        Vector2 toTarget = target.transform.position - transform.position;
        transform.Translate(toTarget * speed * Time.deltaTime);
    }

    /*void IdleMovement()
    {
        float direction = 1f;
        Vector2 localPosition = this.transform.localPosition;
        Vector2 initialPosition = startingPosition.transform.position;

        if (localPosition.x < initialPosition.x + movementidth && localPosition.x > initialPosition.x - movementidth)
        {
            transform.Translate(Vector2.right * 0.5f * direction * Time.fixedDeltaTime);

            if (localPosition.x >= initialPosition.x - movementidth || localPosition.x <= initialPosition.x + movementidth)
                direction *= -1;
        }
    }*/
}
