Shader "CustomBasic/BasicBlue" {
	Properties
	{
		_Color("Color", Color) = (0,0,1,1)
	}
		SubShader
	{
		Color(0, 0, 1)
		Pass {
			Cull Off
		}
	}
		FallBack "Diffuse"
}
