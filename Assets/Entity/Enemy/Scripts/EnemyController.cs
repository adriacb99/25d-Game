using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float maxHorizontalVelocity = 1f;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float gravity = -9.81f;

    [SerializeField] float minVelocity = 1f;

    [SerializeField] GameObject hitBox_Punch;

    private CharacterController controller;
    private float horizontalVelocity = 0.0f;   // m/s
    private float verticalVelocity = 0.0f;     // m/s

    State state = State.WALKING;

    enum State
    {
        WALKING,
        ATTACKING,
        DIED
    }

    // Start is called before the first frame update
    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case State.WALKING:
                if (horizontalVelocity < minVelocity) horizontalVelocity += acceleration * Time.deltaTime;
                verticalVelocity += gravity * Time.deltaTime;

                Vector3 move = new Vector3(0, verticalVelocity, -horizontalVelocity);
                controller.Move(move * Time.deltaTime);

                if (controller.isGrounded) verticalVelocity = 0f;

                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5);
                foreach (var hitCollider in hitColliders)
                {
                    //hitCollider.SendMessage("AddDamage");
                    if (hitCollider.CompareTag("Player") && controller.isGrounded)
                    {
                        Debug.Log("Tocando enemigo");

                        StartCoroutine(ActivateHitBox(hitBox_Punch, 1.5f));
                    }
                }
                break;
            case State.ATTACKING:
                verticalVelocity += gravity * Time.deltaTime;
                move = new Vector3(0, verticalVelocity, -horizontalVelocity);
                controller.Move(move * Time.deltaTime);
                break;

            case State.DIED:
                horizontalVelocity = -20;
                verticalVelocity = 20;

                move = new Vector3(0, verticalVelocity, -horizontalVelocity);
                controller.Move(move * Time.deltaTime);

                transform.Rotate(new Vector3(10, 0, 0));

                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(ActivateHitBox(hitBox_Punch, 2f));
        //Debug.Log("Tocando enemigo");
    }

    public void Damaged(float vel)
    {
        horizontalVelocity = vel;
        verticalVelocity = -vel/2;
    }

    System.Collections.IEnumerator ActivateHitBox(GameObject hitBox, float time)
    {
        horizontalVelocity = 0f;
        state = State.ATTACKING;        
        yield return new WaitForSeconds(time);
        hitBox.SetActive(true);
        StartCoroutine(Dash(0.2f));
        yield return new WaitForSeconds(0.2f);      
        hitBox.SetActive(false);      
    }

    System.Collections.IEnumerator Dash(float time)
    {
        horizontalVelocity = 5f;
        yield return new WaitForSeconds(time);
        horizontalVelocity = 0f;
        state = State.WALKING;
    }

    public void died()
    {
        Destroy(gameObject, 3);
        controller.detectCollisions = false;
        state = State.DIED;
    }
}
