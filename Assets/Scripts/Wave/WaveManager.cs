using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    [SerializeField]
    private TMP_Text waveNumber;

    private List<WaveDescriptor> waveDescriptors;
    private EnemyManager enemyManager;
    private MoneyManager moneyManager;

    private int currentWaveDescriptorIndex = -1;

    private void Start() {
        enemyManager = FindObjectOfType<EnemyManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        updateWaveDescriptors();
    }

    private void OnValidate() {
        updateWaveDescriptors();
    }

    private void Update() {
        if (currentWaveDescriptorIndex >= waveDescriptors.Count) {
            return;
        }

        if (enemyManager.getEnemies().Count == 0 && (currentWaveDescriptorIndex == -1 || !waveDescriptors[currentWaveDescriptorIndex].getIsSpawning())) {
            if (currentWaveDescriptorIndex != -1) {
                moneyManager.addCoins(waveDescriptors[currentWaveDescriptorIndex].coinsRewardForWave);
            }
            currentWaveDescriptorIndex++;

            if (currentWaveDescriptorIndex >= waveDescriptors.Count) {
                waveNumber.text = "You won!";
            } else {
                waveNumber.text = "Wave " + (currentWaveDescriptorIndex + 1);
                startSpawning();
            }

        }
    }

    private void startSpawning() {
        WaveDescriptor currentWaveDescriptor = waveDescriptors[currentWaveDescriptorIndex];
        currentWaveDescriptor.startSpawning();
    }

    private void updateWaveDescriptors() {
        waveDescriptors = new List<WaveDescriptor>();

        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).TryGetComponent<WaveDescriptor>(out var descriptor)) {
                waveDescriptors.Add(descriptor);
            }
        }
    }
}