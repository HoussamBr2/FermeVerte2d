using UnityEngine;

public class AnimalBehavior : MonoBehaviour
{
    public Animator animator;
    public bool isDayActive;

    void Update()
    {
        if (isDayActive)
            animator.Play("Walk");
        else
            animator.Play("Sleep");
    }
}
