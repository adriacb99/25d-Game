using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityLife : MonoBehaviour
{
    [SerializeField] int lives = 3;
    [SerializeField] public UnityEvent<int> onLifeLost;
    [SerializeField] public UnityEvent onAllLifesLost;

    HurtCollider hurtCollider;

    private void Awake()
    {
        hurtCollider = GetComponent<HurtCollider>();        
    }

    private void OnEnable()
    {
        hurtCollider.onHit.AddListener(OnHit);
    }

    private void OnDisable()
    {
        hurtCollider.onHit.RemoveListener(OnHit);
    }

    private void OnHit(HurtCollider hurtCollider, HitCollider hitCollider)
    {
        LoseLife();
    }

    public void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            onAllLifesLost.Invoke();
        }
    }

    public int GetLives() { return lives; }

}
