using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private Vector3 laserLocalTarget;

    [SerializeField] private float stayActiveSecond;
    [SerializeField] private float stayDeactiveSecond;

    [SerializeField] private float activateDuration;
    [SerializeField] private float deactivateDuration;

    //[SerializeField] private Collider collider;
    [SerializeField] private LineRenderer lineRenderer;
    
    [Header("Lasers")]
    [SerializeField] private ParticleSystem laserCreateParticle;
    [SerializeField] private ParticleSystem laserEndBeamParticle;

    private WaitForEndOfFrame endOfFrame;

    private void Start()
    {
        endOfFrame = new WaitForEndOfFrame();

        StartCoroutine(MainEnumerator());
    }

    private IEnumerator MainEnumerator()
    {
        while (true)
        {
            yield return new WaitForSeconds(stayActiveSecond);

            yield return ActivateLaserEnumerator();

            yield return new WaitForSeconds(stayDeactiveSecond);

            yield return DeactivateLaserEnumerator();
        }
    }

    private IEnumerator ActivateLaserEnumerator()
    {
        laserCreateParticle.Play();
        
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / activateDuration;

            lineRenderer.SetPosition(1, Vector3.Lerp(Vector3.zero, laserLocalTarget, t));

            yield return new WaitForEndOfFrame();
        }

        lineRenderer.SetPosition(1, laserLocalTarget);

        laserEndBeamParticle.transform.localPosition = laserLocalTarget;
        laserEndBeamParticle.Play();
    }

    private IEnumerator DeactivateLaserEnumerator()
    {
        laserEndBeamParticle.Stop();

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / deactivateDuration;

            lineRenderer.SetPosition(1, Vector3.Lerp(laserLocalTarget, Vector3.zero, t));

            yield return new WaitForEndOfFrame();
        }

        lineRenderer.SetPosition(1, Vector3.zero);

        laserCreateParticle.Stop();
    }
}
