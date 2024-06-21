using DG.Tweening;
using UnityEngine;

[System.Serializable]
public struct ChargeRangeData
{
    [Range(0, 1)] 
    public float ChargeThreshold;
    public float DetectionRange;
    public Color SphereColor;
}

[CreateAssetMenu(menuName = "Prototype/Create Radar Settings", fileName = "RadarSettings", order = 0)]
public class RadarSettings : ScriptableObject
{
    [SerializeField] 
    private float maxChargeTime;
    
    [SerializeField] 
    public LayerMask scrapDetectionLayer;
    
    [SerializeField] 
    private ChargeRangeData[] charges;

    [SerializeField] 
    private Ease sphereEase;

    [SerializeField] 
    private float sphereExpansionSpeed;
    
    [SerializeField] 
    private float fadeColorSpeed;

    [SerializeField] 
    private Ease fadeColorEase;
    
    public float MaxChargeTime => maxChargeTime;
    public float SphereExpansionSpeed => sphereExpansionSpeed;
    public Ease SphereEase => sphereEase;
    
    public ChargeRangeData[] Charges => charges;
    public float FadeColorSpeed => fadeColorSpeed;
    public Ease FadeColorEase => fadeColorEase;
    public LayerMask ScrapDetectionLayer => scrapDetectionLayer;

    public ChargeRangeData GetChargeRange(float chargeAmount)
    {
        if (chargeAmount <= 0)
        {
            Debug.LogWarning($"There is not a charge data with {chargeAmount} charge amount.");
            return default;
        }
        
        foreach (var chargeData in charges)
        {
            if (chargeAmount <= chargeData.ChargeThreshold)
            {
                return chargeData;
            }
        }

        return charges.Length > 0 ? charges[^1] : default;
    }
}