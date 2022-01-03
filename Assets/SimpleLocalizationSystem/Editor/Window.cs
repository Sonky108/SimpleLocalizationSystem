using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SimpleLocalizationSystem.Editor.Utils;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor
{
	public class Window : EditorWindow
	{
		private readonly List<MultiColumnHeaderState.Column> _columns = new List<MultiColumnHeaderState.Column>();
		private MultiColumnHeader _multiColumnHeader;
		private MultiColumnHeaderState _multiColumnHeaderState;
		private Vector2 _scrollPosition;
		private SearchField _searchBar;
		private string _searchText;
		private IWindowSkin _skin;
		public Backend Backend;

		private void OnGUI()
		{
			Rect windowRect = new Rect {x = 0, y = 0, width = position.width, height = position.height};

			DrawToolbar(ref windowRect);
			DrawLocaleScrollArea(ref windowRect);
		}

		private void DrawToolbar(ref Rect rect)
		{
			Rect toolBarRect = new Rect(rect) {height = EditorGUIUtility.singleLineHeight};

			_searchText = _searchBar.OnToolbarGUI(toolBarRect, _searchText);

			UseMainRectHeight(ref rect, toolBarRect.height);
		}

		private void DrawLocaleScrollArea(ref Rect rect)
		{
			float columnHeight = EditorGUIUtility.singleLineHeight;

			//header
			Rect columnRectPrototype = new Rect(rect)
			{
				height = columnHeight,
			};

			_multiColumnHeader.OnGUI(columnRectPrototype, _scrollPosition.x);

			UseMainRectHeight(ref rect, columnRectPrototype.height);

			int visibleRows = -1;

			Rect viewRect = new Rect(rect)
			{
				width = _columns.Sum(column => column.width), 
				height = Backend.Keys.Count * EditorGUIUtility.singleLineHeight
			};

			_scrollPosition = GUI.BeginScrollView(rect, _scrollPosition, viewRect);

			foreach (string key in Backend.Keys)
			{
				var isRowVisible = Backend.Languages.All(x => IsRowVisible(key, x, Backend.Data[x].Data[key]));

				if (isRowVisible)
				{
					visibleRows++;
				}
				else
				{
					continue;
				}
				
				Rect rowRect = new Rect(columnRectPrototype);

				rowRect.y += columnHeight * (visibleRows + 1);

				EditorGUI.DrawRect(rowRect, visibleRows % 2 == 0 ? _skin.DarkRowBackground : _skin.LightRowBackground);

				int columnIndex = 0;

				if (_multiColumnHeader.IsColumnVisible(columnIndex))
				{
					DrawCell(columnIndex, rowRect, new GUIContent(key));
				}

				columnIndex++;

				foreach (CultureInfo x in Backend.Languages)
				{
					if (_multiColumnHeader.IsColumnVisible(columnIndex))
					{
						DrawCell(columnIndex, rowRect, new GUIContent(Backend.Data[x].Data[key]));
					}

					columnIndex++;
				}
			}

			GUI.EndScrollView(true);
		}

		private bool IsRowVisible(string key, CultureInfo cultureInfo, string value)
		{
			if (string.IsNullOrEmpty(_searchText))
			{
				return true;
			}

			string[] keywords = _searchText.Split(new[]{':'}, 2);

			if (keywords[0] == "key")
			{
				return key.Contains(_searchText, StringComparison.CurrentCultureIgnoreCase);
			}

			if (keywords.Length > 1 && cultureInfo.TwoLetterISOLanguageName == keywords[0])
			{
				return value.Contains(keywords[1], StringComparison.CurrentCultureIgnoreCase);
			}

			return key.Contains(_searchText, StringComparison.CurrentCultureIgnoreCase) || value.Contains(_searchText, StringComparison.CurrentCultureIgnoreCase);
		}
		
		private static void UseMainRectHeight(ref Rect rect, float height)
		{
			Vector2 rectPosition = rect.position;
			rectPosition.y += height;
			rect.height -= EditorGUIUtility.singleLineHeight;
			rect.position = rectPosition;
		}

		private void DrawCell(int columnIndex, Rect rowRect, GUIContent content)
		{
			int visibleColumnIndex = _multiColumnHeader.GetVisibleColumnIndex(columnIndex);

			Rect columnRect = _multiColumnHeader.GetColumnRect(visibleColumnIndex);

			columnRect.y = rowRect.y;

			EditorGUI.LabelField(_multiColumnHeader.GetCellRect(visibleColumnIndex, columnRect), content, _skin.CellStyle);
		}

		public void Start()
		{
			_skin = new WindowSkin();
			
			_searchBar = new SearchField {autoSetFocusOnFindCommand = true};
			
			_columns.Add(new MultiColumnHeaderState.Column
			{
				autoResize = true,
				minWidth = 50,
				canSort = false,
				headerTextAlignment = TextAlignment.Left,
				headerContent = new GUIContent("Key"),
				allowToggleVisibility = false
			});

			foreach (CultureInfo x in Backend.Languages)
			{
				_columns.Add(new MultiColumnHeaderState.Column
				{
					autoResize = true,
					minWidth = 50,
					canSort = false,
					headerTextAlignment = TextAlignment.Left,
					headerContent = new GUIContent(x.Name)
				});
			}

			_multiColumnHeaderState = new MultiColumnHeaderState(_columns.ToArray());
			_multiColumnHeader = new MultiColumnHeader(_multiColumnHeaderState) {canSort = true};

			_multiColumnHeader.ResizeToFit();
		}
	}
}