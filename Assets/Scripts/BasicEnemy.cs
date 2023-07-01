using UnityEngine;

public class BasicEnemy : MonoBehaviour, IEnemy {

    [SerializeField]
    private GraphWayPoint startWayPoint;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private EnemyManager enemyManager;

    private GraphWayPoint currentWayPoint;

    private void Start() {
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