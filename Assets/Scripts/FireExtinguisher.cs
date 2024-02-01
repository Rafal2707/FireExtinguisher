using UnityEngine;
using UnityEngine.UI;
public class FireExtinguisher : MonoBehaviour
{
    [SerializeField] private ParticleSystem powderParticleSystem;
    [SerializeField] private Transform nozzleTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private Slider verticalSlider;
    [SerializeField] private float smoothness = 1.5f;

    private float powderDuration = 10f;
    private float currentPowderTime;

    private bool fireExtinguisherOn;
    private bool readyToPowder;
    private bool isUnlocked;
    private void Start()
    {
        powderParticleSystem.Stop();
        currentPowderTime = powderDuration;

        HideSlider();
    }

    private void Update()
    {
        if (readyToPowder)
        {
            HandleMovementInput();
        }
        if (isUnlocked && readyToPowder && fireExtinguisherOn)
        {
            currentPowderTime -= Time.deltaTime;

            if (currentPowderTime <= 0f)
            {
                currentPowderTime = 0f;
                StopPowder();
            }
        }
    }
    public void HandleMovementInput()
    {
        float targetY = Mathf.Lerp(2.5f, 13f, verticalSlider.value);
        Vector3 newPosition = Vector3.Lerp(nozzleTransform.position, new Vector3(nozzleTransform.position.x, targetY, nozzleTransform.position.z), Time.deltaTime * smoothness);
        nozzleTransform.position = newPosition;
    }
    public void ShootPowder()
    {
        if (readyToPowder)
        {
            fireExtinguisherOn = true;
            powderParticleSystem.Play();
            animator.SetTrigger(Triggers.ToggleHandle);
            GetComponent<AudioSource>().Play();
        }
    }

    public void StopPowder()
    {
        if (isUnlocked && readyToPowder && fireExtinguisherOn)
        {
            animator.SetTrigger(Triggers.ToggleHandle);
            powderParticleSystem.Stop();
            GetComponent<AudioSource>().Stop();
        }
        fireExtinguisherOn = false;
    }

 
    public void Unlock()
    {
        if (!isUnlocked)
        {
            animator.SetTrigger(Triggers.UnlockFireExtinguisher);
            isUnlocked = true;
        }
    }

    // Triggers
    public void MoveNozzle() { animator.SetTrigger(Triggers.MoveNozzle); }
    public void ShowHandleArrowUI() { animator.SetTrigger(Triggers.ShowHandleArrowUI); }
    public void FadeOutHandleArrowUI() { animator.SetTrigger(Triggers.FadeHandleArrowUI); }
    public void showBoltUI() { animator.SetTrigger(Triggers.ShowBoltUI); }

    // Getters
    public float GetPowderLeft() { return currentPowderTime / powderDuration; }
    public bool IsUnlocked() { return isUnlocked; }
    public bool IsReadyToPowder() { return readyToPowder; }

    // Activate/DeactiveSlider
    public void HideSlider() { verticalSlider.gameObject.SetActive(false); }
    public void ShowSlider() { verticalSlider.gameObject.SetActive(true); }

    // Setters
    public void SetIsUnlocked(bool isUnlocked) { this.isUnlocked = isUnlocked; }
    public void OnEndNozzleAnimation() { readyToPowder = true; }

}
