﻿/***********************************************
* Created by dweiss weiss.doron@gmail.com      *
***********************************************/

Shader "Dweiss/MaskSurface" {
	

	Properties{
		_Color("Color",Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	_Reflections("Reflection Cubemap", CUBE) = "black" {}
	_BumpMap("Normalmap", 2D) = "bump" {}
	_RoughnessMap("Roughness Map", 2D) = "white" {}
	_SpecularMap("Specular Map", 2D) = "white" {}
	_SpecularPower("Specular Power", Range(1,255)) = 1
		_Roughness("Roughness",Range(0.01,1)) = 0.01
		_SpecularColor("Specular Color", Color) = (0,0,0,0)

		_MaxY("World Max Y ", Float) = 100
		_MinY("World MinY", Float) = -100
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		Cull Off
		CGPROGRAM
#pragma target 3.0 // needed this otherwise worldRefl + INTERNAL_DATA would exceed texture interpolator limit
#pragma glsl // needed this so I could sample cubemap mips directly
#pragma surface surf Physical

		sampler2D _MainTex;
	samplerCUBE _Reflections;
	sampler2D _BumpMap;
	sampler2D _RoughnessMap;
	sampler2D _SpecularMap;
	float4 _Color;
	float _SpecularPower;
	float4 _SpecularColor;
	float _Roughness;

	float _MinY;
	float _MaxY;

	struct Input {
		float2 uv_MainTex;
		float3 worldRefl;
		float3 worldPos;
		INTERNAL_DATA
	};

	half4 LightingPhysical(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
	{
		//clip(IN.worldPos.y - _MinY);
		//clip(_MaxY - IN.worldPos.y);

		// calculate diffuse term
		float n_dot_l = saturate(dot(s.Normal, lightDir) * atten);
		float3 diffuse = n_dot_l * _LightColor0.rgb;

		// calculate specular term
		float3 h = normalize(lightDir + viewDir);

		float n_dot_h = saturate(dot(s.Normal, h));
		float normalization_term = ((_SpecularPower * _Roughness) + 2.0) / 8.0;
		float blinn_phong = pow(n_dot_h, _SpecularPower * _Roughness);
		float specular_term = blinn_phong * normalization_term;
		float cosine_term = n_dot_l;

		float h_dot_l = dot(h, lightDir);
		float base = 1.0 - h_dot_l;
		float exponential = pow(base, 5.0);

		float3 specColor = _SpecularColor.rgb * s.Gloss;
		float3 fresnel_term = specColor + (1.0 - specColor) * exponential;

		float3 specular = specular_term * cosine_term * fresnel_term * _LightColor0.rgb;

		float3 final_output = s.Albedo * diffuse * (1 - fresnel_term) + specular;
		return float4(final_output, 1);
	}

	void surf(Input IN, inout SurfaceOutput o) {

		
		clip(IN.worldPos.y - _MinY);
		clip(_MaxY - IN.worldPos.y );

		float roughness = _Roughness * tex2D(_RoughnessMap,IN.uv_MainTex).r;
		float4 specularColor = _SpecularColor * tex2D(_SpecularMap,IN.uv_MainTex);

		half4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb * _Color;
		o.Alpha = c.a;
		o.Gloss = roughness;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
		o.Emission = texCUBElod(_Reflections,float4(WorldReflectionVector(IN,o.Normal),(1 - (roughness)) * 8)) * specularColor;
	}
	ENDCG
	}
		FallBack "Diffuse"
}
