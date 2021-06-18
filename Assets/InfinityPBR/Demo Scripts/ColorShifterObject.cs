using System;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityPBR
{
    [CreateAssetMenu(fileName = "Data", menuName = "Infinity PBR/Create Color Shifter Object")]
    [System.Serializable]
    public class ColorShifterObject : ScriptableObject
    {
        
        public int activeColorSetIndex = 0;
        public int activeColors = 49;
        public List<ColorSet> colorSets = new List<ColorSet>();
        public Material material;

        public string exportPath;
        //public RenderTexture renderTexture;

        void Reset()
        {
            colorSets.Clear();
            colorSets.Add(new ColorSet());
            colorSets[0].name = "Color Set 1";
            for (int i = 0; i < colorSets.Count; i++)
            {
                colorSets[i].SetDefaultValues();
            }

            activeColorSetIndex = 0;
            SetActiveColorSet();
        }

        public void OnValidate()
        { 
            activeColors = Mathf.Clamp(activeColors, 0, 49);
        }

        public void SetActiveColorSet()
        {
            SetColorSet(activeColorSetIndex);
        }

        public void SetRandomColorSet(bool updateAcitveIndex = true)
        {
            SetColorSet(UnityEngine.Random.Range(0, colorSets.Count), updateAcitveIndex);
        }

        public void SetColorSet(int index, bool updateActiveIndex = true)
        {
            if (index >= colorSets.Count)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("Warning: index was greater than the available color sets.");
                #endif
                return;
            }

            if (updateActiveIndex)
                activeColorSetIndex = index;
            
            for (int i = 0; i < colorSets[index].colorShifterItems.Count; i++)
            {
                ColorShifterItem item = colorSets[index].colorShifterItems[i];
                bool testView = colorSets[index].colorShifterItems[i].testView;
                #if !UNITY_EDITOR
                testView = false;
                #endif
                
                float testH;
                float testS;
                float testV;
                Color.RGBToHSV(Color.magenta, out testH, out testS, out testV);

                ColorShifterItem parent = item.isChild ? colorSets[index].colorShifterItems[item.parentIndex] : null;
                
                float hue = item.isChild ? parent.hue + item.childHueShift : item.hue;
                float saturation = item.isChild ? parent.saturation + item.childSaturationShift : item.saturation;
                float value = item.isChild ? parent.value + item.childValueShift : item.value;

                material.SetColor("_Color" + i, item.color);
                material.SetFloat("_H" + i, testView ? testH : hue);
                material.SetFloat("_S" + i, testView ? testS : saturation);
                material.SetFloat("_V" + i, testView ? testV : value);

            }
        }

        public void SetColorSet(string name, bool updateActiveIndex = true)
        {
            for (int i = 0; i < colorSets.Count; i++)
            {
                if (colorSets[i].name == name)
                {
                    SetColorSet(i, updateActiveIndex);
                    return;
                }
            }
            #if UNITY_EDITOR
            Debug.LogWarning("Warning: No color set named \"name\" was found.");
            #endif
        }
    }

    [System.Serializable]
    public class ColorSet
    {
        public string name;
        public List<ColorShifterItem> colorShifterItems = new List<ColorShifterItem>();
        
        public void SetDefaultValues()
        {
            colorShifterItems.Clear();
            for (int i = 0; i < 49; i++)
            {
                colorShifterItems.Add(new ColorShifterItem());
                colorShifterItems[i].name = "Color " + i;
                colorShifterItems[i].shaderIndex = i;
                colorShifterItems[i].orderIndex = i;
            }

            float v255 = 1f;
            float v128 = 128f / 255f;
            float v100 = 100f / 255f;
            float v50 = 50f / 255f;
            float v0 = 0f;
            float v196 = 196f / 255f;
            float v96 = 96f / 255f;
            

            colorShifterItems[0].color = new Color(v255, v0, v0);
            colorShifterItems[1].color = new Color(v0, v255, v0);
            colorShifterItems[2].color = new Color(v0, v0, v255);
            colorShifterItems[3].color = new Color(v255, v255, v0);
            colorShifterItems[4].color = new Color(v255, v0, v255);
            colorShifterItems[5].color = new Color(v0, v255, v255);
            colorShifterItems[6].color = new Color(v255, v128, v0);
            colorShifterItems[7].color = new Color(v255, v0, v128);
            colorShifterItems[8].color = new Color(v128, v255, v0);
            colorShifterItems[9].color = new Color(v0, v255, v128);
            colorShifterItems[10].color = new Color(v128, v0, v255);
            colorShifterItems[11].color = new Color(v0, v128, v255);
            colorShifterItems[12].color = new Color(v255, v128, v128);
            colorShifterItems[13].color = new Color(v128, v255, v128);
            colorShifterItems[14].color = new Color(v128, v128, v255);
            colorShifterItems[15].color = new Color(v255, v255, v128);
            colorShifterItems[16].color = new Color(v255, v128, v255);
            colorShifterItems[17].color = new Color(v128, v255, v255);
            colorShifterItems[18].color = new Color(v100, v0, v0);
            colorShifterItems[19].color = new Color(v0, v100, v0);
            colorShifterItems[20].color = new Color(v0, v0, v100);
            colorShifterItems[21].color = new Color(v100, v100, v0);
            colorShifterItems[22].color = new Color(v100, v0, v100);
            colorShifterItems[23].color = new Color(v0, v100, v100);
            colorShifterItems[24].color = new Color(v50, v0, v0);
            colorShifterItems[25].color = new Color(v0, v50, v0);
            colorShifterItems[26].color = new Color(v0, v0, v50);
            colorShifterItems[27].color = new Color(v50, v50, v0);
            colorShifterItems[28].color = new Color(v50, v0, v50);
            colorShifterItems[29].color = new Color(v0, v50, v50);
            colorShifterItems[30].color = new Color(v196, v0, v0);
            colorShifterItems[31].color = new Color(v0, v196, v0);
            colorShifterItems[32].color = new Color(v0, v0, v196);
            colorShifterItems[33].color = new Color(v196, v196, v0);
            colorShifterItems[34].color = new Color(v196, v0, v196);
            colorShifterItems[35].color = new Color(v0, v196, v196);
            colorShifterItems[36].color = new Color(v196, v50, v50);
            colorShifterItems[37].color = new Color(v50, v196, v50);
            colorShifterItems[38].color = new Color(v50, v50, v196);
            colorShifterItems[39].color = new Color(v196, v196, v50);
            colorShifterItems[40].color = new Color(v196, v50, v196);
            colorShifterItems[41].color = new Color(v50, v196, v196);
            colorShifterItems[42].color = new Color(v196, v0, v96);
            colorShifterItems[43].color = new Color(v196, v96, v0);
            colorShifterItems[44].color = new Color(v96, v0, v196);
            colorShifterItems[45].color = new Color(v0, v96, v196);
            colorShifterItems[46].color = new Color(v128, v128, v128);
            colorShifterItems[47].color = new Color(v0, v0, v0);
            colorShifterItems[48].color = new Color(v255, v255, v255);

        }
    }

    [System.Serializable]
    public class ColorShifterItem : IComparable<ColorShifterItem>
    {
        public string name;
        public bool isOn = false;
        public int shaderIndex;
        public int orderIndex;
        public Color color;
        public float hue = 0.5f;
        public float saturation = 0.5f;
        public float value = 0.5f;
        [HideInInspector] public bool testView = false;

        public List<int> children = new List<int>();
        public bool isChild = false;
        public int parentIndex = 0;
        public float childHueShift = 0f;
        public float childSaturationShift = 0f;
        public float childValueShift = 0f;
        
        public int CompareTo(ColorShifterItem item)
        {       // A null value means that this object is greater.
            if (name == null){
                return 1;  
            }
            else {
                return this.name.CompareTo(item.name);
            }
        }
        
      
    }
}
