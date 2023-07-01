using System.Collections.Generic;
using UnityEngine;

public class SpawnerWave : MonoBehaviour {

    [SerializeField]
    private GraphWayPoint spawnerWayPoint;

    private List<SpawnDescriptor> spawnDescriptors;

    private bool isSpawning = false;
    private int currentDescriptorIndex = 0;
    private float secondsSinceLastSpawn = 0f;

    private void Start() {
        updateSpawnDescriptors();
    }

    private void OnValidate() {
        updateSpawnDescriptors();
    }

    private void Update() {
        handleSpawning();
    }

    private void handleSpawning() {
        if (!isSpawning) {
            return;
        }

        if (currentDescriptorIndex >= spawnDescriptors.Count) {
            isSpawning = false;
            currentDescriptorIndex = 0;
            return;
        }

        secondsSinceLastSpawn += Time.deltaTime;
        if (spawnDescriptors[currentDescriptorIndex].sleepBeforeSpawnSeconds > secondsSinceLastSpawn) {
            return;
        }

        spawnByCurrentDescriptor();
        secondsSinceLastSpawn = 0f;
        currentDescriptorIndex++;
    }

    private void spawnByCurrentDescriptor() {
        SpawnDescriptor spawnDescriptor = spawnDescriptors[currentDescriptorIndex];
        GameObject spawnedObject = Instantiate(spawnDescriptor.prefabToSpawn, spawnerWayPoint.transform.position, Quaternion.identity);
        if (!spawnedObject.TryGetComponent<BasicEnemy>(out var spawnedEnemy)) {
            return;
        }

        spawnedEnemy.startWayPoint = spawnerWayPoint;
    }

    public bool getIsSpawning() {
        return isSpawning;
    }

    public void startSpawning() {
        currentDescriptorIndex = 0;
        secondsSinceLastSpawn = 0f;
        isSpawning = true;
    }

    private void updateSpawnDescriptors() {
        spawnDescriptors = new List<SpawnDescriptor>();

        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).TryGetComponent<SpawnDescriptor>(out var descriptor)) {
                spawnDescriptors.Add(descriptor);
            }
        }
    }
}
