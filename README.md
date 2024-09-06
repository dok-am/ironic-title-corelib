# < Ironic Title > CoreLib 
> Still WIP!!!

Unity Framework for quick and simple game architecture building

## Basic idea
This is a universal and simple framework for quick setup of small-scale games architecture. It provides:
- Entry Point
- Contexs as a Service Providers
- Transitions between scenes
- Simple scene bindings
- UI structure base
- UI universal classes
- FSM base with transitional states
- Some useful interfaces
- Universal services base like for static data storage
- Anything I'll consider useful and universal

So, architecturally there are few layers:
- Models layer - *Services*
- Data layer - *Providers*, *Services*
- Binding layer - *SceneBinders*, *Managers*, *SceneUI*
- View layer - whatever happens in MonoBehaviours

## How to use
Clone this repository as a submodule to your project's Assets folder.

In a project you should have at least two scenes:
- Bootstrapping scene (0) with an object with component inherited from `ApplicationContext`
- Main scene (1) with an object with component inherited from `SceneContext`
  
`ApplicationEntryPoint` loading scene 0 and begin initialization progress with contexts.
Inside of contexts you can implemented services you need on this level (application or scene level)

```c#
public class GameplaySceneContext : SceneContext
{
    [SerializeField] private GameObject _playerPrefab;

    private GameObject _player;
    
    protected override void InitializeServices()
    {
        AddService<InputService>();
        AddService<ItemsStorageService>();
        AddService<PlayerDataService>();
    }

    protected override void InitializeScene()
    {
        _player = Instantiate(_playerPrefab);
        _player.GetComponent<PlayerMoveController>().Initialize(GetService<InputService>());
    }
}
```

Every `SceneContext` has a list of `SceneBinderBase`, which you can implement to bind your services to GameObjects.
`SceneBinderBase` has a set of `IManager`s, which can be used to hadle bindings and do the MonoBeh managing stuff. Also, you can use `IUpdatable` and `IFixedUpdatable` interfaces in Managers, to get Unity's ticking

```c#
public class SceneBinder : SceneBinderBase
{
    [Header("Prefabs")]
    [SerializeField] private PlayerController _playerPrefab;

    private PlayerManager _playerManager;

    public override void Bind(IContext context)
    {
         _playerManager = new PlayerManager(_playerPrefab,
            context.GetService<PlayerService>(),
            context.GetService<PlayerInputService>(),
            _spawnPointsManager);

        AddManager(_playerManager);
    }

    public override void Unbind(IContext context)
    {
        //If your bindings happens right in the Binder, you can unbind them here
        base.Unbind(context);
    }
}
```
```c#
public class PlayerManager : IManager, IUpdatable
{
    public event Action<IPlayerInstance> OnPlayerSpawned;


    private PlayerController _playerPrefab;
    private PlayerService _playerService;
    private PlayerInputService _playerInputService;
    private PlayerController _playerInstance;
    private SpawnPointsManager _spawnPointsManager;


    public PlayerManager(PlayerController playerPrefab, 
        PlayerService playerService, 
        PlayerInputService inputService, 
        SpawnPointsManager spawnPointsManager)
    {
        _playerPrefab = playerPrefab;
        _playerService = playerService;
        _playerInputService = inputService;
        _spawnPointsManager = spawnPointsManager;

        //Binding to service
        _playerService.OnPlayerDied += Die;
        _playerService.RequestRespawnPlayer += RespawnPlayer;
        _playerService.RequestPlayerInstance += PlayerInstance;
    }

    public void Unbind()
    {
        //Unbinding from service
        _playerService.OnPlayerDied -= Die;
        _playerService.RequestRespawnPlayer -= RespawnPlayer;
        _playerService.RequestPlayerInstance -= PlayerInstance;
    }

    public IPlayerInstance PlayerInstance() => _playerInstance;

    public void RespawnPlayer(ICharacterData characterData)
    {
        if (_playerInstance != null)
            DestroyPlayer();

        _playerInstance = GameObject.Instantiate(_playerPrefab, 
            _spawnPointsManager.PlayerSpawnPoint.position,
            Quaternion.identity)
            .GetComponent<PlayerController>();

        if (_playerInstance == null)
            throw new Exception("[PLAYER] Player's prefab is wrong!");

        _playerInstance.Initialize(_playerService, characterData);
        OnPlayerSpawned?.Invoke(_playerInstance);
    }      

    public void Die()
    {
        DestroyPlayer();
    }

    public void Update(float dt)
    {
        if (_playerInstance == null)
            return;

        _playerInstance.UpdateInput(_playerInputService.MoveValue, _playerInputService.RotateValue);
    }


    private void DestroyPlayer()
    {
        if (_playerInstance != null)
            GameObject.Destroy(_playerInstance.gameObject);

        _playerInstance = null;
    }
}
```

## Known issues
- There SHOULD be a problem in scene changing with services destruction

## TBD
- Nested scenes for implementing levels
- Update/remake UI system
- EventBus or something like that
- Saving system
- Write a proper documentation, I guess
- Or maybe make it a proper Unity package, idk
- You are really so interested that you are reading this?
- Give me some feedback than, what do you think about all of this? Thank you!
