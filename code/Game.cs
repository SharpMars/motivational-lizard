using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace MotivationalLizard;

public partial class MotivationalGame : GameManager
{
	[Net] private IList<CameraPosition> _cameraPositions { get; set; }
	private Hud _hud;
	[Net] private Sound? _music { get; set; }
	private IList<TimeEvent> _events { get; set; } = new List<TimeEvent>
	{
		new(0, 0, -1, true, false),
		new(10, 1, -1, false, false),
		new(14, 2, 0, false, false),
		new(19, 0, 1, false, false),
		new(24, 1, 2, false, false),
		new(28, 0, -1, false, false),
		new(29, 2, 3, false, false),
		new(34, 1, -1, false, false),
		new(35, 2, -1, false, false),
		new(37, 0, -1, false, false),
		new(38, 2, 4, false, false),
		new(42, 3, 5, false, false),
		new(48, 0, 6, false, false),
		new(56, 0, 6, false, true),
	};

	[Net] private bool _fadeOut { get; set; }
	[Net] private bool _fadeIn { get; set; }
	[Net] private int _currentCamera { get; set; }
	[Net] private TimeSince _timeSinceStart { get; set; }
	private float _fadeTimer { get; set; }

	public MotivationalGame()
	{
		if(!Game.IsClient)
			return;
		
		_hud = new Hud();
		var co = Camera.Main.FindOrCreateHook<ColorOverlay>();
		co.Amount = 1;
		co.Color = Color.Black;
	}

	[GameEvent.Entity.PostSpawn]
	private void PostSpawn()
	{
		_cameraPositions = All.OfType<CameraPosition>().OrderBy( camera => camera.Order ).ToList();
	}

	public override void ClientJoined( IClient cl )
	{
		base.ClientJoined( cl );
		
		_music = Sound.FromScreen( "motivationallizard" );
	}

	public override void FrameSimulate( IClient cl )
	{
		if ( _cameraPositions == null )
			return;
		
		Camera.Position = _cameraPositions[_currentCamera].Position;
		Camera.Rotation = _cameraPositions[_currentCamera].Rotation;
		Camera.FieldOfView = 85f;
	}

	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		if ( !_music.HasValue )
			return;

		if ( !_music.Value.IsPlaying )
		{
			cl.Kick();
			return;
		}

		foreach ( var timeEvent in _events )
		{
			if ( timeEvent.ActivationTime >= _music.Value.ElapsedTime && timeEvent.ActivationTime < _music.Value.ElapsedTime + Time.Delta )
			{
				_currentCamera = timeEvent.CameraToUse;
				_fadeIn = timeEvent.FadeIn;
				_fadeOut = timeEvent.FadeOut;

				if ( !Game.IsClient )
					continue;

				for ( var i = 0; i < _hud._labels.Length; i++ )
				{
					_hud._labels[i].SetClass( "hidden", i != timeEvent.LabelToActivate );
				}
			}
		}

		if ( !Game.IsClient )
			return;
		
		var co = Camera.Main.FindOrCreateHook<ColorOverlay>();

		if ( _fadeIn )
		{
			co.Amount = MathX.Lerp( 1, 0, _fadeTimer / 5 );
			if(_timeSinceStart > 2)
				_fadeTimer += Time.Delta;
		}
		else if ( _fadeOut )
		{
			co.Amount = MathX.Lerp( 0, 1, _fadeTimer / 18 );
			_fadeTimer += Time.Delta;
		}
		else
		{
			co.Amount = 0;
			_fadeTimer = 0;
		}
	}
}
