using System;
using UnityEngine;

namespace DefaultNamespace.Entity.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatorController : MonoBehaviour
    {
        [SerializeField] private string _runBoolName;

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void Run() => _animator.SetBool(_runBoolName, true);

        public void Idle() => _animator.SetBool(_runBoolName, false);
    }
}