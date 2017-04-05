Shader "Hidden/MosaicSquare"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_AverageTex("Average Texture", 2D) = "white" {}
		_TileSize("Tile size", Vector) = (20, 20, 0.0, 0.0)
		_Shrink("Shrink", float) = 1.0
		_Angle("Angle", float) = 0.0
		_MosaicOpacity("MosaicOpacity", float) = 1.0
		_SceneOpacity("SceneOpacity", float) = 0.0

	}
		SubShader
		{
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float2 uvOriginal : TEXCOORD1;
					float4 vertex : SV_POSITION;
				};

				v2f vert(appdata v)
				{

					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.uvOriginal = v.uv;

					#if UNITY_UV_STARTS_AT_TOP
						v.uv.y = 1 - v.uv.y;
					#endif
					o.uv = v.uv;

					return o;
				}

				// ***
				// Defining the uniforms for the fragment shader
				// ***

				// Texture of the rendered scene
				sampler2D _MainTex;
				// Texture containing the average color
				sampler2D _AverageTex;
				// The x and y size of the tiles for the mosaic pattern
				float2 _TileSize;
				// Value between 0 and 1 to shrink the mosaic pattern
				float _Shrink;
				// Angle in radians to rotate the mosaic pattern around the center of the screen
				float _Angle;
				// The opacity of the mosaic pattern
				float _MosaicOpacity;
				// The opacity of the scene (_MainTexture)
				float _SceneOpacity;

				// Function to rotate a point p around another point (cx, cy) with a certain angle
				float2 rotate_point(float cx, float cy, float angle, float2 p)
				{
					float s = sin(angle);
					float c = cos(angle);

					// translate point back to origin:
					p.x -= cx;
					p.y -= cy;

					// rotate point
					float xnew = p.x * c - p.y * s;
					float ynew = p.x * s + p.y * c;

					// translate point back:
					p.x = xnew + cx;
					p.y = ynew + cy;
					return p;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// The original color of the scene
					fixed4 colOrig = tex2D(_MainTex, i.uvOriginal);

				// We inverse the shrink value, so that we can compare it more easily later
				float shrink = 1.0 - _Shrink;

				// Calculate the current position in pixels or this fragment
				float2 posInPixels = (i.uv * _ScreenParams.xy);
				// Based on the _Angle value rotate the fragment back to it's original position
				// This position is used to calculate the mosaic pattern
				float2 posInPixelsRot = rotate_point(0.5 *_ScreenParams.x, 0.5 * _ScreenParams.y, -_Angle, posInPixels);

				// We divide the screen in a regular grid based in the tilesize
				// Based on the position of the fragment, we determine the block index in that grid
				float2 blockIndex = floor(posInPixelsRot / _TileSize.xy);
				// Based on the block, we can calculate the actual center of that block
				float2 blockCenter = blockIndex * _TileSize.xy + 0.5 * _TileSize.xy;

				// Calculates the distance between the fragment and center and scales it between -1 and 1;
				float2 dist = ((blockCenter - posInPixelsRot) / _TileSize.xy) * 2.0;
				// Take the absolute distance
				float2 distAbs = abs(dist);

				/**
				** Next goal is to calculate a transition value to obtain smooth border between square and non-square colors
				**/

				// Determines whether the fragment is within the square based on the shrink value
				// Smoothstep is used to obtain a smooth transition between the square and non-square color
				float inSquare = 1.0 - length(smoothstep(shrink, shrink + 3.0 / _TileSize.xy, distAbs));

				/**
				** We also need to calculate a transition value between the different squares in the case the squares are not shrinked
				**/

				// Determine whether the fragment is left/right/bottom/up from the center
				float2 direction = -1.0 * sign(dist);
				// Which coordinate is further away from the center?
				float xLargest = step(distAbs.y, distAbs.x);
				// Calculate the second nearest neighbor center for this fragment 
				float2 offset = float2(xLargest, (1.0 - xLargest));
				float2 neighborCenter = blockCenter + direction * offset * _TileSize.xy;

				// Get the colors of the blockcenter and it's closest neighbor
				float2 blockCenterRot = rotate_point(0.5 *_ScreenParams.x, 0.5 * _ScreenParams.y, _Angle, blockCenter);
				float2 neighborCenterRot = rotate_point(0.5 *_ScreenParams.x, 0.5 * _ScreenParams.y, _Angle, neighborCenter);

				float2 uv = blockCenterRot / _ScreenParams.xy;
				float2 uvNeighbor = neighborCenterRot / _ScreenParams.xy;

				fixed4 col = tex2D(_AverageTex, uv);
				fixed4 colNeighbor = tex2D(_AverageTex, uvNeighbor);

				//Calculate the colors for the when you are in or outside the square
				float smoothBorderFactor = 1.0 - length(offset * smoothstep((_TileSize.xy - 1.5) / _TileSize.xy, (_TileSize.xy + 1.5) / _TileSize.xy, distAbs));

				fixed4 squareCol = inSquare * lerp(colOrig, smoothBorderFactor * col + (1.0 - smoothBorderFactor) * colNeighbor, _MosaicOpacity);
				fixed4 nonSquareCol = (1 - inSquare) * lerp(fixed4(0.0, 0.0, 0.0, 0.0), colOrig, _SceneOpacity);

				fixed4 colFinal = squareCol + nonSquareCol;
				return colFinal;
			}
			ENDCG
		}
		}
}
