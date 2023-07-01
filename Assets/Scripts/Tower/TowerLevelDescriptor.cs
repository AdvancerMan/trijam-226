using UnityEngine;

public class TowerLevelDescriptor : MonoBehaviour {

    [SerializeField]
    public float damage = 1;

    [SerializeField]
    public float range = 1;

    [SerializeField]
    public float secondsToShoot = 1;

    [SerializeField]
    public int coinsToUpgradeToThisLevel = 1;
}
