using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletProperties : MonoBehaviour {

    public float lifetime = 2.0f;           //Time untill bullets clear
    int health;

    void Start () {
        Destroy(gameObject, lifetime);
    }
	
	// Update is called once per frame
	void Update () {

		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hitbox")
        {
            other.gameObject.GetComponent<EntityHealth>().Health -= 1;
            FindObjectOfType<AudioManager>().Play("codHit");
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Matter" || other.gameObject.tag == "Hazard")
        {
            Destroy(gameObject);
        }

    }
}
