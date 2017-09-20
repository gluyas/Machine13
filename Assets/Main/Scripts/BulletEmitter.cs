using UnityEngine;

public class BulletEmitter : MonoBehaviour
{
    public Transform BulletEmit;                 
    public GameObject BulletGfx;                    
    public float BulletSpeed = 100.0f;              
    public float ShotGunCooldown = 0.25f;           
    public int PelletCount = 6;               
    public float ShotSpread = 5.0f;                 

    private float _nextShot;

    // For bullet spread
    private Quaternion _pelletRot;

    [HideInInspector]
    public bool ShotReady;

    void Start()
    {
        _nextShot = ShotGunCooldown;
    }

    void Update()
    {
        FireShotGun();
        FireNailGun();
    }

    // Fire shotgun with right click
    private void FireShotGun()
    {
        ShotReady = _nextShot >= ShotGunCooldown;

        if (Input.GetMouseButtonDown(1) && ShotReady) // right mouse button
        {
            // Play sound(s)
            FindObjectOfType<AudioManager>().Play("shotgunBass");

            // Begin Cooldown
            _nextShot = 0;

            for (int i = 0; i < PelletCount; i++)
            {
                _pelletRot = Random.rotation;
                
                GameObject activePellet = Instantiate(BulletGfx, BulletEmit.position, BulletEmit.rotation);
                activePellet.transform.rotation = 
                    Quaternion.RotateTowards(activePellet.transform.rotation, _pelletRot, ShotSpread);

                activePellet.GetComponent<Rigidbody>().velocity = activePellet.transform.forward * BulletSpeed;
                Destroy(activePellet, 0.11f); // essentially reduces range
            }
        }
        // Charge next shot
        _nextShot += 1.0f * Time.deltaTime;

        // Stop charging when fire is ready      
        if (_nextShot > ShotGunCooldown) _nextShot = ShotGunCooldown;
    }

    private void FireNailGun()
    {
        if (Input.GetMouseButtonDown(0)) // left mouse button
        {
            Debug.Log("Fire NailGun");
            
        }
    }
}
