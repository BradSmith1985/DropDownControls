// A Searchable Drop-Down Control
// Bradley Smith - 2017/7/21

using System;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

/// <summary>
/// Attaches to a <see cref="Control"/> and provides text entry and caret services.
/// </summary>
public class TextServices {

	/// <summary>
	/// Contains native functionality used by this class.
	/// </summary>
	private static class NativeMethods {

		/// <summary>
		/// Creates a new shape for the system caret and assigns ownership of the caret to the specified window.
		/// </summary>
		/// <param name="hWnd">A handle to the window that owns the caret.</param>
		/// <param name="hBitmap">A handle to the bitmap that defines the caret shape.</param>
		/// <param name="nWidth">The width of the caret, in logical units.</param>
		/// <param name="nHeight">The height of the caret, in logical units.</param>
		/// <returns></returns>
		[DllImport("User32.dll")]
		public static extern bool CreateCaret(IntPtr hWnd, IntPtr hBitmap, int nWidth, int nHeight);

		/// <summary>
		/// Destroys the caret's current shape, frees the caret from the window, and removes the caret from the screen.
		/// </summary>
		/// <returns></returns>
		[DllImport("User32.dll")]
		public static extern bool DestroyCaret();

		/// <summary>
		/// Removes the caret from the screen. Hiding a caret does not destroy its current shape or invalidate the insertion point.
		/// </summary>
		/// <param name="hWnd">A handle to the window that owns the caret.</param>
		/// <returns></returns>
		[DllImport("User32.dll")]
		public static extern bool HideCaret(IntPtr hWnd);

		/// <summary>
		/// Moves the caret to the specified coordinates.
		/// </summary>
		/// <param name="x">The new x-coordinate of the caret.</param>
		/// <param name="y">The new y-coordinate of the caret.</param>
		/// <returns></returns>
		[DllImport("User32.dll")]
		public static extern bool SetCaretPos(int x, int y);

		/// <summary>
		/// Makes the caret visible on the screen at the caret's current position.
		/// </summary>
		/// <param name="hWnd">A handle to the window that owns the caret.</param>
		/// <returns></returns>
		[DllImport("User32.dll")]
		public static extern bool ShowCaret(IntPtr hWnd);
	}

	const TextFormatFlags _textFormatFlags = TextFormatFlags.Left
		| TextFormatFlags.NoPrefix
		| TextFormatFlags.VerticalCenter
		| TextFormatFlags.NoPadding;

	private StringBuilder _searchText;
	private int _cpActive;
	private int _cpEnd;
	private Func<Rectangle> _boundsCallback;
	private bool _active;
	private bool _causedMouseDown;
	private ContextMenuStrip _cms;

	/// <summary>
	/// Gets the control that owns this instance.
	/// </summary>
	public Control Owner { get; private set; }
	/// <summary>
	/// Gets the length of the text captured by the control.
	/// </summary>
	public int Length {
		get {
			return _searchText.Length;
		}
	}
	/// <summary>
	/// Gets the text captured by the control.
	/// </summary>
	public string Text {
		get {
			return _searchText.ToString();
		}
		set {
			if (Text != value) {
				_searchText.Remove(0, _searchText.Length);
				_searchText.Append(value);
				Select(_searchText.Length, 0);
				OnTextChanged();
			}
		}
	}
	/// <summary>
	/// Gets the character position of the start of the selection. 
	/// If equal to <see cref="Length"/>, the insertion point is at the end.
	/// </summary>
	public int SelectionStart {
		get {
			return Math.Min(_cpActive, _cpEnd);
		}
		protected set {
			if (SelectionStart != value) {
				Select(Math.Max(0, Math.Min(_searchText.Length, value)), SelectionLength);
			}
		}
	}
	/// <summary>
	/// Gets the length of the selection.
	/// </summary>
	public int SelectionLength {
		get {
			return Math.Abs(_cpActive - _cpEnd);
		}
		set {
			if (SelectionLength != value) {
				Select(SelectionStart, Math.Max(0, Math.Min(_searchText.Length, value)));
			}
		}
	}
	/// <summary>
	/// Gets a value indicating whether text services are currently enabled.
	/// </summary>
	public bool IsActive {
		get {
			return _active;
		}
	}

