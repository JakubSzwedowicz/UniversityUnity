using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    [SerializeField]
    private Camera fpsCam;
    [SerializeField] 
    private bool AddBulletSpread = true;
    [SerializeField]
    private Vector3 BulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] 
    private ParticleSystem ShootingSystem;
    [SerializeField] 
    private ParticleSystem impartParticleSystem;
    [SerializeField]
    private TrailRenderer BullerTrail;
    [SerializeField]
    private float ShootDelay = 0.5f;
    [SerializeField]
    private LayerMask TargetsLayer;

    private float LastShootTime = 0f;

    public ParticleSystem muzzleFlash;

    public void Shoot()
    {
        if (LastShootTime + ShootDelay > Time.time)
            return;

        ShootingSystem.Play();
        muzzleFlash.Play();
        Vector3 direction = GetDirection();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, TargetsLayer))
        { 
            Debug.Log("Object: " + hit.transform.name + " was hit");

            Target target = hit.transform.GetComponent<Target>();
            TrailRenderer trail = Instantiate(BullerTrail, fpsCam.transform.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));
            if (target != null)
            {
                target.TakeDamage(damage);
                Debug.Log("Dealt " + damage + ", health left = " + target.healt);
            }

        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = fpsCam.transform.forward;
        if (AddBulletSpread)
        {
            direction.x += Random.Range(-BulletSpread.x, BulletSpread.x);
            direction.y += Random.Range(-BulletSpread.y, BulletSpread.y);
            direction.z += Random.Range(-BulletSpread.z, BulletSpread.z);
        }

        direction.Normalize();
        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPostion = trail.transform.position;
        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPostion, hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        trail.transform.position = hit.point;
        Instantiate(impartParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(trail.gameObject, trail.time);
    }
}

