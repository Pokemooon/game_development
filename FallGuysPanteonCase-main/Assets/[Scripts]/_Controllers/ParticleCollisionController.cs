using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleCollisionController : MonoBehaviour
{
    private bool canCollision = true;
    private Coroutine collisionCoroutine = null;
    private void OnTriggerEnter(Collider other)
    {
        if (!canCollision) return;

        if (other.CompareTag("Water"))
        {
            Transform pt = Instantiate(Resources.Load<GameObject>("particles/water"), Level.instance.transform).transform;
            pt.position = transform.position;
            if (collisionCoroutine == null) collisionCoroutine = StartCoroutine(Delay());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!canCollision) return;

        if(collision.gameObject.CompareTag("Obstacle"))
        {
            Transform pt = Instantiate(Resources.Load<GameObject>("particles/puff"), Level.instance.transform).transform;
            pt.position = transform.position;
            if (collisionCoroutine == null) collisionCoroutine = StartCoroutine(Delay());
        }
    }
    private IEnumerator Delay()
    {
        canCollision = false;
        yield return new WaitForSeconds(1f);
        canCollision = true;
        collisionCoroutine = null;
    }
}
