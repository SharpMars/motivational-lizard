using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace MotivationalLizard;

public class OnScreenText : PanelComponent
{
	public readonly Label[] Labels = new Label[7];
	
	protected override void OnTreeFirstBuilt()
	{
		base.OnTreeFirstBuilt();
		
		Labels[0] = Panel.Add.Label( "hey there friend", "label1" );
		Labels[1] = Panel.Add.Label( "i know you've been having\nsome troubles lately", "label2" );
		Labels[2] = Panel.Add.Label( "be the person that i know you can be", "label3" );
		Labels[3] = Panel.Add.Label( "don't leave anything up\nto chance", "label4" );
		Labels[4] = Panel.Add.Label( "i believe in you pal", "label5" );
		Labels[5] = Panel.Add.Label( "we ALL believe in you", "label6" );
		Labels[6] = Panel.Add.Label( "ur a winner kiddo.\ndon't you ever forget", "label7" );
		Panel.StyleSheet.Load( "style.scss" );

		foreach ( var label in Labels )
		{
			label.AddClass( "hidden" );
		}
	}
}
