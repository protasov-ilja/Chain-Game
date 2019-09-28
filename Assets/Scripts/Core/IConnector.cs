using UnityEngine;

namespace ProjectName.Core
{
    public interface IConnector
    {
        Transform Transform { get; }

        void Connect(ChainBlock block);

        void Disconnect(ChainBlock block);
    }
}