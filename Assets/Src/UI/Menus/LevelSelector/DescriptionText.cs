using Gameplay;
using Gameplay.World;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class DescriptionText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();

        UpdateText(DifficultyManager.Instance.currentDifficulty);
        DifficultyManager.Instance.OnDifficultyChange += HandleDifficultyChange;
    }

    public void UpdateText(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                EasyText();
                break;
            case Difficulty.Normal:
                NormalText();
                break;
            case Difficulty.Hard:
                HardText();
                break;
        }
    }

    void EasyText()
    {
        _text.text = (  "Easy\n\n" +
                        "Enemies are weaker and have no resistance\n");
    }

    void NormalText()
    {
        _text.text = ("Normal\n\n" +
                      "Enemies have no modifiers but they have some resistance");
    }
    void HardText()
    {
        _text.text = ("Hard\n\n" +
                      "Enemies are stronger and really tough\n" +
                      "You will have to use all towers!");
    }

    private void OnDestroy()
    {
        DifficultyManager.Instance.OnDifficultyChange -= HandleDifficultyChange;
    }

    private void HandleDifficultyChange(Difficulty difficulty)
    {
        UpdateText(difficulty);
    }
}
