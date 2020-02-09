using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    Rigidbody rigidbody;

    public bool canMove;

    Vector3 movement;
    Quaternion rotation = Quaternion.identity;
    float turnSpeed = 20f;
    float speed = 2f;

    Interactable targetInteractable;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool IsCooledDown()
    {
        return UIManager.instance.isCooledDown;
    }

    void StartCoolDown()
    {
        StartCoroutine(UIManager.instance.CooldownAct());
    }

    void FixedUpdate()
    {
        if (IsCooledDown() && Input.GetButton("Fire1"))
        {
            if (targetInteractable)
            {
                canMove = false;
                targetInteractable.Talk();
            }
        }

        if (canMove)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            movement.Set(horizontal, 0f, vertical);
            movement.Normalize();

            bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

            bool walking = hasHorizontalInput || hasVerticalInput;
            animator.SetBool("Walking", walking);

            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);

            rotation = Quaternion.LookRotation(desiredForward);
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }
    private void OnAnimatorMove()
    {
        if (canMove && !IsDead())
        {
            rigidbody.MovePosition(rigidbody.position + movement * animator.deltaPosition.magnitude * speed);
            rigidbody.MoveRotation(rotation);
        }
    }

    bool IsDead()
    {
        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Interactable>())
        {
            UIManager.instance.ShowTalkPrompt(other.gameObject);
            targetInteractable = other.GetComponent<Interactable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Interactable>())
        {
            UIManager.instance.HideTalkPrompt();
            targetInteractable = null;
        }
    }
}
