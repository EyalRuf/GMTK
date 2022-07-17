using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHUD : MonoBehaviour {

    public Transform hpIconTransform;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI killText;
    
    //public TextMeshProUGUI bombText;
    //public Animator bombDiceAnim;
    //private int currentBombTime;

    public Animator bombTimerAnimator;
    public float bombAnimTime = 5.0f;

    public Animator spawnTimerAnimator;
    public float spawnAnimTime = 5.0f;

    private int enemiesKilledAmount;
    public int kills;

    public Animator H1Anim;
    public Animator H2Anim;
    public Animator H3Anim;

    public void RestartGameUI ()
    {
        kills = 0;
        killText.text = "0";
        UpdateHeartUI(1);
    }

    private void Start() {
        kills = 0;
        //currentBombTime = bombCooldown;
    }

    public void SetHUD(Unit unit) {
        //unit.currentHealth = unit.maxHealth;
        //SetHP(unit.currentHealth);
    }

    internal void KillCounter()
    {
        kills++;
        killText.text = kills + "";
    }

    public void SetHP(int hp) {
        //foreach(Transform child in hpIconTransform) {
        //    child.GetComponent<Image>().color = Color.red;
        //}
    }

    public void SetBomb()
    {
        bombTimerAnimator.SetInteger("bombState", 1);
    }

    public void DetonateBomb(float time)
    {
        StartTimer(time, true);
    }

    public void SpawnAnim(float time)
    {
        StartTimer(time, false);
    }

    public void StartTimer(float time, bool isBombTimer) {
        StartCoroutine(GameTimer());
        
        IEnumerator GameTimer() {

            while(time >= 0) {
                if(isBombTimer) {
                    bombTimerAnimator.SetInteger("bombState", 2);
                    bombTimerAnimator.speed = bombAnimTime / time;
                    yield return new WaitForSecondsRealtime(time);
                    bombTimerAnimator.SetInteger("bombState", 0);
                }
                else {
                    spawnTimerAnimator.SetTrigger("spawn");
                    spawnTimerAnimator.speed = spawnAnimTime / time;
                    yield return new WaitForSecondsRealtime(time);
                    spawnTimerAnimator.ResetTrigger("spawn");
                }
            }
        }
    }

    internal void UpdateHeartUI(float v)
    {
        if (v < .9f)
        {
            H1Anim.SetBool("Fullheart", false);
        } else
        {
            H1Anim.SetBool("Fullheart", true);
        }
        if (v < .6f)
        {
            H2Anim.SetBool("Fullheart", false);
        } else
        {
            H2Anim.SetBool("Fullheart", true);
        }
        if (v < .3f)
        {
            H3Anim.SetBool("Fullheart", false);
        } else
        {
            H3Anim.SetBool("Fullheart", true);
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
