using System;
using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static Action newRoundStarted;
    public int roundNumber = 0;
    [SerializeField] private TextMeshProUGUI roundDisplay;
    [SerializeField] private TextMeshProUGUI remainEnemyDisplay;
    private float lastRoundEndTime;

    void Start()
    {
        roundDisplay.text = "Ready for Next Round";
    }
    void Update()
    {
        if (GameManager.singleton.enemyFolder.transform.childCount == 0 && Time.time - lastRoundEndTime > 4f)
        {
            lastRoundEndTime = Time.time;
            roundDisplay.text = "Ready for Next Round";
            Invoke("RoundEnd", 3f);

        }
        remainEnemyDisplay.text = "Enemy - " + GameManager.singleton.enemyFolder.transform.childCount.ToString();
    }

    private void RoundEnd()
    {
        roundNumber++;
        roundDisplay.text = "Round " + roundNumber.ToString();
        newRoundStarted?.Invoke();
    }
    
}
