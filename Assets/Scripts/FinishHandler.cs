using System;
using UI;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FinishHandler : MonoBehaviour
{
    [SerializeField] private UIController _uiController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player) == true)
        {
            _uiController.LoadScene("WinScene");
        }
    }
}
