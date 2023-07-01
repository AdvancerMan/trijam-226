using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasicTower : MonoBehaviour {

    private EnemyManager enemyManager;
    private MoneyManager moneyManager;

    [SerializeField]
    private GameObject shootLinePrefab;

    [SerializeField]
    private List<TowerLevelDescriptor> levelDescriptors;

    [SerializeField]
    private List<BasicTower> towersToDowngradeOnUpgrade;

    [SerializeField]
    private int currentLevel = 1;

    [SerializeField]
    private TMP_Text levelTextMesh;

    [SerializeField]
    private GameObject upgradeVisual;

    [SerializeField]
    private GameObject downgradeVisual;

    private const float LEVEL_CHANGE_DELAY_SECONDS = 1f;
    private float sinceLastUpgrade = LEVEL_CHANGE_DELAY_SECONDS;
    private float sinceLastDowngrade = LEVEL_CHANGE_DELAY_SECONDS;

    private float secondsSinceShoot = 0;

    private float damage = 1;
    private float range = 1;
    private float secondsToShoot = 1;

    private void Start () {
        enemyManager = FindObjectOfType<EnemyManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void Update() {
        handleShooting();
        handleLevelChangeVisual();
    }

    private void OnMouseDown() {
        handleUpgrade();
    }

    private void OnValidate() {
        applyCurrentLevelDescriptor();
    }

    private void applyCurrentLevelDescriptor() {
        TowerLevelDescriptor descriptor = levelDescriptors[currentLevel];
        damage = descriptor.damage;
        range = descriptor.range;
        secondsToShoot = descriptor.secondsToShoot;
        secondsSinceShoot = 0f;
        levelTextMesh.text = "Level: " + currentLevel;
    }

    private void handleLevelChangeVisual() {
        sinceLastUpgrade += Time.deltaTime;
        upgradeVisual.SetActive(sinceLastUpgrade < LEVEL_CHANGE_DELAY_SECONDS);

        sinceLastDowngrade += Time.deltaTime;
        downgradeVisual.SetActive(sinceLastDowngrade < LEVEL_CHANGE_DELAY_SECONDS);
    }

    public void handleDowngrade() {
        if (currentLevel == 0) {
            return;
        }

        sinceLastDowngrade = 0f;
        currentLevel--;
        applyCurrentLevelDescriptor();
    }

    private void handleUpgrade() {
        if (currentLevel + 1 >= levelDescriptors.Count) {
            return;
        }
        if (!moneyManager.takeCoins(levelDescriptors[currentLevel + 1].coinsToUpgradeToThisLevel)) {
            return;
        }

        sinceLastUpgrade = 0f;
        currentLevel++;
        applyCurrentLevelDescriptor();

        if (towersToDowngradeOnUpgrade != null) {
            foreach (var neighborTower in towersToDowngradeOnUpgrade) {
                neighborTower.handleDowngrade();
            }
        }
    }

    private void handleShooting() {
        secondsSinceShoot += Time.deltaTime;
        if (secondsSinceShoot < secondsToShoot) {
            return;
        }

        secondsSinceShoot = 0;
        HashSet<IEnemy> enemies = enemyManager.getEnemies();
        if (enemies.Count == 0) {
            return;
        }

        IEnemy closest = null;
        foreach (IEnemy enemy in enemies) {
            float sqrDistanceToEnemy = (transform.position - enemy.getTransform().position).sqrMagnitude;
            if (sqrDistanceToEnemy > range * range) {
                continue;
            }
            if (closest == null) {
                closest = enemy;
                continue;
            }

            float sqrDistanceToClosest = (transform.position - closest.getTransform().position).sqrMagnitude;
            if (sqrDistanceToEnemy < sqrDistanceToClosest) {
                closest = enemy;
            }
        }

        if (closest == null) {
            return;
        }
        closest.hit(damage);
        spawnShootLine(transform.position, closest.getTransform().position);
    }

    private void spawnShootLine(Vector3 from, Vector3 to) {
        GameObject line = Instantiate(shootLinePrefab);
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
        Destroy(line, 0.2f);
    }
}
