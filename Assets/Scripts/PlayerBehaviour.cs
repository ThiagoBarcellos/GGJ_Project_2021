using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int playerHealth = 3;
    public Sprite[] healthIndicator = new Sprite[4];
    public GameObject crossHair;
    public Rigidbody2D rb;
    private Camera cam;
    public GameObject currentHead;
    Vector2 movement, mousePos;

    void Start()
    {
        cam = Camera.main;
        Cursor.visible = false;
        //currentHead = GameObject.FindGameObjectWithTag("CurrentHead");
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        CheckPlayerHealth();

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        crossHair.transform.position = mousePos;
        Vector2 lookDirection = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
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
                break;
            default:
                GameObject.FindGameObjectWithTag("Health").GetComponent<Image>().sprite = healthIndicator[3];
                break;
        }
    }
}