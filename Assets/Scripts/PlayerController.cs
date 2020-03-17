using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterAnimation))]
public class PlayerController : MonoBehaviour
{
    MyInputSystem input;
    Vector2 move;
    Vector2 aim;

    public float speed;
    public float rotationSpeed = 90f;
    public bool shooting = false;

    CharacterAnimation anim;
    CharacterAnimation.SpeedState moveState = CharacterAnimation.SpeedState.Idle;

    Rigidbody rigidBody;

    private void Awake()
    {
        anim = GetComponent<CharacterAnimation>();
        input = new MyInputSystem();
        input.PlayerActionControlls.ShootDirection.performed += OnLook;
        input.PlayerActionControlls.Move.performed += OnMove;
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        shooting = aim.x != 0f || aim.y != 0f;
        HandleRotation();
        HandleMovement();

        anim.shooting = shooting;
        if (move != Vector2.zero)
        {
            if (shooting)
            {
                moveState = CharacterAnimation.SpeedState.Walking;
            }
            else
            {
                moveState = CharacterAnimation.SpeedState.Running;
            }
        }
        else
        {
            moveState = CharacterAnimation.SpeedState.Idle;
        }
        anim.state = moveState;
    }

    private void HandleMovement()
    {
        rigidBody.velocity = new Vector3(move.x, 0, move.y) * speed * (shooting ? 0.4f : 1f);
    }

    private void HandleRotation()
    {
        Vector3 look = new Vector3(aim.x, 0, aim.y);
        if (!shooting)
        {
            MoveLook();
            return;
        }

        Debug.DrawLine(transform.position, transform.position + look);
        Quaternion newRot = Quaternion.LookRotation(look, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, rotationSpeed * Time.deltaTime);
    }

    void MoveLook()
    {
        if (move == Vector2.zero)
            return;
        Quaternion newRot = Quaternion.LookRotation(new Vector3(move.x, 0, move.y), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, rotationSpeed * Time.deltaTime * .5f);
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        aim = ctx.ReadValue<Vector2>();
        float magnitude = aim.normalized.magnitude;
        if (magnitude == 0f)
        {
            aim = Vector2.zero;
            return;
        }
        aim /= magnitude;
        Debug.Log("Look");
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
        float magnitude = move.normalized.magnitude;
        if (magnitude == 0f)
        {
            move = Vector2.zero;
            return;
        }
        move /= magnitude;
        Debug.Log("Move");
    }

}
