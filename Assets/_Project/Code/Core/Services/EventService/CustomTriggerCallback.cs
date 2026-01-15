using System;
using System.Collections;
using _Project.Code.Util;
using UnityEngine;

namespace _Project.Code.Core.Services.EventService
{
    [Serializable]
    public abstract class CustomTriggerCallback
    {
        protected MonoBehaviour Owner;
        protected ICoroutineRunner CoroutineRunner;

        public void Init(MonoBehaviour owner, ICoroutineRunner runner)
        {
            Owner = owner;
            CoroutineRunner = runner;
        }

        public abstract void OnTriggered();
    }
    
    [Serializable]
    public class SingleUseTrigger : CustomTriggerCallback
    {
        public override void OnTriggered() => 
            Owner.gameObject.SetActive(false);
    }
    
    [Serializable]
    public class ReloadableTrigger : CustomTriggerCallback
    {
        [Range(0, 200)] public float Cooldown;
        
        private Coroutine _reloadCoroutine = null;
        
        public override void OnTriggered()
        {
            if (_reloadCoroutine != null) {
                CoroutineRunner.StopCoroutine(_reloadCoroutine);
                _reloadCoroutine = null;
            }
            
            Owner.gameObject.SetActive(false);
            _reloadCoroutine = CoroutineRunner.StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(Cooldown);
            Owner.gameObject.SetActive(true);
        }
    }
}