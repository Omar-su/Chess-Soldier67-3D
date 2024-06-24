using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    public List<GameObject> weapons;
    private int activeWeaponIndex = 0;

    private void Start()
    {
        // Ensure only the first weapon is active when the game starts
        SwitchToWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        // Get the mouse scroll input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Check if the scrollInput is not zero, meaning there's mouse scroll movement
        if (scrollInput != 0f)
        {
            // Determine the next weapon index based on scroll direction
            int nextWeaponIndex = activeWeaponIndex + (scrollInput > 0f ? 1 : -1);

            // Wrap the index around to the beginning or end if necessary
            nextWeaponIndex = (nextWeaponIndex + weapons.Count) % weapons.Count;

            // Switch to the next weapon
            SwitchToWeapon(nextWeaponIndex);
        }
    }

    private void SwitchToWeapon(int weaponIndex)
    {
        // Check if the requested weapon index is within the valid range
        if (weaponIndex < 0 || weaponIndex >= weapons.Count)
            return;

        // Deactivate the previously active weapon
        if (activeWeaponIndex >= 0 && activeWeaponIndex < weapons.Count)
        {
            weapons[activeWeaponIndex].SetActive(false);
        }

        // Activate the new weapon
        weapons[weaponIndex].SetActive(true);

        // Update the activeWeaponIndex
        activeWeaponIndex = weaponIndex;
    }
}
