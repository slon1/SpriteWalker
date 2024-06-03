using Server;
using Utils;
using View;
using Zenject;

public class InstallerST : MonoInstaller
{
	public SaveSystem saveLoad;
	public GameManager gameManager;
	public UIManager ui;
	public MouseOrTouchInput input;
	public VirtualScreen view;
	public Player player;
	public ServerClient server;
	public ConfigLoader config;
	public override void InstallBindings() {
			Container.Bind<SaveSystem>().FromInstance(saveLoad).AsSingle();
			Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
			Container.Bind<UIManager>().FromInstance(ui).AsSingle();
			Container.Bind<MouseOrTouchInput>().FromInstance(input).AsSingle();
			Container.Bind<VirtualScreen>().FromInstance(view).AsSingle();
			Container.Bind<Player>().FromInstance(player).AsSingle();
			Container.Bind<ServerClient>().FromInstance(server).AsSingle();
			Container.Bind<ConfigLoader>().FromInstance(config).AsSingle();
	}
}
