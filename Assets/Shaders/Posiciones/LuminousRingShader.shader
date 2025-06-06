Shader "Custom/LuminousRingShader"
{
    Properties
    {
        _RingColor ("Ring Color", Color) = (1, 0, 1, 1) // Color del aro
        _EmissionColor ("Emission Color", Color) = (1, 0, 1, 1) // Color de emisión
        _RingThickness ("Ring Thickness", Range(0.01, 0.5)) = 0.05 // Grosor del aro
        _RingRadius ("Ring Radius", Range(0.1, 1.0)) = 0.5 // Radio del aro
        _EmissionStrength ("Emission Strength", Range(0, 5)) = 2.0 // Fuerza de la emisión
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _RingThickness;
            float _RingRadius;
            float _EmissionStrength;
            half4 _RingColor;
            half4 _EmissionColor;

            // Matriz de Modelo-Vista-Proyección proporcionada por Unity
            v2f vert(appdata_t v)
            {
                v2f o;
                // Transformar el vértice del espacio objeto al espacio de clip usando UNITY_MATRIX_MVP
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv - 0.5; // Mover UV al centro
                float dist = length(uv); // Distancia radial desde el centro

                // Crear el aro
                float ring = smoothstep(_RingRadius + _RingThickness, _RingRadius - _RingThickness, dist);

                // Color base del aro
                half4 color = _RingColor * ring;

                // Añadir emisión
                half4 emission = _EmissionColor * ring * _EmissionStrength;

                return color + emission;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
