using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour {

    public Transform hpIconTransform;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI bombText;
    public TextMeshProUGUI killedText;
    public TextMeshProUGUI gameOverKilledText;
    public GameObject playerAliveCanvas;
    public GameObject playerDeadCanvas;
    public Animator bombDiceAnim;
    
    public int nextWaveTimer;
    public int bombCooldownTimer;

    private int currentBombTime;
    private int enemiesKilledAmount;

    private void Start() {
        currentBombTime = bombCooldownTimer;

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
                if(isBombTimer) {
                    currentBombTime = time;
                    print(currentBombTime);
                    BombDice();
                }
                else
                    timerText.text = "Wave: \n" + time.ToString();
                time -= 1;
                yield return new WaitForSeconds(1);
            }
        }
    }

    private void BombDice() {
        switch(currentBombTime){
            case 3: 
                bombDiceAnim.Play("3TO2");
                break;
            case 2: 
                bombDiceAnim.Play("2TO1");
                break;
            case 1:
                bombDiceAnim.Play("1TO0");
                break;
        }
    }

    public void EnemyKilled() {
        enemiesKilledAmount += 1;
        killedText.text = "Killed:\n" + enemiesKilledAmount.ToString();
        gameOverKilledText.text = "Killed:\n" + enemiesKilledAmount.ToString();
    }

    public void GameOver() {
        playerAliveCanvas.SetActive(false);
        playerDeadCanvas.gameObject.SetActive(true);
        StopAllCoroutines();
    }

}
