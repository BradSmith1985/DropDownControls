using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.IO;
using System.Drawing.Drawing2D;

/// <summary>
/// Drop-down that is attached to a time picker control. 
/// Allows interactive selection of a time of day value.
/// </summary>
/// <typeparam name="TSource">Any <see cref="Control"/> that implements the <see cref="ITimePicker"/> interface.</typeparam>
[ToolboxItem(false), DesignerCategory("")]
public class TimePickerDropDown<TSource> : ToolStripDropDown where TSource : Control, ITimePicker {

	TSource _sourceControl;
	bool _processKeys;
	bool _selectMinutes;
	bool _autoSelectMinutes;
	Font _clockFont;
	Point _mouseDownPosition;
	bool _isDragging;
	TimeSpan? _value;

	/// <summary>
	/// Gets or sets a value indicating whether to process keyboard events.
	/// </summary>
	[DefaultValue(true)]
	public bool ProcessKeys {
		get { return _processKeys; }
		set { _processKeys = value; }
	}
	/// <summary>
	/// Gets or sets the uncommitted time of day value in the dropdown. 
	/// The value is committed to the source control when the dropdown closes.
	/// </summary>
	public TimeSpan? Value {
		get { return _value; }
		set {
			if (_value != value) {
				_value = value;
				OnValueChanged();
				Invalidate();
			}
		}
	}

	/// <summary>
	/// Fired when the <see cref="Value"/> property changes.
	/// </summary>
	public event EventHandler ValueChanged;
	/// <summary>
	/// Raises the <see cref="ValueChanged"/> event.
	/// </summary>
	protected virtual void OnValueChanged() {
		if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
	}

	/// <summary>
	/// Initialises a new instance of the <see cref="TimePickerDropDown{TSource}"/> class using the specified source control.
	/// </summary>
	/// <param name="sourceControl"></param>
	public TimePickerDropDown(TSource sourceControl) {
		_sourceControl = sourceControl;
		_processKeys = true;

		AutoSize = false;
		RenderMode = ToolStripRenderMode.System;
		BackColor = Color.White;
		Size = new Size(160, 220);
		
		Items.Add("");
		CreateClockFont();
	}

	protected override void Dispose(bool disposing) {
		if (disposing && (_clockFont != null)) {
			_clockFont.Dispose();
			_clockFont = null;
		}

		base.Dispose(disposing);
	}

	private void CreateClockFont() {
		if (_clockFont != null) {
			_clockFont.Dispose();
			_clockFont = null;
		}
		_clockFont = new Font(Font.Name, 8.25f, FontStyle.Regular);
	}

	/// <summary>
	/// Displays the dropdown beneath its owning <see cref="DropDownTimePicker"/> control.
	/// </summary>
	public void Open() {
		_selectMinutes = false;
		_autoSelectMinutes = true;
		_mouseDownPosition = Point.Empty;
		_isDragging = false;

		_value = _sourceControl.Value;

		// show above/below the source control
		Show(_sourceControl, CalcDropDownLocation());
	}

	/// <summary>
	/// Calculates the location of the dropdown depending on whether it needs to appear above or below the source control.
	/// </summary>
	/// <returns></returns>
	private Point CalcDropDownLocation() {
		Point location = new Point(0, _sourceControl.ClientRectangle.Height);
		Rectangle workingArea = Screen.FromControl(_sourceControl).WorkingArea;

		if ((_sourceControl.PointToScreen(location).Y + Height) > workingArea.Bottom) {
			return Point.Add(location, new Size(0, -(Height + _sourceControl.ClientRectangle.Height)));
		}

		return location;
	}

	/// <summary>
	/// Registers input keys for the control.
	/// </summary>
	/// <param name="keyData"></param>
	/// <returns></returns>
	protected override bool IsInputKey(Keys keyData) {
		switch (keyData & ~(Keys.Shift | Keys.Alt | Keys.Control)) {
			case Keys.Up:
			case Keys.Down:
			case Keys.Left:
			case Keys.Right:			
			case Keys.Enter:
			case Keys.Delete:
				return true;
			default:
				return base.IsInputKey(keyData);
		}
	}

