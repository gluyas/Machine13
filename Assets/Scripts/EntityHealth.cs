using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    public int Health = 3;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            if (transform.parent == null)
                Destroy(gameObject, 0.0f);
            if (transform.parent != null) { 
                if (transform.parent.parent != null)
                    Destroy(transform.parent.parent.gameObject, 0.0f);

                if (transform.parent != null)
                    Destroy(transform.parent.gameObject, 0.0f);
            }
        }
    }
}
