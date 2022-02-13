Shader "Sprites/Pixel Perfect Outline"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
		
		[PerRendererData] _RectPosSize ("Sprite Rect Position And Size", Vector) = (0,0,0,0)
		[PerRendererData] _Pivot ("Sprite Pivot", Vector) = (0.5, 0.5, 0, 0)
		[PerRendererData] _PixelsPerUnit ("Pixels Per Unit", Float) = 1
		[PerRendererData] _OutlineColor ("Outline Color", Color) = (1,1,1,1)
		
		//Outline directions
		[PerRendererData] _Top ("Top", Float) = 0
		[PerRendererData] _Bottom ("Bottom", Float) = 0
		[PerRendererData] _Left ("Left", Float) = 0
		[PerRendererData] _Right ("Right", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
			"DisableBatching"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex CustomVert
			#pragma fragment CustomFrag
			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnitySprites.cginc"
			
			float4 _MainTex_TexelSize;
			float4 _OutlineColor;
			float4 _RectPosSize;
			float2 _Pivot;
			float _PixelsPerUnit;
			
			float _Top;
			float _Bottom;
			float _Left;
			float _Right;
			
			v2f CustomVert (appdata_t IN) 
			{
			    float2 mult = (_RectPosSize.zw + 2) / _RectPosSize.zw;
			    IN.vertex.xy *= mult;
			    
			    float2 pivotDelta = (_Pivot.xy / _RectPosSize.zw - 0.5);
                IN.vertex.xy += pivotDelta * 2 / _PixelsPerUnit;
                
			    float2 center = (_RectPosSize.xy + _RectPosSize.zw / 2) / _MainTex_TexelSize.zw;
			    IN.texcoord.xy = (IN.texcoord.xy - center) * mult + center;

			    return SpriteVert(IN);
			}
			
			fixed4 CustomFrag(v2f IN) : SV_Target
            {
                fixed4 c = SpriteFrag(IN);
                
                float aTop = 0;
                float aBottom = 0;
                float aLeft = 0;
                float aRight = 0;
                
                float2 minTexcoord = _RectPosSize.xy / _MainTex_TexelSize.zw;
                float2 maxTexcoord = (_RectPosSize.xy + _RectPosSize.zw) / _MainTex_TexelSize.zw;
                 
                if (IN.texcoord.x < minTexcoord.x || IN.texcoord.x > maxTexcoord.x)
                    c = fixed4 (0, 0, 0, 0);
                else
                {
                    aTop = _Bottom * SampleSpriteTexture(IN.texcoord + float2(0, _MainTex_TexelSize.y)).a;
                    aBottom = _Top * SampleSpriteTexture(IN.texcoord + float2(0, -_MainTex_TexelSize.y)).a;
                }
                
                if (IN.texcoord.y < minTexcoord.y || IN.texcoord.y > maxTexcoord.y)
                    c = fixed4 (0, 0, 0, 0);
                else
                {
                    aLeft = _Right * SampleSpriteTexture(IN.texcoord + float2(-_MainTex_TexelSize.x, 0)).a;
                    aRight = _Left * SampleSpriteTexture(IN.texcoord + float2(_MainTex_TexelSize.x, 0)).a;
                }
                
                float aMax = max(aTop, aBottom);
                aMax = max(aMax, aLeft);
                aMax = max(aMax, aRight);
                
                float4 oCol = aMax * _OutlineColor;
                oCol.rgb *= _OutlineColor.a;
                
                c += oCol * (IN.color.a - c.a);
                return c;
            }
		ENDCG
		}
	}
}
