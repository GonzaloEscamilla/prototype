using _Project.Scripts.GameServices;
using UnityEngine;
using Logger = _Project.Scripts.GameServices.Logger;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private InputProvider inputProvider;
    [SerializeField] private Logger globalLogger;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        // Here is were we initialize everything.
        // E.g.
        Services.Add<IDebug>(globalLogger);
        Services.Add<IInputProvider>(inputProvider);
    }
}
