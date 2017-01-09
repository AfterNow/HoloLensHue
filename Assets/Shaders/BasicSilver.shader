Shader "CustomBasic/BasicSilver" {
	Properties
	{
		_Color("Color", Color) = (.5,.5,.5,1)
	}
		SubShader
	{
		Color(.5,.5,.5)
		Pass {
		Cull Off
	}
	}
		FallBack "Diffuse"
}
