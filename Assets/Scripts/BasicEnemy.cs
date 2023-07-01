using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour, IEnemy {

    private EnemyManager enemyManager;
    private MoneyManager moneyManager;
    private PlayerHealthManager playerHealthManager;

    [SerializeField]
    public GraphWayPoint startWayPoint;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private int coinsForKill;

    [SerializeField]
    private int damage = 1;

    private GraphWayPoint currentWayPoint;

    private void Start() {
        enemyManager = FindObjectOfType<EnemyManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        playerHealthManager = FindObjectOfType<PlayerHealthManager>();
        enemyManager.addEnemy(this);
        currentWayPoint = startWayPoint;
    }

    void Update() {
        if (currentWayPoint != null) {
            handleMovement();
        }
    }

    private void handleMovement() {
        Vector3 direction = currentWayPoint.transform.position - transform.position;
        float remainingPathSquared = direction.sqrMagnitude;
        float moveDistance = speed * Time.deltaTime;

        if (remainingPathSquared < moveDistance * moveDistance) {
            transform.position += direction;
            currentWayPoint = currentWayPoint.getNextWayPoint();
        } else {
            transform.position += direction.normalized * moveDistance;
        }

        if (currentWayPoint == null) {
            handleTargetHit();
        }
    }

    private void handleTargetHit() {
        playerHealthManager.takeHealth(damage);
        handleDeath();
    }

    private void handleDeath() {
        enemyManager.removeEnemy(this);
        Destroy(gameObject);
    }

    public void hit(float damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            moneyManager.addCoins(coinsForKill);
            handleDeath();
        }
    }

    public Transform getTransform() {
        return transform;
    }
}
