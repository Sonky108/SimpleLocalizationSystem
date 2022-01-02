using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor
{
	public class Window : EditorWindow
	{
		private readonly Color _darkerColor = Color.white * 0.1f;
		private readonly Color _lighterColor = Color.white * 0.3f;
		private List<MultiColumnHeaderState.Column> _columns = new List<MultiColumnHeaderState.Column>();
		private MultiColumnHeader _multiColumnHeader;
		private MultiColumnHeaderState _multiColumnHeaderState;
		private Vector2 _scrollPosition;
		public Backend Backend;

		public void Start()
		{
			_columns.Add(new MultiColumnHeaderState.Column
			{
				autoResize = true,
				minWidth = 50,
				canSort = true,
				sortingArrowAlignment = TextAlignment.Right,
				headerTextAlignment = TextAlignment.Left,
				headerContent = new GUIContent("Key")
			});
			
			foreach (var x in Backend._localeFileByCultureInfo)
			{
				_columns.Add(new MultiColumnHeaderState.Column
				{
					autoResize = true,
					minWidth = 50,
					canSort = true,
					sortingArrowAlignment = TextAlignment.Right,
					headerTextAlignment = TextAlignment.Left,
					headerContent = new GUIContent(x.Key.TwoLetterISOLanguageName)
				});
			}
			
			this._multiColumnHeaderState = new MultiColumnHeaderState(_columns.ToArray());

			this._multiColumnHeader = new MultiColumnHeader(state: this._multiColumnHeaderState);

			// When we chagne visibility of the column we resize columns to fit in the window.
			this._multiColumnHeader.visibleColumnsChanged += (multiColumnHeader) => multiColumnHeader.ResizeToFit();

			// Initial resizing of the content.
			this._multiColumnHeader.ResizeToFit();
		}

		private void OnGUI()
		{
			//EditorGUILayout.TextArea(Backend.LocaleFilesCount.ToString());

			GUILayout.FlexibleSpace();
			Rect windowRect = GUILayoutUtility.GetLastRect();

			windowRect.width = position.width;
			windowRect.height = position.height;

			float columnHeight = EditorGUIUtility.singleLineHeight;

			Rect columnRectPrototype = new Rect(windowRect)
			{
				height = columnHeight, // This is basically a height of each column including header.
			};

			// Just enormously large view if you want it to span for the whole window. This is how it works [shrugs in confusion].
			Rect positionalRectAreaOfScrollView = GUILayoutUtility.GetRect(0, float.MaxValue, 0, float.MaxValue);

			// Create a `viewRect` since it should be separate from `rect` to avoid circular dependency.
			Rect viewRect = new Rect(windowRect)
			{
				xMax = _columns.Sum(column => column.width) // Scroll max on X is basically a sum of width of columns.
			};

			_scrollPosition = GUI.BeginScrollView(positionalRectAreaOfScrollView, _scrollPosition, viewRect, false, false);

			// Draw header for columns here.
			_multiColumnHeader.OnGUI(columnRectPrototype, 0.0f);

			int languange = -1;
			// For each element that we have in object that we are modifying.
			//? I don't have an appropriate object here to modify, but this is just an example. In real world case I would probably use ScriptableObject here.
			for (int a = 0; a < this.Backend.Keys.Count; a++)
			{
				languange++;
				
				Rect rowRect = new Rect(columnRectPrototype);

				rowRect.y += columnHeight * (a + 1);

				// Draw a texture before drawing each of the fields for the whole row.
				if (a % 2 == 0)
				{
					EditorGUI.DrawRect(rowRect, _darkerColor);
				}
				else
				{
					EditorGUI.DrawRect(rowRect, _lighterColor);
				}

				// Name field.

				int columnIndex = 0;

				if (_multiColumnHeader.IsColumnVisible(columnIndex))
				{
					int visibleColumnIndex = _multiColumnHeader.GetVisibleColumnIndex(columnIndex);

					Rect columnRect = _multiColumnHeader.GetColumnRect(visibleColumnIndex);

					columnRect.y = rowRect.y;

					GUIStyle nameFieldGUIStyle = new GUIStyle(GUI.skin.label) {padding = new RectOffset(10, 10, 2, 2)};

					EditorGUI.LabelField(_multiColumnHeader.GetCellRect(visibleColumnIndex, columnRect), new GUIContent(Backend.Keys[a]), nameFieldGUIStyle);
					columnIndex++;
				}


				foreach (var x in Backend.Languages)
				{
					if (_multiColumnHeader.IsColumnVisible(columnIndex))
					{
						int visibleColumnIndex = _multiColumnHeader.GetVisibleColumnIndex(columnIndex);

						Rect columnRect = _multiColumnHeader.GetColumnRect(visibleColumnIndex);

						// This here basically is a row height, you can make it any value you like. Or you could calculate the max field height here that your object has and store it somewhere then use it here instead of `EditorGUIUtility.singleLineHeight`.
						// We move position of field on `y` by this height to get correct position.
						columnRect.y = rowRect.y;

						GUIStyle nameFieldGUIStyle = new GUIStyle(GUI.skin.label) {padding = new RectOffset(10, 10, 2, 2)};

						EditorGUI.LabelField(_multiColumnHeader.GetCellRect(visibleColumnIndex, columnRect), new GUIContent(Backend.Data[x].Data[Backend.Keys[a]]), nameFieldGUIStyle);
						columnIndex++;
					}
				}
				
			}

			GUI.EndScrollView(true);
		}
	}
}