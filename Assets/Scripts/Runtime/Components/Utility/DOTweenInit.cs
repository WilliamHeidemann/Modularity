using DG.Tweening;
using UnityEngine;

namespace Runtime.Components.Utility
{
    public class DOTweenInit : MonoBehaviour
    {
        private void Awake()
        {
            DOTween.Init();
        }
    }
}
