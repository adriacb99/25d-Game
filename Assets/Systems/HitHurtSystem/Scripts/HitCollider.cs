using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{

    [SerializeField] List<string> hittableTags = new List<string>{ "Untagged", "PunchingBag" };

    private void OnCollisionEnter(Collision collision)
    {
        CheckHit(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckHit(other);
    }

    private void CheckHit(Collider other)
    {
        if (hittableTags.Contains(other.tag))
        {
            other.GetComponent<HurtCollider>()?.NotifyHit(this);
        }
    }
}
