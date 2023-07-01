using System;
using TMPro;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour {

    [SerializeField]
    private int playerHealth = 5;

    [SerializeField]
    private TMP_Text playerHealthVisual;

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
        Debug.Log("You lose :(");
    }

    public void handlePlayerWin() {
        Debug.Log("You win :)");
    }
}
