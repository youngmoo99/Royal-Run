using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float collisionColldown = 1f;

    const string hitsString = "Hit";

    float cooldownTimer = 0f;

    void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {   
        if (cooldownTimer < collisionColldown) return;

        animator.SetTrigger(hitsString);
        cooldownTimer = 0f;
    }
}
