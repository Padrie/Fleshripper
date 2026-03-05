using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperShotgun : Gun
{
    Camera playerCamera;
    Vector3 originalCameraPos;
    Vector3 originalGunPos;
    bool mouseButtonHold = false;

    public int projectileAmount;
    public GameObject shootPosition;

    public void Start()
    {
        playerCamera = Camera.main;
        originalCameraPos = playerCamera.transform.localPosition;
        originalGunPos = gunManager.gameObject.transform.localPosition;
        gunManager.currentAmmoAmount = stats[level].ammoAmount;
    }

    private void OnEnable()
    {
        isReloading = false;
        StopAllCoroutines();
        StartCoroutine(DetermineAmmo());
    }

    private void Update()
    {
        LeftRightSway();
        UpDownSway();
    }

    float offsetY = 0;

    public void UpDownSway()
    {
        offsetY = Mathf.Lerp(offsetY, player.velocity.y, Time.deltaTime * 6);
        gunManager.transform.localRotation = Quaternion.Euler(offsetY, 0, player.rotZ);
    }

    public void LeftRightSway()
    {
        Vector3 gunEuler = gunManager.transform.localEulerAngles;
        gunEuler.y = playerCamera.transform.localEulerAngles.z;
        gunManager.transform.localEulerAngles = gunEuler;
    }

    public IEnumerator DetermineAmmo()
    {
        float elapsed = 0f;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && !isReloading)
            {
                gunManager.ammoSlot = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && gunManager.explosionAmmoUnlocked && !isReloading)
            {
                gunManager.ammoSlot = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && gunManager.lightningAmmoUnlocked && !isReloading)
            {
                gunManager.ammoSlot = 3;
            }

            if (gunManager.currentAmmoAmount <= 0)
            {
                gunManager.currentAmmoAmount = 0;
                if (!isReloading)
                {
                    StartCoroutine(Reload());
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                mouseButtonHold = true;
                StartCoroutine(Charge());
            }
            if (Input.GetMouseButtonUp(0))
            {
                mouseButtonHold = false;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (gunManager.ammoSlot == 1)
                {
                    if (!isReloading)
                    {
                        StartCoroutine(ManualReload());
                    }
                }
                else if (gunManager.ammoSlot == 2)
                {
                    if (!isReloading)
                    {
                        StartCoroutine(ManualReload());
                    }
                }
                else if (gunManager.ammoSlot == 3)
                {
                    if (!isReloading)
                    {
                        StartCoroutine(ManualReload());
                    }
                }
                else
                {
                    Debug.LogError("Error in " + name);
                }
            }

            yield return null;
        }
    }

    public void HandleShooting(GameObject ammoType)
    {
        for (int i = 0; i < stats[level].bulletAmount; i++)
        {
            print("Shotgun");
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            Vector3 shootDirection = ray.direction + Random.insideUnitSphere * stats[level].bulletSpread;

            GameObject projectile = Instantiate(
                ammoType,
                shootPosition.transform.position + ray.direction.normalized * 2f,
                Quaternion.LookRotation(shootDirection)
            );

            projectile.GetComponent<Bullet>().AddVelocity(shootDirection);

            if (AudioManager.instance != null)
                AudioManager.instance.Play("MachineGunShoot");
        }

        StartCoroutine(CameraShake());
        StartCoroutine(Recoil());
        shootParticle.Play();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        AudioManager.instance.Play("MachineGunReload");
        yield return new WaitForSeconds(stats[level].reloadTime);
        ResetValues();
    }

    IEnumerator ManualReload()
    {
        isReloading = true;
        AudioManager.instance.Play("MachineGunReload");
        yield return new WaitForSeconds(stats[level].reloadTime);
        ManualResetValues();
    }

    public void ManualResetValues()
    {
        if (gunManager.ammoSlot == 1)
            gunManager.currentAmmoAmount = stats[level].ammoAmount;
        else if (gunManager.ammoSlot == 2)
        {
            gunManager.AddExplosionAmmo(gunManager.currentExplosionAmmoAmount);
            gunManager.currentExplosionAmmoAmount = 0;
            gunManager.EvaluateExplosionAmmo();
        }
        else if (gunManager.ammoSlot == 3)
        {
            gunManager.AddLightningAmmo(gunManager.currentLightningAmmoAmount);
            gunManager.currentLightningAmmoAmount = 0;
            gunManager.EvaluateLightningAmmo();
        }
        else
            print("Wtf how, no ammo type selected? What dumbass wrote this shit ass code????");

        print("Dont tell me it returns");

        isReloading = false;
        StopAllCoroutines();
        StartCoroutine(DetermineAmmo());
    }

    public void ResetValues()
    {
        if (gunManager.ammoSlot == 1)
            gunManager.currentAmmoAmount = stats[level].ammoAmount;
        else if (gunManager.ammoSlot == 2)
            gunManager.EvaluateExplosionAmmo();
        else if (gunManager.ammoSlot == 3)
            gunManager.EvaluateLightningAmmo();
        else
            print("Wtf how, no ammo type selected? What dumbass wrote this shit ass code????");

        print("Dont tell me it returns");

        isReloading = false;
        StopAllCoroutines();
        StartCoroutine(DetermineAmmo());
    }

    IEnumerator CameraShake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / shakeDuration;

            float seed = Time.time * shakeFrequency;

            float x = (Mathf.PerlinNoise(seed + 1f, 0f) * 2f - 1f) * shakeStrength;
            float y = (Mathf.PerlinNoise(0f, seed + 2f) * 2f - 1f) * shakeStrength;

            Vector3 offset = new Vector3(x, y, 0);

            playerCamera.transform.localPosition = Vector3.Lerp(originalCameraPos + offset, originalCameraPos, percentComplete);

            yield return null;
        }

        playerCamera.transform.localPosition = originalCameraPos;
    }

    IEnumerator Recoil()
    {
        float elapsed = 0f;

        while (elapsed < recoilDuration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / recoilDuration;

            gunManager.gameObject.transform.localPosition = Vector3.Lerp(originalGunPos + new Vector3(Random.Range(-0.025f, 0.025f), Random.Range(-0.025f, 0.025f), -recoilKickBack), originalGunPos, percentComplete);

            yield return null;
        }

        gunManager.gameObject.transform.localPosition = originalGunPos;
    }

    IEnumerator Charge()
    {
        float elapsed = 0f;

        while (elapsed < stats[level].shootSpeed)
        {
            if (!mouseButtonHold || isReloading)
                break;

            elapsed += Time.deltaTime;

            float percentComplete = elapsed / stats[level].shootSpeed;

            gunManager.gameObject.transform.localPosition =
                new Vector3(Random.Range(-0.02f, 0.02f) * percentComplete, Random.Range(-0.02f, 0.02f) * percentComplete, -percentComplete * 0.75f);

            yield return null;
        }

        gunManager.gameObject.transform.localPosition = originalGunPos;

        if (elapsed > stats[level].shootSpeed)
        {
            if (gunManager.ammoSlot == 1)
            {
                HandleShooting(normalProjectileType);
                gunManager.currentAmmoAmount--;
                if (gunManager.currentAmmoAmount <= 0)
                {
                    gunManager.currentAmmoAmount = 0;
                    if (!isReloading)
                    {
                        StartCoroutine(Reload());
                    }
                }

                yield return null;
            }
            else if (gunManager.ammoSlot == 2)
            {
                if (gunManager.currentExplosionAmmoAmount > 0)
                {
                    HandleShooting(explosionProjectileType);
                    gunManager.currentExplosionAmmoAmount--;
                }

                if (gunManager.currentExplosionAmmoAmount <= 0)
                {
                    gunManager.currentExplosionAmmoAmount = 0;
                    if (!isReloading)
                    {
                        StartCoroutine(Reload());
                    }
                }

                yield return null;
            }
            else if (gunManager.ammoSlot == 3)
            {
                if (gunManager.currentLightningAmmoAmount > 0)
                {
                    HandleShooting(lightningExplosionType);
                    gunManager.currentLightningAmmoAmount--;
                }

                if (gunManager.currentLightningAmmoAmount <= 0)
                {
                    gunManager.currentLightningAmmoAmount = 0;
                    if (!isReloading)
                    {
                        StartCoroutine(Reload());
                    }
                }

                yield return null;
            }
            else
            {
                Debug.LogError("Error in " + name);
                yield return null;
            }
        }

        yield return null;
    }
}
