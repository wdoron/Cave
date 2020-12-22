// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurableStencil.shader" company="Supyrb">
//   Copyright (c) 2018 Supyrb. All rights reserved.
// </copyright>
// <repository>
//   https://github.com/supyrb/ConfigurableShaders
// </repository>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// <documentation>
//   https://github.com/supyrb/ConfigurableShaders/wiki/Stencil
// </documentation>
// --------------------------------------------------------------------------------------------------------------------
Shader "Configurable/Reference/Stencil"
{
	Properties
	{		
		[HDR] _Color("Color", Color) = (1,1,1,1)
		
		[HeaderHelpURL(Stencil, https, github.com supyrb ConfigurableShaders wiki Stencil)]
		_Stencil ("Stencil ID [0;255]", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Int) = 3
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Operation", Int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilFail ("Stencil Fail", Int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilZFail ("Stencil ZFail", Int) = 0
		[EightBit] _StencilReadMask ("ReadMask [0;255]", Int) = 255
		[EightBit] _StencilWriteMask ("WriteMask [0;255]", Int) = 255
		
		[HeaderHelpURL(Rendering, https, github.com supyrb ConfigurableShaders wiki Rendering)]
		_Offset("Offset", float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Int) = 2
		[Enum(Off,0,On,1)] _ZWrite("ZWrite", Int) = 1
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Int) = 4
		[Enum(None,0,Alpha,1,Red,8,Green,4,Blue,2,RGB,14,RGBA,15)] _ColorMask("Color Mask", Int) = 14
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	
	fixed4 _Color;
	
	struct v2f
	{
		float4 vertex : SV_POSITION;
	};
	
	v2f vert (float4 vertex : POSITION)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(vertex);
		return o;
	}
	
	fixed4 frag (v2f i) : SV_Target
	{
		return _Color;
	}
	
	struct v2fShadow {
		V2F_SHADOW_CASTER;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	v2fShadow vertShadow( appdata_base v )
	{
		v2fShadow o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
		return o;
	}

	float4 fragShadow( v2fShadow i ) : SV_Target
	{
		SHADOW_CASTER_FRAGMENT(i)
	}
	
	ENDCG
		
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue" = "Geometry" }
		LOD 100
		Cull [_CullMode]
		Offset [_Offset], [_Offset]
		ZWrite [_ZWrite]
		ZTest [_ZTest]
		ColorMask [_ColorMask]

		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			Fail [_StencilFail]
			ZFail [_StencilZFail]
		}

		Pass
		{
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
		
		// Pass to render object as a shadow caster
		Pass
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			
			CGPROGRAM
			#pragma vertex vertShadow	
			#pragma fragment fragShadow
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			ENDCG
		}
	}
}