using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class Gun : MonoBehaviour
{
    [SerializeField] private InputAction shooting;
    [SerializeField] private Transform fpsCam;
    [SerializeField] private float range = 10.0f;
    [SerializeField] private float impactForce = 150.0f;
    [SerializeField] private int fireRate = 10;
    [SerializeField] private float nextTimeToFire = 0;
    [SerializeField] private ParticleSystem impact;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private AudioSource gunn;
    [SerializeField] private AudioClip audioShotting;
    [SerializeField] private Animator animation;
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private int magazineSize = 30;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private bool isReloading;
    [SerializeField] private int secondsDestroy = 5;

    private void Awake()
    {
        currentAmmo = maxAmmo;
        //this.animation = GetComponent<Animator>();
        this.fpsCam = GameObject.Find("Main Camera").transform;
        this.gunn = GetComponent<AudioSource>();
        this.ammoText = GameObject.Find("AmmoText").GetComponent<TextMeshProUGUI>();
        this.isReloading = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        this.shooting = new InputAction("shooting", binding: "<mouse>/leftbutton");
        this.shooting.AddBinding("<Gamepad>/x");
        this.shooting.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        CheckShooting();
        CheckTextAmmo();
    }
    private void CheckShooting()
    {

        bool isShooting = shooting.ReadValue<float>() == 1;

        if (currentAmmo == 0 && magazineSize == 0)
        {
            this.animation.SetBool("shooting", false);
            this.animation.SetBool("shoot", false);
            this.impact.Stop();
            return;
        }
        if (isReloading)
        {
            this.impact.Stop();
            // this.animation.SetBool("shooting", false);
            // this.animation.SetBool("shoot", false);
            return;

        }

        this.animation.SetBool("shooting", isShooting);
        this.animation.SetBool("shoot", isShooting);

        if (isShooting && Time.time >= nextTimeToFire)
        {
            this.impact.Play();
            this.gunn.PlayOneShot(audioShotting, 0.5f);
            nextTimeToFire = Time.time + 1.0f / fireRate;
            this.Fire();
        }

        if (isShooting == false)
        {
            this.impact.Stop();
        }
        if (currentAmmo == 0 && !isReloading)
        {
            StartCoroutine(reload());
        }

    }
    private void Fire()
    {
        currentAmmo--;
        RaycastHit hit;
        if (Physics.Raycast(this.fpsCam.position, this.fpsCam.forward, out hit, this.range))
        {
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * this.impactForce);
            }
            Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
            GameObject VFX = Instantiate(impactEffect, hit.point, impactRotation);
            VFX.transform.parent = hit.transform;
            Destroy(VFX, this.secondsDestroy);
        }
    }
    private void CheckTextAmmo()
    {
        this.ammoText.text = this.currentAmmo + " / " + this.magazineSize;
    }
    IEnumerator reload()
    {
        this.isReloading = true;
        this.animation.SetBool("Realoading", true);
        yield return new WaitForSeconds(2);
        this.animation.SetBool("Realoading", false);

        if (this.magazineSize >= maxAmmo)
        {
            currentAmmo = maxAmmo;
            this.magazineSize -= maxAmmo;
        }
        else
        {
            currentAmmo = magazineSize;
            magazineSize = 0;
        }

        this.isReloading = false;

    }
}
