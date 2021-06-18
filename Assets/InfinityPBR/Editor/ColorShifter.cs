using System;
using UnityEditor;
using UnityEngine;
using InfinityPBR;

namespace InfinityPBR
{
    [System.Serializable]
    public class ColorShifter : EditorWindow
    {
        private string _shaderName = "InfinityPBR/LPColor";
        private string _shaderNameHDRP = "InfinityPBR/LPColorHDRP";
        private string _shaderNameURP = "InfinityPBR/LPColorURP";
        public ColorShifterObject colorShifterObject;
        private Shader _shader;
        private int childSelectIndex = 0;

        private string[] childNames;
        
        Vector2 scrollPos;

        private string[] colorSetOptions;

        [MenuItem("Window/Infinity PBR/Color Shifter")]
        public static void ShowWindow()
        {
            GetWindow<ColorShifter>(false, "Color Shifter", true);
        }

        private ColorSet ActiveColorSet()
        {
            return colorShifterObject.colorSets[colorShifterObject.activeColorSetIndex];
        }

        private void CheckListSize()
        {
            if (!colorShifterObject)
                return;

            for (int i = 0; i < colorShifterObject.colorSets.Count; i++)
            {
                if (colorShifterObject.colorSets[i].colorShifterItems.Count < 49)
                {
                    for (int v = 0; v < 49; v++)
                    {
                        if (colorShifterObject.colorSets[i].colorShifterItems.Count < v + 1)
                        {
                            colorShifterObject.colorSets[i].colorShifterItems.Add(new ColorShifterItem());
                            colorShifterObject.colorSets[i].colorShifterItems[v].name = "Color " + v;
                            colorShifterObject.colorSets[i].colorShifterItems[v].shaderIndex = v;
                            colorShifterObject.colorSets[i].colorShifterItems[v].orderIndex = v;
                        }
                    }
                }
                
            }
        }

        void OnGUI()
        {
            CheckListSize();
            
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            
            if (!Shader.Find(_shaderName) && !Shader.Find(_shaderNameURP) && !Shader.Find(_shaderNameHDRP))
            {
                EditorGUILayout.HelpBox(
                    "Can't find the shader \"" + _shaderName + "\", \"" + _shaderNameURP + "\", or \"" + _shaderNameHDRP + "\". Please make sure the shader is in the project before continuing.", MessageType.Error);
                EditorGUILayout.EndScrollView();
                return;
            }

            if (colorShifterObject)
            {
                SetColorSetOptions();
            }

            EditorGUILayout.BeginHorizontal();
            EditorPrefs.SetBool("ColorShifter_ShowHelp", EditorGUILayout.Toggle("Show Help Boxes", EditorPrefs.GetBool("ColorShifter_ShowHelp")));
            EditorPrefs.SetBool("ColorShifter_ShowFull", EditorGUILayout.Toggle("Show Full Data", EditorPrefs.GetBool("ColorShifter_ShowFull")));
            EditorGUILayout.EndHorizontal();
            
            if (EditorPrefs.GetBool("ColorShifter_ShowHelp"))
            {
                EditorGUILayout.HelpBox(
                    "COLOR SHIFTER by Infinity PBR\nThis tool is meant to make it easy to set the color of low poly / faceted " +
                    "objects which make use of a texture made up of 49 or fewer distinct colors.\n\nFor a quickstart video, please visit the tutorials hosted at http://www.InfinityPBR.com", MessageType.None);
            }

            if (!colorShifterObject)
            {
                if (EditorPrefs.GetBool("ColorShifter_ShowHelp"))
                {
                    EditorGUILayout.HelpBox(
                        "Please select a Color Shifter Object, drag-and-drop one into the field below, or create a new object.\n\nTo create a new ojbect, navigate to the location in the Project where you would " +
                        "like to keep your object, right click and select \"Create/Infinity PBR/Create Color Shifter Object\", and then name your object as you'd like.", MessageType.Warning);
                }
            }
            
            colorShifterObject = EditorGUILayout.ObjectField("Color Shifter Object", colorShifterObject,
                typeof(ColorShifterObject), false) as ColorShifterObject;
            
            if (!colorShifterObject)
            {
                EditorGUILayout.EndScrollView();
                return;
            }
            
            colorShifterObject.material = EditorGUILayout.ObjectField("Material", colorShifterObject.material, typeof(Material), false) as Material;
            if (!colorShifterObject.material)
            {
                if (EditorPrefs.GetBool("ColorShifter_ShowHelp"))
                {

                    EditorGUILayout.HelpBox(
                        "Select a material to manage. The shader for the material should be \"" + _shaderName + "\", \"" + _shaderNameURP + "\", or \"" + _shaderNameHDRP + "\".",
                        MessageType.Warning);
                }

                _shader = null;
                EditorGUILayout.EndScrollView();
                return;
            }
            else
            {
                _shader = colorShifterObject.material.shader;
            }

            if (_shader.name != _shaderName && _shader.name != _shaderNameURP && _shader.name != _shaderNameHDRP)
            {
                EditorGUILayout.HelpBox(
                    "The shader for the material must be \"" + _shaderName + "\", \"" + _shaderNameURP + "\", or \"" + _shaderNameHDRP + "\".",
                    MessageType.Error);
                if (GUILayout.Button("Set Shader"))
                {
                    Undo.RecordObject(colorShifterObject.material, "Set Shader");
                    SetShader();
                }
                EditorGUILayout.EndScrollView();
                return;
            }
            
            colorShifterObject.activeColors = Mathf.Clamp(EditorGUILayout.IntField("Active Colors", colorShifterObject.activeColors), 0, 49);

            
            if (EditorPrefs.GetBool("ColorShifter_ShowFull"))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                colorShifterObject.material.SetFloat("_ColorIDRange", EditorGUILayout.FloatField("Range",colorShifterObject.material.GetFloat("_ColorIDRange")));
                colorShifterObject.material.SetFloat("_ColorIDFuzziness",EditorGUILayout.FloatField("Fuzziness",colorShifterObject.material.GetFloat("_ColorIDFuzziness")));

                EditorGUILayout.EndVertical();
                EditorGUILayout.HelpBox(
                    "In most cases Range and Fuzziness values should be set to 0.01.",
                    MessageType.Warning);
                EditorGUILayout.EndHorizontal();
            }
            
