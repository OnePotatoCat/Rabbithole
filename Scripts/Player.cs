using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform body;
    [SerializeField] Transform lanternTrans;
    [SerializeField] Light2D lantern;
    [SerializeField] PolygonCollider2D lanternCollider;
    [SerializeField] LanternMeter lanternMeter;
    [SerializeField] LanternMeter staminaMeter;
    [SerializeField] CanvasGroup alterCanvas;
    [SerializeField] CanvasGroup escapeCanvas;
    [SerializeField] FadeCanvas fadeCanvas;
    [SerializeField] CanvasGroup gameoverCanvas;
    [SerializeField] List<GameObject> medalUI;

    [SerializeField] Animator animator;

    public float speed = 4.0f;
    public float speedBoostBonus = 0.75f;
    private float speedBoost;
    public float stamina = 100f;
    public float staminaRechargeCD = 3f;
    float staminaCD = 0;
    public float lanternFuel = 100f;

    [SerializeField] Canvas DialogCanvas;
    [SerializeField] Dialog dialog;
    public int medallionFound = 0;
    public string dialogText;

    public float lightIntensity =1f;
    public float maxLightIntensity = 2.8f;
    const float minLightIntensity = 1f;
    float intensityRate;

    float lightOuterRad = 5f;
    const float maxLightOuterRad = 17.5f;
    const float minLightOuterRad = 5f;
    float OuterRadRate;

    float lightInnerRad = 1f;
    const float maxLightInnerRad = 3.5f;
    const float minLightInnerRad = 1f;
    float InnerRadRate;

    float lightOuterAngle = 250f;
    const float maxLightOuterAngle = 250f;
    const float minLightOuterAngle = 30f;
    float OuterAngleRate;

    float lightInnerAngle = 160f;
    const float maxLightInnerAngle = 160f;
    const float minLightInnerAngle = 5f;
    float InnerAngleRate;

    public bool foundLantern = false;

    void Start()
    {
        intensityRate = (maxLightIntensity - minLightIntensity) /0.7f;
        OuterRadRate = (maxLightOuterRad - minLightOuterRad) / 1.4f;
        InnerRadRate = (maxLightInnerRad - minLightInnerRad) / 0.7f;
        OuterAngleRate = (maxLightOuterAngle - minLightOuterAngle)/0.7f;
        InnerAngleRate = (maxLightInnerAngle - minLightInnerAngle)/0.7f;
        lanternMeter.SetMaxMeter(lanternFuel);
        staminaMeter.SetMaxMeter(stamina);
    }

    void Update()
    {
        Movement();
        Rotation();
        UpdateStaminaMeter();

        if (foundLantern)
        {
            FocusLantern();
            UpdateLanternFuel();
            lanternMeter.SetMeter(lanternFuel);
        }
        EnableLaternCollider();
    }

    private void UpdateStaminaMeter()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            staminaCD += Time.deltaTime;
            staminaCD = Mathf.Clamp(staminaCD, 0f, 3.1f);
            speedBoost = 0f;
        }
        else
        {
            staminaCD = 0f;
        }

        if (staminaCD >= staminaRechargeCD)
        {
            stamina += 2.5f * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0f, 100f);
        }

        staminaMeter.SetMeter(stamina);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lantern"))
        {
            EquipLantern();
            //pause and show the menuu
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            Time.timeScale = 0f;
            fadeCanvas.FadeIn(2f);
            gameoverCanvas.gameObject.SetActive(true);
        }
    }

    public void EscapeAttempt()
    {
        Time.timeScale = 0f;
        escapeCanvas.gameObject.SetActive(true);
    }

    public void TouchAlter()
    {
        if (medallionFound == 3)
        {
            Time.timeScale = 0f;
            //escapeCanvas.gameObject.SetActive(true);
            alterCanvas.gameObject.SetActive(true);
        }
        else
        {
            dialogText = "There is a medal slot on the alter. I wonder what fits on it?";
            dialog.SpawnDialogBox(dialogText);
        }
    }

    public void GainAMedallion()
    {
        medallionFound++;
        switch (medallionFound)
        {
            case 1:
                dialogText = "Ooo souvenir.";
                medalUI[0].SetActive(true);
                break;
            case 2:
                dialogText = "Cool! Looks like it could be medal!";
                medalUI[1].SetActive(true);
                break;
            case 3:
                dialogText = "Yes! ";
                medalUI[2].SetActive(true);
                break;
            default:
                break;
        }
        dialog.SpawnDialogBox(dialogText);
    }

    public void EquipLantern()
    {
        dialogText = "...Cool! A carrot shape lantern!";
        lantern.gameObject.SetActive (true);
        dialog.SpawnDialogBox(dialogText);
        foundLantern = true;
    }

    public void RefuelLantern()
    {
        lanternFuel += 30f;
        lanternFuel = Mathf.Clamp(lanternFuel, 0f, 100f);
        UpdateLanternFuel();
    }

    private void UpdateLanternFuel()
    {
        lanternFuel = Mathf.Clamp((lanternFuel-(lightIntensity * 0.25f)*Time.deltaTime), 0 , 100); 
    }

    private void EnableLaternCollider()
    {
        if (lightIntensity >= 2.49f)
        {
            lanternCollider.enabled = true;
        }
        else
        {
            lanternCollider.enabled = false;
        }
    }

    private void FocusLantern()
    {

        if (Input.GetMouseButton(0) && lanternFuel > 0)
        {
            lightIntensity += intensityRate*Time.deltaTime;
            lightOuterRad += OuterRadRate * Time.deltaTime;
            lightInnerRad += InnerRadRate * Time.deltaTime;
            lightOuterAngle -= OuterAngleRate * Time.deltaTime;
            lightInnerAngle -= InnerAngleRate * Time.deltaTime;
        }
        else
        {
            lightIntensity -= intensityRate * Time.deltaTime;
            lightOuterRad -= 2*OuterRadRate * Time.deltaTime;
            lightInnerRad -= 2*InnerRadRate * Time.deltaTime;
            lightOuterAngle += OuterAngleRate * Time.deltaTime;
            lightInnerAngle += InnerAngleRate * Time.deltaTime;
        }

        lightIntensity = Mathf.Clamp(lightIntensity, minLightIntensity, maxLightIntensity);
        lightOuterRad = Mathf.Clamp(lightOuterRad, minLightOuterRad, maxLightOuterRad);
        lightInnerRad = Mathf.Clamp(lightInnerRad, minLightInnerRad, maxLightInnerRad);
        lightOuterAngle = Mathf.Clamp(lightOuterAngle, minLightOuterAngle, maxLightOuterAngle);
        lightInnerAngle = Mathf.Clamp(lightInnerAngle, minLightInnerAngle, maxLightInnerAngle);


        lantern.intensity = lightIntensity* (0.5f + lanternFuel/200f);
        lantern.pointLightOuterRadius = lightOuterRad;
        lantern.pointLightInnerRadius = lightInnerRad;
        lantern.pointLightOuterAngle = lightOuterAngle;
        lantern.pointLightInnerAngle = lightInnerAngle;
    }

    private void Movement()
    {
        var xMovement = Input.GetAxisRaw("Horizontal");
        var yMovement = Input.GetAxisRaw("Vertical");
        bool movementBoost = Input.GetKey(KeyCode.LeftShift);

        if (stamina > 0)
        {
            if (movementBoost)
            {
                stamina -= 5.0f * Time.deltaTime;
                stamina = Mathf.Clamp(stamina, 0f, 100f);
                speedBoost = speedBoostBonus;
            }
        }
        else
        {
            speedBoost = 0f;
        }

        if (Mathf.Abs(xMovement) == Mathf.Abs(yMovement) && (xMovement !=0 || yMovement !=0))
        {
            xMovement = 0.707f * xMovement;
            yMovement = 0.707f * yMovement;
        }

        animator.SetFloat("xSpeed", xMovement);
        animator.SetFloat("ySpeed", yMovement);
        if (yMovement == 0 && yMovement == 0)
        {
            animator.SetBool("Stop", true);
        }
        else
        {
            animator.SetBool("Stop", false);
        }

        rb.velocity = new Vector2(xMovement * (speed * (1+speedBoost)), yMovement * (speed * (1+speedBoost)));

    }

    private void Rotation()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 direction = new Vector2 (mousePos.x - lanternTrans.position.x, mousePos.y - lanternTrans.position.y);

        lanternTrans.up = direction;
    }
}
