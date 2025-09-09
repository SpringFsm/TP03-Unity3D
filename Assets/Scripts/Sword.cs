using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private int swordDamage = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ennemy") && other is CapsuleCollider)
        {
            other.GetComponent<EnnemyBehaviour>().GetHit(swordDamage);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
