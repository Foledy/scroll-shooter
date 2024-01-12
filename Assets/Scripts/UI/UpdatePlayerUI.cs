using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePlayerUI : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField] private Text _healthText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _ammoText;

    [Header("String Templates")]
    [SerializeField] private string _healthTemplate;
    [SerializeField] private string _scoreTemplate;
    [SerializeField] private string _ammoTemplate;

    public void ChangeHealth(float health) => _healthText.text = string.Format(_healthTemplate, health);

    public void ChangeScore(float score) => _scoreText.text = string.Format(_scoreTemplate, score);

    public void ChangeAmmo(int ammo) => _ammoText.text = string.Format(_ammoTemplate, ammo);
    
    public void ChangeAmmo(string reloadText) => _ammoText.text = string.Format(_ammoTemplate, reloadText);
}
