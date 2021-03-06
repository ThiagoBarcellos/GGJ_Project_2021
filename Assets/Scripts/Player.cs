﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region Variaveis
    public static float moveSpeed = 5f;
    public int playerHealth = 3;
    public Sprite[] healthIndicator = new Sprite[4];
    private Animator playerAnimator;
    public Transform playerCharacter;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Camera cam;
    public static GameObject newHead, placeholderHead, crossHair;
    bool isTouchingHead = false;
    Vector2 movement, mousePos;
    private bool lookingRight = true;
    #endregion

    void Start()
    {
        this.playerCharacter = this.transform;
        this.playerAnimator = this.GetComponent<Animator>();
        this.rb = this.GetComponent<Rigidbody2D>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        placeholderHead = GameObject.FindGameObjectWithTag("PlaceholderHead");
        crossHair = GameObject.FindGameObjectWithTag("CrossHair");
        this.cam = Camera.main;
        Cursor.visible = false;
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        playerAnimator.SetFloat("HorizontalSpeed", Mathf.Abs(movement.x));
        playerAnimator.SetFloat("VerticalSpeed", Mathf.Abs(movement.y));
    }

    void FixedUpdate(){
        CheckPlayerHealth();
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        crossHair.transform.position = mousePos;
        if(isTouchingHead && Input.GetButtonDown("Collect")) CollectHead();
        Flip();
        ShootAnimation();
        AttackAnimation();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            playerHealth -= col.gameObject.GetComponent<EnemyBehaviour>().damage;
        }
    }
    void CheckPlayerHealth()
    {
        switch (playerHealth)
        {
            case 3:
                GameObject.FindGameObjectWithTag("Health").GetComponent<Image>().sprite = healthIndicator[3];
                break;
            case 2:
                GameObject.FindGameObjectWithTag("Health").GetComponent<Image>().sprite = healthIndicator[2];
                break;
            case 1:
                GameObject.FindGameObjectWithTag("Health").GetComponent<Image>().sprite = healthIndicator[1];
                break;
            case 0:
                GameObject.FindGameObjectWithTag("Health").GetComponent<Image>().sprite = healthIndicator[0];
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

    void Flip(){
        Vector2 playerLocalScale = this.playerCharacter.localScale;
        if(mousePos.x < rb.transform.position.x && lookingRight) {
            this.spriteRenderer.flipX = true;
            GameObject head = GameObject.FindGameObjectWithTag("PlaceholderHead");
            head.transform.localScale *= -1;
            lookingRight = !lookingRight;
        }
        else if(mousePos.x >= rb.transform.position.x && !lookingRight) {
            this.spriteRenderer.flipX = false;
            GameObject head = GameObject.FindGameObjectWithTag("PlaceholderHead");
            head.transform.localScale *= -1;
            lookingRight = !lookingRight;
        }
    }

    void ShootAnimation(){
        PlayerCombat shooting = this.GetComponent<PlayerCombat>();
        if(shooting.isShooting)
            playerAnimator.SetBool("Shoot", true);
        else
            playerAnimator.SetBool("Shoot", false);
    }
    void AttackAnimation(){
        PlayerCombat Attacking = this.GetComponent<PlayerCombat>();
        if(Attacking.isAttacking && Attacking.attackHorizontal)
            playerAnimator.SetBool("AttackHorizontal", true);
        else if(Attacking.isAttacking && Attacking.attackUp)
            playerAnimator.SetBool("AttackUp", true);
        else if(Attacking.isAttacking && Attacking.attackDown)
            playerAnimator.SetBool("AttackDown", true);
        else
            playerAnimator.SetBool("AttackHorizontal", false);
            playerAnimator.SetBool("AttackUp", false);
            playerAnimator.SetBool("AttackDown", false);
    }
}