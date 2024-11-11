using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Components.Utility
{
    public class HoverSoundEffect : MonoBehaviour, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData) => SoundFX.Instance.PlayUIHover();
    }
}