	/// <summary>
	/// Fired when the text captured for the control changes.
	/// </summary>
	public event EventHandler TextChanged;
	/// <summary>
	/// Raises the <see cref="TextChanged"/> event.
	/// </summary>
	protected virtual void OnTextChanged() {
		if (TextChanged != null) TextChanged(this, EventArgs.Empty);
	}

	/// <summary>
	/// Fired when the context menu provided for the control is about to open.
	/// </summary>
	public event EventHandler ContextMenuOpening;
	/// <summary>
	/// Raises the <see cref="ContextMenuOpening"/> event.
	/// </summary>
	protected virtual void OnContextMenuOpening() {
		if (ContextMenuOpening != null) ContextMenuOpening(this, EventArgs.Empty);
	}

	/// <summary>
	/// Fired when the context menu provided for the control is closed.
	/// </summary>
	public event EventHandler ContextMenuClosed;
	/// <summary>
	/// Raises the <see cref="ContextMenuClosed"/> event.
	/// </summary>
	protected virtual void OnContextMenuClosed() {
		if (ContextMenuClosed != null) ContextMenuClosed(this, EventArgs.Empty);
	}

	/// <summary>
	/// Initalises a new instance of the <see cref="TextServices"/> class 
	/// using the specified control and callback method.
	/// </summary>
	/// <param name="owner"></param>
	/// <param name="boundsCallback"></param>
	public TextServices(Control owner, Func<Rectangle> boundsCallback) {
		Owner = owner;
		_searchText = new StringBuilder();
		_cpActive = 0;
		_cpEnd = 0;
		_boundsCallback = boundsCallback;

		Owner.Disposed += Owner_Disposed;
	}

	/// <summary>
	/// Initialises the context menu provided for the control 
	/// (if it has not yet been initialised).
	/// </summary>
	private void EnsureContextMenu() {
		if (_cms == null) {
			_cms = new ContextMenuStrip();
			_cms.Items.Add("Cu&t", null, (o, e) => Cut()).Name = "tsmiCut";
			_cms.Items.Add("&Copy", null, (o, e) => Copy()).Name = "tsmiCopy";
			_cms.Items.Add("&Paste", null, (o, e) => Paste()).Name = "tsmiPaste";
			_cms.Opening += _cms_Opening;
			_cms.Closed += _cms_Closed;
		}
	}

