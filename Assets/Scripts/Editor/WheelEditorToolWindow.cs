using System.Collections.Generic;
using System.IO;
using System.Linq;
using Level;
using UI.Item;
using UI.Zone;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Editor
{
    public class WheelEditorToolWindow : EditorWindow
    {
        private int _selectedTab;
        private readonly string[] tabs = { "Item", "Level", "Settings" };

        public string levelPath;
        public string zonePath;
        public string iconPath;
        public string itemPath;

        private const string levelPathKey = "VertigoTool_LevelPath";
        private const string zonePathKey = "VertigoTool_ZonePath";
        private const string iconPathKey = "VertigoTool_IconPath";
        private const string itemPathKey = "VertigoTool_ItemPath";

        public List<ZoneSo> zoneList = new();
        public LevelListSo levelList;

        public ReorderableList LevelReorderableList;

        private IWheelEditorPanel itemPanel;
        private WheelEditorLevelPanel levelPanel;
        private IWheelEditorPanel settingsPanel;

        [MenuItem("Tools/Vertigo Tool")]
        public static void ShowWindow()
        {
            var window = GetWindow<WheelEditorToolWindow>("Vertigo Tool");
            window.minSize = new Vector2(600, 450);
        }

        private void OnEnable()
        {
            levelPath = EditorPrefs.GetString(levelPathKey, "Assets/ScriptableObjects/Level");
            zonePath = EditorPrefs.GetString(zonePathKey, "Assets/ScriptableObjects/Zone");
            iconPath = EditorPrefs.GetString(iconPathKey, "Assets/UI/Icon");
            itemPath = EditorPrefs.GetString(itemPathKey, "Assets/ScriptableObjects/Item");

            RefreshZoneList();
            LoadLevelSettings();
            CreateReorderableList();

            itemPanel = new WheelEditorItemPanel(this);
            levelPanel = new WheelEditorLevelPanel(this);
            settingsPanel = new WheelEditorSettingsPanel(this);
        }

        private void OnGUI()
        {
            _selectedTab = GUILayout.Toolbar(_selectedTab, tabs, GUILayout.Height(30));
            EditorGUILayout.Space(10);

            switch (_selectedTab)
            {
                case 0: itemPanel.Draw(); break;
                case 1: levelPanel.Draw(); break;
                case 2: settingsPanel.Draw(); break;
            }
        }

        public void RefreshZoneList()
        {
            zoneList.Clear();

            if (!Directory.Exists(zonePath))
                return;

            var files = Directory.GetFiles(zonePath, "*.asset");
            foreach (var file in files)
            {
                var asset = AssetDatabase.LoadAssetAtPath<ZoneSo>(file);
                if (asset)
                    zoneList.Add(asset);
            }
        }

        public void LoadLevelSettings()
        {
            levelList = null;

            if (!Directory.Exists(levelPath))
                return;

            var files = Directory.GetFiles(levelPath, "*.asset");
            foreach (var file in files)
            {
                var asset = AssetDatabase.LoadAssetAtPath<LevelListSo>(file);
                if (asset)
                {
                    levelList = asset;
                    break;
                }
            }
        }

        public void CreateReorderableList()
        {
            if (!levelList)
                return;

            LevelReorderableList = new ReorderableList(
                levelList.LevelList,
                typeof(LevelSo),
                draggable: false,
                displayHeader: true,
                displayAddButton: false,
                displayRemoveButton: false
            );

            LevelReorderableList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Level List");
            };

            LevelReorderableList.drawElementCallback = (rect, index, _, _) =>
            {
                var level = levelList.LevelList[index];
                if (!level) return;

                rect.y += 2;

                var levelRect = new Rect(rect.x, rect.y, rect.width * 0.45f, EditorGUIUtility.singleLineHeight);
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.ObjectField(levelRect, level, typeof(LevelSo), false);
                EditorGUI.EndDisabledGroup();

                var zoneRect = new Rect(rect.x + rect.width * 0.47f, rect.y, rect.width * 0.40f, EditorGUIUtility.singleLineHeight);
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.ObjectField(zoneRect, level.ZoneSo, typeof(ZoneSo), false);
                EditorGUI.EndDisabledGroup();
            };
        }

        public void CreateNewLevelAsset()
        {
            var newIndex = levelList.LevelList.Count + 1;
            var newName = $"Level {newIndex}.asset";
            var fullPath = Path.Combine(levelPath, newName).Replace("\\", "/");

            var newLevel = CreateInstance<LevelSo>();

            newLevel.Items = new List<ItemSo>(new ItemSo[8]);

            AssetDatabase.CreateAsset(newLevel, fullPath);
            AssetDatabase.SaveAssets();

            levelList.LevelList.Add(newLevel);
            EditorUtility.SetDirty(levelList);

            ApplyLevelRenaming();
            ApplyZoneAssignments();

            CreateReorderableList();
            Repaint();
        }

        public void DeleteLastLevelAsset()
        {
            if (levelList.LevelList.Count == 0)
                return;

            var lastIndex = levelList.LevelList.Count - 1;
            var last = levelList.LevelList[lastIndex];

            var path = AssetDatabase.GetAssetPath(last);

            AssetDatabase.DeleteAsset(path);
            levelList.LevelList.RemoveAt(lastIndex);

            EditorUtility.SetDirty(levelList);
            AssetDatabase.SaveAssets();

            ApplyLevelRenaming();
            ApplyZoneAssignments();

            CreateReorderableList();
            Repaint();
        }

        private void ApplyLevelRenaming()
        {
            for (var i = 0; i < levelList.LevelList.Count; i++)
            {
                var level = levelList.LevelList[i];
                var path = AssetDatabase.GetAssetPath(level);
                AssetDatabase.RenameAsset(path, $"Level {i + 1}");
            }

            AssetDatabase.SaveAssets();
        }

        private void ApplyZoneAssignments()
        {
            if (zoneList.Count == 0) return;

            var sorted = new List<ZoneSo>(zoneList);
            sorted.Sort((a, b) => b.PerLevel.CompareTo(a.PerLevel));

            for (var i = 0; i < levelList.LevelList.Count; i++)
            {
                var level = levelList.LevelList[i];
                var num = i + 1;

                foreach (var zone in sorted.Where(zone => zone.PerLevel > 0 && num % zone.PerLevel == 0))
                {
                    level.ZoneSo = zone;
                    EditorUtility.SetDirty(level);
                    break;
                }
            }

            AssetDatabase.SaveAssets();
        }
    }
}
