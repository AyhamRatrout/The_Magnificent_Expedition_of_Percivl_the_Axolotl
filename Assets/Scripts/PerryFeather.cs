using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerryFeather : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(TimedDeath());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D (Collider2D other) {
        if(other.tag == "Player"){
            other.GetComponent<PlayerHealth>().TakeDamage(1);
            Destroy(this.gameObject);
        }
        if(other.tag == "Boundary"){Destroy(this.gameObject);}
    }
}
