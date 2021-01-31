using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public static float moveSpeed = 5f;
    public int playerHealth = 3;
    public Sprite[] healthIndicator = new Sprite[4];
    public GameObject crossHair;
    public Rigidbody2D rb;
    public Camera cam;
    public static GameObject newHead, placeholderHead;
    bool isTouchingHead = false;
    Vector2 movement, mousePos;

    public Animator animator;

    void Start()
    {
        placeholderHead = GameObject.FindGameObjectWithTag("PlaceholderHead");
        cam = Camera.main;
        Cursor.visible = false;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        
        if(movement.x > 0.0f) {
            animator.SetFloat("speed", Mathf.Abs(movement.x));
        } else {
            animator.SetFloat("speed", Mathf.Abs(movement.y));
        }
    }

    void FixedUpdate()
    {
        CheckPlayerHealth();

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        crossHair.transform.position = mousePos;

        if(isTouchingHead && Input.GetButtonDown("Collect"))
            CollectHead();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            playerHealth -= col.gameObject.GetComponent<EnemyBehaviour>().damage;
            Debug.Log(playerHealth);
        }
    }

    void CheckPlayerHealth()
    {
        switch (playerHealth)
        {
            case 3:
                GameObject.FindGameObjectWithTag("Health").GetComponent<Image>().sprite = healthIndicator[0];
                break;
            case 2:
                GameObject.FindGameObjectWithTag("Health").GetComponent<Image>().sprite = healthIndicator[1];
                break;
            case 1:
                GameObject.FindGameObjectWithTag("Health").GetComponent<Image>().sprite = healthIndicator[2];
                break;
            case 0:
                GameObject.FindGameObjectWithTag("Health").GetComponent<Image>().sprite = healthIndicator[3];
                ResetWhenDie();
                break;
            default:
                GameObject.FindGameObjectWithTag("Health").GetComponent<Image>().sprite = healthIndicator[3];
                break;
        }
    }

    void CollectHead() {
        Transform headPos = placeholderHead.transform;
        GameObject instantiatedHead = Instantiate(newHead, headPos.position, headPos.rotation);
        
        instantiatedHead.transform.SetParent(this.transform);
        instantiatedHead.tag = "CurrentHead";

        Destroy(newHead);
        isTouchingHead = false;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.CompareTag("Head")) {
            isTouchingHead = true;
            newHead = col.gameObject;
        }
    }

    void OnTriggerStay2D(Collider2D col) {
        if(col.gameObject.CompareTag("Head")) {
            isTouchingHead = true;
            newHead = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if(col.gameObject.CompareTag("Head")) {
            isTouchingHead = false;
        }
    }

    void ResetWhenDie() {
        playerHealth = 3;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}