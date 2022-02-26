using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class GroundItem : MonoBehaviour
{
    public ItemObject item;
    public int amount;
    private int despawnTime;
    private VisualEffect effect;
    private GroundItemVFX vfx;

    private void Start()
    {
        effect = transform.GetComponent<VisualEffect>();
        vfx = GameObject.FindGameObjectWithTag("Manager").GetComponent<GroundItemVFX>();
        despawnTime = item.despawnTime;
        if (effect != null)
        {
            switch (item.grade)
            {
                case Grade.Default:
                    
                    break;
                case Grade.Common:
                    effect.SetVector4("GradeColor", vfx.commonGradient);
                    effect.SetTexture("GradeTexture", vfx.commonParticleTexture);
                    effect.SetFloat("Height", vfx.commonHeight);
                    effect.SetFloat("ParticleRate", vfx.commonParticleRate);
                    effect.SetFloat("AttractionSpeed", vfx.commonAtractionSpeed);
                    effect.SetFloat("GlowSize", vfx.commonGlowSize);
                    break;
                case Grade.Uncommon:
                    effect.SetVector4("GradeColor", vfx.unCommonGradient);
                    effect.SetTexture("GradeTexture", vfx.unCommonParticleTexture);
                    effect.SetFloat("Height", vfx.unCommonHeight);
                    effect.SetFloat("ParticleRate", vfx.unCommonParticleRate);
                    effect.SetFloat("AttractionSpeed", vfx.unCommonAtractionSpeed);
                    effect.SetFloat("GlowSize", vfx.unCommonGlowSize);
                    break;
                case Grade.Rare:
                    effect.SetVector4("GradeColor", vfx.rareGradient);
                    effect.SetTexture("GradeTexture", vfx.rareParticleTexture);
                    effect.SetFloat("Height", vfx.rareHeight);
                    effect.SetFloat("ParticleRate", vfx.rareParticleRate);
                    effect.SetFloat("AttractionSpeed", vfx.rareAtractionSpeed);
                    effect.SetFloat("GlowSize", vfx.rareGlowSize);
                    break;
                case Grade.Legendary:
                    effect.SetVector4("GradeColor", vfx.legendaryGradient);
                    effect.SetTexture("GradeTexture", vfx.legendaryParticleTexture);
                    effect.SetFloat("Height", vfx.legendaryHeight);
                    effect.SetFloat("ParticleRate", vfx.legendaryParticleRate);
                    effect.SetFloat("AttractionSpeed", vfx.legendaryAtractionSpeed);
                    effect.SetFloat("GlowSize", vfx.legendaryGlowSize);
                    break;
            }
        }
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(despawnTime * 60);
            if(transform.root.tag == "Player")
            {

            }
            else
            {
                Destroy(gameObject);
            }

        }
    }
}
