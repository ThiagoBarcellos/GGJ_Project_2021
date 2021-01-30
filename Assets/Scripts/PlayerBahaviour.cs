using UnityEngine;

public class PlayerBahaviour : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isTouchingHead = false;
    public Rigidbody2D playerRB, firePointRB;
    public Camera cam;
    Vector2 movement, mousePos;
    private GameObject newHead;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(isTouchingHead && Input.GetButtonDown("Collect")) {
            CollectHead();
        }
    }

    void FixedUpdate()
    {
        playerRB.MovePosition(playerRB.position + movement * moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDirection = mousePos - firePointRB.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        firePointRB.rotation = angle;
    }

    void CollectHead() {
        GameObject currentHead = GameObject.FindGameObjectWithTag("CurrentHead");

        Destroy(currentHead);
        newHead.tag = "CurrentHead";
        newHead.transform.SetParent(this.gameObject.transform);
        isTouchingHead = false;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.CompareTag("Head")) {
            isTouchingHead = true;
            newHead = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if(col.gameObject.CompareTag("Head")) {
            isTouchingHead = false;
            newHead = new GameObject();
        }
    }
}   