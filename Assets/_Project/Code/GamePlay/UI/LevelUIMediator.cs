using System;
using _Project.Code.Core.Services.EventService;
using _Project.Code.GamePlay.UI.ToolTip;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Code.GamePlay.UI
{
    public class LevelUIMediator : IInitializable, IDisposable
    {
        private readonly ToolTipView _toolTipView;
        private readonly IEventBus _eventBus;
        
        private CustomEvent<OnInteractableFound> _onInteractableFound;
        private CustomEvent<OnInteractableLost> _onInteractableLost;

        public LevelUIMediator(ToolTipView toolTipView, IEventBus eventBus)
        {
            _toolTipView = toolTipView;
            _eventBus = eventBus;
        }
        
        public void Initialize()
        {
            Debug.Log("Initialize LevelUIMediator");
            _onInteractableFound = new CustomEvent<OnInteractableFound>(PreviewFoundInteractable);
            _onInteractableLost = new CustomEvent<OnInteractableLost>(HidePreview);
            
            _eventBus.Subscribe(_onInteractableFound);
            _eventBus.Subscribe(_onInteractableLost);
        }

        private void PreviewFoundInteractable(OnInteractableFound signal) => 
            _toolTipView.ShowFor(signal.Interactable.Info);  
        private void HidePreview(OnInteractableLost signal) => 
            _toolTipView.Hide();

        public void Dispose()
        {
            if (_eventBus != null)
            {
                _eventBus.Unsubscribe(_onInteractableFound);
                _eventBus.Unsubscribe(_onInteractableLost);
            }
        }
    }
}