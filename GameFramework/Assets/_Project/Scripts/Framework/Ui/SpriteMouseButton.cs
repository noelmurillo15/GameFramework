/*
 * SpriteMouseClick - Detects mouse click on sprites and Invokes a Custom Event
 * Created by : Allan N. Murillo
 * Last Edited : 3/4/2021
 */

using UnityEngine;
using UnityEngine.EventSystems;

namespace ANM.Ui
{
    public class SpriteMouseClick : MonoBehaviour, IPointerClickHandler
    {
        public delegate void LeftClickEventHandler(string slotName);

        public static event LeftClickEventHandler OnLeftClickEvent;
        private LeftClickEventHandler _eventHandler;


        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging) return;

            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    OnLeftClickEvent?.Invoke(eventData.pointerCurrentRaycast.gameObject.name);
                    break;
            }
        }

        public void SetCustomEventHandler(LeftClickEventHandler eventHandler)
        {
            if (_eventHandler != null)
                Debug.LogWarning("[SpriteMouseClick]: event handler may have not been unregistered");
            _eventHandler = eventHandler;
            OnLeftClickEvent += _eventHandler;
        }

        public void Unregister()
        {
            if (_eventHandler == null) return;
            OnLeftClickEvent -= _eventHandler;
            _eventHandler = null;
        }

        private void OnDestroy() => Unregister();
    }
}