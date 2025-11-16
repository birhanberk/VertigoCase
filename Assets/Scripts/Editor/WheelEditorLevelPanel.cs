using System.Collections.Generic;
using System.Linq;
using Level;
using UI.Item;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Editor
{
    public class WheelEditorLevelPanel : IWheelEditorPanel
    {
        private readonly WheelEditorToolWindow window;

        private Vector2 zoneScroll;
        private Vector2 levelScroll;

        private int selectedLevelIndex = -1;

        public WheelEditorLevelPanel(WheelEditorToolWindow window)
        {
            this.window = window;

            if (window.LevelReorderableList != null)
            {
                window.LevelReorderableList.onSelectCallback = OnLevelSelected;
            }
        }

        public void Draw()
        {
            if (!window.levelList)
            {
                EditorGUILayout.HelpBox("Cant find LevelSettingsSo", MessageType.Error);
                return;
            }
            EditorGUILayout.LabelField("Levels:", EditorStyles.boldLabel);
            DrawLevelList();

            DrawSelectedLevelItemsPanel();
        }

        private void DrawLevelList()
        {
            var list = window.LevelReorderableList;

            if (list == null)
            {
                EditorGUILayout.HelpBox("LevelSettingsSo not contains level", MessageType.Info);
                return;
            }

            levelScroll = EditorGUILayout.BeginScrollView(levelScroll, GUILayout.Height(200)); 
            list.DoLayoutList();
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Add New Level", GUILayout.Height(30)))
            {
                window.CreateNewLevelAsset();
            }

            GUI.enabled = window.levelList.LevelList.Count > 0;
            if (GUILayout.Button("Delete Last Level", GUILayout.Height(30)))
            {
                window.DeleteLastLevelAsset();
            }
            GUI.enabled = true;
        }

        private void OnLevelSelected(ReorderableList list)
        {
            selectedLevelIndex = list.index;
        }

        private void DrawSelectedLevelItemsPanel()
        {
            if (window.LevelReorderableList != null)
                selectedLevelIndex = window.LevelReorderableList.index;
            
            if (selectedLevelIndex < 0 ||
                selectedLevelIndex >= window.levelList.LevelList.Count)
                return;

            var selectedLevel = window.levelList.LevelList[selectedLevelIndex];
            if (!selectedLevel) return;
            
            if (selectedLevel.ZoneSo && selectedLevel.ZoneSo.UseBomb)
            {
                EditorGUILayout.HelpBox(
                    "At least one item slot must remain empty for Bomb.",
                    MessageType.Error
                );
            }

            Ensure8Slots(selectedLevel.Items);

            EnsureOneEmptySlot(selectedLevel);

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField($"Items for {selectedLevel.name}", EditorStyles.boldLabel);

            var panelSize = 300f;
            var radius = 125f;
            var center = new Vector2(panelSize / 2f, panelSize / 2f);

            var rect = GUILayoutUtility.GetRect(panelSize, panelSize, GUILayout.ExpandWidth(true));
            rect.x = (EditorGUIUtility.currentViewWidth - panelSize) * 0.5f;

            Handles.BeginGUI();

            for (var i = 0; i < 8; i++)
            {
                var angle = (360f / 8f) * i * Mathf.Deg2Rad - Mathf.PI / 2;
                var x = center.x + Mathf.Cos(angle) * radius;
                var y = center.y + Mathf.Sin(angle) * radius;

                var slotRect = new Rect(
                    rect.x + x - 45f,
                    rect.y + y - 20f,
                    90f,
                    40f
                );

                DrawItemSlot(slotRect, selectedLevel, i);
            }

            Handles.EndGUI();
        }

        private void Ensure8Slots(List<ItemSo> items)
        {
            while (items.Count < 8)
                items.Add(null);

            while (items.Count > 8)
                items.RemoveAt(items.Count - 1);
        }

        private void EnsureOneEmptySlot(LevelSo level)
        {
            if (!level.ZoneSo)
                return;

            if (!level.ZoneSo.UseBomb)
                return;

            var hasEmpty = false;

            foreach (var item in level.Items)
            {
                if (!item)
                {
                    hasEmpty = true;
                    break;
                }
            }

            if (hasEmpty)
                return;

            var index = Random.Range(0, 8);
            level.Items[index] = null;

            EditorUtility.SetDirty(level);
            AssetDatabase.SaveAssets();
        }

        private void DrawItemSlot(Rect rect, LevelSo level, int index)
        {
            var zone = level.ZoneSo;
            if (!zone)
            {
                EditorGUI.HelpBox(rect, "No Zone!", MessageType.Warning);
                return;
            }

            var zoneItems = zone.Items; 
            if (zoneItems == null)
            {
                EditorGUI.HelpBox(rect, "Zone has no item list!", MessageType.Warning);
                return;
            }

            var displayNames = new List<string> { "(Empty)" };

            displayNames.AddRange(zoneItems.Select(item => item ? item.ItemHeader : "(null)"));

            var currentIndex = 0;
            var currentItem = level.Items[index];

            if (currentItem)
            {
                var zoneIndex = zoneItems.IndexOf(currentItem);
                if (zoneIndex >= 0)
                    currentIndex = zoneIndex + 1;
            }

            var newIndex = EditorGUI.Popup(rect, currentIndex, displayNames.ToArray());

            if (newIndex != currentIndex)
            {
                if (newIndex == 0)
                    level.Items[index] = null;
                else
                    level.Items[index] = zoneItems[newIndex - 1]; 

                EditorUtility.SetDirty(level);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
