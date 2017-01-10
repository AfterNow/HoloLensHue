Shader "CustomBasic/BasicGreen" {
	Properties
	{
		_Color("Color", Color) = (0,0.5,0,1)
	}
		SubShader
	{
		Color(0, 0.5, 0)
		Pass {
			Cull Off
		}
	}
		FallBack "Diffuse"
}
