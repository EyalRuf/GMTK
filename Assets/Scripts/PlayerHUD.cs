using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour {

    public Transform hpIconTransform;
    public TextMeshProUGUI timerText;
    
    public int amountOfTime;

    private void Start() {
        StartTimer();
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

    public void StartTimer() {
        _ = StartCoroutine(GameTimer());
        IEnumerator GameTimer() {
            while(amountOfTime != 0) {
                timerText.text = "Time: \n" + amountOfTime.ToString();
                amountOfTime -= 1;
                yield return new WaitForSeconds(1);
            }
        }
    }


}
