using ProjectName.Core;
using UnityEngine;

namespace ProjectName.Config
{
    [CreateAssetMenu(fileName = "Config", menuName = "ChainGame/Config")]
    public class Config : ScriptableObject
    {
        [SerializeField] private WayPoint _wayPointPrefab;
        [SerializeField] private ChainBlock _chainBlock;

        public WayPoint WayPointPrefab => _wayPointPrefab;
        public ChainBlock ChainBlockPrefab => _chainBlock;
    }
}