            if (colorShifterObject.material.GetFloat("_ColorIDRange") < 0.01f)
                colorShifterObject.material.SetFloat("_ColorIDRange", 0.01f);
            if (colorShifterObject.material.GetFloat("_ColorIDFuzziness") < 0.01f)
                colorShifterObject.material.SetFloat("_ColorIDFuzziness", 0.01f);

            if (colorShifterObject.exportPath == "" || colorShifterObject.exportPath == null)
            {
                EditorGUILayout.HelpBox(
                    "Please click the button below to set the exportPath for saving PNG files of your textures.",
                    MessageType.Error);
            }
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Export Path", colorShifterObject.exportPath, EditorStyles.wordWrappedLabel);
            if (GUILayout.Button("Choose"))
            {
                colorShifterObject.exportPath = EditorUtility.OpenFolderPanel("Choose export destination", "", "");
            }

            EditorGUILayout.EndHorizontal();
            if (colorShifterObject.exportPath == "")
                colorShifterObject.exportPath = Application.dataPath + "/";

            /*
            EditorPrefs.SetBool("Show Full Texture in Shifter", EditorGUILayout.Foldout(EditorPrefs.GetBool("Show Full Texture in Shifter"), "Color ID Texture"));
            if (EditorPrefs.GetBool("Show Full Texture in Shifter"))
            {
                GUILayout.Label(colorShifterObject.material.mainTexture, GUILayout.Width(300));
            }
            */
            
            
            // ---------------------------------------
            // COLOR SET SELECTION
            // ---------------------------------------
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space();
            if (EditorPrefs.GetBool("ColorShifter_ShowHelp"))
            {
                EditorGUILayout.HelpBox(
                    "You can have multiple \"Color Sets\", and easily switch between them at run time. Select a color set below, or create a new one. Each set " +
                    "can have unique color outputs, allowing for multiple ready-to-use looks for your material.",
                    MessageType.None);
            }
            EditorGUILayout.BeginHorizontal();
            colorShifterObject.activeColorSetIndex = EditorGUILayout.Popup(colorShifterObject.activeColorSetIndex, colorSetOptions);
          
