using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour {

    [SerializeField]
    private int playerHealth = 5;

    [SerializeField]
    private TMP_Text playerHealthVisual;

    [SerializeField]
    private GameObject youLostWindow;

    [SerializeField]
    private GameObject youWonWindow;

    private bool gameIsFinished = false;

    private void Update() {
        if (gameIsFinished && Input.GetMouseButtonDown(0)) {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }

    public void takeHealth(int amount) {
        if (playerHealth <= 0) {
            return;
        }

        playerHealth = Math.Max(playerHealth - amount, 0);
        playerHealthVisual.text = playerHealth.ToString();

        if (playerHealth <= 0) {
            handlePlayerLose();
        }
    }

    private void handlePlayerLose() {
        youLostWindow.SetActive(true);
        gameIsFinished = true;
    }

    public void handlePlayerWin() {
        if (playerHealth == 0) {
            handlePlayerLose();
            return;
        }
        youWonWindow.SetActive(true);
        gameIsFinished = true;
    }
}
