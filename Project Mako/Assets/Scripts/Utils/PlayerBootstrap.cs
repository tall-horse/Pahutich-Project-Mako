
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
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Shooter _primaryShooter;
        [SerializeField] private Shooter _secondaryShooter;
        [SerializeField] private CanonBaseRotator _canonBaseRotator;
        [SerializeField] private Scanner _scanner;
        [SerializeField] private Nitro _nitro;
        [SerializeField] private Jumper _jumper;
        private void Awake()
        {
            _inputManager = Require.Component(_inputManager, nameof(_inputManager));
            _gameManager = Require.Component(_gameManager, nameof(_gameManager));
            _playerController = Require.Component(_playerController, nameof(_playerController));
            _primaryShooter = Require.Component(_primaryShooter, nameof(_primaryShooter));
            _secondaryShooter = Require.Component(_secondaryShooter, nameof(_secondaryShooter));
            _canonBaseRotator = Require.Component(_canonBaseRotator, nameof(_canonBaseRotator));
            _scanner = Require.Component(_scanner, nameof(_scanner));
            _nitro = Require.Component(_nitro, nameof(_nitro));
            _jumper = Require.Component(_jumper, nameof(_jumper));

            if (_inputManager == null || _playerController == null)
                throw new System.Exception("Missing critical components");

            _gameManager?.Initialize(_inputManager);
            _playerController.Initialize(_inputManager);
            _primaryShooter.Initialize(_inputManager);
            _secondaryShooter.Initialize(_inputManager);
            _canonBaseRotator.Initialize(_inputManager);
            _scanner.Initialize(_inputManager);
            _nitro.Initialize(_inputManager);
            _jumper.Initialize(_inputManager);
        }
    }
}
