using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    public TextMeshProUGUI pointDisplay;
    private int points;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI finalScore;
    public Image screenTint;

    public Image[] ballIcons = new Image[3];
    
    // Singleton pattern setup
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int pointsToAdd)
    {
        points += pointsToAdd;

        pointDisplay.text = points.ToString("D6");

    }

    public void EndGame()
    {
        pointDisplay.gameObject.SetActive(false);
        
        finalScore.text = "Score: " + points.ToString("D6");
        screenTint.gameObject.SetActive(true);
        finalScore.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
    }

    public void UpdatePinball()
    {
        int iconIndex = 3 - GameManager.instance.pinballsSpawned;
        ballIcons[iconIndex].gameObject.SetActive(false);
    }
    
}
