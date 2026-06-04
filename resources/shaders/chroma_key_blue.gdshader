shader_type canvas_item;

uniform vec4 key_color : source_color = vec4(0.196078, 0.376471, 0.650980, 1.0);
uniform float tolerance : hint_range(0.0, 0.2) = 0.01;

void fragment() {
	vec4 texel = texture(TEXTURE, UV);

	if (distance(texel.rgb, key_color.rgb) <= tolerance) {
		texel.a = 0.0;
	}

	COLOR = texel;
}
