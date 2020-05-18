using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighscoreText : MonoBehaviour
{
    Text highscore;

    void OnEnable() {
        highscore = GetComponent<Text>();
        highscore.text = "Highscore: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
}
