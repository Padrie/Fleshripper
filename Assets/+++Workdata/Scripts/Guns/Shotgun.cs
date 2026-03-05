using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    Camera playerCamera;
    Vector3 originalCameraPos;
    Vector3 originalGunPos;

    public int projectileAmount;

    public void Start()
    {
        playerCamera = Camera.main;
        originalCameraPos = playerCamera.transform.localPosition;
        originalGunPos = gunManager.gameObject.transform.localPosition;
        gunManager.currentAmmoAmount = stats[level].ammoAmount;
    }

    private void OnEnable()
    {
        ResetValues();
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
        while (true)
        {
            if (gunManager.currentAmmoAmount <= 0)
            {
                gunManager.currentAmmoAmount = 0;
                if (!isReloading)
                {
                    StartCoroutine(Reload());
                }
            }

            if (Input.GetMouseButton(0))
            {
                projectileForce = explosionProjectileType.GetComponent<Bullet>().bulletSpeed;
                HandleShooting(normalProjectileType);
                yield return new WaitForSeconds(stats[level].shootSpeed);

            }
            else if (Input.GetMouseButton(1))
            {
                projectileForce = lightningExplosionType.GetComponent<Bullet>().bulletSpeed;
                HandleShooting(normalProjectileType);
                yield return new WaitForSeconds(stats[level].shootSpeed);

            }

            yield return null;
        }
    }

    public void HandleShooting(GameObject ammoType)
    {
        if(!isReloading)
        {
            for (int i = 0; i < stats[level].bulletAmount; i++)
            {
                Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
                Vector3 shootDirection = ray.direction + Random.insideUnitSphere * stats[level].bulletSpread;

                GameObject projectile = Instantiate(
                    ammoType,
                    playerCamera.transform.position + ray.direction.normalized * 2f,
                    Quaternion.LookRotation(shootDirection)
                );

                projectile.GetComponent<Bullet>().AddVelocity(shootDirection);

                if (AudioManager.instance != null)
                    AudioManager.instance.Play("MachineGunShoot");

                shootParticle.Play();

                StartCoroutine(CameraShake());
                StartCoroutine(Recoil());
            }
        }

        if (gunManager.currentAmmoAmount == 0) return;
        gunManager.currentAmmoAmount--;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        AudioManager.instance.Play("MachineGunReload");
        yield return new WaitForSeconds(stats[level].reloadTime);
        ResetValues();
    }

    public void ResetValues()
    {
        gunManager.currentAmmoAmount = stats[level].ammoAmount;
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
}
