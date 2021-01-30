using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            this.GetComponentInParent<EnemyAI>().speed *= 3;}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            this.GetComponentInParent<EnemyAI>().speed /= 3;
    }
}