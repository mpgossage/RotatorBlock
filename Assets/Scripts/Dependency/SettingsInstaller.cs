using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SettingsInstaller", menuName = "RotatorBlock/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
{
    public GridView.Settings gridSettings;
    public GridFrameView.Settings gridFrameSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(gridSettings);
        Container.BindInstance(gridFrameSettings);
    }
}