using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Weapon : MonoBehaviour
{
    public GameObject trail;
    public GameObject particlePrefab;
    public Transform firingPosition;
    public TMP_Text bulletText;

    public int currentBullet = 8;
    public int totalBullet = 32;
    public int maxBulletMagazine = 8;
    public int damage = 0;

    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletText != null)
        {
            bulletText.text = currentBullet.ToString() + " / " + totalBullet.ToString();
        }
    }

    public void FireWeapon()
    {
        if (currentBullet > 0)
        {
            if (animator != null)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    animator.SetTrigger("Fire");
                    currentBullet--;
                    Fire();
                }
            }
            else
            {
                currentBullet--;
                Fire();
            }
        }   
    }

    protected virtual void Fire()
    {
        RayCastFire();
    }

    public void ReloadWeapon()
    {
        if (totalBullet > 0)
        {
            if (animator != null)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    animator.SetTrigger("Reload");
                    Reload();
                }
            }
            else
            {
                Reload();
            }
        }
    }

    void Reload()
    {
        if (totalBullet >= maxBulletMagazine - currentBullet)
        {
            totalBullet -= maxBulletMagazine - currentBullet;
            currentBullet = maxBulletMagazine;
        }
        else
        {
            currentBullet += totalBullet;
            totalBullet = 0;
        }
    }

    void RayCastFire()
    {
        Camera cam = Camera.main;

        RaycastHit hit;
        Ray r = cam.ViewportPointToRay(Vector3.one / 2);

        Vector3 hitPosition = r.origin + r.direction * 200;

        if (Physics.Raycast(r, out hit, 1000))
        {
            hitPosition = hit.point;

            GameObject particle = Instantiate(particlePrefab);
            particle.transform.position = hitPosition;
            particle.transform.forward = hit.normal;

            if(hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<Health>().Damage(damage);
            }
        }

        GameObject go = Instantiate(trail);
        Vector3[] pos = new Vector3[] { firingPosition.position, hitPosition };

        go.GetComponent<LineRenderer>().SetPositions(pos);

        StartCoroutine(DestroyTrail(go));
    }

    IEnumerator DestroyTrail(GameObject obj)
    {
        yield return new WaitForSeconds(0.1f);

        Destroy(obj);
    }
}
