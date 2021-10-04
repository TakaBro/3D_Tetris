using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private Text scoreText, levelText, layersText;
    [SerializeField] 
    private GameObject gameOverWindow;

    private void OnEnable()
    {
        GameManager.instance.onGameOver += SetGameOverWindow;
        GameManager.instance.onScoreUpdated += UpdateUI;
        GameManager.instance.onLayersCleared += UpdateUI;
        GameManager.instance.onLevelUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        GameManager.instance.onGameOver -= SetGameOverWindow;
        GameManager.instance.onScoreUpdated -= UpdateUI;
        GameManager.instance.onLayersCleared -= UpdateUI;
        GameManager.instance.onLevelUpdated -= UpdateUI;
    }

    private void Start()
    {
        gameOverWindow.SetActive(false);
    }

    public void UpdateUI(int score, int level, int layers)
    {
        scoreText.text = "SCORE: " + score.ToString("D8");
        levelText.text = "LEVEL: " + level.ToString("D2");
        layersText.text = "LAYERS: " + layers.ToString("D8");
    }

    public void SetGameOverWindow(bool value)
    {
        gameOverWindow.SetActive(value);
    }
}
