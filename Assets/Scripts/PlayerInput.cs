using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public float MoveSpeed;
    public InputAction MoveAction;
    public InputAction PauseAction;

    private Camera camera;
    private float boundRight;
    private float boundLeft;

    private Vector2 halfSpriteSize;

    private float input;

    private void Awake()
    {
        camera = Camera.main;

        boundRight = Helper.GetScreenBoundRight(camera);
        boundLeft = Helper.GetScreenBoundLeft(camera);

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = spriteRenderer.sprite;
        halfSpriteSize = new Vector2(spriteRenderer.bounds.size.x / 2, spriteRenderer.bounds.size.y / 2);

        MoveAction.performed += OnMovePerformed;
        MoveAction.canceled += OnMoveCancelled;

        PauseAction.performed += OnPauseActionPerformed;
    }

    private void Update()
    {
        transform.Translate(input * MoveSpeed * Time.deltaTime * Vector3.right);

        if (transform.position.x > boundRight - halfSpriteSize.x)
        {
            transform.position = new Vector3(boundRight - halfSpriteSize.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < boundLeft + halfSpriteSize.x)
        {
            transform.position = new Vector3(boundLeft + halfSpriteSize.x, transform.position.y, transform.position.z);
        }
    }

    private void OnEnable()
    {
        MoveAction.Enable();
        PauseAction.Enable();
    }

    private void OnDisable()
    {
        MoveAction.Disable();
        PauseAction.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        input = context.ReadValue<float>();
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        input = 0;
    }

    private void OnPauseActionPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.GameState != GameController.GAME_STATE.START_GAME_STATE &&
            GameController.Instance.GameState != GameController.GAME_STATE.GAME_OVER_STATE)
        {
            GameController.Instance.GameState = GameController.GAME_STATE.PAUSE_STATE;
        }
    }
}
