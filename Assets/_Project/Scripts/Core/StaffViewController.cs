using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class StaffViewController : MonoBehaviour
    {
        [SerializeField] private Transform initialRotationPivot;
        [SerializeField] private Transform middleRotationPivot;
        [SerializeField] private Transform endRotationPivot;
        [SerializeField] private Ease moveEase;
        
        
        public void AttackAnimation(float timeAmount)
        {
            float step = timeAmount / 4;
            Sequence mySequence = DOTween.Sequence();

            mySequence.Append(transform.DOLocalMove(middleRotationPivot.localPosition, step));
            mySequence.Insert(0, transform.DORotateQuaternion(middleRotationPivot.rotation,step));
            
            mySequence.Append(transform.DOLocalMove(endRotationPivot.localPosition, step));
            mySequence.Insert(step, transform.DORotateQuaternion(endRotationPivot.rotation,step));
            
            mySequence.Append(transform.DOLocalMove(middleRotationPivot.localPosition, step));
            mySequence.Insert((step)*2, transform.DORotateQuaternion(middleRotationPivot.rotation,step));
            
            mySequence.Append(transform.DOLocalMove(initialRotationPivot.localPosition, step));
            mySequence.Insert((step)*3, transform.DORotateQuaternion(initialRotationPivot.rotation,step));
            
            mySequence.SetEase(moveEase);
        }
    }
}