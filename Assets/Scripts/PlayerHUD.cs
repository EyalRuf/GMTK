using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour {

    public Transform hpIconTransform;
    public Transform gameOverPanel;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI bombText;
    public TextMeshProUGUI killedText;
    public TextMeshProUGUI gameOverKilledText;
    public GameObject CanvasGO;
    
    public int nextWaveTimer;
    public int bombCooldownTimer;

    private int enemiesKilledAmount;

    private void Start() {
        StartTimer(nextWaveTimer, false);
        StartTimer(bombCooldownTimer, true);
    }

    public void SetHUD(Unit unit) {
        unit.currentHealth = unit.maxHealth;
        SetHP(unit.currentHealth);
    }

    public void SetHP(int hp) {
        foreach(Transform child in hpIconTransform) {
            child.GetComponent<Image>().color = Color.red;
        }
    }

    public void StartTimer(int time, bool isBombTimer) {
        StartCoroutine(GameTimer());
        
        IEnumerator GameTimer() {
            while(time != 0) {
                if(isBombTimer)
                    bombText.text = "Bomb: \n" + time.ToString();
                else
                    timerText.text = "Wave: \n" + time.ToString();

                time -= 1;
                yield return new WaitForSeconds(1);
            }
        }
    }

    public void EnemyKilled() {
        enemiesKilledAmount += 1;
        killedText.text = "Killed:\n" + enemiesKilledAmount.ToString();
        gameOverKilledText.text = "Killed:\n" + enemiesKilledAmount.ToString();
    }

    public void GameOver() {
        gameOverPanel.gameObject.SetActive(true);
        timerText.transform.parent.gameObject.SetActive(false);
    }

}
