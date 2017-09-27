﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletEmitter : MonoBehaviour
{
    public Transform BulletEmit;
    public GameObject BulletGfx;  
    public GameObject RailGfx;   
     
    // Shotgun fields
    public float ShotGunCooldown = 0.5f;           
    public int ShotGunPelletCount = 120;               
    public float ShotGunSpread = 40.0f;       
    public float ShotGunBulletSpeed = 100.0f;
    public float ShotGunClickTime = 0.2f;
    private float _nextShot;
    private Quaternion _pelletRot; // For bullet spread
    private float _shotTimer;
    
    // Nailgun fields
    public float NailGunRateOfFire = 40;
    public float NailGunBulletSpeed = 100.0f;
    public float NailGunSpread = 1.5f;
    public float QuadDamageSpread = 3f;
    public bool QuadDamage;
    private float _nailTimer;
    
    // Railgun fields
    public float RailGunChargeTime = 1f;
    private float _railTimer;

    [HideInInspector]
    public bool ShotReady;

    void Start()
    {
        _nextShot = ShotGunCooldown;
        NailGunRateOfFire = 1 / NailGunRateOfFire;
    }

    void Update()
    {
        // timer for setting nailgun rate of fire, clamped to avoid going higher than rate of fire
        _nailTimer += Time.deltaTime;
        _nailTimer = Mathf.Clamp(_nailTimer, 0, NailGunRateOfFire);
        
        FireShotGun();
        FireNailGun();
        FireRailGun();
    }

    /// <summary>
    /// Fires shotgun when tapping the right mouse button
    /// </summary>
    private void FireShotGun()
    {
        ShotReady = _nextShot >= ShotGunCooldown;

        // start timer to distinguish shotgun from railgun by firing shotgun on fast click 
        if (Input.GetMouseButton(1))
        {
            _shotTimer += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (_shotTimer < ShotGunClickTime && ShotReady) // right mouse button
            {
                // Play sound(s)
                FindObjectOfType<AudioManager>().Play("shotgunBass");

                Quaternion rot = new Quaternion(BulletGfx.transform.rotation.x, BulletGfx.transform.rotation.x,
                    BulletGfx.transform.rotation.x, BulletGfx.transform.rotation.w);

                BulletGfx.transform.rotation =
                    Quaternion.RotateTowards(BulletGfx.transform.rotation, rot, ShotGunSpread);

                // Begin Cooldown
                _nextShot = 0;

                for (int i = 0; i < ShotGunPelletCount; i++)
                {
                    _pelletRot = Random.rotation;

                    GameObject activePellet = Instantiate(BulletGfx, BulletEmit.position, BulletEmit.rotation);
                    activePellet.transform.rotation =
                        Quaternion.RotateTowards(activePellet.transform.rotation, _pelletRot, ShotGunSpread);

                    activePellet.GetComponent<Rigidbody>().velocity =
                        activePellet.transform.forward * ShotGunBulletSpeed;
                    Destroy(activePellet, 0.11f); // essentially reduces range
                }
            }
            _shotTimer = 0; 
        }

        // Charge next shot
        _nextShot += 1.0f * Time.deltaTime;

        // Stop charging when fire is ready      
        if (_nextShot > ShotGunCooldown) _nextShot = ShotGunCooldown;
    }

    /// <summary>
    /// Fires nailgun with left click
    /// </summary>
    private void FireNailGun()
    {
        if (Input.GetMouseButton(0) && _nailTimer >= NailGunRateOfFire) // left mouse button
        {        
            GameObject activeBullet = Instantiate(BulletGfx, BulletEmit.position, BulletEmit.rotation);
            activeBullet.transform.rotation = 
                Quaternion.RotateTowards(activeBullet.transform.rotation, Random.rotation, NailGunSpread);
            activeBullet.GetComponent<Rigidbody>().velocity = activeBullet.transform.forward * NailGunBulletSpeed;
            
            if (QuadDamage) // if quad damage is on then shoot four bullets at a time
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject bullet = Instantiate(BulletGfx, BulletEmit.position, BulletEmit.rotation);
                    bullet.transform.rotation = 
                        Quaternion.RotateTowards(activeBullet.transform.rotation, Random.rotation, QuadDamageSpread);
                    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * NailGunBulletSpeed;
                }
            }
            _nailTimer = 0; // reset rate of fire   
        }
    }

    /// <summary>
    /// Fires the rail gun after holding the right mouse button for a period of time
    /// </summary>
    private void FireRailGun()
    {   
        if (Input.GetMouseButton(1))
        {
            // begin charging
            _railTimer += Time.deltaTime;
            Debug.Log("Charging " + _railTimer);
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (_railTimer >= RailGunChargeTime)
            {
                Debug.Log("Fire Rail Gun");
                GameObject activeBullet = Instantiate(RailGfx, BulletEmit.position, BulletEmit.rotation);
                
            }
            _railTimer = 0;
        }
    }
}
