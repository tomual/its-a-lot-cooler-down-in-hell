using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    Rigidbody rigidbody;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            if (targetInteractable)
            {
                targetInteractable.Talk();
            }
        }

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
    private void OnAnimatorMove()
    {
        if (!IsDead())
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
