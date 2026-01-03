using System.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Core.Factory
{
    public interface IGameObjectBuilderFactory
    {
        EmptyGameObjectBuilder<T> BuildNewFor<T>(string name) where T : Component;
    }
}