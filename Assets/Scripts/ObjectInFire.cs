using UnityEngine;
public class ObjectInFire : MonoBehaviour
{
    [SerializeField] private ParticleSystem PSFire;
    private float initialStartLifetime;
    [SerializeField] private float fireDuration = 6f;
    private float currentFireTime;
    private bool isPowdered;
    private float increaseDelay = 0.3f; 
    private float currentIncreaseTimer;

    private void Start()
    {
    initialStartLifetime = PSFire.main.startLifetime.constant;
    currentFireTime = fireDuration;
    currentIncreaseTimer = 0f;
    }

    void OnParticleCollision(GameObject other)
    {
    isPowdered = true;
    currentIncreaseTimer = increaseDelay;
    }
    private void Update()
    {
        HandleFireStrengthValue();
        AdjustFireEffects();
        UpdateAudioVolume();
    }

    private void HandleFireStrengthValue()
    {
        currentIncreaseTimer -= Time.deltaTime;

        if (currentIncreaseTimer <= 0f)
        {
            isPowdered = false;
        }
        if (currentFireTime > 0 && isPowdered)
        {
            currentFireTime -= Time.deltaTime;
            if (currentFireTime == 0)
            {
                Destroy(PSFire);
            }
        }
        if (currentFireTime > 0 && !isPowdered && currentIncreaseTimer <= 0)
        {
            currentFireTime += Time.deltaTime;
        }
        currentFireTime = Mathf.Clamp(currentFireTime, 0, fireDuration);
    }
    private void AdjustFireEffects()
    {
        float adjustedStartLifetime = initialStartLifetime * (currentFireTime / fireDuration);
        var mainModule = PSFire.main;
        mainModule.startLifetime = adjustedStartLifetime;
    }

    private void UpdateAudioVolume()
    {
        GetComponent<AudioSource>().volume = (currentFireTime / fireDuration);
    }

    // Getters
    public float GetFireStrength() { return currentFireTime / fireDuration; }
}
