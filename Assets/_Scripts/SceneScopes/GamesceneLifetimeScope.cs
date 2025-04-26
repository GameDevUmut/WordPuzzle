using GameCore.GridSystem;
using Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SceneScopes
{
    public class GamesceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private GridManager gridManager;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(gridManager).As<IGridService>();
        }
    }
}