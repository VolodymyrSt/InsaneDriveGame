using _Project.Code.Core.Factory.Pool;

namespace _Project.Code.Core.Services.Audio
{
    public enum SoundType
    {
        SFX, MUSIC
    }

    public class SfxEmitter : SoundEmitter, IPoolable<WarpPoolableData>
    {
        public void OnSpawn(WarpPoolableData data)
        {
            
        }

        public void OnDespawn()
        {
            
        }

        public void OnDestroySelf()
        {
            
        }
    }
}