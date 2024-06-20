using System;
using _Project.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public enum ScrapState
{
    Hidden,
    Revealed,
    Grabbed
}

public enum ScrapType
{
    Normal,
    Hidden
}

public enum ScrapRarity
{
    None,
    Common,      // 20 to 100 gold
    Uncommon,    // 100 to 500 gold
    Rare,        // 500 to 2000 gold
    Legendary    // 2000+ gold
}

public struct ScrapData
{
    public readonly ScrapRarity Rarity;

    public ScrapData(ScrapRarity rarity)
    {
        Rarity = rarity;
    }
}

public class Scrap : BaseInteractable
{
    [SerializeField] 
    private GameObject graphicsRoot;
    
    [SerializeField] 
    private ScrapRarity rarity;

    [SerializeField] 
    private ScrapType scrapType;

    [SerializeField] 
    private float secondsToFill;
    
    [ShowInInspector]
    public float FillPercentage => _fillPercentage;
    private float _fillPercentage;    
    
    [SerializeField] 
    private CanvasGroup canvasGroup;
    
    [SerializeField] 
    private Image fillImage;
    
    private float _currentFillAmount;
    
    private float CurrentFillAmount
    {
        get => _currentFillAmount;
        set
        {
            _currentFillAmount = value;
                
            if (_currentFillAmount <= 0)
            {
                _currentFillAmount = 0;
            }

            if (_currentFillAmount > secondsToFill)
            {
                _currentFillAmount = secondsToFill;
            }
        }
    }
    
    public bool CanBeGrabbed => _currentState is ScrapState.Revealed;
    
    private ScrapState _currentState;
    private bool _grabActionCompleted;
    
    private void Awake()
    {
        _currentState = scrapType switch
        {
            ScrapType.Normal => ScrapState.Revealed,
            ScrapType.Hidden => ScrapState.Hidden,
            _ => throw new ArgumentOutOfRangeException()
        };

        canvasGroup.alpha = 0;

        switch (scrapType)
        {
            case ScrapType.Hidden:
                _currentState = ScrapState.Hidden;
                graphicsRoot.SetActive(false);
                break;
            case ScrapType.Normal:
                _currentState = ScrapState.Revealed;
                graphicsRoot.SetActive(true);
                break;
        }
    }

    public override bool Interact(GameObject interactionSource, out object interactionResultData)
    {
        interactionResultData = null;
        
        if (!CanBeGrabbed || _grabActionCompleted)
        {
            return false;
        }
        canvasGroup.alpha = 1;

        CurrentFillAmount += Time.deltaTime;
        _fillPercentage = CurrentFillAmount / secondsToFill;
        fillImage.fillAmount = _fillPercentage;
            
        if (Math.Abs(CurrentFillAmount - secondsToFill) < 0.05f)
        {
            _grabActionCompleted = true;
           
            interactionResultData = new ScrapData(rarity);
            gameObject.SetActive(false);
            _currentState = ScrapState.Grabbed;
           
            return true;
        }
        
        return false;
    }

    [ContextMenu("Detect")]
    public void Detect()
    {
        switch (scrapType)
        {
            case ScrapType.Normal:
                HandleDetectForNormal();
                break;
            case ScrapType.Hidden:
                HandleDetectForHidden();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void HandleDetectForHidden()
    {
        if (_currentState == ScrapState.Grabbed)
        {
            return;
        }

        _currentState = ScrapState.Revealed;
        graphicsRoot.SetActive(true);
    }

    private void HandleDetectForNormal()
    {
        if (_currentState == ScrapState.Grabbed)
        {
            return;
        }

        _currentState = ScrapState.Revealed;
        graphicsRoot.SetActive(true);
    }
}
