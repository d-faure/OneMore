﻿//************************************************************************************************
// Copyright © 2024 Steven M Cohn. All rights reserved.
//************************************************************************************************

namespace River.OneMoreAddIn.UI
{
	using System.Windows.Forms;


	internal class MoreLabel : Label, IThemedControl
	{

		public string ThemedBack { get; set; }

		public string ThemedFore { get; set; }


		public void ApplyTheme(ThemeManager manager)
		{
			BackColor = Parent != null
				? Parent.BackColor
				: manager.GetThemedColor("Control", ThemedBack);

			ForeColor = Enabled
				? manager.GetThemedColor("ControlText", ThemedFore)
				: manager.GetThemedColor("GrayText");
		}
	}
}