	/// <summary>
	/// Prevents the clicking of items from closing the dropdown.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnClosing(ToolStripDropDownClosingEventArgs e) {
		if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked) e.Cancel = true;
		if (e.CloseReason == ToolStripDropDownCloseReason.AppClicked) {
			if (_sourceControl.ClientRectangle.Contains(_sourceControl.PointToClient(Cursor.Position))) e.Cancel = true;
		}

		base.OnClosing(e);
	}

	/// <summary>
	/// Determines the hour represented when the mouse pointer is at the specified location.
	/// </summary>
	/// <param name="p"></param>
	/// <returns></returns>
	private int GetHourAtPoint(Point p) {
		Point o = GetOrigin();
		double distance = Math.Sqrt(Math.Pow(p.X - o.X, 2) + Math.Pow(p.Y - o.Y, 2));

		if (distance != 0) {
			double radians = Math.Acos((o.Y - p.Y) / distance);
			double angle = radians * (180 / Math.PI);

			if (p.X < o.X) {
				angle = (360 - angle) + (360 / 24);
			}
			else {
				angle += (360 / 24);
			}

			int hour = (int)(angle / (360 / 12));
			if (hour <= 6)
				return hour + 12;
			else
				return hour;
		}
		else {
			return 12;
		}
	}

	/// <summary>
	/// Determines the minute represented when the mouse pointer is at the specified location.
	/// </summary>
	/// <param name="p"></param>
	/// <returns></returns>
	private int GetMinuteAtPoint(Point p) {
		Point o = GetOrigin();
		double distance = Math.Sqrt(Math.Pow(p.X - o.X, 2) + Math.Pow(p.Y - o.Y, 2));

		if (distance != 0) {
			double radians = Math.Acos((o.Y - p.Y) / distance);
			double angle = radians * (180 / Math.PI);

			if (p.X < o.X) angle = (360 - angle);

			return (int)(angle / (360 / 60));
		}
		else {
			return 0;
		}
	}

	private Rectangle GetHHBounds() {
		return new Rectangle(
			6 + ((ClientSize.Width - 6) - 64) / 2,
			6,
			32,
			24
		);
	}

	private Rectangle GetMMBounds() {
		return new Rectangle(
			6 + 32 + ((ClientSize.Width - 6) - 64) / 2,
			6,
			32,
			24
		);
	}

	private Rectangle GetClockBounds() {
		return new Rectangle(
			6, 
			6 + 24 + 6, 
			ClientSize.Width - 12, 
			ClientSize.Width - 12
		);
	}

	private Rectangle GetAMBounds() {
		return new Rectangle(
			6,
			ClientSize.Width + 6 + 24,
			32,
			24
		);
	}

	private Rectangle GetPMBounds() {
		return new Rectangle(
			6 + 32,
			ClientSize.Width + 6 + 24,
			32,
			24
		);
	}

	private Rectangle GetNowBounds() {
		return new Rectangle(
			ClientSize.Width - (6+64),
			ClientSize.Width + 6 + 24,
			64,
			24
		);
	}

	private Point GetOrigin() {
		Rectangle r = GetClockBounds();
		r.Width--;
		r.Height--;

		r.X += 2;
		r.Y += 2;
		r.Width -= 4;
		r.Height -= 4;

		return new Point(r.X + (r.Width) / 2, r.Y + (r.Height / 2));
	}

	private Point GetRadialPoint(Point origin, double angle, double radius) {
		double radians = angle * (Math.PI / 180);

		double x = Math.Sin(radians) * radius;
		double y = Math.Cos(radians) * radius;

		return new Point(origin.X + (int)x, origin.Y - (int)y);
	}

	/// <summary>
	/// Gets a value indicating whether the specified point is within the bounds of the clock face.
	/// </summary>
	/// <param name="p"></param>
	/// <returns></returns>
	private bool InClockBounds(Point p) {
		Rectangle r = GetClockBounds();

		if (r.Contains(p)) {
			Point center = new Point(r.X + (r.Width / 2), r.Y + (r.Height / 2));

			double _xRadius = r.Width / 2;
			double _yRadius = r.Height / 2;

			if (_xRadius <= 0.0 || _yRadius <= 0.0) return false;

			/* This is a more general form of the circle equation
             *
             * X^2/a^2 + Y^2/b^2 <= 1
             */

			Point normalized = new Point(p.X - center.X, p.Y - center.Y);

			return ((double)(normalized.X * normalized.X) / (_xRadius * _xRadius)) + ((double)(normalized.Y * normalized.Y) / (_yRadius * _yRadius)) <= 1.0;
		}

		return false;
	}

	/// <summary>
	/// Paints the contents of the drop-down.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnPaint(PaintEventArgs e) {
		base.OnPaint(e);		

		Graphics g = e.Graphics;
		g.SmoothingMode = SmoothingMode.AntiAlias;

		Rectangle r = GetClockBounds();
		r.Width--;
		r.Height--;

		Rectangle am = GetAMBounds();
		Rectangle pm = GetPMBounds();
		Rectangle now = GetNowBounds();
		Rectangle hh = GetHHBounds();
		Rectangle mm = GetMMBounds();

		// draw the clockface
		g.FillEllipse(SystemBrushes.Control, r);
		g.DrawEllipse(SystemPens.ControlDark, r);

		double radius = r.Width / 2;
		Point origin = new Point(r.X + (r.Width) / 2, r.Y + (r.Height / 2));
		using (GraphicsPath clipPath = new GraphicsPath()) {
			if (_value.HasValue) {
				// draw the hour and minute hands
				double hourAngle = (_value.Value.Hours % 12) * (360.0 / 12.0);
				double minAngle = (_value.Value.Minutes) * (360.0 / 60.0);
				Point p;

				Color color = SystemColors.Highlight;

				using (Pen pen = new Pen(color, 2f)) {
					pen.StartCap = pen.EndCap = LineCap.Round;

					if (_selectMinutes) {
						g.DrawLine(pen, origin, p = GetRadialPoint(origin, minAngle, radius - 12));
					}
					else {
						g.DrawLine(pen, origin, p = GetRadialPoint(origin, hourAngle, radius - 12));
					}
				}

				// outer ring
				Rectangle ring = new Rectangle(p.X - 12, p.Y - 12, 24, 24);
				g.FillEllipse(SystemBrushes.Highlight, ring);
				clipPath.AddEllipse(ring);

				// centre dot
				g.FillEllipse(SystemBrushes.Highlight, new Rectangle(origin.X - 3, origin.Y - 3, 6, 6));
			}

			// draw the hour/minute markings
			for (int i = 0; i < 12; i++) {
				double angle = i * (360 / 12);

				bool selected;
				string t;
				if (_selectMinutes) {
					t = (i == 0) ? "60" : (i * 5).ToString();
					selected = _value.HasValue && ((_value.Value.Minutes % 60) == (i * 5));
				}
				else {
					t = (i == 0) ? "12" : i.ToString();
					selected = _value.HasValue && ((_value.Value.Hours % 12) == i);
				}

				Point p = GetRadialPoint(origin, angle, radius - 12);
				Point mpos = PointToClient(Cursor.Position);
				bool hot = false;
				Rectangle ring = new Rectangle(p.X - 12, p.Y - 12, 24, 24);

				Action drawHotRing = () => {
					WithPen(WinVistaColorTable.HotBorder, pen => g.DrawLine(pen, origin, GetRadialPoint(origin, angle, radius - 12)));
					if (!_value.HasValue) WithBrush(WinVistaColorTable.HotBorder, b => g.FillEllipse(b, new Rectangle(origin.X - 3, origin.Y - 3, 6, 6)));
					WithBrush(WinVistaColorTable.HotBackground, b => g.FillEllipse(b, ring));
					
					ring.X++;
					ring.Y++;
					ring.Width -= 2;
					ring.Height -= 2;

					WithPen(WinVistaColorTable.HotBorder, pen => g.DrawEllipse(pen, ring));
				};

				if ((MouseButtons == MouseButtons.None) && InClockBounds(mpos)) {
					if (_selectMinutes)
						hot = Math.Abs((GetMinuteAtPoint(mpos) % 60) - (i * 5)) < 3;
					else
						hot = (GetHourAtPoint(mpos) % 12) == i;
				}

				Size s = TextRenderer.MeasureText(g, t, _clockFont);
				p.Offset((-s.Width / 2) + 1, (-s.Height / 2) - 1);

				if (_value.HasValue) {
					// text in the circle
					g.SetClip(clipPath);
					g.DrawString(t, _clockFont, SystemBrushes.HighlightText, p);
					g.ResetClip();

					// text outside the circle
					using (Region reg = new Region(clipPath)) {
						g.ExcludeClip(reg);
						if (hot) drawHotRing();
						g.DrawString(t, _clockFont, SystemBrushes.ControlText, p);
						g.ResetClip();
					}
				}
				else {
					// all text is outside the circle
					if (hot) drawHotRing();
					g.DrawString(t, _clockFont, SystemBrushes.ControlText, p);
				}
			}
		}

		if (!_value.HasValue) {
			TextFormatFlags tff = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
			r.Offset(1, 1);
			using (Font italicFont = new Font(_clockFont, FontStyle.Italic)) {
				TextRenderer.DrawText(g, _selectMinutes ? "Click to\nset minute" : "Click to\nset hour", italicFont, r, Color.Gray, tff);
			}
		}

		g.SmoothingMode = SmoothingMode.Default;

		// am/pm and now buttons
		bool isPM = _value.HasValue && (_value.Value.Hours >= 12);
		bool isAM = _value.HasValue && (_value.Value.Hours < 12);

		FillAndDrawWithText(g, !_selectMinutes, "HH", _clockFont, hh);
		FillAndDrawWithText(g, _selectMinutes, "mm", _clockFont, mm);
		FillAndDrawWithText(g, isAM, "AM", _clockFont, am);
		FillAndDrawWithText(g, isPM, "PM", _clockFont, pm);
		FillAndDrawWithText(g, false, "Now", _clockFont, now);
	}

	/// <summary>
	/// Fills and strokes a rectangle.
	/// </summary>
	/// <param name="g"></param>
	/// <param name="selected"></param>
	/// <param name="bounds"></param>
	private void FillAndDraw(Graphics g, bool selected, Rectangle bounds) {
		WithBrush(GetBackgroundColor(selected, bounds), b => g.FillRectangle(b, bounds));
		WithPen(GetBorderColor(selected, bounds), pen => g.DrawRectangle(pen, bounds));
	}

	/// <summary>
	/// Fills and strokes a rectangle, then draws text on top.
	/// </summary>
	/// <param name="g"></param>
	/// <param name="selected"></param>
	/// <param name="text"></param>
	/// <param name="font"></param>
	/// <param name="bounds"></param>
	private void FillAndDrawWithText(Graphics g, bool selected, string text, Font font, Rectangle bounds) {
		FillAndDraw(g, selected, bounds);
		TextRenderer.DrawText(g, text, font, bounds, GetForegroundColor(selected, bounds), TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
	}

	/// <summary>
	/// Executes an action using a <see cref="SolidBrush"/>.
	/// </summary>
	/// <param name="color"></param>
	/// <param name="action"></param>
	private void WithBrush(Color color, Action<Brush> action) {
		using (Brush b = new SolidBrush(color)) {
			action(b);
		}
	}

	/// <summary>
	/// Executes an action using a <see cref="Pen"/>.
	/// </summary>
	/// <param name="color"></param>
	/// <param name="action"></param>
	private void WithPen(Color color, Action<Pen> action) {
		using (Pen p = new Pen(color)) {
			action(p);
		}
	}

	/// <summary>
	/// Gets the border color to use when painting an object within the specified bounds.
	/// </summary>
	/// <param name="selected"></param>
	/// <param name="bounds"></param>
	/// <returns></returns>
	private Color GetBorderColor(bool selected, Rectangle bounds) {
		if (selected)
			return SystemColors.Highlight;
		else if (bounds.Contains(PointToClient(Cursor.Position)))
			return MouseButtons.HasFlag(MouseButtons.Left) ? WinVistaColorTable.PressedBorder : WinVistaColorTable.HotBorder;

		return SystemColors.ControlDark;
	}

	/// <summary>
	/// Gets the background color to use when painting an object within the specified bounds.
	/// </summary>
	/// <param name="selected"></param>
	/// <param name="bounds"></param>
	/// <returns></returns>
	private Color GetBackgroundColor(bool selected, Rectangle bounds) {
		if (selected)
			return SystemColors.Highlight;
		else if (bounds.Contains(PointToClient(Cursor.Position)))
			return MouseButtons.HasFlag(MouseButtons.Left) ? WinVistaColorTable.PressedBackground : WinVistaColorTable.HotBackground;

		return SystemColors.Control;
	}

	/// <summary>
	/// Gets the foreground color to use when painting an object within the specified bounds.
	/// </summary>
	/// <param name="selected"></param>
	/// <param name="bounds"></param>
	/// <returns></returns>
	private Color GetForegroundColor(bool selected, Rectangle bounds) {
		if (selected)
			return SystemColors.HighlightText;		

		return SystemColors.ControlText;
	}

	/// <summary>
	/// Updates the time of day value using the hour/minute at the specified point.
	/// </summary>
	/// <param name="p"></param>
	/// <param name="roundMins">Whether to round to the nearest 5 minutes.</param>
	private void UpdateValue(Point p, bool roundMins) {
		if (_selectMinutes) {
			int m = GetMinuteAtPoint(p);
			if (roundMins) {
				m = (int)(Math.Round(m / 5.0) * 5) % 60;
			}
			Value = new TimeSpan(_value.HasValue ? _value.Value.Hours : 0, m, 0);
		}
		else {
			int h = GetHourAtPoint(p);
			Value = new TimeSpan(h, _value.HasValue ? _value.Value.Minutes : 0, 0);
		}
	}

	/// <summary>
	/// Updates the clock font when the control's font changes. 
	/// The clock font always uses the same size and style.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnFontChanged(EventArgs e) {
		base.OnFontChanged(e);
		CreateClockFont();
	}	

	#region Keyboard Events

	/// <summary>
	/// Processes a key press event.
	/// </summary>
	/// <param name="keyCode"></param>
	/// <param name="modifiers"></param>
	/// <returns></returns>
	public bool ProcessKey(Keys keyCode, Keys modifiers) {
		if (keyCode == Keys.Enter) {
			Close();
		}
		else if (keyCode == Keys.Left) {
			if (_selectMinutes) {
				int m = _value.HasValue ? _value.Value.Minutes : 0;
				m -= modifiers.HasFlag(Keys.Shift) ? 5 : 1;
				if (m < 0) m = 59;
				Value = new TimeSpan(_value.HasValue ? _value.Value.Hours : 0, m, 0);
			}
			else {
				int h = _value.HasValue ? _value.Value.Hours : 0;
				h--;
				if (h < 0) h = 23;
				Value = new TimeSpan(h, _value.HasValue ? _value.Value.Minutes : 0, 0);
			}
		}
		else if (keyCode == Keys.Right) {
			if (_selectMinutes) {
				int m = _value.HasValue ? _value.Value.Minutes : 0;
				m += modifiers.HasFlag(Keys.Shift) ? 5 : 1;
				if (m > 59) m = 0;
				Value = new TimeSpan(_value.HasValue ? _value.Value.Hours : 0, m, 0);
			}
			else {
				int h = _value.HasValue ? _value.Value.Hours : 0;
				h++;
				if (h > 23) h = 0;
				Value = new TimeSpan(h, _value.HasValue ? _value.Value.Minutes : 0, 0);
			}
		}
		else if ((keyCode == Keys.Up) || (keyCode == Keys.Down)) {
			_selectMinutes = !_selectMinutes;
			Invalidate();
		}		
		else if (keyCode == Keys.Delete) {
			Value = null;
		}
		else {
			return false;
		}

		return true;
	}

	/// <summary>
	/// Handles keyboard shortcuts.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnKeyDown(KeyEventArgs e) {
		if (ProcessKeys) {
			if (ProcessKey(e.KeyCode, e.Modifiers)) {
				e.Handled = e.SuppressKeyPress = true;
			}
		}

		if (IsDisposed) return;

		base.OnKeyDown(e);
	}

	#endregion

	#region Mouse Events

	/// <summary>
	/// Handles dragging the clock hands and hot-tracking in response to movement of the mouse.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnMouseMove(MouseEventArgs e) {
		base.OnMouseMove(e);

		if ((e.Button == MouseButtons.Left) && InClockBounds(e.Location)) {
			if (!_isDragging) {
				int dist = (int)Math.Sqrt(Math.Pow((e.Location.X - _mouseDownPosition.X), 2) + Math.Pow((e.Location.Y - _mouseDownPosition.Y), 2));
				if (dist >= 4) _isDragging = true;
			}
			if (_isDragging) UpdateValue(e.Location, false);
		}

		Invalidate();
	}

	/// <summary>
	/// Handles visual state changes in response to the left mouse button being clicked.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnMouseDown(MouseEventArgs e) {
		base.OnMouseDown(e);

		if ((e.Button == MouseButtons.Left) && InClockBounds(e.Location)) {
			_mouseDownPosition = e.Location;
			UpdateValue(e.Location, true);
		}

		Invalidate();
	}

	/// <summary>
	/// Handles hot-tracking in response to the mouse button being released.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnMouseUp(MouseEventArgs e) {
		base.OnMouseUp(e);

		if ((e.Button == MouseButtons.Left) && InClockBounds(e.Location)) {
			_mouseDownPosition = Point.Empty;
			_isDragging = false;
			if (!_selectMinutes && _autoSelectMinutes) {
				_selectMinutes = true;
				_autoSelectMinutes = false;
			}
		}

		Invalidate();
	}

	/// <summary>
	/// Handles the 'Now' button in response to the mouse being clicked.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnMouseClick(MouseEventArgs e) {
		base.OnMouseClick(e);

		if (IsDisposed) return;

		if (e.Button == MouseButtons.Left) {
			if (_value.HasValue) {
				if (GetAMBounds().Contains(e.Location)) {
					Value = new TimeSpan(_value.Value.Hours % 12, _value.Value.Minutes, 0);
				}
				else if (GetPMBounds().Contains(e.Location)) {
					Value = new TimeSpan((_value.Value.Hours % 12) + 12, _value.Value.Minutes, 0);
				}
			}

			if (GetHHBounds().Contains(e.Location)) {
				_selectMinutes = false;
				Invalidate();
			}
			if (GetMMBounds().Contains(e.Location)) {
				_selectMinutes = true;
				Invalidate();
			}
			else if (GetNowBounds().Contains(e.Location)) {
				Value = DateTime.Now.TimeOfDay;
			}
		}
	}	

	#endregion
}
