using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool clickArrow = true;
    public Button btn_touch;
    public Sprite[] Image;

    public float opponentHealthValue;
    public TMP_Text opponentHealth;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void click_arrow()
    {
        clickArrow = !clickArrow;
        if (clickArrow)
            btn_touch.image.sprite = Image[1];
        else
            btn_touch.image.sprite = Image[0];
    }

    public void UpdateHealthUI(float value)
    {
        opponentHealth.text = value.ToString();
    }
}
