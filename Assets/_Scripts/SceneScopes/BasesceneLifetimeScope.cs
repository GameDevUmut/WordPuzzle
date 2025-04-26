using Interfaces;
using Managers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SceneScopes
{
    public class BasesceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private SceneLoadManager sceneLoadManager;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(sceneLoadManager).As<ISceneLoadService>();
        }
    }
}
