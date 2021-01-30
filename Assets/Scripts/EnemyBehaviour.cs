using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private int hp = 1;
    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.GetComponentInParent<EnemyAI>().isTouching = false;
        if(collision.gameObject.CompareTag("Player")){
            collision.gameObject.GetComponent<PlayerBehaviour>().playerHealth -= damage;
            Debug.Log(collision.gameObject.GetComponent<PlayerBehaviour>().playerHealth);
            this.GetComponentInParent<CircleCollider2D>().enabled = false;
        }
    }
}