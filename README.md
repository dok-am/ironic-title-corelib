# < Ironic Title > CoreLib 
> Still WIP!!!

Unity Framework for quick and simple bootstrapping

## Basic idea
This is a universal and simple framework for quick setup of small-scale games architecture. It provides:
- Entry Point
- Services holder
- UI structure base
- FSM base with transitional states
- Some useful interfaces
- Universal services base like for static data storage
- Anything I'll consider useful and universal

## How to use
You should have at least two scenes:
- Bootstrapping scene (0) with an object with component inherited from `ApplicationBootstrap`
- Main scene (1) with an object with component inherited from `SceneBootstrap`
  
`ApplicationEntryPoint` loading scene 0 and begin initialization progress with boostraps.
Inside of bootstraps you can implemented services you need on this level (application or scene level)

```c#
public class GameplaySceneBootstrap : SceneBootstrap
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

## TBD
- Nested scenes for implementing levels
- Improve async scene loading for proper loading screens
- Remake UI system
- EventBus or something like that
- Saving system
- Write a proper documentation, I guess
