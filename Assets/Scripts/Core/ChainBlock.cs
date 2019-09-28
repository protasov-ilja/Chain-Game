using System;
using ProjectName.Core.Enums;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectName.Core
{
    public class ChainBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public FieldManager DEBUGFiled;
        public DirectionType DEBUG_DIRECTION;

        public RectTransform Rect => (RectTransform)transform;
        
        private RectTransform m_DraggingPlane;

        private void Awake()
        {
            Direction = DEBUG_DIRECTION;
            m_DraggingPlane = FindObjectOfType<Canvas>().transform as RectTransform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // We have clicked something that can be dragged.
            // What we want to do is create an icon for this.
            
            transform.SetAsLastSibling();

            SetDraggedPosition(eventData);
        }
        
        private void SetDraggedPosition(PointerEventData data)
        {
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
            {
                Rect.position = globalMousePos;
            }
        }
        
        public void OnDrag(PointerEventData data)
        {
            SetDraggedPosition(data);
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("this");
            DEBUGFiled.ConnectToBlock(this);
        }
        
        #region Connection options
        
        [SerializeField] private GameObject _firstConncetor;
        [SerializeField] private GameObject _secondConnector;

        private int _firstDirection = 0;
        private int _secondDirection = 0;
        
        public DirectionType Direction { get; set; }
        public int FirstDirection => _firstDirection;
        public int SecondDirection => _secondDirection;

        private void SetFirstRotation(int direction)
        {
            _firstDirection = direction;
            _firstConncetor.transform.rotation = Quaternion.Euler(90 * direction, 0, 0);
        }

        private void SetSecondRotation(int direction)
        {
            _secondDirection = direction;
            _secondConnector.transform.rotation = Quaternion.Euler(90 * direction, 0, 0);
        }

        public void SetConnectorsState(Vector2Int sideDirections)
        {
            SetFirstRotation(sideDirections.x);
            SetSecondRotation(sideDirections.y);
        }
        #endregion
    }
}