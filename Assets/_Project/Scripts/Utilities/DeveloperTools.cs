using UnityEngine;

namespace _Project.Scripts.Utilities
{
    public class DeveloperTools: MonoBehaviour
    {
        /*
        [SerializeField] private CanvasGroup pausePanel;
        [SerializeField] private List<Renderer> renderersToTurnOff;
        [SerializeField] private List<CanvasGroup> canvasGroupsToTurnOff;
        private bool _isGamePaused;
    
        private IInputProvider _controls;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(3f);
            _controls = ServiceLocator.Instance.GetService<IInputProvider>();
            
            _controls.OnSpaceBarPerformed += HandlePause;
            _controls.OnTabPerformed += HandleReset;
        }

        private void OnDisable()
        {
            _controls.OnSpaceBarPerformed -= HandlePause;
            _controls.OnTabPerformed -= HandleReset;
        }

        private void HandleReset()
        {
            ServiceLocator.Instance.GetService<ISceneLoader>().LoadScene(0);
        }
        
        private void HandlePause()
        {
            _isGamePaused = !_isGamePaused;

            foreach (var rendererToTurn in renderersToTurnOff)
                rendererToTurn.enabled = !_isGamePaused;

            foreach (var canvasGroup in canvasGroupsToTurnOff)
                canvasGroup.alpha = _isGamePaused ? 1 : 0;
            
            Time.timeScale = _isGamePaused ? 1 : 0;
            pausePanel.alpha = _isGamePaused ? 0 : 1;;
        }
        */
    }
    
}