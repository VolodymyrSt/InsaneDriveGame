using System;
using System.Threading.Tasks;
using _Project.Code.Core.Infrastructure.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Project.Code.Core.Services.SceneLoadService
{
    public class SceneLoader : ISceneLoader
    {
        private readonly LoadingCurtain _loadingCurtain;
        
        public SceneLoader(LoadingCurtain  loadingCurtain) => 
            _loadingCurtain = loadingCurtain;

        public async void Load(string sceneName, Action onComplete = null)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                onComplete?.Invoke();
                return;
            }
                
            _loadingCurtain.Appear();
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            if (!operation.isDone)
                await Task.Yield();
            
            onComplete?.Invoke();
            
            await Task.Delay(500);
            _loadingCurtain.Fade();
        }
    }
}