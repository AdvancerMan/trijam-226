using System.Collections.Generic;
using UnityEngine;

public class WaveDescriptor : MonoBehaviour {

    [SerializeField]
    public int coinsRewardForWave;

    private List<SpawnerWave> spawnerWaves;

    private void Start() {
        updateSpawnerWaves();
    }

    private void OnValidate() {
        updateSpawnerWaves();
    }

    public void startSpawning() {
        foreach (var spawnerWave in spawnerWaves) {
            spawnerWave.startSpawning();
        }
    }

    public bool getIsSpawning() {
        foreach (var spawnerWave in spawnerWaves) {
            if (spawnerWave.getIsSpawning()) {
                return true;
            }
        }

        return false;
    }

    private void updateSpawnerWaves() {
        spawnerWaves = new List<SpawnerWave>();

        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).TryGetComponent<SpawnerWave>(out var wave)) {
                spawnerWaves.Add(wave);
            }
        }
    }
}
