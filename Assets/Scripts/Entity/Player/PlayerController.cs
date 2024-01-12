using Assets.Scripts.Enemy;
using Assets.Scripts.Weapon;
using Audio;
using DefaultNamespace;
using DefaultNamespace.Entity.Player;
using Settings;
using UI;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerAnimatorController))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : GameEntity
{
    [Header("Main Settings")]
    [SerializeField] private UpdatePlayerUI _updateUI;
    [SerializeField] private UIController _uiController;
    [SerializeField] private ShooterWeapon _weapon;

    [Header("Volume Settings")] 
    [SerializeField] private AudioSource _musicAudio;
    [SerializeField] private AudioSource _effectsAudio;
    
    public static float Score { get; private set; }
    public GameObject Entity => gameObject;
    
    private PlayerMovement _playerMovement;
    private PlayerInput _playerInput;
    private PlayerAnimatorController _animator;
    private Health _health;
    private SpriteRenderer _sprite;

    private bool _isAlive;
    private bool _isGameActiveAndEnabled;

    private void Start()
    {
        Init();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    private void Init()
    {
        InitComponents();
        Subscribe();
    }

    private void InitComponents()
    {
        _musicAudio.volume = SettingsManager.MusicVolume;
        _effectsAudio.volume = SettingsManager.EffectsVolume;
        
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<PlayerAnimatorController>();
        _health = GetComponent<Health>();
        _sprite = GetComponent<SpriteRenderer>();

        _isAlive = true;
        _isGameActiveAndEnabled = true;
        
        Score = 0;

        _updateUI.ChangeScore(Score);
        _updateUI.ChangeAmmo(_weapon.CurrentMagazineAmmo);
        _updateUI.ChangeHealth(_health.Value);
    }

    private void Subscribe()
    {
        _playerInput.OnPlayerFire += Attack;
        _playerInput.OnPlayerJump += Jump;
        _playerInput.OnPlayerMove += Move;
        _playerInput.OnPlayerReload += Reload;
        _playerInput.OnPlayerStopRunning += OnPlayerStopRunning;
        _health.OnEntityDead += PlayerDead;
        _health.OnEntityHeal += Heal;
        _health.OnEntityTakeDamage += TakeDamage;
        _weapon.OnAmmoChanged += _updateUI.ChangeAmmo;
        _weapon.OnPlayerReload += Reload;
        Enemy.OnEnemyDead += ChangeScore;
    }

    private void UnSubscribe()
    {
        _playerInput.OnPlayerFire -= Attack;
        _playerInput.OnPlayerJump -= Jump;
        _playerInput.OnPlayerMove -= Move;
        _playerInput.OnPlayerReload -= Reload;
        _playerInput.OnPlayerStopRunning += OnPlayerStopRunning;
        _health.OnEntityDead -= PlayerDead;
        _health.OnEntityHeal -= Heal;
        _health.OnEntityTakeDamage -= TakeDamage;
        _weapon.OnAmmoChanged -= _updateUI.ChangeAmmo;
        _weapon.OnPlayerReload -= Reload;
        Enemy.OnEnemyDead -= ChangeScore;
    }

    private void Heal(float amount)
    {
        AudioManager.HealSound();
        
        _updateUI.ChangeHealth(amount);
    }

    private void TakeDamage(float amount)
    {
        AudioManager.TakeDamageSound();
        
        _updateUI.ChangeHealth(amount);
    }

    private void ChangeScore(int amount)
    {
        Score += amount;

        _updateUI.ChangeScore(Score);
    }

    private void Move(float direction)
    {
        if (_isAlive == true && _isGameActiveAndEnabled == true)
        {
            _sprite.flipX = direction < 0;
            
            _playerMovement.Move(direction);
            
            _animator.Run();
        }
    }

    private void Jump()
    {
        if(_isAlive == true && _isGameActiveAndEnabled == true)
            _playerMovement.Jump();
    }

    private void Attack(float direction)
    {
        if (_isAlive == true && _isGameActiveAndEnabled == true)
        {
            _weapon.Attack(direction);
        }
    }

    private void Reload()
    {
        if (_isAlive == true && _isGameActiveAndEnabled == true && _weapon.CanAttack == true && _weapon.CurrentMagazineAmmo != _weapon.MaxMagazineAmmo)
        {
            _updateUI.ChangeAmmo("Reloading....");
            
            AudioManager.ReloadSound();
            
            _weapon.Reload();
        }
    }

    private void PlayerDead()
    {
        _uiController.LoadScene("LoseScene");
    }

    private void OnPlayerStopRunning() => _animator.Idle();
}