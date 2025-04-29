using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace General
{
    public class SubjectBase : MonoBehaviour, ISubject
    {
        protected virtual List<IObserver> Observers { get; } = new List<IObserver>();
        
        public virtual void AddObserver(IObserver observer)
        {
            if (observer == null)
            {
                Debug.LogError("Observer cannot be null.");
                return;
            }

            if (!Observers.Contains(observer))
            {
                Observers.Add(observer);
            }
            else
            {
                Debug.LogWarning("Observer already exists in the list.");
            }
        }

        public virtual void RemoveObserver(IObserver observer)
        {
            if (observer == null)
            {
                Debug.LogError("Observer cannot be null.");
                return;
            }

            if (Observers.Contains(observer))
            {
                Observers.Remove(observer);
            }
            else
            {
                Debug.LogWarning("Observer not found in the list.");
            }
        }

        public virtual void NotifyObservers(GameObject entity, ObserverEventType eventType)
        {
            if (entity == null)
            {
                Debug.LogError("Entity cannot be null.");
                return;
            }

            foreach (var observer in Observers)
            {
                observer.OnNotify(entity, eventType);
            }
        }
    }
}
