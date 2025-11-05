using UnityEngine;
using UnityEngine.Rendering.Universal; // Required for accessing Light2D

public class DayNightCycle : MonoBehaviour
{
    // === Settings ===
    [Header("Cycle Settings")]
    [Tooltip("The total time in seconds for one full day/night cycle.")]
    public float cycleDuration = 120f; 

    // === Light References ===
    private Light2D globalLight;

    // === Cycle State ===
    [Header("Current State")]
    // The current time of the cycle, from 0 to cycleDuration
    private float currentTime = 0f; 
    
    // Public flag for other scripts to read (e.g., the Chicken)
    public bool IsNightTime { get; private set; } = false;

    // --- Light Color Presets ---
    // Use the color picker in the Inspector to set these colors
    [Header("Light Color Presets")]
    public Color dayColor = Color.white;
    public Color nightColor = new Color(0.1f, 0.1f, 0.4f, 1f); // Dark Blue

    void Start()
    {
        // Get the Light2D component from this GameObject
        globalLight = GetComponent<Light2D>(); 
        if (globalLight == null)
        {
            Debug.LogError("Light2D component not found on the GameObject!");
            enabled = false;
        }
    }

    void Update()
    {
        // 1. Advance the time
        currentTime += Time.deltaTime;
        
        // Wrap the time around to create a loop
        if (currentTime >= cycleDuration)
        {
            currentTime = 0f;
        }

        // 2. Calculate the normalized time (0 to 1)
        float normalizedTime = currentTime / cycleDuration; // 0.0 at dawn, 0.5 at dusk, 1.0 at dawn

        // 3. Determine if it's Day or Night (assuming Night is the second half)
        // Night starts at 50% through the cycle (normalizedTime > 0.5)
        IsNightTime = normalizedTime > 0.5f;

        // 4. Interpolate the Light Color and Intensity
        // We use a normalized value that goes from 0 (start of transition) to 1 (end of transition)
        
        // This 't' value controls the transition between dayColor and nightColor
        // It peaks around midnight (0.75) and dawn (0.25) to simulate gradual change.
        float t = Mathf.Sin(normalizedTime * Mathf.PI * 2) * 0.5f + 0.5f; 
        
        // Lerp (Linear Interpolate) the color based on 't'
        globalLight.color = Color.Lerp(nightColor, dayColor, t);
        
        // Lerp the intensity (Day is high, Night is low)
        globalLight.intensity = Mathf.Lerp(0.3f, 1.0f, t); // Intensity between 0.3 (Night) and 1.0 (Day)
    }
}




