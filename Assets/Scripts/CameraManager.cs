using UnityEngine;

public class CameraManager : MonoBehaviour {
  
    [SerializeField]
    private float worldWidth = 26.39125f;

    [SerializeField]
    private float worldHeight = 12.5333f;

    private void Start() {
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = Camera.main.aspect * cameraHeight;

        transform.localScale = new Vector3(cameraWidth / worldWidth, cameraHeight / worldHeight, 1f);
    }
}
