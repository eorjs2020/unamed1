
using System;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Teleport event, function, enum

public class PlayerControllerService : MonoBehaviour, IPlayerControllerService
{
    private IGameManager _gameManager;
    private IGlobalEventService _globalEvent;

    [SerializeField] private PlayerCharacter _playerCharacter;
    [SerializeField] private Vector3 _startPosition = new Vector3(0.0f, 4.0f, 0.0f);
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    public PlayerCharacter PlayerCharacter
    {
        get
        {
            return _playerCharacter;
        }
    }
    
    // FadeIn/Out Temporary
    [SerializeField] private Image _fadeImage;

    public void TargetPosition(Vector3 position)
    {
        _targetPosition = position;
    }

    public void Init(IGameManager gameManager)
    {
        //_virtualCamera = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>();
        _gameManager = gameManager;
        _globalEvent = _gameManager.GetService<IGlobalEventService>();
        _globalEvent.InputUpdatedGlobal += MoveToTargetPosition;
        _globalEvent.PlayerTeleportGlobal += TeleportToTargetPosition;
        _globalEvent.MapUpdatedGlobal += UpdateHungerGage;
        _targetPosition = _startPosition;
        _playerCharacter = Resources.Load<PlayerCharacter>("prefabs/Pawn/PlayerCharacter");
        _playerCharacter = Instantiate(_playerCharacter, _startPosition, Quaternion.identity);
        _playerCharacter.Init(_gameManager);
        _mainCamera = Camera.main;
        _virtualCamera.Follow = _playerCharacter.transform;
    }

    private void OnDestroy()
    {
        _globalEvent.InputUpdatedGlobal -= MoveToTargetPosition;
        _globalEvent.PlayerTeleportGlobal -= TeleportToTargetPosition;
        _globalEvent.MapUpdatedGlobal -= UpdateHungerGage;
    }

    private void Update()
    {
        if (_gameManager.State != GameStateType.playing)
            return;
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 currentPosition = _playerCharacter.transform.position;
        if ((Vector2)currentPosition == (Vector2)_targetPosition)
        {
            _playerCharacter.IsMoving = false;
            return;
        }
        
        // Player Direction
        Vector3 moveDirection = (_targetPosition - currentPosition).normalized;
        _playerCharacter.IsLeft = (moveDirection.x < 0);
        _playerCharacter.IsMoving = true;

        _playerCharacter.transform.position = Vector2.MoveTowards(currentPosition, 
            _targetPosition, _playerCharacter.MoveSpeed * Time.deltaTime);
    }

    private void MoveToTargetPosition(IGameManager sender, InputEventArgs inputEvent)
    {
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(inputEvent.TouchPoint);
        _targetPosition = worldPosition;
        //TeleportToTargetPosition();
    }

    private void UpdateHungerGage(IGameManager sender, MapUpadateArg mapUpadateArg)
    {
        if (mapUpadateArg.MapNumber != 0) // Lobby
            _playerCharacter.UpdateHungerValue();
    }
    
    //private void TeleportToTargetPosition(IGameManager sender, MapEventArgs mapEvent)
    private void TeleportToTargetPosition(IGameManager sender, Vector3 position)
    {
        _playerCharacter.transform.position = position;
        _targetPosition = position;
        //StartCoroutine(TeleportCharacterAndFade(position));
    }

        
    // FadeIn/Out Temporary
    IEnumerator TeleportCharacterAndFade(Vector3 position)
    {
        while (_fadeImage.color.a < 1.0f)
        {
            float alphaValue = _fadeImage.color.a + (Time.deltaTime*2);
            _fadeImage.color = new Color(0, 0, 0, alphaValue);
            yield return null;
        }
        
        // TODO Get Event from MapEvent and use it instead of Temporary position
        Vector3 temporayPosition = new Vector3(0.0f, -4.0f, 0.0f);
        _playerCharacter.transform.position = temporayPosition;

        while (_fadeImage.color.a > 0.0f)
        {
            float alphaValue = _fadeImage.color.a - (Time.deltaTime*2);
            _fadeImage.color = new Color(0, 0, 0, alphaValue);
            yield return null;
        }

        _playerCharacter.transform.position = position;
    }
}
