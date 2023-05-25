using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace MotivationalLizard;

public class Hud : HudEntity<RootPanel>
{
	public Label[] _labels = new Label[7];

	public Hud()
	{
		_labels[0] = RootPanel.Add.Label( "hey there friend", "label1" );
		_labels[1] = RootPanel.Add.Label( "i know you've been having\nsome troubles lately", "label2" );
		_labels[2] = RootPanel.Add.Label( "be the person that i know you can be", "label3" );
		_labels[3] = RootPanel.Add.Label( "don't leave anything up\nto chance", "label4" );
		_labels[4] = RootPanel.Add.Label( "i believe in you pal", "label5" );
		_labels[5] = RootPanel.Add.Label( "we ALL believe in you", "label6" );
		_labels[6] = RootPanel.Add.Label( "ur a winner kiddo.\ndon't you ever forget", "label7" );
		RootPanel.StyleSheet.Load( "style.scss" );

		foreach ( var label in _labels )
		{
			label.AddClass( "hidden" );
		}
	}
}
