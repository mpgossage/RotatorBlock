using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenject;

// helper class to hold all the signals
public static class Signals
{
    // when any/all grids are updated
    public class GridUpdated : Signal<GridUpdated> {}

    // to keep it clear, the installer for all signals is here in the with the signals
    // so when you add a signal, you can also add the bindings
    public static void InstallBindings(DiContainer container)
    {
        // why must we have 2 lines to make these things work?
        RegisterSignal<GridUpdated>(container);
    }

    private static void RegisterSignal<T>(DiContainer container) where T: ISignalBase
    {
        // why must we have 2 lines to make these things work?
        // Note: if you only BindSignal you get some weird null exception error
        container.DeclareSignal<T>();
        container.BindInterfacesTo<T>().AsSingle();
    }
}
