using UnityEngine;
using UnityEngine.UI;
public class FireUI : MonoBehaviour
{
    [SerializeField] private Slider fireStrengthBar;
    [SerializeField] private ObjectInFire objectInFire;
    private void Update()
    {
        UpdateFireStrengthBar();
    }
    private void UpdateFireStrengthBar()
    {
        fireStrengthBar.value = objectInFire.GetFireStrength();
    }
}
