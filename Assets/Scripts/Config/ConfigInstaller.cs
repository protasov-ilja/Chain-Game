using ProjectName.Config;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Installers/ConfigInstaller")]
public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
{
    [SerializeField] private Config _config;

    public override void InstallBindings()
    {
        Container.BindInstance(_config);
    }
}