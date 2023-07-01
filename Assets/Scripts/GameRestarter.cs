using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameRestarter : MonoBehaviour {

    [SerializeField]
    private Button button;

    private void Start() {
        button.onClick.AddListener(restartGame);
    }

    private void restartGame() {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
