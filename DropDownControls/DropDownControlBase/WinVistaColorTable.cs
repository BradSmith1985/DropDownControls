using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

/// <summary>
/// Holds some frequently-used <see cref="Color"/> constants that apply to Windows Vista and above.
/// </summary>
internal static class WinVistaColorTable {

	/// <summary>
	/// The pressed border color.
	/// </summary>
	public static readonly Color PressedBorder = Color.FromArgb(0, 84, 153);
	/// <summary>
	/// The hot border color.
	/// </summary>
	public static readonly Color HotBorder = Color.FromArgb(0, 120, 215);
	/// <summary>
	/// The pressed background color.
	/// </summary>
	public static readonly Color PressedBackground = Color.FromArgb(204, 228, 247);
	/// <summary>
	/// The hot background color.
	/// </summary>
	public static readonly Color HotBackground = Color.FromArgb(229, 241, 251);
}
