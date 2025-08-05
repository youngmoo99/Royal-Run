using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float collisionColldown = 1f;
    [SerializeField] float adjustChangeMoveSpeedAmount = -2f;

    LevelGenerator levelGenerator;

    void Start()
    {
        levelGenerator = FindFirstObjectByType<LevelGenerator>();
    }

    const string hitsString = "Hit";

    float cooldownTimer = 0f;

    void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {   
        if (cooldownTimer < collisionColldown) return;

        levelGenerator.ChangeChunkMoveSpeed(adjustChangeMoveSpeedAmount);
        animator.SetTrigger(hitsString);
        cooldownTimer = 0f;
    }
}
