using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI maxAmmoText;

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetAmmo(int ammo)
    {
        ammoText.text = ammo.ToString();
    }

    public void SetMaxAmmo(int ammo)
    {
        maxAmmoText.text = ammo.ToString();
    }

}
