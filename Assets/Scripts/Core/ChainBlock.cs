using ProjectName.Core.Enums;
using UnityEngine;

namespace ProjectName.Core
{
    public class ChainBlock : MonoBehaviour
    {
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

        public void SetConnectors(Vector2Int sideDirections)
        {
            SetFirstRotation(sideDirections.x);
            SetSecondRotation(sideDirections.y);
        }
    }
}