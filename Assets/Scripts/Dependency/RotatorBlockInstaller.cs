using UnityEngine;
using Zenject;

public class RotatorBlockInstaller : MonoInstaller<RotatorBlockInstaller>
{
    public override void InstallBindings()
    {
        // we need 2 GameGridModel's for the left & the right
        // so we use WithId("...") and [Inject(Id="...")]
        Container.Bind<GameGridModel>().WithId("left").AsSingle();
        Container.Bind<GameGridModel>().WithId("right").AsSingle();
    }
}