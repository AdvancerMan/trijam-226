using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour {

    [SerializeField]
    private TMP_Text coinsVisual;

    [SerializeField]
    private int coins = 0;

    private void OnValidate() {
        updateCoinsVisual();
    }

    private void updateCoinsVisual() {
        coinsVisual.text = coins.ToString();
    }

    public void addCoins(int amount) {
        coins += amount;
        updateCoinsVisual();
    }

    public bool takeCoins(int amount) {
        if (coins < amount) {
            return false;
        }

        coins -= amount;
        updateCoinsVisual();
        return true;
    }
}
