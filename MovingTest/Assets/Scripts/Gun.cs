using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool IsUnlock = false; //Check is the player has buy that gun

    public bool isRealoading=false;                 //check if the gun is realoading
    public Camera fpsCam;
    public Camera WeaponCam;
    public ParticleSystem muzzleFlash = null;   //effect muzzle flash
    public GameObject impactEffect = null;      //impact effect
    //Gun static   
    public float damage = 10f;                 //damage of gun
    public float range = 100f;                 //shooting range
    public float impactForce = 200f;            //impact force
    public float MaxDeviation = 5;
    public float Deviation = 0;
    public float speedSpread = 20;
    float timeToShoot = 0f;
    public int ammoclip;                       //how many ammo in a clip
    public int maxAmmo;                         //MAX ammo for gun
    public int totalAmmo;                                   //total ammo for gun
    public int ammo;                                   //remain ammo in a clip
    public float timeTakeReloading;              //Time take to reload
    public float fireRate = 12f;

    //for shotgun
    public bool isSpread=false;                 //Is this weapons can fire many bullet at once shot   
    public int shotgunBullet;                   //how many bullet in 1 shell of the shotgun

    public float fieldCam;                      //set cam field of view that will set on user click right click
    private float originalFieldCam;              //the original of fieldCam
    public Vector3 upRecoil;                    //recoil for animation
    Vector3 originalRotaion;                    //original for animation
    bool IsMenu = false;
    GameSystem gameSystem;
    //for Shop
    public int MoneyDamage=20;
    public int MoneyAccuracy=20;
    public int MoneyUnlock = 50;
    public int MoreDamage = 10;
    public int MoreAccuracy = 1;
    public int MaxReduceAccuracy = 2;
    private void Awake()
    {
        totalAmmo = maxAmmo;
        originalRotaion = transform.localEulerAngles;
        upRecoil += transform.localEulerAngles;
        originalFieldCam = fpsCam.fieldOfView;
        ammo = ammoclip;
        gameSystem = GameObject.Find("Game System Manager").GetComponent<GameSystem>();
        gameSystem.setReloading(timeTakeReloading);
        gameSystem.showAmmo(ammo, totalAmmo);
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsMenu)       //Check if the player is opening the menu
        {
            if (Input.GetMouseButtonDown(1)) setFocuse(0.8f, 1, fpsCam.depth - 1);                 //check if the player scope in or not
            else if (Input.GetMouseButtonUp(1)) setFocuse(1.25f, 0, fpsCam.depth + 1);
            /// <summary>
            /// Check for reloading by:
            /// 
            /// player is press button R
            /// gun is not fulled clip
            /// 
            /// or
            /// 
            /// player is press left mouse
            /// gun is out of ammo
            /// 
            /// and both condition with
            /// 
            /// not reloading
            /// Still have ammo left for reloading
            /// </summary>
            if (((Input.GetKeyDown(KeyCode.R) && ammo != ammoclip) || (Input.GetMouseButton(0) && ammo == 0)) && !isRealoading && totalAmmo != 0)
            {
                StartCoroutine(realoading());
            }
            //check if the player is press left mouse, still have ammo and not reloading
            if (Input.GetMouseButton(0) && Time.time >= timeToShoot && !isRealoading && ammo != 0)
            {
                timeToShoot = Time.time + 1f / fireRate;   //calculate next time to shot
                shoot();
                if (Deviation < MaxDeviation) Deviation += Time.deltaTime * speedSpread;  //Bullet spread more as long as the player is shooting
                StartCoroutine(recoil());                           //set the gun has recoil           
            }
            if ((!Input.GetMouseButton(0) || isRealoading) && !isSpread && Deviation >= 0) Deviation -= Time.deltaTime*(speedSpread/3);  //bullet spread slowly cool down (not apply for shotgun since it a static value)
            if (Input.GetMouseButtonDown(0) && isSpread && isRealoading)                  //allow shotgun to shoot if in reloading which will cancel the reloading
            {
                isRealoading = false; 
                gameSystem.deleteReloading();
                ammo--;                             //to fix a bug that cause a shotgun to have extra ammo even though the reloading progress not go all the way
                totalAmmo++;                        //same to a bug above but cause total ammo to lost 1 ammo
                gameSystem.showAmmo(ammo, totalAmmo);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) IsMenu = !IsMenu;
    }
    /// Add bullet spread for the gun by using vector to make the line of bullet fly randomly base on Deviation
    Vector3 addBulletSpread(float maxDeviation)
    {
        Vector3 forwardVector = Vector3.forward;              //create Vector 3 that is (0,0,1)
        float deviation = Random.Range(0f, maxDeviation);     //generate random deviation
        float angle = Random.Range(0f, 360f);                 //generate random angle from 0f to 360
        forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector; // Sets the Vector current rotation to a new rotation that rotates deviation degrees around the y-axis(Vector3.up)
        forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;// Sets the Vector current rotation to a new rotation that rotates angle degrees around the z-axis(Vector3.forward)
        forwardVector = fpsCam.transform.rotation * forwardVector;
        return forwardVector;
    }
    void shoot()
    {
        muzzleFlash.Play();
        if (isSpread)                           //check is this a shotgun
        {
            Deviation = MaxDeviation;
            RaycastHit[] hits = new RaycastHit[shotgunBullet];           //Create a multi line of bullets for shotgun
            for (int i = 0; i < shotgunBullet; i++)
            {
                if (Physics.Raycast(fpsCam.transform.position, addBulletSpread(Deviation), out hits[i], range))  //Check if the gun shoot hit the object (position start, position to go,name the object going to get hit,range)
                {
                    onHit(hits[i]);
                }
            }

        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, addBulletSpread(Deviation), out hit, range))  //Check if the gun shoot hit the object (position start, position to go,name the object going to get hit,range)
            {
                onHit(hit);
            }
        }
        ammo--;//less bullet on shoot
        gameSystem.showAmmo(ammo, totalAmmo);//show ammo and total left
    }

    IEnumerator recoil()              //set recoil for the gun
    {
        transform.localEulerAngles = Vector3.Lerp(originalRotaion, upRecoil, 1);           //Set gun rotation that is shooting
        yield return new WaitForSeconds(0.05f);                                             //wait for 0.05 second
        transform.localEulerAngles = Vector3.Lerp(originalRotaion, upRecoil, 0);            //Set gun rotaion to original
    }
    private void setFocuse(float reduceRecoil, int choice,float cam)                   //set focuse aim for the gun, which can look futher and less recoil (doesn't work on shotgun)
    {      
        if (!isSpread)
        {
            WeaponCam.depth = cam;
            Deviation *= reduceRecoil;
            fpsCam.fieldOfView = Mathf.Lerp(originalFieldCam, fieldCam, choice);
        }
    }
    IEnumerator realoading()
    {
        isRealoading = true;                                    //set to true to make the weapon can't fire while reloading
        if (isSpread)                                           //check if it a shotgun
        {
            while (ammo != ammoclip&& totalAmmo != 0&& isRealoading)  //while ammo is not full cliped and total is still have ammo for reloading and it is Realoading
            {                                           
                totalAmmo--;                                            //Total ammo lost 1
                ammo++;                                                 //ammo + 1
                yield return new WaitForSeconds(timeTakeReloading);     //wait for timeTakeReloading seconds
                gameSystem.showAmmo(ammo, totalAmmo);                   //show ammo and total ammo
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
            gameSystem.showAmmo(ammo, totalAmmo);
            isRealoading = false;                                       //set back to false so the weapon can fire again
        }
        
    }
    private void onHit(RaycastHit hit)
    {
        Debug.Log(hit.transform.name);
        Target target = hit.transform.GetComponent<Target>();                                                  //get compoment from the hit object
        Head targetHead = hit.transform.GetComponent<Head>();
        if (targetHead != null)
        {      
            targetHead.takeHitHead(damage);
        }
        if (target != null)
        {            
            target.takeDamage(damage);
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
    //Upgrade menu
    public void UpgradeMoreDamage()
    {
        damage += 10;
    }
    public void DecreseAccuracy()
    {
        if(MaxDeviation > 0)
            MaxDeviation -= 1;
    }
    public void UnlockWeapon()
    {
        IsUnlock = true;
    }
    }
