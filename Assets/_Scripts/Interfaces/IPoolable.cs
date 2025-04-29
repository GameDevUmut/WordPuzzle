using System;
using Addler.Runtime.Core.Pooling;

namespace Interfaces
{
    public interface IPoolable
    {
        PooledObject PooledObject { get; set; }
        Action OnReturnToPool { get; set; }
    }
}
