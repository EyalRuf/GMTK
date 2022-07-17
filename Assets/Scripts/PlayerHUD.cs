using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour {

    public Transform hpIconTransform;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI killText;
    
    //public TextMeshProUGUI bombText;
    //public Animator bombDiceAnim;
    //private int currentBombTime;

    public int nextWaveTimer;
    public int bombCooldown;
    public Animator bombTimerAnimator;
    
    private int enemiesKilledAmount;

    private void Start() {
        //currentBombTime = bombCooldown;


        StartTimer(nextWaveTimer, false);
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

    public void SetBomb()
    {
        bombTimerAnimator.SetTrigger("UseBomb");
    }

    public void DetonateBomb()
    {
        StartTimer(bombCooldown, true);
    }

    public void StartTimer(float time, bool isBombTimer) {
        StartCoroutine(GameTimer());
        
        IEnumerator GameTimer() {

            while(time >= 0) {
                if(isBombTimer) {
                    bombTimerAnimator.ResetTrigger("UseBomb");
                    bombTimerAnimator.SetTrigger("Detonate");
                    yield return new WaitForSecondsRealtime(bombCooldown);
                    bombTimerAnimator.ResetTrigger("Detonate");
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

}
