
using Mako.Input;
using Mako.Movement;
using Mako.Shooting;
using Mako.State;
using Mako.VehicleDevices;
using UnityEngine;

namespace Mako
{
    public class PlayerBootstrap : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private GameObject _player;
        [SerializeField] private AudioSource _healthImpactSound;
        [SerializeField] private HealthNamespace.Health _playerHealth;
        [SerializeField] private Shields _playerShields;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Transform _projectileSpawnPosition;
        [SerializeField] private ProjectilesPool _primaryProjectilesPool;
        [SerializeField] private ProjectilesPool _secondaryProjectilesPool;
        [SerializeField] private ParticleSystem _primaryShooterFlash;
        [SerializeField] private ParticleSystem _secondaryShooterFlash;
        [SerializeField] private Shooter _primaryShooter;
        [SerializeField] private Shooter _secondaryShooter;
        [SerializeField] private CanonBaseRotator _canonBaseRotator;
        [SerializeField] private Scanner _scanner;
        [SerializeField] private Stabilizer _stabilizer;
        [SerializeField] private Nitro _nitro;
        [SerializeField] private Jumper _jumper;
        [SerializeField] private Rigidbody _playerRigidbody;
        [SerializeField] private AudioSource _playerAudioSource;
        [SerializeField] private AudioSource _nitroAudioSource;
        [SerializeField] private AudioSource _jumperAudioSource;
        [SerializeField] private AudioSource _shieldsAudioSource;
        [SerializeField] private AudioSource _primaryShooterAudioSource;
        [SerializeField] private AudioSource _secondaryShooterAudioSource;
        private void Awake()
        {
            _inputManager = Require.Component(_inputManager, nameof(_inputManager));
            _player = Require.Component(_player, nameof(_player));
            _healthImpactSound = Require.Component(_healthImpactSound, nameof(_healthImpactSound));
            _playerHealth = Require.Component(_playerHealth, nameof(_playerHealth));
            _playerShields = Require.Component(_playerShields, nameof(_playerShields));
            _gameManager = Require.Component(_gameManager, nameof(_gameManager));
            _playerController = Require.Component(_playerController, nameof(_playerController));
            _projectileSpawnPosition = Require.Component(_projectileSpawnPosition, nameof(_projectileSpawnPosition));
            _primaryProjectilesPool = Require.Component(_primaryProjectilesPool, nameof(_primaryProjectilesPool));
            _secondaryProjectilesPool = Require.Component(_secondaryProjectilesPool, nameof(_secondaryProjectilesPool));
            _primaryShooterFlash = Require.Component(_primaryShooterFlash, nameof(_primaryShooterFlash));
            _secondaryShooterFlash = Require.Component(_secondaryShooterFlash, nameof(_secondaryShooterFlash));
            _primaryShooter = Require.Component(_primaryShooter, nameof(_primaryShooter));
            _secondaryShooter = Require.Component(_secondaryShooter, nameof(_secondaryShooter));
            _canonBaseRotator = Require.Component(_canonBaseRotator, nameof(_canonBaseRotator));
            _scanner = Require.Component(_scanner, nameof(_scanner));
            _stabilizer = Require.Component(_stabilizer, nameof(_stabilizer));
            _nitro = Require.Component(_nitro, nameof(_nitro));
            _jumper = Require.Component(_jumper, nameof(_jumper));
            _playerRigidbody = Require.Component(_playerRigidbody, nameof(_playerRigidbody));
            _playerAudioSource = Require.Component(_playerAudioSource, nameof(_playerAudioSource));
            _nitroAudioSource = Require.Component(_nitroAudioSource, nameof(_nitroAudioSource));
            _jumperAudioSource = Require.Component(_jumperAudioSource, nameof(_jumperAudioSource));
            _shieldsAudioSource = Require.Component(_shieldsAudioSource, nameof(_shieldsAudioSource));
            _primaryShooterAudioSource = Require.Component(_primaryShooterAudioSource, nameof(_primaryShooterAudioSource));
            _secondaryShooterAudioSource = Require.Component(_secondaryShooterAudioSource, nameof(_secondaryShooterAudioSource));

            if (_inputManager == null || _player == null || _playerController == null || _playerRigidbody == null || _projectileSpawnPosition == null ||
            _primaryProjectilesPool == null || _secondaryProjectilesPool == null)
                throw new System.Exception("Missing critical components");

            _playerHealth?.Initialize(_healthImpactSound);
            _playerShields.Initialize(_playerHealth, _shieldsAudioSource);
            _gameManager?.Initialize(_inputManager, _player, _playerHealth);
            _playerController.Initialize(_inputManager, _playerRigidbody, _playerAudioSource);
            _canonBaseRotator.Initialize(_inputManager);
            _primaryShooter.Initialize(_inputManager, _gameManager, _canonBaseRotator, _projectileSpawnPosition, _primaryProjectilesPool,
            _primaryShooterAudioSource, _primaryShooterFlash);
            _secondaryShooter.Initialize(_inputManager, _gameManager, _canonBaseRotator, _projectileSpawnPosition, _secondaryProjectilesPool,
            _secondaryShooterAudioSource, _secondaryShooterFlash);
            _scanner.Initialize(_inputManager);
            _stabilizer.Initialize(_playerController, _playerRigidbody);
            _nitro.Initialize(_inputManager, _playerRigidbody, _nitroAudioSource);
            _jumper.Initialize(_inputManager, _playerController, _playerRigidbody, _jumperAudioSource);
        }
    }
}
