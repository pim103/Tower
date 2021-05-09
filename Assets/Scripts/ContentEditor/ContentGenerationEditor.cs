#if UNITY_EDITOR_64 || UNITY_EDITOR

using System;
using System.Collections;
using ContentEditor.UtilsEditor;
using Games.Global;
using Networking;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ContentEditor
{
    public static class EditorConstant
    {
        public static float MID_WIDTH;
        public static float MID_HEIGHT;
        public static float WIDTH;
        public static float HEIGHT;
        public static float QUART_WIDTH;
        public static float QUART_HEIGHT;
        public static float EIGHTH_WIDTH;
        public static float EIGHTH_HEIGHT;

        public static Texture2D weaponSprite;
        public static Texture2D spellSprite;
        public static Texture2D monsterSprite;
        public static Texture2D armorSprite;
        public static Texture2D categorySprite;
        public static Texture2D playerSprite;
        public static Texture2D spellGeneratorSprite;

        public static void ResetConst()
        {
            WIDTH = Screen.width;
            HEIGHT = Screen.height;
            MID_WIDTH = WIDTH / 2;
            MID_HEIGHT = HEIGHT / 2;
            QUART_WIDTH = WIDTH / 4;
            QUART_HEIGHT = HEIGHT / 4;
            EIGHTH_WIDTH = WIDTH / 8;
            EIGHTH_HEIGHT = HEIGHT / 8;

            if (weaponSprite == null)
            {
                LoadTextures();
            }
        }

        public static void LoadTextures()
        {
            weaponSprite = Resources.Load<Texture2D>("Editor/sword");
            spellSprite = Resources.Load<Texture2D>("Editor/spell");
            playerSprite = Resources.Load<Texture2D>("Editor/Warrior");
            monsterSprite = Resources.Load<Texture2D>("Editor/monster");
            armorSprite = Resources.Load<Texture2D>("Editor/armor");
            categorySprite = armorSprite;
        }
    }
    
    [Serializable]
    public class ContentGenerationEditor : EditorWindow
    {
        private enum Category
        {
            WEAPON,
            ARMOR,
            CATEGORY,
            SPELL,
            MONSTER,
            PLAYER,
            SPELL_GENERATOR
        }

        [SerializeField]
        private Category currentCategory;

        [MenuItem("Tools/Content generation")]
        public static void ShowWindow ()
        {
            Type gameType = Type.GetType("UnityEditor.GameView,UnityEditor.dll");
            ContentGenerationEditor window = GetWindow<ContentGenerationEditor>("Content generation", new Type[]{gameType});
        }

        private void CheckInitEditor()
        {
            if (weaponEditor.contentGenerationEditor == null)
            {
                weaponEditor.contentGenerationEditor = this;
            }
            if (armorEditor.contentGenerationEditor == null)
            {
                armorEditor.contentGenerationEditor = this;
            }
            if (monsterEditor.contentGenerationEditor == null)
            {
                monsterEditor.contentGenerationEditor = this;
            }

            if (!instance)
            {
                instance = this;
            }
        }
        
        private Vector2 scrollPos;
        public static WeaponEditor weaponEditor = new WeaponEditor();
        public static ArmorEditor armorEditor = new ArmorEditor();
        public static MonsterEditor monsterEditor = new MonsterEditor();
        public static PlayerEditor playerEditor = new PlayerEditor();
        public static SpellEditor spellEditor = new SpellEditor();
        public static SpellGeneratorEditor spellGenerator = new SpellGeneratorEditor();
        public static CategoryEditor categoryEditor = new CategoryEditor();
        public static IEditorInterface currentEditor;

        public static ContentGenerationEditor instance;

        public void DisplayHeader()
        {
            Color defaultColor = GUI.color;
            
            GUI.color = Color.black;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button(EditorConstant.weaponSprite, GUILayout.Width(75), GUILayout.Height(75)))
            {
                currentCategory = Category.WEAPON;
                currentEditor = weaponEditor;
            }
            else if (GUILayout.Button(EditorConstant.armorSprite, GUILayout.Width(75), GUILayout.Height(75)))
            {
                currentCategory = Category.ARMOR;
                currentEditor = armorEditor;
            }
            else if (GUILayout.Button(EditorConstant.categorySprite, GUILayout.Width(75), GUILayout.Height(75)))
            {
                currentCategory = Category.CATEGORY;
                currentEditor = categoryEditor;
            }
            else if (GUILayout.Button(EditorConstant.monsterSprite, GUILayout.Width(75), GUILayout.Height(75)))
            {
                monsterEditor.CreateWeaponChoiceList();
                currentCategory = Category.MONSTER;
                currentEditor = monsterEditor;
            }
            else if (GUILayout.Button(EditorConstant.playerSprite, GUILayout.Width(75), GUILayout.Height(75)))
            {
                currentCategory = Category.PLAYER;
                currentEditor = playerEditor;
            }
            else if (GUILayout.Button(EditorConstant.spellSprite, GUILayout.Width(75), GUILayout.Height(75)))
            {
                currentCategory = Category.SPELL;
                currentEditor = spellEditor;
                spellEditor.InitSpellEditor();
            }
            else if (GUILayout.Button(EditorConstant.spellGeneratorSprite, GUILayout.Width(75), GUILayout.Height(75)))
            {
                currentCategory = Category.SPELL_GENERATOR;
                spellGenerator.OpenTrajectorySceneMode();
                currentEditor = spellGenerator;
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load data!"))
            {
                LoadData();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            currentEditor?.DisplayHeaderContent();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }

        void OnGUI ()
        {
            GUILayout.Label("Test custom editor", EditorStyles.boldLabel);

            // INIT VAR
            CheckInitEditor();

            // RESET CONST
            EditorConstant.ResetConst();

            // INIT DISPLAY HEADER
            GUILayout.BeginArea(new Rect(0,0, EditorConstant.WIDTH, EditorConstant.EIGHTH_HEIGHT * 2));
            DisplayHeader();
            GUILayout.EndArea();
            // END DISPLAY HEADER
            
            // INIT FORM DISPLAY
            GUILayout.BeginArea(new Rect(0,EditorConstant.EIGHTH_HEIGHT * 2, EditorConstant.WIDTH, EditorConstant.EIGHTH_HEIGHT * 5));
            
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            currentEditor?.DisplayBodyContent();

            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
            // END FORM DISPLAY

            // INIT DISPLAY FOOTER
            DisplayFooter();
        }

        public void DisplayFooter()
        {
            Color defaultColor = GUI.color;
            GUILayout.FlexibleSpace();

            GUI.color = Color.black;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            currentEditor?.DisplayFooterContent();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Save", GUILayout.Width(75), GUILayout.Height(25)))
            {
                SaveData();
            }
            if (GUILayout.Button("Open test scene", GUILayout.Width(150), GUILayout.Height(25)))
            {
                EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/TestSpell.unity");
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        void SaveData()
        {
            PrepareSaveRequest.SaveChanges();
        }

        void LoadData()
        {
            this.StartCoroutine(LoadDataAsync());
        }

        private IEnumerator LoadDataAsync()
        {
            RequestLoadSpell();

            while (!DictionaryManager.hasSpellsLoad)
            {
                yield return new WaitForSeconds(0.5f);
            }

            instance.StartCoroutine(RequestLoadClasses());
            instance.StartCoroutine(RequestLoadCategory());
            
            while (!DictionaryManager.hasClassesLoad && !DictionaryManager.hasCategoriesLoad)
            {
                yield return new WaitForSeconds(0.5f);
            }

            instance.StartCoroutine(RequestLoadClassesCategory());
            instance.StartCoroutine(RequestLoadEquipment());
            instance.StartCoroutine(RequestLoadMonster());
        }

        public static void RequestLoadSpell()
        {
            DictionaryManager.hasSpellsLoad = false;
            instance.StartCoroutine(DatabaseManager.GetSpells());
        }

        public static IEnumerator RequestLoadClasses()
        {
            DictionaryManager.hasClassesLoad = false;
            instance.StartCoroutine(DatabaseManager.GetClasses());

            while (!DictionaryManager.hasClassesLoad)
            {
                yield return new WaitForSeconds(0.5f);
            }

            playerEditor.CloneOriginalClasses();
        }
        
        public static IEnumerator RequestLoadCategory()
        {
            DictionaryManager.hasCategoriesLoad = false;
            instance.StartCoroutine(DatabaseManager.GetCategoryWeapon());

            while (!DictionaryManager.hasCategoriesLoad)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        public static IEnumerator RequestLoadClassesCategory()
        {
            instance.StartCoroutine(DatabaseManager.GetClassesCategoryWeapon());

            yield break;
        }

        public static IEnumerator RequestLoadEquipment()
        {
            DictionaryManager.hasWeaponsLoad = false;
            instance.StartCoroutine(DatabaseManager.GetWeapons());

            while (!DictionaryManager.hasWeaponsLoad)
            {
                yield return new WaitForSeconds(0.5f);
            }

            weaponEditor.CloneWeaponDictionary();
            armorEditor.CloneArmorDictionary();
            monsterEditor.CreateWeaponChoiceList();
        }

        public static IEnumerator RequestLoadMonster()
        {
            DictionaryManager.hasMonstersLoad = false;
            instance.StartCoroutine(DatabaseManager.GetGroupsMonster());

            while (!DictionaryManager.hasMonstersLoad)
            {
                yield return new WaitForSeconds(0.5f);
            }

            monsterEditor.CloneMonsterDictionary();
        }
    }
}
#endif