using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour, IEnemy {

    private EnemyManager enemyManager;
    private MoneyManager moneyManager;

    [SerializeField]
    public GraphWayPoint startWayPoint;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private int coinsForKill;

    private GraphWayPoint currentWayPoint;

    private void Start() {
        enemyManager = FindObjectOfType<EnemyManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
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
        Debug.Log("Target hit");
        handleDeath();
    }

    private void handleDeath() {
        moneyManager.addCoins(coinsForKill);
        enemyManager.removeEnemy(this);
        Destroy(gameObject);
    }

    public void hit(float damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            handleDeath();
        }
    }

    public Transform getTransform() {
        return transform;
    }
}
