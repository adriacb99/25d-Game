using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HurtCollider : MonoBehaviour
{
    [SerializeField] public UnityEvent<HurtCollider, HitCollider> onHit;
    internal void NotifyHit(HitCollider hitCollider)
    {
        onHit.Invoke(this, hitCollider);
    }
}
