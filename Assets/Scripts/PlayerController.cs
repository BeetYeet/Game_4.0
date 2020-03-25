using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static GameObject player;
    public static PlayerController playerController;

    private MyInputSystem input;
    private Vector2 move;
    private Vector2 aim;

    public float speed = 10f;
    public float rotationSpeed = 900f;
    public bool isShooting = false;
    private bool onTarget = false;

    private Rigidbody rigidBody;
    public CharacterAnimation anim;
    public ShootLaser laser;
    public Fighting.WeaponHandler weapon;
    private CharacterAnimation.SpeedState moveState = CharacterAnimation.SpeedState.Idle;

    private void Awake()
    {
        if (playerController != null)
        {
            Debug.LogError("Multiple players!");
            enabled = false;
            return;
        }
        player = gameObject;
        playerController = this;
        if (anim == null)
        {
            Debug.LogError($"Unassigned CharacterAnimation script!\nat {gameObject.name}");
            enabled = false;
        }
        if (laser == null)
        {
            Debug.LogError($"Unassigned ShootLaser script!\nat {gameObject.name}");
            enabled = false;
        }
        if (weapon == null)
        {
            Debug.LogError($"Unassigned WeaponHandler script!\nat {gameObject.name}");
            enabled = false;
        }
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

    private void Update()
    {
        isShooting = aim.x != 0f || aim.y != 0f;
        HandleRotation();
        HandleMovement();

        anim.isShooting = isShooting;
        laser.isShooting = isShooting;
        weapon.isShooting = isShooting && onTarget;
        if (move != Vector2.zero)
        {
            if (isShooting)
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
        rigidBody.velocity = new Vector3(move.x, 0, move.y) * speed * (isShooting ? 0.4f : 1f);
    }

    private void HandleRotation()
    {
        Vector3 look = new Vector3(aim.x, 0, aim.y);
        if (!isShooting)
        {
            onTarget = false;
            MoveLook();
            return;
        }

        Debug.DrawLine(transform.position, transform.position + look);
        Quaternion newRot = Quaternion.LookRotation(look, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, rotationSpeed * Time.deltaTime);
        if (Quaternion.Angle(transform.rotation, newRot) < .1f)
        {
            onTarget = true;
        }
    }

    private void MoveLook()
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
    }
}