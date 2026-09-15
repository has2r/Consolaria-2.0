sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float2 uTargetPosition;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;

uniform float anxiety = 0.0;
uniform float2 anxietyOrigin = float2(0.5, 0.5);
uniform float gamerate = 1.0;
uniform float waterSine = 0.0;
uniform float waterCameraY = 0.0;
uniform float waterAlpha = 1.0;

float glitch;
float seed;
float amplitude;
float minimum;

float rand(float2 co)
{
    return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
}

float4 GetAnxietyColor(float2 texcoord)
{
    // get anxiety amount
    float len = length(texcoord - anxietyOrigin) * 2.0;
    float anx = 0.02 * len * anxiety;

    // offset R & B samples by anxiety amount
    float4 r = tex2D(uImage0, texcoord + float2(anx, 0.0));
    float4 g = tex2D(uImage0, texcoord + float2(0.0, 0.0));
    float4 b = tex2D(uImage0, texcoord + float2(-anx, 0.0));
    
    return float4(r.x, g.y, b.z, b.w);
}

float4 GetGrayscaleColor(float4 color)
{
    // gamerate -> black & white
    float gray = float(color.r * 0.3 + color.g * 0.59 + color.b * 0.11);
    return lerp(color, float4(gray, gray, gray, color.w), 1 - gamerate);
}

float NormalSin(float time)
{
    return 1 - (sin(time) + 1) / 2;
}

float2 FrameFix(float2 coords)
{
    float frameSizeX = uSourceRect.z / uImageSize0.x;
    float x = coords.x % frameSizeX;
    float frameSizeY = uLegacyArmorSourceRect.w / uImageSize0.y;
    float y = coords.y % frameSizeY;
    return float2(x * 1 / frameSizeX, y * 1 / frameSizeY);
}

float4 Recolor(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{  
    float4 origColor = tex2D(uImage0, coords);
    float time = uTime * 10;
    float2 textureCoords = FrameFix(coords);
    
    float mult = abs(max(minimum, sin(time + textureCoords.y * amplitude))) * glitch;

    float pixelSize = max(1, floor(16.0 * mult));
    float offset = rand(float2(seed, pixelSize));

    float2 org = float2(max(0, min(1, textureCoords.x + (offset * 0.1 - 0.05) * mult)), textureCoords.y);
    float2 size = textureCoords.xy / pixelSize;
    float2 xy = floor(org * size) / size + pixelSize / textureCoords.xy * 0.5;

    // grab the color
    float4 color = GetGrayscaleColor(GetAnxietyColor(textureCoords));
    return (color + float4(offset, (offset + 0.34) % 1, (offset + 0.66) % 1, 1) * color.a * 0.25 * mult) * sampleColor;
}

technique Technique1
{
    pass DistortPass
    {
        PixelShader = compile ps_3_0 Recolor();
    }
}