            if (GUILayout.Button("Copy Color Set"))
            {
                Undo.RecordObject(colorShifterObject, "Copy Color Set");
                CopyColorSet();
            }
            if (GUILayout.Button("Delete"))
            {
                if (EditorUtility.DisplayDialog("Delete " + ActiveColorSet().name + "",
                    "Do you really want to delete this color set?", "Yes", "Cancel"))
                {
                    Undo.RecordObject(colorShifterObject, "Delete Color Set");
                    DeleteActiveColorSet();
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Create New Color Set"))
            {
                Undo.RecordObject(colorShifterObject, "Create new color set");
                CreateColorSet();
            }
            /*
            if (GUILayout.Button("Export all as PNG"))
            {
                ExportAllColorSets();
            }
*/
            if (GUILayout.Button("Export as PNG"))
            {
                ExportActiveColorSet();
            }
            EditorGUILayout.EndHorizontal();
            
            
            

            // ---------------------------------------
            // DISPLAY COLORS HEADER
            // ---------------------------------------
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space();
            if (EditorPrefs.GetBool("ColorShifter_ShowHelp"))
            {
                EditorGUILayout.HelpBox(
                    "Each of the active colors are displayed below the name of this color set. Each color requires a Color value for the Color ID, and a Hue, Saturation, and Value for the output color.",
                    MessageType.None);
            }
            Undo.RecordObject(colorShifterObject, "Change Color Set Name");
            ActiveColorSet().name = EditorGUILayout.TextField("Color set name", ActiveColorSet().name);
            if (ActiveColorSet().name == "")
            {
                ActiveColorSet().name = "Unnamed Color Set " + colorShifterObject.activeColorSetIndex;
            }
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Expand All"))
            {
                //Undo.RecordObject(colorShifterObject, "Expand All");
                ExpandAll(true);
            }
            if (GUILayout.Button("Collapse All"))
            {
                //Undo.RecordObject(colorShifterObject, "Collapse All");
                ExpandAll(false);
            }

            if (GUILayout.Button("Sort by name"))
            {
                Undo.RecordObject(colorShifterObject, "Sort by name");
                ActiveColorSet().colorShifterItems.Sort();
            }
            
            EditorGUILayout.EndHorizontal();
            
            
            
            // ---------------------------------------
            // DISPLAY COLORS ETC
            // ---------------------------------------
            for (int c = 0; c < colorShifterObject.activeColors; c++)
            {
                ColorShifterItem colorShifterItem = ActiveColorSet().colorShifterItems[c];
                if (colorShifterItem.isOn && colorShifterItem.isChild)
                    DisplayChildOpen(colorShifterItem);
                else if (!colorShifterItem.isOn && colorShifterItem.isChild)
                    DisplayChildClosed(colorShifterItem);
                else if (colorShifterItem.isOn && !colorShifterItem.isChild)
                    DisplayItemOpen(colorShifterItem);
                else if (!colorShifterItem.isOn && !colorShifterItem.isChild)
                    DisplayItemClosed(colorShifterItem);
            }
            
            if (EditorPrefs.GetBool("ColorShifter_ShowHelp"))
            {
                EditorGUILayout.HelpBox(
                    "COLOR ID DEFAULT VALUES\nWhile you can set Color ID (RGB) values for all 49 colors yourself, the extension defaults to these colors:\n\n" +
                    "Color 0: 255,0,0\n" +
                    "Color 1: 0,255,0\n" +
                    "Color 2: 0,0,255\n" +
                    "Color 3: 255,255,0\n" +
                    "Color 4: 255,0,255\n" +
                    "Color 5: 0,255,255\n" +
                    "Color 6: 255,128,0\n" +
                    "Color 7: 255,0,128\n" +
                    "Color 8: 128,255,0\n" +
                    "Color 9: 0,255,128\n" +
                    "Color 10: 128,0,255\n" +
                    "Color 11: 0,128,255\n" +
                    "Color 12: 255,128,128\n" +
                    "Color 13: 128,255,128\n" +
                    "Color 14: 128,128,255\n" +
                    "Color 15: 255,255,128\n" +
                    "Color 16: 255,128,255\n" +
                    "Color 17: 128,255,255\n" +
                    "Color 18: 100,0,0\n" +
                    "Color 19: 0,100,0\n" +
                    "Color 20: 0,0,100\n" +
                    "Color 21: 100,100,0\n" +
                    "Color 22: 0,100,100\n" +
                    "Color 23: 100,0,100\n" +
                    "Color 24: 50,0,0\n" +
                    "Color 25: 0,50,0\n" +
                    "Color 26: 0,0,50\n" +
                    "Color 27: 50,50,0\n" +
                    "Color 28: 50,0,50\n" +
                    "Color 29: 0,50,50\n" +
                    "Color 30: 196,0,0\n" +
                    "Color 31: 0,196,0\n" +
                    "Color 32: 0,0,196\n" +
                    "Color 33: 196,196,0\n" +
                    "Color 34: 196,0,196\n" +
                    "Color 35: 0,196,196\n" +
                    "Color 36: 196,50,50\n" +
                    "Color 37: 50,196,50\n" +
                    "Color 38: 50,50,196\n" +
                    "Color 39: 196,196,50\n" +
                    "Color 40: 196,50,196\n" +
                    "Color 41: 50,196,196\n" +
                    "Color 42: 196,0,96\n" +
                    "Color 43: 196,96,0\n" +
                    "Color 44: 96,0,196\n" +
                    "Color 45: 0,96,196\n" +
                    "Color 46: 128,128,128\n" +
                    "Color 47: 0,0,0\n" +
                    "Color 48: 255,255,255", MessageType.None);
            }
            
            EditorGUILayout.EndScrollView();
            colorShifterObject.SetActiveColorSet();
            
            EditorUtility.SetDirty(colorShifterObject);
        }

        // ---------------------------------------
        // PRIVATE METHODS
        // ---------------------------------------
        private void SetShader()
        {
            colorShifterObject.material.shader = Shader.Find(_shaderName);
        }
        
        private void CreateColorSet()
        {
            colorShifterObject.colorSets.Add(new ColorSet());
            int newIndex = colorShifterObject.colorSets.Count - 1;
            colorShifterObject.colorSets[newIndex].SetDefaultValues();
            colorShifterObject.activeColorSetIndex = newIndex;
            colorShifterObject.colorSets[newIndex].name = "New Color Set " + newIndex;
        }

        private void DeleteActiveColorSet()
        {
            if (colorShifterObject.colorSets.Count == 1)
            {
                Debug.LogWarning("You can't delete the last color set.");
                return;
            }

            int newIndex = colorShifterObject.activeColorSetIndex - 1;
            if (newIndex < 0)
                newIndex = 0;

            colorShifterObject.colorSets.RemoveAt(colorShifterObject.activeColorSetIndex);
            colorShifterObject.activeColorSetIndex = newIndex;
        }
        
        private void SetColorSetOptions()
        {
            colorSetOptions = new string[colorShifterObject.colorSets.Count];
            for (int i = 0; i < colorShifterObject.colorSets.Count; i++)
            {
                colorSetOptions[i] = colorShifterObject.colorSets[i].name;
            }
        }

        private void CopyColorSet()
        {
            ColorSet copyFromColorSet = ActiveColorSet();
            CreateColorSet();
            ColorSet copyToColorSet = ActiveColorSet();

            for (int i = 0; i < copyFromColorSet.colorShifterItems.Count; i++)
            {
                ColorShifterItem copyTo = copyToColorSet.colorShifterItems[i];
                ColorShifterItem copyFrom = copyFromColorSet.colorShifterItems[i];
                
                copyTo.color = copyFrom.color;
                copyTo.name = copyFrom.name;
                copyTo.hue = copyFrom.hue;
                copyTo.saturation = copyFrom.saturation;
                copyTo.value = copyFrom.value;
                copyTo.isOn = copyFrom.isOn;
                copyTo.orderIndex = copyFrom.orderIndex;
                copyTo.shaderIndex = copyFrom.shaderIndex;

                
                for (int c = 0; c < copyFrom.children.Count; c++)
                {
                    copyTo.children.Add(copyFrom.children[c]);
                }

                copyTo.isChild = copyFrom.isChild;
                copyTo.testView = copyFrom.testView;
                copyTo.childHueShift = copyFrom.childHueShift;
                copyTo.childSaturationShift = copyFrom.childSaturationShift;
                copyTo.childValueShift = copyFrom.childValueShift;
                copyTo.parentIndex = copyFrom.parentIndex;
            }

            copyToColorSet.name = copyFromColorSet.name + " Copy " + colorShifterObject.activeColorSetIndex;
        }

        private void ExpandAll(bool v)
        {
            for (int i = 0; i < ActiveColorSet().colorShifterItems.Count; i++)
            {
                ActiveColorSet().colorShifterItems[i].isOn = v;
            }
        }

        private void ExportAllColorSets()
        {
            for (int i = 0; i < colorShifterObject.colorSets.Count; i++)
            {
                ExportColorSet(i);
            }
        }

        private void ExportActiveColorSet()
        {
            ExportColorSet(colorShifterObject.activeColorSetIndex);
        }
        
        private void ExportColorSet(int index)
        {
            //int currentIndex = colorShifterObject.activeColorSetIndex;

            colorShifterObject.activeColorSetIndex = index;
            Repaint();
            
            Texture2D outputTex = new Texture2D(512, 512, TextureFormat.ARGB32, false, true);
            RenderTexture buffer = new RenderTexture(
                512, 
                512, 
                0,                            // No depth/stencil buffer
                RenderTextureFormat.ARGB32//RenderTextureReadWrite.Linear // No sRGB conversions
            );
            
            
            Graphics.Blit(colorShifterObject.material.GetTexture("_MainTex"), buffer, colorShifterObject.material, 2);

            //RenderTexture.active = colorShifterObject.renderTexture;           // If not using a scene camera
            outputTex.ReadPixels(
                new Rect(0, 0, buffer.width, buffer.height), // Capture the whole texture
                0, 0,                          // Write starting at the top-left texel
                false                          // No mipmaps
            );
            
           

            System.IO.File.WriteAllBytes( colorShifterObject.exportPath + "/" + ActiveColorSet().name + ".png", outputTex.EncodeToPNG());


            //colorShifterObject.activeColorSetIndex = currentIndex;
            AssetDatabase.Refresh();
        }

        private void DisplayItemClosed(ColorShifterItem colorShifterItem)
        {
            EditorGUILayout.BeginHorizontal();
            colorShifterItem.isOn = EditorGUILayout.Foldout(colorShifterItem.isOn, colorShifterItem.name);
            Color.RGBToHSV(EditorGUILayout.ColorField("", Color.HSVToRGB(colorShifterItem.hue, colorShifterItem.saturation, colorShifterItem.value)), out colorShifterItem.hue, out colorShifterItem.saturation, out colorShifterItem.value);
                    
            if (GUILayout.Button(colorShifterItem.testView ? "Revert" : "Test"))
            {
                colorShifterItem.testView = !colorShifterItem.testView;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DisplayItemOpen(ColorShifterItem colorShifterItem)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            //Undo.RecordObject(colorShifterObject, "Update Item isOn value");
            colorShifterItem.isOn = EditorGUILayout.Foldout(colorShifterItem.isOn, colorShifterItem.name);
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            Undo.RecordObject(colorShifterObject, "Change Name");
            colorShifterItem.name = EditorGUILayout.TextField("Name", colorShifterItem.name);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            //EditorGUILayout.LabelField("COLOR ID VALUE");
            colorShifterItem.color = EditorGUILayout.ColorField("ColorID color", colorShifterItem.color);
            colorShifterItem.color.r = (EditorGUILayout.Slider("R",  255 * colorShifterItem.color.r, 0,255) / 255);
            colorShifterItem.color.g = (EditorGUILayout.Slider("G",  255 * colorShifterItem.color.g, 0,255) / 255);
            colorShifterItem.color.b = (EditorGUILayout.Slider("B",  255 * colorShifterItem.color.b, 0,255) / 255);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            //EditorGUILayout.LabelField("FINAL COLOR VALUE");
            //Undo.RecordObject(colorShifterObject, "Change Color");
            Color.RGBToHSV(EditorGUILayout.ColorField("Output Color", Color.HSVToRGB(colorShifterItem.hue, colorShifterItem.saturation, colorShifterItem.value)), out colorShifterItem.hue, out colorShifterItem.saturation, out colorShifterItem.value);
            colorShifterItem.hue = EditorGUILayout.Slider("Hue", colorShifterItem.hue, 0f,1f);
            colorShifterItem.saturation = EditorGUILayout.Slider("Saturation", colorShifterItem.saturation, 0f,1f);
            colorShifterItem.value = EditorGUILayout.Slider("Value", colorShifterItem.value, 0f,1f);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            childNames = GetChildNames();
            childSelectIndex = EditorGUILayout.Popup(childSelectIndex, childNames);
            
            if (GUILayout.Button("Control This"))
            {
                AddParentLink(colorShifterItem, childNames[childSelectIndex]);
            }
            EditorGUILayout.EndHorizontal();

            if (colorShifterItem.children.Count > 0)
            {
                EditorGUILayout.LabelField("This color controls these items:");
                for (int c = 0; c < colorShifterItem.children.Count; c++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(ActiveColorSet().colorShifterItems[colorShifterItem.children[c]].name);
                    if (GUILayout.Button("Remove Link"))
                    {
                        RemoveParentLink(ActiveColorSet().colorShifterItems[colorShifterItem.children[c]], colorShifterItem);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                
            }
            
            if (EditorPrefs.GetBool("ColorShifter_ShowFull"))
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.LabelField("Full Details from Scriptable Object", EditorStyles.boldLabel);

                Undo.RecordObject(colorShifterObject, "Change Name");
                colorShifterItem.name = EditorGUILayout.TextField("Name", colorShifterItem.name);
                colorShifterItem.shaderIndex = EditorGUILayout.IntField("Shader Index", colorShifterItem.shaderIndex);
                colorShifterItem.orderIndex = EditorGUILayout.IntField("Order Index", colorShifterItem.orderIndex);
                colorShifterItem.color = EditorGUILayout.ColorField("Color ID color", colorShifterItem.color);
                colorShifterItem.hue = EditorGUILayout.FloatField("Hue", colorShifterItem.hue);
                colorShifterItem.saturation = EditorGUILayout.FloatField("Saturation", colorShifterItem.saturation);
                colorShifterItem.value = EditorGUILayout.FloatField("Lightness", colorShifterItem.value);
                
                EditorGUILayout.EndVertical();
            }
            
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private void DisplayChildClosed(ColorShifterItem colorShifterItem)
        {
            EditorGUILayout.BeginHorizontal();
            colorShifterItem.isOn = EditorGUILayout.Foldout(colorShifterItem.isOn, colorShifterItem.name);

            ColorShifterItem parentItem = ActiveColorSet().colorShifterItems[colorShifterItem.parentIndex];
            EditorGUILayout.LabelField("This color copies " + parentItem.name);
            if (GUILayout.Button("Remove Link"))
            {
                RemoveParentLink(colorShifterItem, parentItem);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DisplayChildOpen(ColorShifterItem colorShifterItem)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            colorShifterItem.isOn = EditorGUILayout.Foldout(colorShifterItem.isOn, colorShifterItem.name);
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            Undo.RecordObject(colorShifterObject, "Change Name");
            colorShifterItem.name = EditorGUILayout.TextField("Name", colorShifterItem.name);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            ColorShifterItem parentItem = ActiveColorSet().colorShifterItems[colorShifterItem.parentIndex];
            EditorGUILayout.LabelField("This color copies " + parentItem.name);
            if (GUILayout.Button("Remove Link"))
            {
                RemoveParentLink(colorShifterItem, parentItem);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.LabelField("Shift the final color by the values below");
            colorShifterItem.childHueShift = EditorGUILayout.Slider("Hue Shift", colorShifterItem.childHueShift, -1f,1f);
            colorShifterItem.childSaturationShift = EditorGUILayout.Slider("Saturation Shift", colorShifterItem.childSaturationShift, -1f,1f);
            colorShifterItem.childValueShift = EditorGUILayout.Slider("Value Shift", colorShifterItem.childValueShift, -1f,1f);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private void RemoveParentLink(ColorShifterItem colorShifterItem, ColorShifterItem parentItem)
        {
            for (int i = 0; i < parentItem.children.Count; i++)
            {
                if (parentItem.children[i] == GetColorShifterItemIndex(colorShifterItem))
                {
                    parentItem.children.RemoveAt(i);
                    colorShifterItem.isChild = false;
                }
            }
        }

        private void AddParentLink(ColorShifterItem colorShifterItem, string childName)
        {
            ColorShifterItem childItem = GetColorShifterItem(childName);
            if (childItem == colorShifterItem)
            {
                Debug.Log("Error: Can't add a parent as its own child");
                return;
            }
            colorShifterItem.children.Add(GetColorShifterItemIndex(childItem));
            childItem.isChild = true;
            childItem.parentIndex = GetColorShifterItemIndex(colorShifterItem);
        }

        private int GetColorShifterItemIndex(ColorShifterItem colorShifterItem)
        {
            for (int i = 0; i < ActiveColorSet().colorShifterItems.Count; i++)
            {
                if (ActiveColorSet().colorShifterItems[i] == colorShifterItem)
                    return i;
            }

            return 9999;
        }
        
        private ColorShifterItem GetColorShifterItem(string name)
        {
            for (int i = 0; i < ActiveColorSet().colorShifterItems.Count; i++)
            {
                if (ActiveColorSet().colorShifterItems[i].name == name)
                    return ActiveColorSet().colorShifterItems[i];
            }

            return null;
        }

        private string[] GetChildNames()
        {
            string[] childNames = new string[ActiveColorSet().colorShifterItems.Count];

            for (int i = 0; i < ActiveColorSet().colorShifterItems.Count; i++)
            {
                if (ActiveColorSet().colorShifterItems[i].isChild)
                    continue;
                childNames[i] = ActiveColorSet().colorShifterItems[i].name;
            }

            return childNames;
        }
    }
}
