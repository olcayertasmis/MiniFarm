using Cysharp.Threading.Tasks;
using MiniFarm.Gameplay.Factories;

namespace MiniFarm.Interfaces
{
    public interface IProductionStrategy
    {
        UniTask ProduceAsync(BaseFactory factory);
    }
}