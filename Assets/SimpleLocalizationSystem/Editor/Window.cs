using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SimpleLocalizationSystem.Editor.ProjectSettings;
using SimpleLocalizationSystem.Editor.Utils;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor
{
	public class Window : EditorWindow
	{
		private readonly float _bottomAreaHeight = 100;
		private readonly List<MultiColumnHeaderState.Column> _columns = new List<MultiColumnHeaderState.Column>();
		private readonly float _newKeyBarHeight = 25f;
		private readonly float _buttonHeight = 30;
		private readonly float _buttonWidth = 150;
		private bool _editorQuitting;
		private int _index;
		private MultiColumnHeader _multiColumnHeader;
		private MultiColumnHeaderState _multiColumnHeaderState;
		private string _newKeyText;
		private Vector2 _scrollPosition;
		private SearchField _searchBar;
		private string _searchText;
		private IWindowSkin _skin;
		public Backend Backend;

		private void OnDestroy()
		{
			if (Backend != null && !_editorQuitting)
			{
				Backend.Error -= OnError;
				Backend.Clear();
			}
		}

		private void OnGUI()
		{
			if (Backend == null && Backend.CanRestore())
			{
				Backend = Backend.Restore();
				Start();
			}

			Rect windowRect = new Rect {x = 0, y = 0, width = position.width, height = position.height};

			DrawToolbar(ref windowRect);
			DrawLocaleScrollArea(ref windowRect);
			DrawBottomArea(ref windowRect);
		}

		private void DrawBottomArea(ref Rect rect)
		{
			GUI.Box(rect, "", _skin.BottomAreaStyle);

			if (Backend.Languages.Count != 0)
			{
				DrawNewKeyArea(ref rect);
			}

			Rect buttonRect = new Rect(rect) {y = rect.height / 2f + rect.y - _buttonHeight / 2, height = _buttonHeight, width = _buttonWidth, x = rect.width - _buttonWidth};

			if (GUI.Button(buttonRect, "Export"))
			{
				Backend.Export();
			}

			buttonRect.x -= _buttonWidth;

			if (GUI.Button(buttonRect, "Add new language"))
			{
				CulturesDropdown foo = new CulturesDropdown(new AdvancedDropdownState(), Backend.Languages, OnAddNewLanguage);
				foo.Show(buttonRect);
			}
		}

		private void OnAddNewLanguage(string name)
		{
			if (Backend.AddNewLanguage(name))
			{
				AddLanguageHeader(name);
				OnHeaderReady();
			}
		}

		private void DrawNewKeyArea(ref Rect windowRect)
		{
			Rect newKeyRect = new Rect(windowRect) {height = _newKeyBarHeight};

			newKeyRect.width -= _newKeyBarHeight;
			newKeyRect.x += _newKeyBarHeight;

			UseMainRectHeight(ref windowRect, newKeyRect.height);

			_newKeyText = GUI.TextField(newKeyRect, _newKeyText);

			newKeyRect.width = _newKeyBarHeight;
			newKeyRect.height = _newKeyBarHeight;
			newKeyRect.x = 0;

			if (GUI.Button(newKeyRect, _skin.AddButton))
			{
				if (Backend.TryAddKey(_newKeyText))
				{
					_newKeyText = "";
				}
			}
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

			Rect columnRectPrototype = new Rect(rect) {height = columnHeight,};

			_multiColumnHeader.OnGUI(columnRectPrototype, _scrollPosition.x);

			UseMainRectHeight(ref rect, columnRectPrototype.height);

			int visibleRows = -1;

			Rect contentRect = new Rect(rect) {width = _columns.Sum(column => column.width), height = Backend.Keys.Count * EditorGUIUtility.singleLineHeight};

			Rect scrollRect = new Rect(rect) {height = rect.height - _bottomAreaHeight};

			if (scrollRect.height <= 0)
			{
				return;
			}

			_scrollPosition = GUI.BeginScrollView(scrollRect, _scrollPosition, contentRect);

			foreach (string key in Backend.Keys)
			{
				bool isRowVisible = Backend.Languages.All(x => IsRowVisible(key, x, Backend.Data[x].Data[key]));

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

			UseMainRectHeight(ref rect, scrollRect.height);
		}

		private bool IsRowVisible(string key, CultureInfo cultureInfo, string value)
		{
			if (string.IsNullOrEmpty(_searchText))
			{
				return true;
			}

			string[] keywords = _searchText.Split(new[] {':'}, 2);

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
			rect.height -= height;
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
			Backend.Error += OnError;
			EditorApplication.wantsToQuit += OnEditorWantsToQuit;

			_skin = new WindowSkin();

			_searchBar = new SearchField {autoSetFocusOnFindCommand = true};
			_columns.Clear();
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
				AddLanguageHeader(x.DisplayName);
			}

			OnHeaderReady();
		}

		private bool OnEditorWantsToQuit()
		{
			_editorQuitting = true;
			return true;
		}

		private void OnHeaderReady()
		{
			_multiColumnHeaderState = new MultiColumnHeaderState(_columns.ToArray());
			_multiColumnHeader = new MultiColumnHeader(_multiColumnHeaderState) {canSort = true};
			_multiColumnHeader.ResizeToFit();
		}

		private void AddLanguageHeader(string name)
		{
			_columns.Add(new MultiColumnHeaderState.Column
			{
				autoResize = true,
				minWidth = 50,
				canSort = false,
				headerTextAlignment = TextAlignment.Left,
				headerContent = new GUIContent(name)
			});
		}

		private void OnError(int obj)
		{
			Debug.LogError(obj);
		}
	}
}