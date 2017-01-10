Shader "CustomBasic/BasicRed" {
	Properties
	{
		_Color("Color", Color) = (1,0,0,1)
	}
		SubShader
	{
		Color(1, 0, 0)
		Pass {
		Cull Off
	}
	}
		FallBack "Diffuse"
}
