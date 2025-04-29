using UnityEngine;

namespace Interfaces
{
    public interface ISubject
    {
        void AddObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers(GameObject entity, ObserverEventType eventType);
    }
}
