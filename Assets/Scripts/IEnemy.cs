using UnityEngine;

public interface IEnemy {

    public Transform getTransform();

    public void hit(float damage);
}
