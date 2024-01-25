using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    [SerializeField] float maxHorizontalVelocity = 1f;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpVelocity = 1f;

    [SerializeField] float punchVelocity = 1f;
    [SerializeField] float smashVelocity = 1f;

    [SerializeField] float minVelocity = 1f;
    [SerializeField] float maxVelocity = 3f;

    [SerializeField] GameObject hitBox_Punch;
    [SerializeField] GameObject hitBox_Uppercut;
    [SerializeField] GameObject hitBox_Smash;

    [Header("Input Action References")]
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference punch;
    [SerializeField] InputActionReference gancho;

    private CharacterController controller;
    private float horizontalVelocity = 0.0f;   // m/s
    private float verticalVelocity = 0.0f;     // m/s

    float time = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        jump.action.Enable();
        punch.action.Enable();
        gancho.action.Enable();
    }

    void OnDisable()
    {
        jump.action.Disable();
        punch.action.Enable();
        gancho.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (horizontalVelocity < minVelocity) horizontalVelocity += acceleration * Time.deltaTime;
        verticalVelocity += gravity * Time.deltaTime;

        if (jump.action.WasPerformedThisFrame() && controller.isGrounded) verticalVelocity = jumpVelocity;

        // Horizontal Movement
        if (punch.action.WasPerformedThisFrame()) {
            horizontalVelocity += punchVelocity;
            StartCoroutine(ActivateHitBox(hitBox_Punch, 0.2f));
        }
        horizontalVelocity = Mathf.Min(horizontalVelocity, maxVelocity);

        if (gancho.action.WasPerformedThisFrame() && controller.isGrounded)
        {
            verticalVelocity = jumpVelocity;
            horizontalVelocity /= 2;
            StartCoroutine(ActivateHitBox(hitBox_Uppercut, 0.2f));
        }
        else if (gancho.action.WasPerformedThisFrame() && !controller.isGrounded)
        {
            verticalVelocity = -smashVelocity;
            horizontalVelocity /= 2;
            StartCoroutine(ActivateHitBox(hitBox_Smash, 0.2f));
        }

        Vector3 move = new Vector3(0, verticalVelocity, horizontalVelocity);
        controller.Move(move * Time.deltaTime);

        if (controller.isGrounded) verticalVelocity = 0f;

        //Debug.Log($"Is Grounded - {controller.isGrounded}");
        //Debug.Log($"Is Grounded - {verticalVelocity}");
        //Debug.Log($"Is Grounded - {jump.action.triggered}");
    }

    public void Damaged(float vel)
    {
        horizontalVelocity = vel;
        verticalVelocity = -vel;
    }

    System.Collections.IEnumerator ActivateHitBox(GameObject hitBox, float time)
    {
        hitBox.SetActive(true);
        yield return new WaitForSeconds(time);
        hitBox.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScenarioEnd")) {
            LevelGenerator.Generate(other.transform.position);
        }
    }
}
