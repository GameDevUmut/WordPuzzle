using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace General
{
    public class ObserverBase : MonoBehaviour, IObserver
    {
        protected List<ISubject> Subjects { get; set; }

        protected ISubject _currentSubject;

        public virtual void ObserveSubject(ISubject subject)
        {
            _currentSubject = subject;
            Subjects.Add(_currentSubject);

            if (_currentSubject != null)
                _currentSubject.AddObserver(this);
        }

        protected void OnDestroy()
        {
            if (Subjects != null)
                foreach (var subject in Subjects)
                {
                    if (subject != null)
                        subject.RemoveObserver(this);
                }
        }

        public virtual void OnNotify(GameObject entity, ObserverEventType eventType)
        {
        }
    }
}
