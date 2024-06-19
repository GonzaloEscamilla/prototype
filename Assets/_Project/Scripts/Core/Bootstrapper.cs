using _Project.Scripts.GameServices;
using UnityEngine;
using Logger = _Project.Scripts.GameServices.Logger;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private InputProvider inputProvider;
    [SerializeField] private Logger globalLogger;
    
    private CoroutineService coroutineService;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        Services.Add<IDebug>(globalLogger);
        Services.Add<IInputProvider>(inputProvider);
        
        coroutineService = new CoroutineService(this);
        Services.Add<CoroutineService>(coroutineService);
    }
}
