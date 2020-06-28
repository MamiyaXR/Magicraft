using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private GameObject attachment;
    private bool hasAttachment;
    private void Start()
    {
        attachment = null;
        hasAttachment = false;
    }
    private void Update()
    {
        if (hasAttachment && attachment == null)
            Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        attachment = collision.gameObject;
        hasAttachment = true;
        StartCoroutine("Wait", 5);
    }
    private IEnumerator Wait(float time)
    {
        for(float i = time; i >= 0; i -= Time.deltaTime)
        {
            transform.localScale = Vector3.one * i / time;
            yield return 0;
        }
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        StopCoroutine("Wait");
    }
}
