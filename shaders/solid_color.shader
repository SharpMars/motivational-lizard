HEADER
{
	Description = "Template Shader for S&box";
}

FEATURES
{
    #include "common/features.hlsl"
}

COMMON
{
	#include "common/shared.hlsl"
}

struct VertexInput
{
	#include "common/vertexinput.hlsl"
};

struct PixelInput
{
	#include "common/pixelinput.hlsl"
};

VS
{
	#include "common/vertex.hlsl"

	PixelInput MainVs( INSTANCED_SHADER_PARAMS( VertexInput i ) )
	{
		PixelInput o = ProcessVertex( i );
		return FinalizeVertex( o );
	}
}

//=========================================================================================================================

PS
{
    float3 color < UiType( Color ); Default3( 1.0, 1.0, 1.0 ); UiGroup( "Color,10/20" ); >;

	float4 MainPs( PixelInput i ) : SV_Target0
	{
		return float4( SrgbGammaToLinear(color), 1 );
	}
}
