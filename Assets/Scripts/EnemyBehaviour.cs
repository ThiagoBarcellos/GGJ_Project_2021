using UnityEngine;
public class EnemyBehaviour : MonoBehaviour
{
    private int hp = 1;
    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.GetComponentInParent<EnemyAI>().isTouching = false;
        if(collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<Player>().playerHealth -= damage;
            this.GetComponentInParent<CircleCollider2D>().enabled = false;
        }

        if(collision.gameObject.CompareTag("Shoot")) {
            hp--;
            if(hp == 0)
                Destroy(this.gameObject);
        }
    }


}