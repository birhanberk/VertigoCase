using System.Collections.Generic;
using System.IO;
using Item;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class WheelEditorItemPanel : IWheelEditorPanel
    {
        private readonly WheelEditorToolWindow window;

        private Vector2 itemScroll;
        private Vector2 suggestionScroll;

        private readonly List<ItemSo> itemList = new();
        private readonly List<Sprite> suggestionSprites = new();

        private int selectedSuggestionIndex = -1;

        private string newItemHeader = "";
        private readonly bool[] zoneSelections;

        public WheelEditorItemPanel(WheelEditorToolWindow window)
        {
            this.window = window;
            RefreshItemList();
            RefreshSuggestionList();

            zoneSelections = new bool[window.zoneList.Count];
        }

        public void Draw()
        {
            EditorGUILayout.LabelField("Items", EditorStyles.boldLabel);

            RefreshItemList();
            RefreshSuggestionList();

            DrawItemList();

            EditorGUILayout.LabelField("Suggestions", EditorStyles.boldLabel);

            DrawSuggestionList();

            if (selectedSuggestionIndex >= 0)
            {
                EditorGUILayout.Space(15);
                DrawItemCreationPanel();
            }
        }

        private void DrawItemList()
        {
            if (itemList.Count == 0)
            {
                EditorGUILayout.HelpBox("Cant find ItemSo", MessageType.Info);
                return;
            }

            itemScroll = EditorGUILayout.BeginScrollView(itemScroll, GUILayout.Height(150));

            foreach (var item in itemList)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField(item.name, item, typeof(ItemSo), false);
                EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.EndScrollView();
        }

        private void RefreshItemList()
        {
            itemList.Clear();

            if (!Directory.Exists(window.itemPath))
                return;

            var files = Directory.GetFiles(window.itemPath, "*.asset");

            foreach (var f in files)
            {
                var so = AssetDatabase.LoadAssetAtPath<ItemSo>(f);
                if (so)
                    itemList.Add(so);
            }
        }

        private void DrawSuggestionList()
        {
            if (suggestionSprites.Count == 0)
            {
                EditorGUILayout.HelpBox("No suggestion.", MessageType.Info);
                return;
            }

            suggestionScroll = EditorGUILayout.BeginScrollView(suggestionScroll, GUILayout.Height(150));

            for (var i = 0; i < suggestionSprites.Count; i++)
            {
                var s = suggestionSprites[i];

                var style = new GUIStyle(GUI.skin.button)
                {
                    alignment = TextAnchor.MiddleLeft
                };

                if (GUILayout.Button(s.name, style))
                {
                    selectedSuggestionIndex = i;
                    newItemHeader = s.name;
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void RefreshSuggestionList()
        {
            suggestionSprites.Clear();

            if (!Directory.Exists(window.iconPath))
                return;

            List<Sprite> allSprites = new();
            var guids = AssetDatabase.FindAssets("t:Sprite", new[] { window.iconPath });

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var s = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                if (s)
                    allSprites.Add(s);
            }

            HashSet<Sprite> usedSprites = new();
            foreach (var item in itemList)
            {
                if (item.ItemImage)
                    usedSprites.Add(item.ItemImage);
            }

            foreach (var sprite in allSprites)
            {
                if (!usedSprites.Contains(sprite))
                    suggestionSprites.Add(sprite);
            }

        }

        private void DrawItemCreationPanel()
        {
            if (selectedSuggestionIndex < 0 || selectedSuggestionIndex >= suggestionSprites.Count)
                return;

            var sprite = suggestionSprites[selectedSuggestionIndex];

            EditorGUILayout.LabelField("Create Item", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Item Image", sprite, typeof(Sprite), false);
            EditorGUI.EndDisabledGroup();

            newItemHeader = EditorGUILayout.TextField("Header", newItemHeader);

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Assign to Zones:", EditorStyles.boldLabel);

            for (var i = 0; i < window.zoneList.Count; i++)
            {
                zoneSelections[i] = EditorGUILayout.ToggleLeft(window.zoneList[i].name, zoneSelections[i]);
            }

            EditorGUILayout.Space(10);

            if (GUILayout.Button("Add Item", GUILayout.Height(30)))
            {
                CreateItem(sprite);
            }
        }

        private void CreateItem(Sprite sprite)
        {
            if (string.IsNullOrWhiteSpace(newItemHeader))
            {
                EditorUtility.DisplayDialog("Error", "Header cannot be empty!", "OK");
                return;
            }

            var fileName = newItemHeader + ".asset";
            var fullPath = Path.Combine(window.itemPath, fileName).Replace("\\", "/");
            fullPath = AssetDatabase.GenerateUniqueAssetPath(fullPath);

            var item = ScriptableObject.CreateInstance<ItemSo>();
            item.ItemHeader = newItemHeader;
            item.ItemImage = sprite;

            AssetDatabase.CreateAsset(item, fullPath);
            AssetDatabase.SaveAssets();

            for (var i = 0; i < window.zoneList.Count; i++)
            {
                if (zoneSelections[i])
                {
                    var zone = window.zoneList[i];
                    if (!zone.Items.Contains(item))
                        zone.Items.Add(item);

                    EditorUtility.SetDirty(zone);
                }
            }

            AssetDatabase.SaveAssets();

            RefreshItemList();
            RefreshSuggestionList();

            selectedSuggestionIndex = -1;
            newItemHeader = "";

            EditorUtility.SetDirty(item);
        }
    }
}
