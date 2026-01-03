using System;
using Cysharp.Threading.Tasks;

namespace _Project.Code.Core.Services.SceneLoadService
{
    public interface ISceneLoader
    {
        void Load(string sceneName, Action onComplete = null);
    }
}