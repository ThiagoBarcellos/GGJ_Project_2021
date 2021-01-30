using UnityEngine;  

public class EnemyAI : MonoBehaviour
{
    private GameObject Player;
    public bool isTouching = false;
    public bool attacked = false;
    public bool onSpawn = true;
    public int movementidth = 5;
    public GameObject startingPosition;

    public float speed = 0.8f;

    void Start(){
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate(){
        if (onSpawn)
            IdleMovement();

        if (isTouching && !attacked)
            MoveTo(Player);
        else if (!isTouching && !attacked)
            MoveTo(startingPosition);
        else if (attacked)
            MoveTo(startingPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision){
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
    private void OnTriggerExit2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Player") && attacked == false){
            isTouching = false;
            attacked = false;
        }
    }

    void MoveTo(GameObject target){
        Vector2 toTarget = target.transform.position - transform.position;

        transform.Translate(toTarget * speed * Time.deltaTime);
    }

    void IdleMovement()
    {
        if (this.transform.position.x < startingPosition.transform.position.x + movementidth)
            transform.Translate(Vector2.right * 0.5f * Time.deltaTime);
        else if (this.transform.position.x > startingPosition.transform.position.x - movementidth)
            transform.Translate(Vector2.left * 0.5f * Time.deltaTime);
    }
}
