using UnityEngine;

public class TapController : MonoBehaviour
{

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;
    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector2 startPos;

    public AudioSource tapAudio;
    public AudioSource deathAudio;
    public AudioSource scoreAudio;

    Rigidbody2D rigidBody;
    Quaternion downRotation;
    Quaternion forwardRotation;

    GameController gameController;

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        forwardRotation = Quaternion.Euler(0, 0, 35);
        gameController = GameController.Instance;
        rigidBody.simulated = false;
    }

    void OnEnable() {
        GameController.OnGameStarted += OnGameStarted;
        GameController.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable() {
        GameController.OnGameStarted -= OnGameStarted;
        GameController.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted() {
        rigidBody.velocity = Vector3.zero;
        rigidBody.simulated = true;
    }

    void OnGameOverConfirmed() {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }
    void Update()
    {
        if (gameController.GameOver) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            transform.rotation = forwardRotation;
            rigidBody.velocity = Vector3.zero;
            rigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
            tapAudio.Play();
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "ScoreZone") {
            OnPlayerScored();
            scoreAudio.Play();
        }

        if (other.gameObject.tag == "DeathZone") {
            rigidBody.simulated = false;
            OnPlayerDied();
            deathAudio.Play();
        }
    }
}
