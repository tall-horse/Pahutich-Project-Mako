
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
            if (_gameManager == null) Debug.LogError("GameManager is null");
            _gameManager?.Initialize(_inputManager);
            if (_playerController == null) Debug.LogError("PlayerController is null");
            _playerController?.Initialize(_inputManager);
            if (_primaryShooter == null) Debug.LogError("Primary Shooter is null");
            _primaryShooter?.Initialize(_inputManager);
            if (_secondaryShooter == null) Debug.LogError("Secondary Shooter is null");
            _secondaryShooter?.Initialize(_inputManager);
            if (_canonBaseRotator == null) Debug.LogError("CanonBaseRotator is null");
            _canonBaseRotator?.Initialize(_inputManager);
            if (_scanner == null) Debug.LogError("Scanner is null");
            _scanner?.Initialize(_inputManager);
            if (_nitro == null) Debug.LogError("Nitro is null");
            _nitro?.Initialize(_inputManager);
            if (_jumper == null) Debug.LogError("Jumper is null");
            _jumper?.Initialize(_inputManager);
        }
    }
}
