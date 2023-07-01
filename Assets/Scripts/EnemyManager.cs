using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    
    private HashSet<IEnemy> enemies = new HashSet<IEnemy>();

    public HashSet<IEnemy> getEnemies() {
        return enemies;
    }

    public void addEnemy(IEnemy enemy) {
        enemies.Add(enemy);
    }

    public void removeEnemy(IEnemy enemy) {
        enemies.Remove(enemy);
    }
}
