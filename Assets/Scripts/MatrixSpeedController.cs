using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixSpeedController : MonoBehaviour
{
    public TheMatrixFallingTextUI Matrix;
    public float IncreaseTimeSpeed = 2f;
    public float MaxSpeed = 13f;
    public float MinSpeed = 1f;
    private float _currentSpeed = 1f;
    // Update is called once per frame
    void Update()
    {
        if (_currentSpeed <= MinSpeed) return;
        _currentSpeed -=  Time.deltaTime * IncreaseTimeSpeed;
        Matrix.textDownSpeed = _currentSpeed;
    }

    public void IncreaseSpeed(float velocity)
    {
        _currentSpeed += velocity;
        if (_currentSpeed > MaxSpeed) _currentSpeed = MaxSpeed;
    }

    private void Start()
    {
        GameManager.OnMatrixEffect += IncreaseSpeed;
    }

    private void OnDestroy()
    {
        GameManager.OnMatrixEffect -= IncreaseSpeed;
    }
}
