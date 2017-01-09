Shader "CustomBasic/BasicIndigo" {
	Properties
	{
		_Color("Color", Color) = (0.3,0,0.5,1)
	}
		SubShader
	{
		Color(0.3, 0, 0.5)
		Pass {
			Cull Off
		}
	}
		FallBack "Diffuse"
}
