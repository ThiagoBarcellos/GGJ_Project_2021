using UnityEngine;  

public class EnemyAI : MonoBehaviour
{
    private GameObject Player;
    private bool isTouching = false;
    public GameObject startingPosition;

    public float speed = 0.5f;

    void Start(){
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate(){
        if (isTouching)
            MoveTo(Player);
        else
            MoveTo(startingPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Player")
            isTouching = true;
    }
    private void OnTriggerExit2D(Collider2D collision){
        if (collision.gameObject.tag == "Player")
            isTouching = false;
    }

    void MoveTo(GameObject target){
        Vector2 toTarget = target.transform.position - transform.position;

        transform.Translate(toTarget * speed * Time.deltaTime);
    }
}
