using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisPiece : MonoBehaviour
{
    [SerializeField] private float previousTime, fallTime = 1f, defaultFallTime = 1, fastFallTime = .1f;
    [SerializeField] private string inputHorizontal, inputVertical, inputRotation, inputChangeAxisRotation, inputFastFall;
    private bool isFallingFast = false;

    private enum AxisRotation { X, Y, Z };
    private AxisRotation axisRotation = AxisRotation.Z;

    private void OnEnable()
    {
        fallTime = GameManager.instance.GetFallSpeed();

        // If no valid move left then game is over
        if (!CheckValidMove())
        {
            GameManager.instance.SetIsGameOver(true);
        }
    }

    void Update()
    {
        if (Time.time - previousTime > fallTime)
        {
            transform.position += Vector3.down;

            if (CheckValidMove())
            {
                GridSpace.instance.UpdateGrid(this);
            }
            else
            {
                transform.position += Vector3.up;
                GridSpace.instance.DeleteFullLayer();
                enabled = false;

                // If game is not over then spawn new piece
                if (!GameManager.instance.GetIsGameOver()) GridSpace.instance.SpanwTetrisPiece();
            }
            previousTime = Time.time;
        }
        DetectInput();
    }

    private void DetectInput()
    {
        // Move
        if (Input.anyKeyDown)
        {
            float horizontal = +Input.GetAxisRaw(inputHorizontal);
            float vertical = +Input.GetAxisRaw(inputVertical);

            if (horizontal == -1)
            {
                SetMoveInput(Vector3.left);
            }
            if (horizontal == 1)
            {
                SetMoveInput(Vector3.right);
            }
            if (vertical == -1)
            {
                SetMoveInput(Vector3.back);
            }
            if (vertical == 1)
            {
                SetMoveInput(Vector3.forward);
            }
        }

        // Rotate
        if (Input.GetButtonDown(inputRotation))
        {
            switch (axisRotation)
            {
                case AxisRotation.X:
                    SetRotationInput(new Vector3(90, 0, 0));
                    break;
                case AxisRotation.Y:
                    SetRotationInput(new Vector3(0, 90, 0));
                    break;
                case AxisRotation.Z:
                    SetRotationInput(new Vector3(0, 0, 90));
                    break;
            }
        }

        // Axis Rotate
        if (Input.GetButtonDown(inputChangeAxisRotation))
        {
            if ((int)axisRotation < 2)
            {
                axisRotation++;
            }
            else
            {
                axisRotation = 0;
            }
            Debug.Log((int)axisRotation);
        }

        // Fast fall
        if (Input.GetButtonDown(inputFastFall))
        {
            if (isFallingFast)
            {
                fallTime = defaultFallTime;
                isFallingFast = false;
            }
            else
            {
                fallTime = fastFallTime;
                isFallingFast = true;
            }
        }
    }

    private void SetMoveInput(Vector3 direction)
    {
        transform.position += direction;
        if (CheckValidMove())
        {
            GridSpace.instance.UpdateGrid(this);
        }
        else
        {
            transform.position -= direction;
        }
    }

    private void SetRotationInput(Vector3 rotation)
    {
        transform.Rotate(rotation, Space.World);
        if (CheckValidMove())
        {
            GridSpace.instance.UpdateGrid(this);
        }
        else
        {
            transform.Rotate(-rotation, Space.World);
        }
    }

    private bool CheckValidMove()
    {
        foreach(Transform child in transform)
        {
            Vector3 position = GameManager.instance.Round(child.position);

            // Not valid if not inside grid
            if (!GridSpace.instance.CheckInsideGrid(position))
            {
                return false;
            }

            // Not valid if position is not null and not the piece itself
            Transform t = GridSpace.instance.GetTransformOnGridPosition(position);
            if (t != null && t.parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}
