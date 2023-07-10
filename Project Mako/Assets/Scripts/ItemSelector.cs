using UnityEngine;
using TMPro;

public class ItemSelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown primaryWeaponDropdown;
    [SerializeField] private TMP_Dropdown secondaryWeaponDropdown;
    public void SelectPrimaryWeapon()
    {
        Inventory.instance.primaryWeaponIndex = primaryWeaponDropdown.value;
    }
    public void SelectSecondaryWeapon()
    {
        Inventory.instance.secondaryWeaponIndex = secondaryWeaponDropdown.value;
    }
}
