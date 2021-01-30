﻿using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.GetComponentInParent<EnemyAI>().isTouching = false;
    }
}