using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] private int switchingWeapon = 0;
    [SerializeField] private InputAction scrollMouse;
    // Start is called before the first frame update
    void Start()
    {
        this.scrollMouse = new InputAction("Scroll", binding: "<Mouse>/scroll");
        scrollMouse.Enable();
        this.switching();
    }

    // Update is called once per frame
    void Update()
    {
        this.MouseScroll();
    }

    private void switching()
    {
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }
        transform.GetChild(switchingWeapon).gameObject.SetActive(true);
    }

    private void MouseScroll()
    {
        int previousSwitch = switchingWeapon;
        float y = this.scrollMouse.ReadValue<Vector2>().y;
        if (y > 0)
        {
            switchingWeapon++;
            if (switchingWeapon == 3)
            {
                switchingWeapon = 0;
            }
        }

        else if (y < 0)
        {
            switchingWeapon--;
            if (switchingWeapon == -1)
            {
                switchingWeapon = 2;
            }
        }
        
        if (switchingWeapon != previousSwitch)
        {
            this.switching();
        }
    }
}
