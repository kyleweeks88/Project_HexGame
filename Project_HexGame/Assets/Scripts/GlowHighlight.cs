using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowHighlight : MonoBehaviour
{
    Dictionary<Renderer, Material[]> glowMaterialDict = new Dictionary<Renderer, Material[]>();
    Dictionary<Renderer, Material[]> originalMaterialDict = new Dictionary<Renderer, Material[]>();

    Dictionary<Color, Material> cachedGlowMaterials = new Dictionary<Color, Material>();

    public Material glowMat;

    bool isGlowing = false;

    Color validSpaceColor = Color.green;
    Color originalGlowColor;

    private void Awake()
    {
        PrepareMaterialDictionaries();
        originalGlowColor = glowMat.GetColor("_GlowColor");
    }

    public void ToggleGlow()
    {
        if(isGlowing == false)
        {
            foreach (Renderer renderer in originalMaterialDict.Keys)
            {
                renderer.materials = glowMaterialDict[renderer];
            }
        }
        else
        {
            foreach (Renderer renderer in originalMaterialDict.Keys)
            {
                renderer.materials = originalMaterialDict[renderer];
            }
        }
        isGlowing = !isGlowing;
    }

    internal void ResetGlowHighlight()
    {
        if(isGlowing == false) { return; }

        foreach (Renderer renderer in glowMaterialDict.Keys)
        {
            foreach (Material material in glowMaterialDict[renderer])
            {
                material.SetColor("_GlowColor", originalGlowColor);
            }
        }
    }

    internal void HighlightValidPath()
    {
        if (isGlowing == false) { return; }

        foreach (Renderer renderer in glowMaterialDict.Keys)
        {
            foreach (Material material in glowMaterialDict[renderer])
            {
                material.SetColor("_GlowColor", validSpaceColor);
            }
        }
    }

    public void ToggleGlow(bool state)
    {
        if (isGlowing == state)
            return;
        isGlowing = !state;
        ToggleGlow();
    }

    private void PrepareMaterialDictionaries()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            Material[] originalMaterials = renderer.materials;
            originalMaterialDict.Add(renderer, originalMaterials);

            Material[] newMaterials = new Material[renderer.materials.Length];

            for (int i = 0; i < originalMaterials.Length; i++)
            {
                Material mat = null;
                if(cachedGlowMaterials.TryGetValue(originalMaterials[i].color, out mat) == false)
                {
                    mat = new Material(glowMat);
                    // Be default, Unity considers a color with the property -
                    // name "_Color" to be the main color.
                    mat.color = originalMaterials[i].color;
                    cachedGlowMaterials[mat.color] = mat;
                }
                newMaterials[i] = mat;
            }
            glowMaterialDict.Add(renderer, newMaterials);
        }
    }
}
