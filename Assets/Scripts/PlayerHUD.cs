using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour {

    public Transform hpIconTransform;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI killedText;
    public TextMeshProUGUI gameOverKilledText;
    public GameObject playerAliveCanvas;
    
    
    //public TextMeshProUGUI bombText;
    //public Animator bombDiceAnim;
    //private int currentBombTime;

    public Slider bombSlider;
    
    public int nextWaveTimer;
    public int bombCooldown;

    
    private int enemiesKilledAmount;

    private void Start() {
        //currentBombTime = bombCooldown;

        bombSlider.maxValue = bombCooldown;
        bombSlider.value = 0;

        StartTimer(nextWaveTimer, false);
        StartTimer(bombSlider.value, true);
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

    public void StartTimer(float time, bool isBombTimer) {
        StartCoroutine(GameTimer());
        
        IEnumerator GameTimer() {
            while(time >= 0) {
                if(isBombTimer) {
                    bombSlider.value = time;
                    time += bombSlider.maxValue/(100*bombCooldown);
                    //BombDice();
                    yield return new WaitForSecondsRealtime(0.01f);
                }
                else {
                    timerText.text = "Wave: \n" + time.ToString();
                    
                    time -= 1;
                    yield return new WaitForSeconds(1f);
                }
                
                
            }
        }
    }

    /*private void BombDice() {
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
    }*/

    public void EnemyKilled() {
        enemiesKilledAmount += 1;
        killedText.text = "Killed:\n" + enemiesKilledAmount.ToString();
        gameOverKilledText.text = "Killed:\n" + enemiesKilledAmount.ToString();
    }

    /*public void GameOver() {
        playerAliveCanvas.SetActive(false);
        playerDeadCanvas.gameObject.SetActive(true);
        StopAllCoroutines();
    }*/

}
