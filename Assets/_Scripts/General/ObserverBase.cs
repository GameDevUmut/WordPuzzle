using System;
using Interfaces;
using UnityEngine;

namespace General
{
    public class ObserverBase : MonoBehaviour, IObserver
    {
        protected ISubject Subject { get; set; }

        public virtual void ObserveSubject(ISubject subject)
        {
            Subject = subject;

            if (Subject != null)
                Subject.AddObserver(this);
        }

        protected void OnDestroy()
        {
            if (Subject != null)
                Subject.RemoveObserver(this);
        }

        public virtual void OnNotify(GameObject entity, ObserverEventType eventType)
        {
        }
    }
}
