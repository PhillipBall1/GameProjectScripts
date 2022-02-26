using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItemVFX : MonoBehaviour
{
    [Header("Gradients")]
    public Vector4 commonGradient;
    public Vector4 unCommonGradient;
    public Vector4 rareGradient;
    public Vector4 legendaryGradient;

    [Header("Particle Texture")]
    public Texture2D commonParticleTexture;
    public Texture2D unCommonParticleTexture;
    public Texture2D rareParticleTexture;
    public Texture2D legendaryParticleTexture;

    [Header("Height")]
    public float commonHeight;
    public float unCommonHeight;
    public float rareHeight;
    public float legendaryHeight;

    [Header("Particle Rate")]
    public float commonParticleRate;
    public float unCommonParticleRate;
    public float rareParticleRate;
    public float legendaryParticleRate;

    [Header("Attraction Speed")]
    public float commonAtractionSpeed;
    public float unCommonAtractionSpeed;
    public float rareAtractionSpeed;
    public float legendaryAtractionSpeed;

    [Header("Glow Size")]
    public float commonGlowSize;
    public float unCommonGlowSize;
    public float rareGlowSize;
    public float legendaryGlowSize;
}
