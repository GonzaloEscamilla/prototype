using System;
using System.Collections.Generic;
using _Project.Scripts.Utilities;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Core
{
    public class RadarController : MonoBehaviour
    {
        [SerializeField] 
        private RadarSettings settings;

        [SerializeField] 
        private DetectorSettings detectorSettings;
        
        [SerializeField] 
        private Image chargeFillImage;

        [SerializeField] 
        private CanvasGroup chargeImageCanvasGroup;
        
        [SerializeField] 
        private Transform radarSphereGraphic;

        [ShowInInspector] 
        public bool IsCharging => _isCharging;

        [ShowInInspector] 
        public float ChargePercentage => _chargePercentage;

        private float _rawChargeLevel;

        private float RawChargeLevel
        {
            get => _rawChargeLevel;
            set
            {
                _rawChargeLevel = value;

                if (_rawChargeLevel >= settings.MaxChargeTime)
                {
                    _rawChargeLevel = settings.MaxChargeTime;
                }
            }
        }

        private Detector<Scrap> _scrapDetector;
        [SerializeField] private List<Scrap> _currentDetectedScrap;
        
        private float _chargePercentage;
        private bool _isCharging;
        private bool _isMaxCharge;

        private void Awake()
        {
            var material = radarSphereGraphic.GetComponent<MeshRenderer>().material;
            material.DOFade(0, 0);
            chargeImageCanvasGroup.DOFade(0, 0);
            _scrapDetector = new Detector<Scrap>(transform, detectorSettings);
            _currentDetectedScrap = new List<Scrap>();
        }

        public void BeginCharge()
        {
            RawChargeLevel = 0;
            _isCharging = true;
            chargeImageCanvasGroup.DOFade(1, 0.35f).SetEase(Ease.OutCubic);
        }

        public void Charge()
        {
            if (_isMaxCharge)
            {
                return;
            }

            RawChargeLevel += Time.deltaTime;
            _chargePercentage = RawChargeLevel / settings.MaxChargeTime;
            chargeFillImage.fillAmount = _chargePercentage;
            if (Math.Abs(settings.MaxChargeTime - RawChargeLevel) < 0.05f)
            {
                _isMaxCharge = true;
            }
        }

        public void EndCharge()
        {
            ShowAnimatedSphere();
            ExecuteDetection();
            
            _isCharging = false;
            RawChargeLevel = 0;
            _chargePercentage = 0;
            _isMaxCharge = false;
            chargeImageCanvasGroup.DOFade(0, 0.35f).SetEase(Ease.InCubic);
        }

        private void ExecuteDetection()
        {
            var chargeRangeData = settings.GetChargeRange(_chargePercentage);

            _currentDetectedScrap = _scrapDetector.Detect(chargeRangeData.DetectionRange);

            foreach (var scrap in _currentDetectedScrap)
            {
                scrap.Detect();
            }
        }

        private void ShowAnimatedSphere()
        {
            var chargeRangeData = settings.GetChargeRange(_chargePercentage);

            var material = radarSphereGraphic.GetComponent<MeshRenderer>().material;

            material.color = chargeRangeData.SphereColor;
            material.DOFade(1, 0).OnComplete(FadeColor);

            radarSphereGraphic.localScale = Vector3.zero;
            radarSphereGraphic.DOScale(chargeRangeData.DetectionRange*2, settings.SphereExpansionSpeed)
                .SetSpeedBased()
                .SetEase(settings.SphereEase);

            void FadeColor()
            {
                material.DOFade(0, settings.FadeColorSpeed).SetEase(settings.FadeColorEase);
            }
        }
    }
}