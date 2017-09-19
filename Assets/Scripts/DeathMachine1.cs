using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMachine1 : MonoBehaviour {
    public GameObject projectileGFX;                            //Get projectile graphic
    public float projectileSpeed = 10.0f;                       //Speed of projectile
    public float rotationsPerMinute = 5.0f;
    public float shotRate = 3.0f;

    private float counter;
    private Quaternion bulletRot;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 6.0f * rotationsPerMinute * Time.deltaTime, 0);
        counter += 1 * Time.deltaTime;

        if (counter >= shotRate){
        for (int i = 0; i < 4; i++) {
            bulletRot *= Quaternion.Euler (0,90, 0);

            GameObject activebullet = Instantiate(projectileGFX, transform.position, transform.rotation) as GameObject;
            activebullet.transform.rotation = Quaternion.RotateTowards(activebullet.transform.rotation, bulletRot, 90);
            
            
            

            Rigidbody activeBulletRB = activebullet.GetComponent<Rigidbody>();
            activeBulletRB.velocity = activebullet.transform.forward * projectileSpeed;
            Destroy(activebullet, 6);
            }
            counter = 0;
        }
    }
}
