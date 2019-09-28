using ProjectName.Config;
using ProjectName.Core;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Installers/ConfigInstaller")]
public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
{
    [SerializeField] private Config _config;
    [SerializeField] private GameManager _gameManager;
    
    public override void InstallBindings()
    {
        Container.BindInstance(_config);
        Container.Bind<GameManager>().FromComponentInNewPrefab(_gameManager).AsSingle().NonLazy();
    }
}