Shader "Custom/Highlighted" {
	Properties
	{
		_Color("Color", Color) = (1,1,0,1)
	}
		SubShader
	{
		Color(1, 1, 0)
		Pass {
			Cull Off
		}
	}
		FallBack "Diffuse"
}
