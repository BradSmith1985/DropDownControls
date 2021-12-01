using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Drop-down control used to select the time of day. 
/// Also supports text entry.
/// </summary>
[ToolboxItem(true), DesignerCategory("")]
public class DropDownTimePicker : DropDownControlBase, ITimePicker {

	/// <summary>
	/// Gets or sets the default short time format string.
	/// </summary>
	public static string DefaultTimeFormat { get; set; }

	static DropDownTimePicker() {
		DefaultTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
	}

	internal const TextFormatFlags TEXT_FORMAT_FLAGS = TextFormatFlags.TextBoxControl | TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.PathEllipsis;

	TimeSpan? _value;
	bool _showCheckBox;
	TimePickerDropDown<DropDownTimePicker> _dropDown;
	TextServices _services;
	bool _textEdit;
	string _timeFormat;

	/// <summary>
	/// Gets or sets the time of day represented by the control.
	/// </summary>
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public TimeSpan? Value {
		get {
			return _value;
		}
		set {
			if (!ShowCheckBox && !value.HasValue) throw new ArgumentNullException(nameof(Value));

			if (_value != value) {
				_value = value;
				if (DroppedDown) _dropDown.Value = _value;
				OnValueChanged();
				Invalidate();
			}
		}
	}
	/// <summary>
	/// Gets or sets a value indicating whether a checkbox is shown on the control, allowing null values to be entered.
	/// </summary>
	[DefaultValue(true)]
	public bool ShowCheckBox {
		get {
			return _showCheckBox;
		}
		set {
			if (_showCheckBox != value) {
				_showCheckBox = value;

				if (!_showCheckBox && !Value.HasValue) {
					// value cannot be null if checkbox is not shown
					Value = DateTime.Now.TimeOfDay;
				}
				else {
					Invalidate();
				}
			}
		}
	}
	/// <summary>
	/// Gets the toolstrip drop-down control that is used to select the time of day.
	/// </summary>
	protected TimePickerDropDown<DropDownTimePicker> DropDownControl {
		get {
			return _dropDown;
		}
	}
	/// <summary>
	/// Gets or sets whether the dropdown portion of the control is displayed.
	/// </summary>
	[Browsable(false)]
	public override bool DroppedDown {
		get {
			return base.DroppedDown;
		}
		set {
			SetDroppedDown(value, true);
		}
	}
	/// <summary>
	/// Gets the <see cref="TextServices"/> instance used to provide text entry.
	/// </summary>
	protected TextServices TextServices {
		get {
			return _services;
		}
	}
	/// <summary>
	/// Gets or sets the text displayed on the control.
	/// </summary>
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
	public override string Text { 
		get => Format(_value); 
		set => Value = Parse(value); 
	}
	/// <summary>
	/// Gets or sets the format string used to display the time of day. 
	/// Leave empty to use the default short time format.
	/// </summary>
	[DefaultValue("")]
	public string TimeFormat {
		get { return _timeFormat; }
		set {
			if (_timeFormat != value) {
				_timeFormat = value;
				Invalidate();
			}
		}
	}

	/// <summary>
	/// Fired when the value of the control changes.
	/// </summary>
	public event EventHandler ValueChanged;
	/// <summary>
	/// Raises the <see cref="ValueChanged"/> event.
	/// </summary>
	protected virtual void OnValueChanged() {
		if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
	}

	/// <summary>
	/// Initialises a new instance of the <see cref="DropDownTimePicker"/> using default values.
	/// </summary>
	public DropDownTimePicker() {
		_showCheckBox = true;
		_dropDown = new TimePickerDropDown<DropDownTimePicker>(this);
		_services = new TextServices(this, GetTextBoxBounds);
		_timeFormat = String.Empty;

		DropDownStyle = DropDownControlStyles.Editable;

		_dropDown.ValueChanged += DropDown_ValueChanged;
		_dropDown.Closed += DropDown_Closed;
	}	

	/// <summary>
	/// Parses the specified string as either a time of day value or null.
	/// </summary>
	/// <param name="text"></param>
	/// <returns></returns>
	private TimeSpan? Parse(string text) {
		if (!String.IsNullOrWhiteSpace(text)) {
			DateTime d;
			if (DateTime.TryParse(text, out d)) {
				return d.TimeOfDay;
			}
			else if (text.Trim().Equals("now", StringComparison.OrdinalIgnoreCase)) {
				return DateTime.Now.TimeOfDay;
			}
		}

		return null;
	}

