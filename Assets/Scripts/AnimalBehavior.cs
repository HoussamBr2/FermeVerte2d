using UnityEngine;

public class AnimalBehavior : MonoBehaviour
{
<<<<<<< HEAD
    
=======
<<<<<<< HEAD
    public Animator animator;
    public bool isDayActive;

    void Update()
    {
        if (isDayActive)
            animator.Play("Walk");
        else
            animator.Play("Sleep");
=======
    // === State Management ===
>>>>>>> 8bc831299161ad3aa5f47b3508df185dd8833585
    private enum AnimalState { Wandering, Sleeping }
    private AnimalState currentState = AnimalState.Wandering;

    
    private DayNightCycle dayNightCycle;
    private Rigidbody2D rb;
    private Animator anim;        
    private AudioSource audioSource;

    // === Audio Settings ===
    [Header("Audio Settings")]
    public AudioClip cluckSound;
    public AudioClip sleepSound;      
    public float minCluckInterval = 5f;
    private float cluckTimer;

    // === Movement Settings ===
    [Header("Wandering Settings")]
    public float moveSpeed = 1.5f;
    public float wanderingRadius = 5.0f;
    public float changeTargetInterval = 3.0f;

    // === Internal State ===
    private Vector2 startPosition;
    private Vector2 currentTarget;
    private float targetTimer;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>(); 

        
        if (rb == null) Debug.LogError("Rigidbody2D component not found!");
        if (anim == null) Debug.LogError("Animator component not found!");

        
        dayNightCycle = FindObjectOfType<DayNightCycle>();
        if (dayNightCycle == null)
        {
            Debug.LogError("DayNightCycle script not found! Animal cannot sync behavior.");
        }

        
        startPosition = transform.position;
        SetNewWanderTarget();
        cluckTimer = Random.Range(0f, minCluckInterval);
    }

    void Update()
    {
        
        if (dayNightCycle != null)
        {
            if (dayNightCycle.IsNightTime)
            {
                TransitionToSleep();
            }
            else
            {
                TransitionToWander();
            }
        }

        
        if (currentState == AnimalState.Wandering)
        {
            Wander();
            HandleClucking();

            
            if (anim != null)
            {
                
                bool isMoving = rb.linearVelocity.sqrMagnitude > 0.1f;
                anim.SetBool("IsMoving", isMoving);

                
                if (rb.linearVelocity.x < -0.01f)
                {
                    
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                }
                
                else if (rb.linearVelocity.x > 0.01f)
                {
                    
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                }
            }
        }
        else if (currentState == AnimalState.Sleeping)
        {
            
            rb.linearVelocity = Vector2.zero;

            
            if (anim != null)
            {
                anim.SetBool("IsMoving", false);
                anim.SetBool("IsSleeping", true); 
            }
        }
    }

    

    private void TransitionToSleep()
    {
        if (currentState != AnimalState.Sleeping)
        {
            currentState = AnimalState.Sleeping;

            
            if (audioSource != null && sleepSound != null)
            {
                audioSource.PlayOneShot(sleepSound);
            }
            
            rb.linearVelocity = Vector2.zero;

            Debug.Log(gameObject.name + " is going to sleep.");
        }
    }

    private void TransitionToWander()
    {
        if (currentState != AnimalState.Wandering)
        {
            currentState = AnimalState.Wandering;

            
            if (anim != null)
            {
                anim.SetBool("IsSleeping", false);
            }

            SetNewWanderTarget();
            cluckTimer = minCluckInterval; 

            Debug.Log(gameObject.name + " woke up and is wandering.");
        }
    }

    
    private void HandleClucking()
    {
        cluckTimer -= Time.deltaTime;
        if (cluckTimer <= 0f && audioSource != null && cluckSound != null)
        {
            audioSource.PlayOneShot(cluckSound);
            
            cluckTimer = minCluckInterval + Random.Range(-1f, 1f);
        }
    }

    
    private void Wander()
    {
        
        targetTimer -= Time.deltaTime;
        if (targetTimer <= 0f)
        {
            SetNewWanderTarget();
        }

        
        Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;

        
        rb.linearVelocity = direction * moveSpeed;

        
        if (Vector2.Distance(transform.position, currentTarget) < 0.2f)
        {
            targetTimer = 0f;
        }
    }

    private void SetNewWanderTarget()
    {
        Vector2 randomOffset = Random.insideUnitCircle * wanderingRadius;
        currentTarget = startPosition + randomOffset;
        targetTimer = changeTargetInterval;
>>>>>>> 9d4174ce1b7ab702d8d064d3f13d9698563e7308
    }
}


