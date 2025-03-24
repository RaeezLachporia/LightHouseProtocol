Shader "Unlit/TransparentGlow"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 0, 0.5) // Yellow transparent
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Color[_Color]
        }
    }
}