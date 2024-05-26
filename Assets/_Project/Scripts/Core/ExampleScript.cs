using _Project.Scripts.GameServices;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    private IDebug _log;
    private IInputProvider _inputProvider;
    
    private void Awake()
    {
        Services.WaitFor<IDebug>(SetLogger);
    }

    private void Start()
    {
        var debugService = Services.Get<IDebug>();
        debugService.Log("Alternatively, use it directly in Start if needed since we know it's already initialized.");

        // Or handle it here since it's guaranteed to be initialized.
        _inputProvider = Services.Get<IInputProvider>();
    }

    private void SetLogger(IDebug logger)
    {
        _log = logger;
        _log.Log("Now we can use the service.");
    }
}

