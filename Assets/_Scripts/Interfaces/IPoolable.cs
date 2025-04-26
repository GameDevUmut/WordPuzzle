using System;
using Addler.Runtime.Core.Pooling;

namespace Interfaces
{
    public interface IPoolable
    {
        public PooledObject PooledObject { get; set; }
        public Action OnReturnToPool { get; set; }
    }
}
