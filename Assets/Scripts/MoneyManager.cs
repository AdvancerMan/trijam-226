using UnityEngine;

public class MoneyManager : MonoBehaviour {

    private int coins;

    public void addCoins(int amount) {
        coins += amount;
    }

    public bool takeCoins(int amount) {
        if (coins < amount) {
            return false;
        }

        coins -= amount;
        return true;
    }
}
