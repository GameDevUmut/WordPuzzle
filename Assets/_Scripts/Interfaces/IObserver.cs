using UnityEngine;

namespace Interfaces
{
    public interface IObserver
    {
        void OnNotify(GameObject entity, ObserverEventType eventType);
    }
    
    public enum ObserverEventType { GridCreated }
}
