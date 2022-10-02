using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Luna.GalaxyMap.Testing
{
    public static class ShaderLibrary
    {
        public const string SmoothStep2 = @"
float2 _smoothstep(float edge0, float edge1, float2 value) {
  float2 t = clamp((value - edge0) / (edge1 - edge0), 0.0, 1.0);
  return t * t * (3.0 - 2.0 * t);
}";
        public const string SmoothStep = @"
float _smoothstep(float edge0, float edge1, float value) {
  float t = clamp((value - edge0) / (edge1 - edge0), 0.0, 1.0);
  return t * t * (3.0 - 2.0 * t);
}";

        public const string Noise = @"
float hash( float n )
{
  return fract(sin(n)*43758.5453123);
}

float noise( float2 x )
{
  float2 p = floor(x);
  float2 f = fract(x);

  f = f*f*(3.0-2.0*f);

  float n = p.x + p.y*57.0;

  float res = mix(mix( hash(n+  0.0), hash(n+  1.0),f.x),
                  mix( hash(n+ 57.0), hash(n+ 58.0),f.x),f.y);
  return res;
}
";
        public const string StarFieldBackground = ShaderLibrary.SmoothStep + @"
// Goals:
//  - Star sizes should not change when switching to fullscreen
//  - Perceived brightness should not change with resolution (somewhat exclusive with goal one!)
//  - Reduce flickering when scrolling



// Dave Hoskins hash functions
vec4 hash42(float2 p)
{
	vec4 p4 = fract(vec4(p.xyxy) * vec4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.wzxy+19.19);
    return fract((p4.xxyz+p4.yzzw)*p4.zywx) - 0.5;
}

float2 hash22(float2 p)
{
	float3 p3 = fract(float3(p.xyx) * float3(443.897, 441.423, 437.195));
    p3 += dot(p3, p3.yzx+19.19);
    return -1.0+2.0*fract((p3.xx+p3.yz)*p3.zy);
}

// IQ's Gradient Noise
float Gradient2D( float2 p )
{
    float2 i = floor( p );
    float2 f = fract( p );
	float2 u = f*f*(3.0-2.0*f);

    return mix( mix( dot( hash22( i + float2(0.0,0.0) ), f - float2(0.0,0.0) ), 
                     dot( hash22( i + float2(1.0,0.0) ), f - float2(1.0,0.0) ), u.x),
                mix( dot( hash22( i + float2(0.0,1.0) ), f - float2(0.0,1.0) ), 
                     dot( hash22( i + float2(1.0,1.0) ), f - float2(1.0,1.0) ), u.x), u.y);
}

float3 StarFieldLayer(float2 p, float du, float count, float brightness, float size)
{
    float3 cold = float3(255.0, 244.0, 189.0)/255.0;
    float3 hot  = float3(181.0, 236.0, 255.0)/255.0;

    // Tiling:
    float2 pi;
    du *= count;    p *= count;
    pi  = floor(p); p  = fract(p)-0.5;
  
    // Randomize position, brightness and spectrum:
    vec4 h = hash42(pi);

    // Resolution independent radius:
    float s = brightness*(0.7+0.6*h.z)*_smoothstep(0.8*du, -0.2*du, length(p+0.9*h.xy) - size*du);

    return s*mix(mix(float3(1.), cold, min(1.,-2.*h.w)), hot, max(0.,2.*h.w));
}

float3 StarField(float2 p, float du)
{
    float3 c;
    c  = StarFieldLayer(p, du, 25.0, 0.18, 0.5); 
    c += StarFieldLayer(p, du, 15.0, 0.25, 0.5); 
    c += StarFieldLayer(p, du, 12.0, 0.50, 0.5); 
    c += StarFieldLayer(p, du,  5.0, 1.00, 0.5); 
    c += StarFieldLayer(p, du,  3.0, 1.00, 0.9); 

    // Cluster:
    float s = 3.5*(max(0.2, Gradient2D(2.0*p*float2(1.2,1.9)))-0.2)/(1.0-0.2);
    c += s*StarFieldLayer(p, du, 160.0, 0.10, 0.5); 
    c += s*StarFieldLayer(p, du,  80.0, 0.15, 0.5); 
    c += s*StarFieldLayer(p, du,  40.0, 0.25, 0.5); 
    c += s*StarFieldLayer(p, du,  30.0, 0.50, 0.5); 
    c += s*StarFieldLayer(p, du,  20.0, 1.00, 0.5); 
    c += s*StarFieldLayer(p, du,  10.0, 1.00, 0.9); 

    c *= 1.3;
    
    // Resolution independent brightness:
    float f = 1.0 / sqrt(660.0*du);

    return f*min(c, 1.0);
}

uniform float2 iResolution;
uniform float iZoom;
uniform float2 iPos;

half4 main(float2 v)
{
  //v -= iPos;
  v /= iZoom;

  float du = 1.0 / iResolution.y;
  float2 uv = du*(v-0.5*iResolution.xy);

  float3 col = StarField(uv, du);
  return vec4(col, 1);
}";

        public const string Star = ShaderLibrary.SmoothStep + @"
uniform float2 iResolution; 
uniform float2 iPos;
uniform half4 tint;

half4 main(float2 fragcoord) {
  float2 uv = (fragcoord - iPos.xy) / iResolution.y;
  float d = length(uv);
  float m = 0.02 / d;
  m *= _smoothstep(0.5, 0.2, d);
  return tint  * m;
}";

        public static SKRuntimeEffect Compile(string shader)
        {
            string errors;
            SKRuntimeEffect effect = SKRuntimeEffect.Create(shader, out errors);

            if (errors != null)
            {
                Debug.WriteLine(errors);
                throw new ArgumentException("Shader failed to compile. Errors: " + errors);
            }
            return effect;
        }
    }
}
