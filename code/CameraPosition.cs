using Editor;
using Sandbox;

namespace MotivationalLizard;

[EditorModel( "models/editor/camera.vmdl" ), HammerEntity, Icon( "camera" )]
public partial class CameraPosition : Entity
{
	[Property( Title = "Order" ), Net] public int Order { get; private set; } = 0;

	public override void Spawn()
	{
		Transmit = TransmitType.Always;
		
		base.Spawn();
	}
}
