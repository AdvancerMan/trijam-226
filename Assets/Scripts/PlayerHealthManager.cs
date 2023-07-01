using System;
using TMPro;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour {

    [SerializeField]
    private int playerHealth = 5;

    [SerializeField]
    private TMP_Text playerHealthVisual;

    [SerializeField]
    private GameObject youLostWindow;

    [SerializeField]
    private GameObject youWonWindow;

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
    }

    public void handlePlayerWin() {
        if (playerHealth == 0) {
            handlePlayerLose();
            return;
        }
        youWonWindow.SetActive(true);
    }
}
