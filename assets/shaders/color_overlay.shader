HEADER
{
    Description = "Example post processing shader";
    DevShader = true;
}

MODES
{
    Default();
    VrForward();
}

FEATURES
{
}

COMMON
{
    #include "postprocess/shared.hlsl"
}

struct VertexInput
{
    float3 vPositionOs : POSITION < Semantic( PosXyz ); >;
    float2 vTexCoord : TEXCOORD0 < Semantic( LowPrecisionUv ); >;
};

struct PixelInput
{
    float2 vTexCoord : TEXCOORD0;

	// VS only
	#if ( PROGRAM == VFX_PROGRAM_VS )
		float4 vPositionPs		: SV_Position;
	#endif

	// PS only
	#if ( ( PROGRAM == VFX_PROGRAM_PS ) )
		float4 vPositionSs		: SV_ScreenPosition;
	#endif
};

VS
{
    PixelInput MainVs( VertexInput i )
    {
        PixelInput o;
        o.vPositionPs = float4(i.vPositionOs.xyz, 1.0f);
        o.vTexCoord = i.vTexCoord;
        return o;
    }
}

PS
{
    #include "postprocess/common.hlsl"

    RenderState( DepthWriteEnable, false );
    RenderState( DepthEnable, false );

    CreateTexture2D( g_tColorBuffer ) < Attribute( "ColorBuffer" ); SrgbRead( true ); Filter( MIN_MAG_LINEAR_MIP_POINT ); AddressU( MIRROR ); AddressV( MIRROR ); >;

    float amount < Attribute("coloroverlay.amount"); Default(0); >;
    float3 color < Attribute("coloroverlay.color"); Default3( 1.0, 1.0, 1.0 ); >;

    float4 MainPs( PixelInput i ) : SV_Target0
    {
        float2 vScreenUv = i.vPositionSs.xy / g_vRenderTargetSize;

        float3 screenColor = Tex2D( g_tColorBuffer, vScreenUv.xy ).rgb;
        
        return float4(lerp(screenColor, color, amount), 1.0f);
    }
}
