using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private float destroySpeed = 0.2f;
    private Rigidbody rBody;
    private GameObject attachment;
    private bool hasAttachment;
    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
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
        if (collision.transform.tag == "Fire")
            return;
        attachment = collision.gameObject;
        hasAttachment = true;
        //gameObject.transform.SetParent(attachment.transform);
        //rBody.MovePosition(collision.contacts[0].point);
        //Destroy(rBody);
        StartCoroutine("DelayDestroy");
    }
    private IEnumerator DelayDestroy()
    {
        while (transform.localScale.x >= 0)
        {
            transform.localScale -= Vector3.one * destroySpeed * Time.deltaTime;
            yield return 0;
        }
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        StopCoroutine("Wait");
    }
}
