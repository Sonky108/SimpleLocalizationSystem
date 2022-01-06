using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor.IMGUI.Controls;

namespace SimpleLocalizationSystem.Editor
{
	public class CulturesDropdown : AdvancedDropdown
	{
		private readonly List<CultureInfo> _blacklistedCultures;
		private readonly Action<string> _cultureSelected;
		private readonly Dictionary<int, CultureInfo> _cultureByID;

		public CulturesDropdown(AdvancedDropdownState state, List<CultureInfo> blacklistedCultures, Action<string> cultureSelected) : base(state)
		{
			_cultureByID = new Dictionary<int, CultureInfo>();
			_blacklistedCultures = blacklistedCultures;
			_cultureSelected = cultureSelected;
		}
		protected override AdvancedDropdownItem BuildRoot()
		{
			var root = new AdvancedDropdownItem("Languages");
			
			foreach (var x in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
			{
				if (!_blacklistedCultures.Contains(x))
				{
					AdvancedDropdownItem advancedDropdownItem = new AdvancedDropdownItem(x.DisplayName);

					if (!_cultureByID.ContainsKey(advancedDropdownItem.id))
					{
						_cultureByID.Add(advancedDropdownItem.id, x);
						root.AddChild(advancedDropdownItem);
					}
				}
			}

			return root;
		}

		protected override void ItemSelected(AdvancedDropdownItem item)
		{
			_cultureSelected?.Invoke(_cultureByID[item.id].Name);
		}
	}
}