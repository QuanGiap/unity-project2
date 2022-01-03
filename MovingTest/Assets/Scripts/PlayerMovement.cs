using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controler;
    public float speed = 12f;
    Vector3 velocity;
    public float gravity=-9.8f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public float jumpHeight = 3f;
    public LayerMask groundmask;
    bool isGrounded;
    public GameObject[] Guns;
    public int GunChoose=0;
    GameSystem gameSystem;
    public GameObject ItemMenu;
    public GameObject Gameover;
    public float MaxHealth = 300f;
    public float Health;
    public float MaxShield = 300f;
    public float Shield;
    public float ShieldRegenSpeed = 10;
    public float TimeTakeToRegen = 5f;
    public float Total = 0f;
    public SliderManager slider;
    public Camera cam;
    //For shop
    public int MoneyHealth = 30;
    public int MoneyShieldRate = 10;
    public int MoneyShield = 30;
    public int MoneyRestore = 60;
    public int ShieldRateIncrease = 5;
    public int HealthIncrease = 50;
    public int ShieldIncrease = 50;
    //PowerUp
    public bool IsHealing = false;
    public bool IsShield = false;
    MoneySystem moneySystem;
    // Start is called before the first frame update
    private void Start()
    {
        gameSystem = GameObject.FindGameObjectWithTag("Game System").GetComponent<GameSystem>();
        moneySystem = GameObject.FindGameObjectWithTag("Game System").GetComponent<MoneySystem>();
        Guns[0].gameObject.SetActive(true);
        Health = MaxHealth;
        Shield = MaxShield;
        slider.SetHealthAndShield(MaxHealth, MaxShield);
        slider.ChangeHealthValue(MaxHealth);
        slider.ChangeShieldValue(MaxShield);
        Total = TimeTakeToRegen;
    }
    // Update is called once per frame
    void FixedUpdate()
    {       
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundmask);           //Check if the player is on the ground (Position to check, the distance that will turn boolean true on touch,the LAyer that checked like ground)
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;         //If it set 0, the top line will register before the player is completely on the ground
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        Vector3 move = (transform.right * x + transform.forward * z)*speed + transform.up*velocity.y;
        controler.Move(move*Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Q)||Input.GetAxis("Mouse ScrollWheel")!= 0)
        {
            Guns[GunChoose].GetComponent<Gun>().isRealoading = false;
            gameSystem.deleteReloading();
            Guns[GunChoose].SetActive(false);
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                do
                {
                    GunChoose++;
                    if (GunChoose > Guns.Length - 1) GunChoose = 0;
                } while (!Guns[GunChoose].GetComponent<Gun>().IsUnlock);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") <0)
            {
                do
                {
                    GunChoose--;
                    if (GunChoose < 0) GunChoose = Guns.Length - 1;
                } while (!Guns[GunChoose].GetComponent<Gun>().IsUnlock);
            }
            gameSystem.setReloading(Guns[GunChoose].GetComponent<Gun>().timeTakeReloading);
            Guns[GunChoose].SetActive(true);
            gameSystem.showAmmo(Guns[GunChoose].GetComponent<Gun>().ammo, Guns[GunChoose].GetComponent<Gun>().totalAmmo);
        }
        if ((Time.time > Total && Shield != MaxShield) || IsHealing)
        {
            regenShield();
            if (IsHealing) RegenHealth();
        }
        if (Health <= 0) Die();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Power Up"))
        {
            PowerUp power = other.GetComponent<PowerUp>();
            if (power.IsMoney)
            {
                moneySystem.AddMoney(80);
            }
            else
            {
                StartCoroutine(CountDown(power.duration, power));
            }
            Destroy(other.gameObject);
        }
    }
    void Die()
    {
        Destroy(gameObject);
        ItemMenu.SetActive(false);
        Gameover.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void TakeHit(float damage)
    {
        if (Shield > 0)
        {
            Shield -= damage;
            slider.ChangeShieldValue(Shield);
            if (Shield < 0) Shield = 0;            
        }
        else
        {
            Health -= damage;
            slider.ChangeHealthValue(Health);
        }
        Total = TimeTakeToRegen + Time.time;
    }
    void regenShield()
    {
        Shield += ShieldRegenSpeed * Time.deltaTime;
        slider.ChangeShieldValue(Shield);
        if (Shield > MaxShield) Shield = MaxShield;
    }
    void RegenHealth()
    {
        Health += ShieldRegenSpeed * Time.deltaTime;
        slider.ChangeHealthValue(Health);
        if (Health > MaxHealth) Health = MaxHealth;
    }
    IEnumerator CountDown(float Duration, PowerUp power)
    {
        gameSystem.ActiveText(power, Duration);
        if (power.IsHealing)
        {
            IsHealing = true;
            yield return new WaitForSeconds(Duration);
            IsHealing = false;
        }
        if (power.IsShield)
        {
            IsShield = true;
            yield return new WaitForSeconds(Duration);
            IsShield = false;
        }
    }
}
