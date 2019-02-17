// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/UlitTest"
{
	Properties
	{
		_TopColor("Top Color", Color) = (1,1,1,1)
		_BottomColor("Bottom Color", Color) = (1,1,1,1)
		_BlendAmount("Blend Amount", float) = 1
		
	}
	SubShader
	{
		//Tags { "RenderType"="Opaque" }
		//LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;

				
			};

			float4 _TopColor;
			float4 _BottomColor;
			float _BlendAmount;

			struct v2f
			{
				
				//UNITY_FOG_COORDS(1)
				//float4 vertex : SV_POSITION;
				float4 position : SV_POSITION;
				float4 col : COLOR;
				float4 col2 : COLOR1;
				float4 col3 : COLOR2;
				float3 worldPosition : TEXCORD0;
				float3 localPos : TEXCORD1;

			};

			//sampler2D _MainTex;
			//float4 _MainTex_ST;
			
			
			v2f vert (appdata v)
			{
				v2f o;

				o.position = UnityObjectToClipPos(v.vertex);
				
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;


				o.localPos = v.vertex.xyz;
				//o.localPos = o.worldPosition - mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;

				//o.col = lerp(_BottomColor, _TopColor,  o.localPos.y * _BlendAmount);

				//o.col2 = lerp(_BottomColor, _TopColor, v.vertex.z * -_BlendAmount);
				
				//o.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
				
				//o.col = lerp(_TopColor, _BottomColor, step(o.localPos.y, 0.5));
				
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//i.localPos = normalize(i.localPos);
				float4 gradColor = lerp(_TopColor, _BottomColor, step(i.localPos.y,_BlendAmount));
				

				/*
				if (i.localPos.z > 0.43)
				{
					gradColor = _TopColor;
				}
				else
				{
					gradColor = _BottomColor;
				}
				*/


				//gradColor.a = 1;
				// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				//float3 localPos = IN.worldPos - mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
			
				//float4 gradColor = lerp(_BottomColor, _TopColor, i.localPos.z * 1000);
				return gradColor;
			}
			ENDCG
		}
	}
}
