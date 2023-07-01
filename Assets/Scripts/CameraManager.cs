using UnityEngine;

public class CameraManager : MonoBehaviour {
  
    [SerializeField]
    private float worldWidth = 26.39125f;

    [SerializeField]
    private float worldHeight = 12.5333f;

    private Vector3 currentScreenSize;

    private void Start() {
        updateCurrentScreenSize();
        currentScreenSize = calculateScreenSize();
    }

    private Vector3 calculateScreenSize() {
        Vector3 bottomLeftPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRightPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        return topRightPoint - bottomLeftPoint;
    }

    private void updateCurrentScreenSize() {
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = Camera.main.aspect * cameraHeight;
        transform.localScale = new Vector3(cameraWidth / worldWidth, cameraHeight / worldHeight, 1f);
    }

    private void Update() {
        Vector3 screenSize = calculateScreenSize();
        if (screenSize == currentScreenSize) {
            return;
        }
        updateCurrentScreenSize();
    }
}
