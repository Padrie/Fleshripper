using System;
using System.Collections;
using ___Workdata.Scripts.Entity;
using UnityEngine;

public class Player : MonoBehaviour, IEntity
{
    [Header("Objects")] 
    public CharacterController characterController;
    public Transform body;

    [Header("Logic")] 
    public Vector3 velocity;
    public bool noGravity;
    public float gravity = -9.81f;
    public bool isGrounded;
    public bool canDash = true;
    public bool bufferJump;
    public float bufferTime = 0.4f;
    public float recoverTime = 0.2f;
    public bool canRecover;
    public float groundDistance = 1.2f;

    [Header("Movement")] 
    public float speed = 4f;
    public float airSpeed = 6f;
    public float jumpHeight = 5f;
    public float airDashTime = 0.3f, airDashSpeed = 50f;
    public float dashTime = 0.25f, dashSpeed = 75f;
    public float inputX, inputZ;

    [Header("Camera")] 
    public Camera camera;
    public float sensitivity = 400;
    public float cameraTilt = 2.5f, tiltSpeed = 2;
    public float fov = 60, speedFov = 10, fovSpeed = 2;
    public float rotX, rotY, rotZ;

    [Header("Stamina")] 
    public int staminaMax = 3;
    public int stamina;
    public int pointsToRegen = 6;
    public int currentPoints;
    public float staminaRegen = 2f;
    public bool regenStart;

    [Header("Combat")] 
    public bool inCombat;
    public bool inCombatArea;
    public int enemyDeathRegenAmount;
    public float combatTime = 10;
    public int resetTime = 10;


    [Header("Condition")] 
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Cheats")] 
    public bool infiniteStamina;
    private float input_rotX, input_rotY, input_rotZ;
    private float offsetX, offsetY, offsetZ;

    public static event Action pressedEsc;
    public static event Action onPlayerDied;

    private void Awake()
    {
        //rotY = 0;
        currentHealth = maxHealth;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //stamina = staminaMax;
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDied += AddHealth;
        FlyingEnemy.OnFlyingEnemyDied += AddHealth;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= AddHealth;
        FlyingEnemy.OnFlyingEnemyDied -= AddHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleStamina();

        CheckGrounded();

        HandleInput();

        HandleCameraMovement();
        
        CombatTimer();
        
        CheckCombat();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(StartBuffer());
            StartCoroutine(StartBuffer());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pressedEsc?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (!noGravity) HandleGravity();

        HandleVelocity();
    }

    public void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance);
    }

    public void HandleGravity()
    {
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
    }

    public static event Action OnPlayerDashed;

    public void HandleMomentum()
    {
        velocity.x += (transform.right * inputX + transform.forward * inputZ).x * airSpeed * 0.02f;
        velocity.z += (transform.right * inputX + transform.forward * inputZ).z * airSpeed * 0.02f;

        characterController.Move(velocity * Time.deltaTime);
    }

    public void HandleMovement()
    {
        if (isGrounded)
        {
            velocity.x = (transform.right * inputX + transform.forward * inputZ).x * speed;
            velocity.z = (transform.right * inputX + transform.forward * inputZ).z * speed;
        }
        else
        {
            velocity.x += (transform.right * inputX + transform.forward * inputZ).x * airSpeed * 0.02f;
            velocity.z += (transform.right * inputX + transform.forward * inputZ).z * airSpeed * 0.02f;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    public void HandleJump()
    {
        bufferJump = false;
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public void HandleJump(bool dashJump)
    {
        bufferJump = false;
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        if (dashJump) OnPlayerDashed.Invoke();
    }

    public void HandleAirDash()
    {
        velocity = (transform.right * InputDirectionH() + transform.forward * InputDirectionV()) * airDashSpeed;
        OnPlayerDashed.Invoke();
    }

    public void HandleDash()
    {
        velocity = (transform.right * InputDirectionH() + transform.forward * InputDirectionV()) * dashSpeed;
        OnPlayerDashed.Invoke();
    }

    public void HandleVelocity()
    {
        velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime);
        velocity.z = Mathf.Lerp(velocity.z, 0, Time.deltaTime);

        characterController.Move(velocity * Time.deltaTime);
    }

    public void HandleStamina()
    {
        if (infiniteStamina)
        {
            canDash = true;
        }
        else
        {
            canDash = stamina != 0;

            stamina = Mathf.Clamp(stamina, 0, staminaMax);
        }
    }


    public void HandleInput()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
    }

    private int InputDirectionH()
    {
        if (inputX == 0) return 0;

        return inputX > 0 ? 1 : -1;
    }

    private int InputDirectionV()
    {
        if (inputZ == 0) return 0;

        return inputZ > 0 ? 1 : -1;
    }

    public bool noInput()
    {
        return inputZ == 0 && inputX == 0;
    }

    private IEnumerator StartBuffer()
    {
        bufferJump = true;

        yield return new WaitForSeconds(bufferTime);

        bufferJump = false;
    }

    public IEnumerator StartRecover()
    {
        canRecover = true;

        yield return new WaitForSeconds(recoverTime);

        canRecover = false;
    }

    public bool TakeDamage(int damage)
    {
        currentHealth -= damage;
        AudioManager.instance.Play("PlayerDamaged");
	   ResetCombatTimer();
        if (currentHealth <= 0)
            Death();

        return true;
    }

    public void AddHealth(Enemy enemy)
    {
        currentHealth += enemyDeathRegenAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }    
    
    public void AddHealth(FlyingEnemy enemy)
    {
        currentHealth += enemyDeathRegenAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void Death()
    {
        onPlayerDied?.Invoke();
    }

    public void CheckCombat()
    {
        if (combatTime > resetTime)
        {
            inCombat = false;
        }
    }

    public void CombatTimer()
    {
        combatTime += Time.deltaTime;
    }

    public void ResetCombatTimer()
    {
        inCombat = true;
        combatTime = 0;
    }

    public void HandleCameraMovement()
    {
        //Mouse Input
        input_rotX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        input_rotY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        input_rotY = Mathf.Clamp(input_rotY, -90, 90);

        //Limit rot
        rotY = Mathf.Clamp(input_rotY, -90, 90);
        rotZ = Mathf.Lerp(rotZ, -inputX * cameraTilt, Time.deltaTime * tiltSpeed);

        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, fov + speedFov * InputDirectionV(),
            Time.deltaTime * fovSpeed);

        offsetY = Mathf.Lerp(offsetY, velocity.y, Time.deltaTime * 6);

        //Rotate Camera and Player(Body)
        camera.transform.localRotation = Quaternion.Euler(rotY + offsetY, 0, rotZ);

        //body.Rotate(Vector3.up * rotX);
        transform.rotation = Quaternion.Euler(0, input_rotX, 0);
    }
}