	/// <summary>
	/// Formats the specified time of day value according to the time format string.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	private string Format(TimeSpan? value) {
		return value.HasValue
			? (String.IsNullOrEmpty(_timeFormat)
				? DateTime.Today.Add(value.Value).ToString(DefaultTimeFormat)
				: DateTime.Today.Add(value.Value).ToString(_timeFormat))
			: String.Empty;
	}

	/// <summary>
	/// Disposes of the control and its dropdown.
	/// </summary>
	/// <param name="disposing"></param>
	protected override void Dispose(bool disposing) {
		if (disposing) _dropDown.Dispose();
		base.Dispose(disposing);
	}

	/// <summary>
	/// Sets the value of the <see cref="DroppedDown"/> property, optionally without raising any events.
	/// </summary>
	/// <param name="droppedDown"></param>
	/// <param name="raiseEvents"></param>
	internal void SetDroppedDown(bool droppedDown, bool raiseEvents) {
		base.DroppedDown = droppedDown;

		if (raiseEvents) {
			if (droppedDown)
				_dropDown.Open();
			else
				_dropDown.Close();
		}
	}

	/// <summary>
	/// Places the control into text edit mode.
	/// </summary>
	/// <returns></returns>
	private bool BeginEdit() {
		if (_textEdit) return false;
		_services.Begin();
		_services.Text = Text;
		_services.Select(0, Text.Length);
		_textEdit = true;
		Invalidate();
		return true;
	}

	/// <summary>
	/// Ends text edit mode.
	/// </summary>
	/// <param name="cancel"></param>
	/// <returns></returns>
	private bool EndEdit(bool cancel) {
		if (!_textEdit) return false;
		_services.End();

		if (!cancel) {
			try {
				Value = Parse(_services.Text);
			}
			catch (ArgumentNullException) { }
		}

		_services.Clear();
		_textEdit = false;
		Invalidate();
		return true;
	}

	/// <summary>
	/// Handles shortcut keys.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnKeyDown(KeyEventArgs e) {
		e.Handled = e.SuppressKeyPress = true;

		if (e.Control && (e.KeyCode == Keys.V)) {
			// paste
			if (Clipboard.ContainsText()) {
				BeginEdit();
				_services.Clear();
				_services.Paste();
				return;
			}
		}
		else if (_textEdit && (e.KeyCode == Keys.Escape)) {
			EndEdit(true);
		}
		else if (_textEdit && (e.KeyCode == Keys.Enter)) {
			EndEdit(false);
		}
		else if (e.Alt && (e.KeyCode == Keys.Down)) {
			DroppedDown = true;
		}
		else if (_showCheckBox && (e.KeyCode == Keys.Delete)) {
			Value = null;
		}
		else {
			e.Handled = e.SuppressKeyPress = false;
		}

		if (_textEdit) _services.HandleKeyDown(e);

		base.OnKeyDown(e);
	}

	/// <summary>
	/// Handles shortcut keys.
	/// </summary>
	/// <param name="msg"></param>
	/// <param name="keyData"></param>
	/// <returns></returns>
	protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
		if (keyData == Keys.F4) {
			DroppedDown = !DroppedDown;
			return true;
		}
		else if (keyData == Keys.F2) {
			BeginEdit();
			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	/// <summary>
	/// Handles key presses which place the control into text edit mode.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnKeyPress(KeyPressEventArgs e) {
		if (!Char.IsControl(e.KeyChar)) {
			if (BeginEdit()) _services.Clear();
		}

		if (_textEdit) _services.HandleKeyPress(e);
		
		base.OnKeyPress(e);
	}

	/// <summary>
	/// Handles mouse events in text edit mode and repaints the control when the pointer moves.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnMouseMove(MouseEventArgs e) {
		base.OnMouseMove(e);

		if (_textEdit) _services.HandleMouseMove(e);

		Invalidate();
	}

	/// <summary>
	/// Returns the bounds of the editable portion of the control.
	/// </summary>
	/// <returns></returns>
	protected override Rectangle GetTextBoxBounds() {
		Rectangle bounds = base.GetTextBoxBounds();
		if (_showCheckBox) {
			Rectangle chk = GetCheckBoxBounds(ClientRectangle);
			bounds.X += chk.Right;
			bounds.Width -= (chk.Width + 4);
		}
		return bounds;
	}

	/// <summary>
	/// Toggles the visibility of the dropdown portion of the control.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnMouseClick(MouseEventArgs e) {
		base.OnMouseClick(e);

		if (GetCheckBoxBounds(ClientRectangle).Contains(e.Location)) {
			if (Value.HasValue)
				Value = null;
			else
				Value = DateTime.Now.TimeOfDay;
		}

		if (_textEdit) _services.HandleMouseClick(e);
	}

	/// <summary>
	/// Opens/closes the dropdown when the button is clicked.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnDropDownButtonClick(EventArgs e) {
		base.OnDropDownButtonClick(e);

		DroppedDown = !DroppedDown;
	}

	/// <summary>
	/// Updates the dropdown's font when the control's font changes.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnFontChanged(EventArgs e) {
		base.OnFontChanged(e);
		_dropDown.Font = Font;
	}

	/// <summary>
	/// Closes the dropdown portion of the control when it loses focus.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnLostFocus(EventArgs e) {
		base.OnLostFocus(e);
		if (!_dropDown.Focused) _dropDown.Close();
		EndEdit(false);
	}

	/// <summary>
	/// Registers input keys.
	/// </summary>
	/// <param name="keyData"></param>
	/// <returns></returns>
	protected override bool IsInputKey(Keys keyData) {
		if ((keyData == Keys.Escape) || (keyData == Keys.Enter) || (keyData == Keys.Delete)) {
			return true;
		}
		
		return base.IsInputKey(keyData);
	}

	/// <summary>
	/// Handles entering/leaving text edit mode.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnMouseDown(MouseEventArgs e) {
		base.OnMouseDown(e);

		if (GetTextBoxBounds().Contains(e.Location)) {
			if (!DroppedDown) BeginEdit();
		}
		else {
			EndEdit(false);
		}

		if (_textEdit) _services.HandleMouseDown(e);

		Invalidate();
	}

	/// <summary>
	/// Repaints the control when the mouse button is released.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnMouseUp(MouseEventArgs e) {
		base.OnMouseUp(e);

		Invalidate();
	}

	/// <summary>
	/// Gets the visual state of the checkbox.
	/// </summary>
	/// <param name="chkBounds"></param>
	/// <returns></returns>
	private System.Windows.Forms.VisualStyles.CheckBoxState GetCheckBoxState(Rectangle chkBounds) {
		bool isChecked = DroppedDown ? _dropDown.Value.HasValue : Value.HasValue;
		
		if (!Enabled) {
			// disabled
			if (isChecked)
				return System.Windows.Forms.VisualStyles.CheckBoxState.CheckedDisabled;
			else
				return System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedDisabled;
		}
		else if (chkBounds.Contains(PointToClient(Cursor.Position))) {
			if (MouseButtons.HasFlag(MouseButtons.Left)) {
				// pressed
				if (isChecked)
					return System.Windows.Forms.VisualStyles.CheckBoxState.CheckedPressed;
				else
					return System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedPressed;
			}
			else {
				// hot
				if (isChecked)
					return System.Windows.Forms.VisualStyles.CheckBoxState.CheckedHot;
				else
					return System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedHot;
			}
		}
		else {
			// normal
			if (isChecked)
				return System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal;
			else
				return System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
		}
	}

	/// <summary>
	/// Gets the bounds of the checkbox.
	/// </summary>
	/// <param name="controlBounds"></param>
	/// <returns></returns>
	private Rectangle GetCheckBoxBounds(Rectangle controlBounds) {
		if (_showCheckBox) {
			Size chkSize;
			using (Graphics g = CreateGraphics()) {
				chkSize = CheckBoxRenderer.GetGlyphSize(g, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
			}

			return new Rectangle(4, controlBounds.Height / 2 - chkSize.Height / 2, chkSize.Width, chkSize.Height);
		}

		return new Rectangle(1, 0, 0, 0);
	}

	/// <summary>
	/// Paints the selected node in the control.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnPaintContent(DropDownPaintEventArgs e) {
		base.OnPaintContent(e);

		Rectangle chkBounds = GetCheckBoxBounds(e.Bounds);
		Rectangle txtBounds = GetTextBoxBounds();

		if (_showCheckBox) CheckBoxRenderer.DrawCheckBox(e.Graphics, chkBounds.Location, GetCheckBoxState(chkBounds));

		if (_textEdit) {
			if (DrawWithVisualStyles && ComboBoxRenderer.IsSupported) {
				Rectangle r = ClientRectangle;
				r.Width--;
				r.Height--;
				using (Pen pen = new Pen(WinVistaColorTable.HotBorder)) {					
					e.Graphics.DrawRectangle(pen, r);
				}
			}
			_services.DrawText(e.Graphics);
		}
		else {
			string text = DroppedDown ? Format(_dropDown.Value) : Text;

			TextRenderer.DrawText(e.Graphics, text, Font, txtBounds, Enabled ? ForeColor : SystemColors.GrayText, TEXT_FORMAT_FLAGS);

			// focus rectangle
			if (Focused && ShowFocusCues && !DroppedDown) e.DrawFocusRectangle();
		}
	}

	private void DropDown_ValueChanged(object sender, EventArgs e) {
		Invalidate();
	}

	private void DropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e) {
		// update DroppedDown after close
		SetDroppedDown(false, false);

		try {
			Value = _dropDown.Value;
		}
		catch (ArgumentNullException) { }
	}
}