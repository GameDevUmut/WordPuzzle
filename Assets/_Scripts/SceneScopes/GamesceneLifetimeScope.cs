using GameCore;
using GameCore.GridSystem;
using GameCore.Trie;
using Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SceneScopes
{
    public class GamesceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private GridManager gridManager;
        [SerializeField] private TrieManager trieManager;
        [SerializeField] private GameManager gameManager;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(gridManager).As<IGridService>();
            builder.RegisterInstance(trieManager).As<ITrieService>();
            builder.RegisterInstance(gameManager).As<IGameService>();
        }
    }
}