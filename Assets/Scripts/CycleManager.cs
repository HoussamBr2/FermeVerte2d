using UnityEngine;

public class CycleManager : MonoBehaviour
{
    
    public GameObject sun;
    public GameObject moon;
    public AudioSource daySound;
    public AudioSource nightSound;
    private bool isDay = true;

    public void ToggleDayNight()
    {
        isDay = !isDay;
        sun.SetActive(isDay);
        moon.SetActive(!isDay);
        daySound.mute = !isDay;
        nightSound.mute = isDay;
        Debug.Log(isDay ? "Mode Jour" : "Mode Nuit");
    }
}
