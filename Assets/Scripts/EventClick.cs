using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class EventClick : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private InstructionsUI instructionUI;

    public event EventHandler OnBoltClicked;
    public event EventHandler OnNozzleClicked;
    public event EventHandler OnFirstTimeHeightChange;
    public event EventHandler OnFirstTimeUseExtinguisher;

    private bool firstTimeChangeHeight = true;
    private bool firstTimeUseExtinguisher = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!instructionUI.IsAbleToClick3DObjects()) { return; }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag(Tags.Bolt))
            {
                FireExtinguisher fireExtinguisher = hit.collider.GetComponentInParent<FireExtinguisher>();
                if (fireExtinguisher != null)
                {
                    if (!fireExtinguisher.IsUnlocked())
                    {
                        fireExtinguisher.Unlock();
                        OnBoltClicked?.Invoke(this, EventArgs.Empty);
                        Destroy(hit.collider.gameObject.GetComponent<BoxCollider>());
                    }
                }
            }
            else if (hit.collider.CompareTag(Tags.Nozzle))
            {
                FireExtinguisher fireExtinguisher = hit.collider.GetComponentInParent<FireExtinguisher>();
                if (fireExtinguisher.IsUnlocked())  
                {
                    fireExtinguisher.MoveNozzle();
                    fireExtinguisher.ShowSlider();
                    OnNozzleClicked?.Invoke(this, EventArgs.Empty);
                    Destroy(hit.collider.gameObject.GetComponent<BoxCollider>());
                }
            }
            else if (hit.collider.CompareTag(Tags.Handle))
            {
                FireExtinguisher fireExtinguisher = hit.collider.GetComponentInParent<FireExtinguisher>();
                if (firstTimeUseExtinguisher && fireExtinguisher.IsReadyToPowder() && !firstTimeChangeHeight)
                {
                    fireExtinguisher.FadeOutHandleArrowUI();
                    firstTimeUseExtinguisher = false;
                    OnFirstTimeUseExtinguisher?.Invoke(this, EventArgs.Empty);
                }
            }
        } 
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag(Tags.Handle) && !firstTimeChangeHeight)
            {
                FireExtinguisher fireExtinguisher = GetComponent<FireExtinguisher>();
                fireExtinguisher.ShootPowder();
                if (firstTimeUseExtinguisher)
                {
                    OnFirstTimeUseExtinguisher?.Invoke(this, EventArgs.Empty);
                    fireExtinguisher.FadeOutHandleArrowUI();
                    firstTimeUseExtinguisher = false;
                }
            }
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        FireExtinguisher fireExtinguisher = GetComponent<FireExtinguisher>();
        fireExtinguisher.StopPowder();
    }

    public void FirstTimeNozzleHeightChangeForTutorial()
    {
        if(firstTimeChangeHeight)
        {
            FireExtinguisher fireExtinguisher = GetComponent<FireExtinguisher>();
            fireExtinguisher.ShowHandleArrowUI();
            OnFirstTimeHeightChange?.Invoke(this, EventArgs.Empty);
            firstTimeChangeHeight = false;
        }
    }
}
