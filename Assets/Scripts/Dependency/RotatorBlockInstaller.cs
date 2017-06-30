using UnityEngine;
using Zenject;

public class RotatorBlockInstaller : MonoInstaller<RotatorBlockInstaller>
{
    public override void InstallBindings()
    {
        // we need 2 GameGridModel's for the left & the right
        // so we use WithId("...") and [Inject(Id="...")]
        // because they are different, use AsCached rather than AsSingle
        Container.Bind<GameGridModel>().WithId("left").AsCached();
        Container.Bind<GameGridModel>().WithId("right").AsCached();

        // install the signals bindings
        Signals.InstallBindings(Container);
    }
}