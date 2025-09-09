using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnnemyBehaviour : MonoBehaviour
{

    private float speed = 2;
    private Transform target;
    public Animator animator;
    private int damage = 1;
    private int maxHealth = 5;
    private int health;
    public ParticleSystem hitFx;
    public Image healthBar;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        healthBar.fillAmount = (float)health / maxHealth;

        if (health <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            transform.LookAt(target);
            Quaternion currentRotation = transform.rotation;
            transform.rotation = Quaternion.Euler(0, currentRotation.eulerAngles.y, 0);

            animator.SetBool("Run", true);
        }
    }

    // detection de présence du joueur sur une zone
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
        }
    }

    // detection de collision directe avec le joueur
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().GetHit(damage);
        }
    }

    public void GetHit(int damage)
    {
        health -= damage;
        hitFx.Play();
    }
}