	/// <summary>
	/// Handles the <see cref="Control.MouseMove"/> event. 
	/// Returns true if the event was consumed.
	/// </summary>
	/// <param name="e"></param>
	/// <returns></returns>
	public bool HandleMouseMove(MouseEventArgs e) {
		if (_active && _causedMouseDown) {
			if (e.Button == MouseButtons.Left) {
				_cpActive = CharacterHitTest(e.Location);
				UpdateCaretPos();
				Owner.Invalidate();
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Handles the <see cref="Control.MouseDown"/> event. 
	/// Returns true if the event was consumed.
	/// </summary>
	/// <param name="e"></param>
	/// <returns></returns>
	public bool HandleMouseDown(MouseEventArgs e) {
		if (_active && _boundsCallback().Contains(e.Location)) {
			if (e.Button == MouseButtons.Left) {
				Select(CharacterHitTest(e.Location), 0);
				_causedMouseDown = true;
			}
			else if (e.Button == MouseButtons.Right) {
				int index = CharacterHitTest(e.Location);
				if ((index < SelectionStart) || (index >= (SelectionStart + SelectionLength))) {
					Select(index, 0);
				}
			}

			return true;
		}

		return false;
	}

	/// <summary>
	/// Handles the <see cref="Control.MouseClick"/> event. 
	/// Returns true if the event was consumed.
	/// </summary>
	/// <param name="e"></param>
	/// <returns></returns>
	public bool HandleMouseClick(MouseEventArgs e) {
		if (_active && _boundsCallback().Contains(e.Location)) {
			if (e.Button == MouseButtons.Right) {
				EnsureContextMenu();
				_cms.Show(Cursor.Position);
			}

			return true;
		}

		return false;
	}

	/// <summary>
	/// Handles the <see cref="Control.KeyPress"/> event.
	/// </summary>
	/// <param name="e"></param>
	public void HandleKeyPress(KeyPressEventArgs e) {
		if (_active && !Char.IsControl(e.KeyChar)) {
			if (SelectionLength > 0) {
				_searchText.Remove(SelectionStart, SelectionLength);
			}

			if (SelectionStart < _searchText.Length)
				_searchText.Insert(SelectionStart, e.KeyChar);
			else
				_searchText.Append(e.KeyChar);

			Select(SelectionStart + 1, 0);
			OnTextChanged();

			e.Handled = true;
			Owner.Invalidate();
		}
	}

	/// <summary>
	/// Handles the <see cref="Control.KeyDown"/> event.
	/// </summary>
	/// <param name="e"></param>
	public void HandleKeyDown(KeyEventArgs e) {
		if (_active) {
			bool handled = true;

			if (e.Control && (e.KeyCode == Keys.V)) {
				// paste
				if (Clipboard.ContainsText()) {
					Paste();
					handled = true;
				}
			}
			else if (e.KeyCode == Keys.Back) {
				// backspace
				if (SelectionLength > 0) {
					_searchText.Remove(SelectionStart, SelectionLength);
					Select(SelectionStart, 0);
				}
				else if (SelectionStart > 0) {
					_searchText.Remove(SelectionStart - 1, 1);
					Select(SelectionStart - 1, 0);
				}

				OnTextChanged();
				Owner.Invalidate();
			}
			else if (e.KeyCode == Keys.Delete) {
				// delete
				if (SelectionLength > 0)
					_searchText.Remove(SelectionStart, SelectionLength);
				else if (SelectionStart < _searchText.Length)
					_searchText.Remove(SelectionStart, 1);

				Select(SelectionStart, 0);
				OnTextChanged();
				Owner.Invalidate();
			}
			else if (e.KeyCode == Keys.Left) {
				// left
				if (_cpActive > 0) {
					if (e.Shift) {
						_cpActive--;
						UpdateCaretPos();
						Owner.Invalidate();
					}
					else {
						Select(_cpActive - 1, 0);
					}
				}
				else if (!e.Shift) {
					Select(0, 0);
				}
			}
			else if (e.KeyCode == Keys.Right) {
				// right
				if (_cpActive < _searchText.Length) {
					if (e.Shift) {
						_cpActive++;
						UpdateCaretPos();
						Owner.Invalidate();
					}
					else {
						Select(_cpActive + 1, 0);
					}
				}
				else if (!e.Shift) {
					Select(_searchText.Length, 0);
				}
			}
			else if ((e.KeyCode == Keys.Home) || (e.KeyCode == Keys.PageUp)) {
				// home/pageup
				if (e.Shift) {
					_cpActive = 0;
					UpdateCaretPos();
					Owner.Invalidate();
				}
				else {
					Select(0, 0);
				}
			}
			else if ((e.KeyCode == Keys.End) || (e.KeyCode == Keys.PageDown)) {
				// end/pagedown
				if (e.Shift) {
					_cpActive = _searchText.Length;
					UpdateCaretPos();
					Owner.Invalidate();
				}
				else {
					Select(_searchText.Length, 0);
				}
			}
			else if (e.Control && (e.KeyCode == Keys.X)) {
				// cut
				Cut();
			}
			else if (e.Control && (e.KeyCode == Keys.C)) {
				// copy
				Copy();
			}
			else {
				handled = false;
			}

			if (handled) {
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}
	}

	/// <summary>
	/// Returns the index of the character nearest to the specified coordinates.
	/// </summary>
	/// <param name="location"></param>
	/// <returns></returns>
	private int CharacterHitTest(Point location) {
		Rectangle rect = _boundsCallback();
		rect.Inflate(-3, -3);

		string text = _searchText.ToString();

		if ((location.X <= rect.X) || (text.Length == 0)) return 0;

		for (int i = 1; i < _searchText.Length; i++) {
			Size sz = TextRenderer.MeasureText(text.Substring(0, i), Owner.Font, rect.Size, _textFormatFlags);
			sz.Width -= 6;
			if (location.X <= (rect.X + sz.Width)) return i;
		}

		return _searchText.Length;
	}

	/// <summary>
	/// Displays the caret and enables text services.
	/// </summary>
	public void Begin() {
		_active = true;

		Owner.Invalidate();
		Select(_searchText.Length, 0);
		NativeMethods.CreateCaret(Owner.Handle, IntPtr.Zero, 0, Owner.Font.Height);
		UpdateCaretPos();
		NativeMethods.ShowCaret(Owner.Handle);
	}

	/// <summary>
	/// Hides the caret and disables text services.
	/// </summary>
	public void End() {
		NativeMethods.DestroyCaret();
		Owner.Invalidate();

		_active = false;
	}

	/// <summary>
	/// Clears the text captured for the control.
	/// </summary>
	public void Clear() {
		_searchText.Remove(0, _searchText.Length);
		_cpActive = 0;
		_cpEnd = 0;
		OnTextChanged();
		Owner.Invalidate();
	}

	/// <summary>
	/// Sets the bounds of the selection.
	/// </summary>
	/// <param name="start"></param>
	/// <param name="length"></param>
	public void Select(int start, int length) {
		_cpEnd = start;
		_cpActive = start + length;
		UpdateCaretPos();
		Owner.Invalidate();
	}

	/// <summary>
	/// Updates the position of the caret. 
	/// If the caret is out-of-bounds, it is hidden.
	/// </summary>
	private void UpdateCaretPos() {
		Rectangle rect = _boundsCallback();
		rect.Inflate(-3, -3);

		string caretText = _searchText.ToString().Substring(0, _cpActive);
		Size sz = TextRenderer.MeasureText(caretText, Owner.Font, rect.Size, _textFormatFlags);
		Point caretPos = new Point(rect.X + Math.Max(0, sz.Width - 7), rect.Y + ((rect.Height - Owner.Font.Height) / 2));

		if (rect.Contains(caretPos)) {
			NativeMethods.SetCaretPos(caretPos.X, caretPos.Y);
			if (_active) NativeMethods.ShowCaret(Owner.Handle);
		}
		else {
			NativeMethods.HideCaret(Owner.Handle);
		}
	}

	/// <summary>
	/// Cuts the selection to the clipboard.
	/// </summary>
	public void Cut() {
		string text = _searchText.ToString();
		Clipboard.SetText(text.Substring(SelectionStart, SelectionLength));
		_searchText.Remove(SelectionStart, SelectionLength);
		Select(SelectionStart, 0);
		OnTextChanged();
	}

	/// <summary>
	/// Copies the selection to the clipboard.
	/// </summary>
	public void Copy() {
		string text = _searchText.ToString();
		Clipboard.SetText(text.Substring(SelectionStart, SelectionLength));
	}

	/// <summary>
	/// Pastes the text on the clipboard into the insertion point. 
	/// Any existing selection is replaced.
	/// </summary>
	public void Paste() {
		_searchText.Remove(SelectionStart, SelectionLength);
		Select(SelectionStart, 0);

		string inserted = Clipboard.GetText();

		if (SelectionStart < _searchText.Length)
			_searchText.Insert(SelectionStart, inserted);
		else
			_searchText.Append(inserted);

		Select(SelectionStart + inserted.Length, 0);
		OnTextChanged();
	}

	/// <summary>
	/// Draws the captured text on the control.
	/// </summary>
	/// <param name="graphics"></param>
	public void DrawText(Graphics graphics) {
		Rectangle rect = _boundsCallback();
		rect.Inflate(-3, -3);

		string text = _searchText.ToString();

		TextRenderer.DrawText(graphics, text, Owner.Font, rect, Owner.ForeColor, _textFormatFlags);

		if (SelectionLength > 0) {
			Size sz = TextRenderer.MeasureText(graphics, text.Substring(0, SelectionStart), Owner.Font, rect.Size, _textFormatFlags);
			rect.X += sz.Width;

			sz = TextRenderer.MeasureText(graphics, text.Substring(SelectionStart, SelectionLength), Owner.Font, rect.Size, _textFormatFlags);
			rect.Width = sz.Width;

			SmoothingMode oldMode = graphics.SmoothingMode;
			graphics.SmoothingMode = SmoothingMode.Default;
			graphics.FillRectangle(SystemBrushes.Highlight, rect);
			if (oldMode != SmoothingMode.Default) graphics.SmoothingMode = oldMode;

			TextRenderer.DrawText(graphics, text.Substring(SelectionStart, SelectionLength), Owner.Font, rect, SystemColors.HighlightText, _textFormatFlags);
		}
	}

	void Owner_Disposed(object sender, EventArgs e) {
		if (_cms != null) {
			_cms.Dispose();
			_cms = null;
		}
	}

	void _cms_Closed(object sender, ToolStripDropDownClosedEventArgs e) {
		OnContextMenuClosed();
	}

	void _cms_Opening(object sender, CancelEventArgs e) {
		OnContextMenuOpening();
		_cms.Items["tsmiPaste"].Enabled = Clipboard.ContainsText();
	}
}