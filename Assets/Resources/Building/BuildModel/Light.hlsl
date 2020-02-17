//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

void MainLight_half(float3 ObjPos, out float3 Direction, out float3 Color, out half DistanceAtten, out half ShadowAtten)
{
   
    #if SHADERGRAPH_PREVIEW
        Direction = half3(0, 0.5, 0.5);
        Color = 1;
        DistanceAtten = 1;
        ShadowAtten = 1;
    #else
    #ifdef LIGHTWEIGHT_LIGHTING_INCLUDED
    #if SHADOWS_SCREEN
        half4 clipPos = TransformWorldToHClip(WorldPos);
        half4 shadowCoord = ComputeScreenPos(clipPos);
    #else
        half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    #endif
        Light mainLight = GetMainLight(shadowCoord);
        Direction = mainLight.direction;
        Color = mainLight.color;
        DistanceAtten = mainLight.distanceAttenuation;
        ShadowAtten = mainLight.shadowAttenuation;
    #else
        Direction = half3(0.5, 0.5, 0);
        Color = 1;
        DistanceAtten = 1;
        ShadowAtten = 1;
    #endif
    
    #endif
   
}

void DirectSpecular_half(half3 Specular, half Smoothness, half3 Direction, half3 Color, half3 WorldNormal, half3 WorldView, out half3 Out)
{
    #if SHADERGRAPH_PREVIEW
       Out = 0;
    #else
    #ifdef LIGHTWEIGHT_LIGHTING_INCLUDED
       Smoothness = exp2(10 * Smoothness + 1);
       WorldNormal = normalize(WorldNormal);
       WorldView = SafeNormalize(WorldView);
       Out = LightingSpecular(Color, Direction, WorldNormal, WorldView, half4(Specular, 0), Smoothness);
    #else
        Out = 0;
    #endif
    #endif
}
#endif
