using System;
using ProjectName.Core.Enums;
using ProjectName.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectName.Core
{
    public enum ConnectionType
    {
        ToPanel,
        ToField
    };
    
    public class ChainBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private bool _isInitial;
        [SerializeField] private Vector2 _nativeSize;
        
        [Header("ForChangeConnectorsState")]
        [SerializeField] private Vector2Int _connectorsStates;

        private void OnValidate()
        {
            SetConnectorsState(_connectorsStates);
        }

        public FieldManager FieldManager { get; set; }
        public BlocksPanel BlocksPanel {get; set; }

        public bool IsInitial
        {
            set => _isInitial = value;
            get => _isInitial;
        }
        
        private RectTransform _draggingPlane;

        private IConnector _connector;

        public RectTransform Rect => (RectTransform)transform;

        public IConnector Connector
        {
            get => _connector;
            set
            {
                _connector?.Disconnect(this);
                _connector = value;
                transform.SetParent(_connector.Transform);
            }
        }
        public ConnectionType ConnectionType { get; set; }

        private void Awake()
        {
            _draggingPlane = FindObjectOfType<Canvas>().transform as RectTransform;
        }
        
        private void Disconnect()
        {
            transform.SetParent(_draggingPlane);
            
            if (ConnectionType == ConnectionType.ToPanel)
            {
                Rect.sizeDelta = new Vector2(_nativeSize.x,_nativeSize.y);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsInitial) return;
            
            Disconnect();
            // We have clicked something that can be dragged.
            transform.SetAsLastSibling();
            SetDraggedPosition(eventData);
        }
        
        private void SetDraggedPosition(PointerEventData data)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_draggingPlane, data.position, data.pressEventCamera, out var globalMousePos))
            {
                Rect.position = globalMousePos;
            }
        }
        
        public void OnDrag(PointerEventData data)
        { 
            if (IsInitial) return;
            SetDraggedPosition(data);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsInitial) return;
            
            if (ConnectionType == ConnectionType.ToPanel)
            {
                if (FieldManager.ConnectToBlock(this))
                {
                    _connector.Connect(this);
                }
                else
                {
                    _connector.Connect(this);
                }
            }
            else if (ConnectionType == ConnectionType.ToField)
            {
                if (Rect.rect.Overlaps((BlocksPanel.Transform as RectTransform).rect))
                {
                    BlocksPanel.Connect(this);
                }
                else
                {
                    _connector.Connect(this);
                }
            }
        }
        
        #region Connectors options
        
        [SerializeField] private GameObject _firstConncetor;
        [SerializeField] private GameObject _secondConnector;

        private int _firstDirection = 0;
        private int _secondDirection = 0;
        
        private DirectionType _direction;

        public DirectionType Direction
        {
            get => _direction;
            set
            {
                transform.rotation = value == DirectionType.Horizontal ? 
                    Quaternion.identity : 
                    Quaternion.Euler(0, 0, 90);
                
                _direction = value;
            }
        }
        
        public int FirstDirection => _firstDirection;
        public int SecondDirection => _secondDirection;

        private void SetFirstRotation(int direction)
        {
            _firstDirection = direction;
            _firstConncetor.transform.localRotation = Quaternion.Euler(0, 0, 90 * direction);
        }

        private void SetSecondRotation(int direction)
        {
            _secondDirection = direction;
            _secondConnector.transform.localRotation = Quaternion.Euler(0, 0, 90 * direction);
        }

        public void SetConnectorsState(Vector2Int sideDirections)
        {
            Debug.Log(sideDirections);
            SetFirstRotation(sideDirections.x);
            SetSecondRotation(sideDirections.y);
        }
        #endregion
    }
}