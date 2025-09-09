using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnnemyBehaviour : MonoBehaviour
{

    public float speed;
    private Transform target;

    public Animator animator;

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            Vector3 targetNoY = new Vector3(target.position.x, 0, target.position.z); // ignore height
            transform.LookAt(targetNoY);
            animator.SetBool("Run", true);
        }
    }





    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
        }
    }
}
