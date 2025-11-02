using UnityEngine;

public class AnimalBehavior : MonoBehaviour
{
    // === State Management ===
    private enum AnimalState { Wandering, Sleeping }
    private AnimalState currentState = AnimalState.Wandering;

    // === References ===
    private DayNightCycle dayNightCycle;
    private Rigidbody2D rb;
    private Animator anim;        // NEW: Reference to the Animator component
    private AudioSource audioSource;

    // === Audio Settings ===
    [Header("Audio Settings")]
    public AudioClip cluckSound;
    public AudioClip sleepSound;      // Optional: A soft sigh or "zzz" sound
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
        // 1. Get Components
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>(); // GET THE ANIMATOR

        // Error Checks
        if (rb == null) Debug.LogError("Rigidbody2D component not found!");
        if (anim == null) Debug.LogError("Animator component not found!");

        // 2. Find the Day/Night Controller
        dayNightCycle = FindObjectOfType<DayNightCycle>();
        if (dayNightCycle == null)
        {
            Debug.LogError("DayNightCycle script not found! Animal cannot sync behavior.");
        }

        // 3. Set up initial state
        startPosition = transform.position;
        SetNewWanderTarget();
        cluckTimer = Random.Range(0f, minCluckInterval);
    }

    void Update()
    {
        // Check the time of day and transition states
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

        // Execute behavior based on the current state
        if (currentState == AnimalState.Wandering)
        {
            Wander();
            HandleClucking();

            // ANIMATION & SPRITE FLIPPING:
            if (anim != null)
            {
                // Set IsMoving based on if the chicken is actually moving
                // sqrMagnitude is more efficient than magnitude for checking if velocity is non-zero
                bool isMoving = rb.linearVelocity.sqrMagnitude > 0.1f;
                anim.SetBool("IsMoving", isMoving);

                // Flip sprite to face direction of movement
                if (rb.linearVelocity.x < -0.01f)
                {
                    // SWAP THIS: Use scale 1 if your chicken sprite faces LEFT by default, or you had the logic backwards.
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                }
                // If moving right (X is positive)
                else if (rb.linearVelocity.x > 0.01f)
                {
                    // SWAP THIS: Use scale -1 if your chicken sprite faces LEFT by default, or you had the logic backwards.
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                }
            }
        }
        else if (currentState == AnimalState.Sleeping)
        {
            // Animal is sleeping (Idling/Stationary at night)
            rb.linearVelocity = Vector2.zero;

            // Ensure the Idle animation plays while stationary
            if (anim != null)
            {
                anim.SetBool("IsMoving", false);
                anim.SetBool("IsSleeping", true); // Trigger the sleep transition
            }
        }
    }

    // --- State Transition Methods ---

    private void TransitionToSleep()
    {
        if (currentState != AnimalState.Sleeping)
        {
            currentState = AnimalState.Sleeping;

            // Play the sleep sound once (if available)
            if (audioSource != null && sleepSound != null)
            {
                audioSource.PlayOneShot(sleepSound);
            }
            // Stop Movement (Using linearVelocity FIX)
            rb.linearVelocity = Vector2.zero;

            Debug.Log(gameObject.name + " is going to sleep.");
        }
    }

    private void TransitionToWander()
    {
        if (currentState != AnimalState.Wandering)
        {
            currentState = AnimalState.Wandering;

            // Set the Animator flag to wake up
            if (anim != null)
            {
                anim.SetBool("IsSleeping", false);
            }

            SetNewWanderTarget();
            cluckTimer = minCluckInterval; // Reset cluck timer on wake up

            Debug.Log(gameObject.name + " woke up and is wandering.");
        }
    }

    // --- Audio Logic ---
    private void HandleClucking()
    {
        cluckTimer -= Time.deltaTime;
        if (cluckTimer <= 0f && audioSource != null && cluckSound != null)
        {
            audioSource.PlayOneShot(cluckSound);
            // Reset the timer for a random new interval
            cluckTimer = minCluckInterval + Random.Range(-1f, 1f);
        }
    }

    // --- Wandering Logic ---
    private void Wander()
    {
        // Check if we need a new target direction
        targetTimer -= Time.deltaTime;
        if (targetTimer <= 0f)
        {
            SetNewWanderTarget();
        }

        // Move towards the current target
        Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;

        // Applying velocity (Using linearVelocity FIX)
        rb.linearVelocity = direction * moveSpeed;

        // If we are very close to the target, reset the timer to find a new spot
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
    }
}


