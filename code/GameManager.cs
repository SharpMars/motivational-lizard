using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Component = Sandbox.Component;

namespace MotivationalLizard;

public class GameManager : Component
{
	[Property] public List<CameraComponent> Cameras { get; set; } = new();
	[Property] public OnScreenText ScreenText { get; set; }
	
	private MusicPlayer _music;
	private bool fadeIn;
	private float fadeInTimer;
	private const float FadeInTime = 7;
	private bool fadeOut;
	private float fadeOutTimer;
	private const float FadeOutTime = 18;

	public bool MapLoaded { get; set; }
	
	private readonly TimeEvent[] _events = {
		new(0, 1, -1, true, false),
		new(10, 0, -1, false, false),
		new(14, 2, 0, false, false),
		new(19, 1, 1, false, false),
		new(24, 0, 2, false, false),
		new(28, 1, -1, false, false),
		new(29, 2, 3, false, false),
		new(34, 0, -1, false, false),
		new(35, 2, -1, false, false),
		new(37, 1, -1, false, false),
		new(38, 2, 4, false, false),
		new(42, 3, 5, false, false),
		new(48, 1, 6, false, false),
		new(56, 1, 6, false, true),
	};

	protected override void OnDestroy()
	{
		if(_music != null)
			_music.OnFinished -= Game.Disconnect;
	}

	protected override void OnUpdate()
	{
		if ( MapLoaded && ScreenText != null && ScreenText.Labels.All( label => label is { IsValid: true } ) )
		{
			if ( Scene.IsEditor ) return;
		
			foreach ( var camera in Cameras )
			{
				camera.GameObject.Enabled = false;
			}
		
			_music = MusicPlayer.Play( FileSystem.Mounted, "sound/motivationallizard.mp3" );
			_music.ListenLocal = true;
			_music.OnFinished += Game.Disconnect;
			MapLoaded = false;
		}
		
		if(_music == null) return;
		
		foreach ( var timeEvent in _events )
		{
			if ( _music.PlaybackTime >= timeEvent.ActivationTime && _music.PlaybackTime < timeEvent.ActivationTime + 0.1f )
			{
				Cameras.ForEach( camera => camera.GameObject.Enabled = false);
				Cameras[timeEvent.CameraToUse].GameObject.Enabled = true;
				foreach ( var label in ScreenText.Labels )
				{
					label.AddClass( "hidden" );
				}

				if ( timeEvent.LabelToActivate != -1 )
					ScreenText.Labels[timeEvent.LabelToActivate].RemoveClass( "hidden" );

				if ( timeEvent.FadeIn )
					fadeIn = true;

				if ( timeEvent.FadeOut )
					fadeOut = true;
			}
		}

		if ( fadeIn )
		{
			if ( Scene.Components.TryGet<CameraComponent>( out var enabledCamera, FindMode.EnabledInSelfAndChildren ) )
			{
				var pp = enabledCamera.Components.GetOrCreate<ColorAdjustments>();
				if ( fadeInTimer >= FadeInTime )
				{
					pp.Brightness = 1;
					fadeIn = false;
					return;
				}
				
				pp.Brightness = MathX.Lerp( 0, 1, fadeInTimer / FadeInTime );
				fadeInTimer += Time.Delta;
			}
		}

		if ( fadeOut )
		{
			if ( Scene.Components.TryGet<CameraComponent>( out var enabledCamera, FindMode.EnabledInSelfAndChildren ) )
			{
				var pp = enabledCamera.Components.GetOrCreate<ColorAdjustments>();
				if ( fadeOutTimer >= FadeOutTime )
				{
					pp.Brightness = 0;
					fadeOut = false;
					return;
				}
				
				pp.Brightness = MathX.Lerp( 1, 0, fadeOutTimer / FadeOutTime );
				fadeOutTimer += Time.Delta;
			}
		}
	}
}
