using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private float reloadtime;
    [SerializeField] Rig aimRig;
    [SerializeField] WeaponHolder weaponHolder;

    private Animator anim;
    private bool isReloading;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnReload(InputValue value)
    {
        if (isReloading)
            return;

        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        anim.SetTrigger("Reload");
        isReloading = true;
        aimRig.weight = 0f;
        yield return new WaitForSeconds(reloadtime);
        isReloading=false;
        aimRig.weight = 1f;
    }

    public void Fire()
    {
        weaponHolder.Fire();
        anim.SetTrigger("Fire");
    }

    private void OnFire(InputValue value)
    {
        if (isReloading)
            return;

        Fire();
    }
}
