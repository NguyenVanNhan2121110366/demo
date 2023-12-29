using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Scope : MonoBehaviour
{
    [SerializeField] private bool isScope = false;
    [SerializeField] private InputAction scope;
    [SerializeField] private Animator animation;
    [SerializeField] private GameObject scopeOverLay;
    [SerializeField] private Camera FpsCam;
    // Start is called before the first frame update
    void Start()
    {
        this.scope = new InputAction("scope", binding: "<mouse>/rightButton");
        this.scope.Enable();
        this.animation = GetComponent<Animator>();
        //this.FpsCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateScope();
    }
    private void UpdateScope()
    {
        if (scope.triggered)
        {
            isScope = !isScope;
            this.animation.SetBool("scope", isScope);
            if (isScope)
            {
                this.scopeOverLay.SetActive(true);
                this.FpsCam.cullingMask = this.FpsCam.cullingMask & ~(1 << 7);
            }
            else
            {
                this.scopeOverLay.SetActive(false);
                this.FpsCam.cullingMask = this.FpsCam.cullingMask | (1 << 7);
            }
        }
    }
}
