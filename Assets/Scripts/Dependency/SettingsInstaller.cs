using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SettingsInstaller", menuName = "RotatorBlock/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
{
    public GridView.Settings gridSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(gridSettings);
    }
}