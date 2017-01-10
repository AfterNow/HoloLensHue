Shader "CustomBasic/BasicOrange" {
	Properties
	{
		_Color("Color", Color) = (1,0.65,0,1)
	}
		SubShader
	{
		Color(1,0.65,0)
		Pass {
			Cull Off
		}
	}
		FallBack "Diffuse"
}
