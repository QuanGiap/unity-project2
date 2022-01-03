using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public bool isRealoading = false;                 //check if the gun is realoading
    public GameObject Enemy;
    public ParticleSystem muzzleFlash=null;   //effect muzzle flash
    public GameObject impactEffect = null;      //impact effect
    //Gun static   
    public float damage = 10f;                 //damage of gun
    public float range = 100f;                 //shooting range
    public float impactForce = 200f;            //impact force
    public float MaxDeviation = 5;
    float timeToShoot = 0f;
    public int ammoclip;                       //how many ammo in a clip
    public int maxAmmo;                         //MAX ammo for gun
    public int totalAmmo;                                   //total ammo for gun
    public int ammo;                                   //remain ammo in a clip
    public float timeTakeReloading;              //Time take to reload
    public float fireRate = 12f;

    //for shotgun
    public bool isSpread = false;                 //Is this weapons can fire many bullet at once shot   
    public int shotgunBullet;                   //how many bullet in 1 shell of the shotgun

    private void Awake()
    {
        totalAmmo = maxAmmo;
        ammo = ammoclip;
    }
    // Update is called once per frame
    void Update()
    {
        /// When AI is out of ammo, it will auto reload the gun
        if (ammo == 0 && !isRealoading && totalAmmo != 0)
        {
            StartCoroutine(realoading());
        }
        //check if the player is press left mouse, still have ammo and not reloading
        if (Enemy.GetComponent<Target>().playerInAttackRange && Time.time >= timeToShoot && !isRealoading && ammo != 0)
        {
            timeToShoot = Time.time + 1f / fireRate;   //calculate next time to shot
            shoot();
        }
    }
    /// Add bullet spread for the gun by using vector to make the line of bullet fly randomly base on Deviation
    Vector3 addBulletSpread(float maxDeviation)
    {
        Vector3 forwardVector = Vector3.forward;              //create Vector 3 that is (0,0,1)
        float deviation = Random.Range(0f, maxDeviation);     //generate random deviation
        float angle = Random.Range(0f, 360f);                 //generate random angle from 0f to 360
        forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector; // Sets the Vector current rotation to a new rotation that rotates deviation degrees around the y-axis(Vector3.up)
        forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;// Sets the Vector current rotation to a new rotation that rotates angle degrees around the z-axis(Vector3.forward)
        forwardVector = transform.rotation * forwardVector;
        return forwardVector;
    }
    void shoot()
    {
        muzzleFlash.Play();
        if (isSpread)                           //check is this a shotgun
        {
            RaycastHit[] hits = new RaycastHit[shotgunBullet];           //Create a multi line of bullets for shotgun
            for (int i = 0; i < shotgunBullet; i++)
            {
                if (Physics.Raycast(transform.position, addBulletSpread(MaxDeviation), out hits[i], range))  //Check if the gun shoot hit the object (position start, position to go,name the object going to get hit,range)
                {
                    onHit(hits[i]);
                }
            }

        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, addBulletSpread(MaxDeviation), out hit, range))  //Check if the gun shoot hit the object (position start, position to go,name the object going to get hit,range)
            {
                onHit(hit);
            }
        }
        ammo--;//less bullet on shoot
    }
    IEnumerator realoading()
    {
        isRealoading = true;                                    //set to true to make the weapon can't fire while reloading
        if (isSpread)                                           //check if it a shotgun
        {
            while (ammo != ammoclip && totalAmmo != 0 && isRealoading)  //while ammo is not full cliped and total is still have ammo for reloading and it is Realoading
            {
                totalAmmo--;                                            //Total ammo lost 1
                ammo++;                                                 //ammo + 1
                yield return new WaitForSeconds(timeTakeReloading);     //wait for timeTakeReloading seconds
            }
            isRealoading = false;                                       //set reloading is complete
        }
        else
        {
            yield return new WaitForSeconds(timeTakeReloading);         //wait for 
            if (totalAmmo >= (ammoclip - ammo))                              //check if the ammo total is greater than ammo need to add in
            {
                totalAmmo -= (ammoclip - ammo);                         //take some ammo from totalAmmo
                ammo = ammoclip;
            }
            else
            {
                ammo += totalAmmo;
                totalAmmo = 0;
            }
            isRealoading = false;                                       //set back to false so the weapon can fire again
        }

    }
    private void onHit(RaycastHit hit)
    {
        //Debug.Log(hit.transform.name);
        PlayerMovement Player = hit.transform.GetComponent<PlayerMovement>();                     //Remember to change back PlayerMoverment when making real game
        if (Player != null)
        {
            if(!Player.IsShield) Player.TakeHit(damage);
            DI_Ssytem.CreateIndicator(this.transform);
        }
        if (impactEffect != null)
        {
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 0.5f);
        }
        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(-hit.normal * impactForce);
        }
    }
}
