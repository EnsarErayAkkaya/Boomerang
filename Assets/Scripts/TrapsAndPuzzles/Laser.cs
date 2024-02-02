using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private Vector3 laserLocalTarget;
    [SerializeField] private float laserWidth;
    [SerializeField] private LayerMask laserLayerMask;

    [SerializeField] private float stayActiveSecond;
    [SerializeField] private float stayDeactiveSecond;

    [SerializeField] private float activateDuration;
    [SerializeField] private float deactivateDuration;

    //[SerializeField] private Collider collider;
    [SerializeField] private LineRenderer lineRenderer;
    
    [Header("Lasers")]
    [SerializeField] private ParticleSystem laserCreateParticle;
    [SerializeField] private ParticleSystem laserEndBeamParticle;

    private void Start()
    {
        StartCoroutine(MainEnumerator());
    }

    private IEnumerator MainEnumerator()
    {
        float t;

        while (true)
        {
            yield return new WaitForSeconds(stayDeactiveSecond);

            yield return ActivateLaserEnumerator();

            t = 0;

            while (t < stayActiveSecond)
            {
                t += Time.deltaTime;

                Vector3 pos = (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * laserLocalTarget);

                RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + pos, laserLayerMask);
                if (hit.collider != null)
                {
                    hit.collider.GetComponent<CharacterController>().Die("Laser");
                }

                yield return new WaitForFixedUpdate();
            }

            yield return DeactivateLaserEnumerator();
        }
    }
    private IEnumerator ActivateLaserEnumerator()
    {
        laserCreateParticle.Play();
        float t = 0;
        Vector2 origin;
        float magn;
        Vector2 size;

        while (t < 1)
        {
            t += Time.deltaTime / activateDuration;

            Vector3 pos = Vector3.Lerp(Vector3.zero, laserLocalTarget, t);

            lineRenderer.SetPosition(1, pos);

            RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + (Quaternion.AngleAxis(transform.rotation.z, Vector3.forward) * pos), laserLayerMask);
            if (hit.collider != null)
            {
                hit.collider.GetComponent<CharacterController>().Die("Laser");
            }

            yield return new WaitForFixedUpdate();
        }

        lineRenderer.SetPosition(1, laserLocalTarget);

        laserEndBeamParticle.transform.localPosition = laserLocalTarget + (-laserLocalTarget.normalized * .5f);
        laserEndBeamParticle.Play();
    }

    private IEnumerator DeactivateLaserEnumerator()
    {
        laserEndBeamParticle.Stop();

        float t = 0;

        Vector2 origin;
        float magn;
        Vector2 size;

        while (t < 1)
        {
            t += Time.deltaTime / deactivateDuration;

            Vector3 pos = Vector3.Lerp(laserLocalTarget, Vector3.zero, t);

            lineRenderer.SetPosition(1, pos);

            RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + (Quaternion.AngleAxis(transform.rotation.z, Vector3.forward) * pos), laserLayerMask);
            if (hit.collider != null)
            {
                hit.collider.GetComponent<CharacterController>().Die("Laser");
            } 

            yield return new WaitForFixedUpdate();
        }

        lineRenderer.SetPosition(1, Vector3.zero);

        laserCreateParticle.Stop();
    }
}
