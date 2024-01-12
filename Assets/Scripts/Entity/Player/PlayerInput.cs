using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private string _horizontalAxis;
    [SerializeField] private string _jumpButton;
    [SerializeField] private string _fireButton;

    public event Action<float> OnPlayerFire;
    public event Action<float> OnPlayerMove;
    public event Action OnPlayerStopRunning;
    public event Action OnPlayerJump;
    public event Action OnPlayerReload;
    
    private float _horizontalDirection;
    private float _currentDirection;
    private bool _isJumpButtonPressed;
    private bool _isFireButtonPressed;
    private bool _isReloadButtonPressed;

    public void Update()
    {
        GetInputValues();

        if (_horizontalDirection != 0)
        {
            _currentDirection = _horizontalDirection;

            OnPlayerMove?.Invoke(_currentDirection);
        }
        else
        {
            OnPlayerStopRunning?.Invoke();
        }

        if (_isJumpButtonPressed == true)
        {
            OnPlayerJump?.Invoke();
        }

        if (_isFireButtonPressed == true)
        {
            OnPlayerFire?.Invoke(_currentDirection);
        }

        if (_isReloadButtonPressed)
        {
            OnPlayerReload?.Invoke();
        }
    }

    private void GetInputValues()
    {
        _horizontalDirection = Input.GetAxis(_horizontalAxis);
        _isJumpButtonPressed = Input.GetButtonDown(_jumpButton);
        _isFireButtonPressed = Input.GetButtonDown(_fireButton);
        _isReloadButtonPressed = Input.GetKeyDown(KeyCode.R);
    }
}