using System.Collections.Generic;
using UnityEngine;

public class BasicTower : MonoBehaviour {

    [SerializeField]
    private EnemyManager enemyManager;

    [SerializeField]
    private GameObject shootLinePrefab;

    [SerializeField]
    private float damage = 1;

    [SerializeField]
    private float range = 1;

    [SerializeField]
    private float secondsToShoot = 1;

    private float secondsSinceShoot = 0;

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void Update() {
        secondsSinceShoot += Time.deltaTime;
        if (secondsSinceShoot > secondsToShoot) {
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
    }

    private void spawnShootLine(Vector3 from, Vector3 to) {
        GameObject line = Instantiate(shootLinePrefab);
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.SetColors(Color.red, Color.red);
        lineRenderer.SetWidth(0.1f, 0.1f);
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
        Destroy(line, 0.2f);
    }
}
