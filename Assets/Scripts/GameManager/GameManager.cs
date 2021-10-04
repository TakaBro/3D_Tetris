using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event Action<bool> onGameOver;
    public event Action<int, int, int> onScoreUpdated, onLayersCleared, onLevelUpdated;

    [SerializeField]
    private int score, level, layersCleared;
    [SerializeField]
    private float fallSpeed;
    private bool isGameOver;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public bool GetIsGameOver()
    {
        return isGameOver;
    }

    public void SetIsGameOver(bool value)
    {
        isGameOver = value;
        onGameOver.Invoke(value);
    }

    public float GetFallSpeed()
    {
        return fallSpeed;
    }

    public void SetScore(int amount)
    {
        score += amount;
        CalculateLevel();
        onScoreUpdated.Invoke(score, level, layersCleared);
    }

    public Vector3 Round(Vector3 vec)
    {
        return new Vector3(Mathf.RoundToInt(vec.x),
                            Mathf.RoundToInt(vec.y),
                            Mathf.RoundToInt(vec.z));
    }

    public void LayersCleared(int amount)
    {
        // Calculate combo by layers cleared amount
        switch (amount)
        {
            // Just one layer
            case 1:
                SetScore(400);
                break;

            // Combo two layers
            case 2:
                SetScore(800);
                break;

            // Combo three 
            case 3:
                SetScore(1600);
                break;

            // Combo TETRIS or more
            case int a when (a >= 4):
                SetScore(3200);
                break;
        }
        layersCleared += amount;
        onLayersCleared.Invoke(score, level, layersCleared);
    }

    public void CalculateLevel()
    {
        level++;
        fallSpeed = fallSpeed - .5f;
        onLevelUpdated.Invoke(score, level, layersCleared);
    }
}
