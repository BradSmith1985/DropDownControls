// A Searchable Drop-Down Control
// Bradley Smith - 2017/7/21

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

/// <summary>
/// Extends the <see cref="ComboTreeBox"/> control by adding text searching functionality.
/// </summary>
[ToolboxItem(true), DesignerCategory("")]
public class DropDownSearchBox : ComboTreeBox {

	private TextServices _services;
	private ComboTreeNodeCollection _normalNodes;
	private ComboTreeNode _normalSelectedNode;
	private bool _inSearchMode;
	private CancellationTokenSource _cts;

	/// <summary>
	/// Gets a collection containing the nodes normally displayed in the 
	/// drop-down while search mode is active.
	/// </summary>
	public ComboTreeNodeCollection NormalNodes {
		get {
			return _inSearchMode ? _normalNodes : Nodes;
		}
	}
	/// <summary>
	/// Gets a collection containing the nodes normally displayed in the 
	/// drop-down while search mode is active, including all child nodes.
	/// </summary>
	public IEnumerable<ComboTreeNode> AllNormalNodes {
		get {
			IEnumerator<ComboTreeNode> nodes = ComboTreeNodeCollection.GetNodesRecursive(NormalNodes, false);
			while (nodes.MoveNext()) yield return nodes.Current;
		}
	}
	/// <summary>
	/// Gets or sets the minimum length for a search term. No searching 
	/// will be performed until the term contains this many characters.
	/// </summary>
	[DefaultValue(3)]
	[Description("The minimum length for a search term. A search is not performed unless the term contains this many characters.")]
	public int MinSearchTermLength { get; set; }
	/// <summary>
	/// Gets a value indicating whether the control is in search mode.
	/// </summary>
	[Browsable(false)]
	public bool InSearchMode {
		get {
			return _inSearchMode;
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
	/// Fired when a search needs to be performed.
	/// </summary>
	public event EventHandler<PerformSearchEventArgs> PerformSearch;
	/// <summary>
	/// Fired when a search result is selected by the user.
	/// </summary>
	public event EventHandler<CommitSearchEventArgs> CommitSearch;

	/// <summary>
	/// Initialises a new instance of the <see cref="DropDownSearchBox"/> class using default values.
	/// </summary>
	public DropDownSearchBox() {
		_services = new TextServices(this, GetTextBoxBounds);
		DropDownStyle = DropDownControlStyles.FakeEditable;
		ShowGlyphs = false;
		_normalNodes = new ComboTreeNodeCollection(null);
		MinSearchTermLength = 3;
		DropDownHeight = 300;

		_services.TextChanged += _services_TextChanged;
		_services.ContextMenuClosed += _services_ContextMenuClosed;
		_services.ContextMenuOpening += _services_ContextMenuOpening;

		DropDownControl.KeyDown += DropDownControl_KeyDown;
		DropDownControl.KeyPress += DropDownControl_KeyPress;
		DropDownControl.MouseMove += DropDownControl_MouseMove;
	}

	/// <summary>
	/// Releases any resources used by the control.
	/// </summary>
	/// <param name="disposing"></param>
	protected override void Dispose(bool disposing) {
		if (disposing) {
			if (_cts != null) {
				_cts.Dispose();
				_cts = null;
			}
		}

		base.Dispose(disposing);
	}

	/// <summary>
	/// Places the control in search mode.
	/// </summary>
	private void EnterSearchMode() {
		_inSearchMode = true;

		BeginUpdate();

		_normalSelectedNode = SelectedNode;

		// copy normal nodes
		_normalNodes.Clear();
		_normalNodes.AddRange(Nodes);

		// clear and make way for search results
		Nodes.Clear();
		SelectedNode = null;

		EndUpdate();

		DropDownControl.ProcessKeys = false;
		Cursor = Cursors.IBeam;
	}

	/// <summary>
	/// Places the control in normal mode, optionally committing the search result.
	/// </summary>
	/// <param name="commit"></param>
	private void LeaveSearchMode(bool commit) {
		_inSearchMode = false;

		BeginUpdate();

		ComboTreeNode searchSelectedNode = SelectedNode;

		// clear and replace with normal nodes
		Nodes.Clear();
		Nodes.AddRange(_normalNodes);

		if (commit)
			OnCommitSearch(new CommitSearchEventArgs(searchSelectedNode));
		else
			SelectedNode = _normalSelectedNode;

		EndUpdate();

		DropDownControl.ProcessKeys = true;
		Cursor = Cursors.Default;
	}

	/// <summary>
	/// Returns a value indicating whether the specified node matches <paramref name="searchTerm"/>.
	/// </summary>
	/// <param name="node"></param>
	/// <param name="searchTerm"></param>
	/// <returns></returns>
	/// <remarks>
	/// The base implementation of this method performs a case-insensitive 
	/// comparison on the node text and ignores nodes which are not selectable.
	/// </remarks>
	public virtual bool DefaultSearchPredicate(ComboTreeNode node, string searchTerm) {
		return node.Selectable && (node.Text.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);
	}

	/// <summary>
	/// Returns a value indicating whether the <paramref name="test"/> node is equivalent to the <paramref name="result"/> node.
	/// </summary>
	/// <param name="result"></param>
	/// <param name="test"></param>
	/// <returns></returns>
	/// <remarks>
	/// The base implementation of this method performs a case-insensitive 
	/// comparison firstly on the <see cref="ComboTreeNode.Name"/> property 
	/// and then on the <see cref="ComboTreeNode.Text"/> property.
	/// </remarks>
	public virtual bool DefaultEquivalencePredicate(ComboTreeNode result, ComboTreeNode test) {
		if (!String.IsNullOrEmpty(result.Name)) {
			if (String.Equals(result.Name, test.Name, StringComparison.OrdinalIgnoreCase)) return true;
		}

		if (!String.IsNullOrEmpty(result.Text)) {
			if (String.Equals(result.Text, test.Text, StringComparison.OrdinalIgnoreCase)) return true;
		}

		return false;
	}

	/// <summary>
	/// Searches for the specified term and outputs any matching nodes to the 
	/// results collection. The method runs in a separate thread.
	/// </summary>
	/// <param name="e"></param>
	/// <remarks>
	/// The default logic of this method performs a linear search (using 
	/// the <see cref="DefaultSearchPredicate"/> method) on the existing nodes 
	/// in the drop-down.
	/// </remarks>
	[DebuggerHidden]
	protected virtual void OnPerformSearch(PerformSearchEventArgs e) {
		if (PerformSearch != null) PerformSearch(this, e);

		if (!e.Handled) {
			// default search logic
			foreach (ComboTreeNode node in AllNormalNodes) {
				e.CancellationToken.ThrowIfCancellationRequested();

				if (DefaultSearchPredicate(node, e.SearchTerm)) {
					e.Results.Add(node.Clone());
				}
			}
		}
	}

	/// <summary>
	/// Commits the search by selecting the equivalent node in the drop-down or 
	/// adding the result as a new node.
	/// </summary>
	/// <param name="e"></param>
	protected virtual void OnCommitSearch(CommitSearchEventArgs e) {
		if (CommitSearch != null) CommitSearch(this, e);

		if (!e.Handled) {
			ComboTreeNode match = null;

			// try to find an equivalent match in the normal list
			if (e.Result != null) {
				if (OwnsNode(e.Result)) {
					match = e.Result;
				}
				else if ((match = AllNormalNodes.FirstOrDefault(x => DefaultEquivalencePredicate(e.Result, x))) == null) {
					// search result not in original collection; add
					match = e.Result.Clone();
					Nodes.Add(match);
				}
				else if (!match.Selectable) {
					match = null;
				}
			}

			SelectedNode = match;
		}
	}

	/// <summary>
	/// Applies the specified search result collection to the control by adding the nodes to the drop-down.
	/// </summary>
	/// <param name="results"></param>
	private void ApplySearchResults(ComboTreeNodeCollection results) {
		if (InvokeRequired) {
			Invoke(new Action<ComboTreeNodeCollection>(ApplySearchResults), results);
		}
		else {
			if (_inSearchMode) {
				BeginUpdate();
				Nodes.Clear();

				if (results.Count > 0)
					Nodes.AddRange(results);
				else
					Nodes.Add(new ComboTreeNode("(no results found)") { Selectable = false, FontStyle = FontStyle.Italic });

				EndUpdate();
			}
		}
	}

	/// <summary>
	/// Registers the left and right arrow keys as input keys.
	/// </summary>
	/// <param name="keyData"></param>
	/// <returns></returns>
	protected override bool IsInputKey(Keys keyData) {
		if (keyData.HasFlag(Keys.Left) || keyData.HasFlag(Keys.Right)) {
			return true;
		}

		return base.IsInputKey(keyData);
	}

	/// <summary>
	/// Suppresses the <see cref="ComboTreeBox.SelectedNodeChanged"/> event 
	/// while the control is in search mode and ensures that selecting a 
	/// search result commits its value.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnSelectedNodeChanged(EventArgs e) {
		if (_inSearchMode && (SelectedNode != null)) LeaveSearchMode(true);
		if (!_inSearchMode) base.OnSelectedNodeChanged(e);
	}

	/// <summary>
	/// Hides the caret when the drop-down is closed.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnDropDownClosed(EventArgs e) {
		base.OnDropDownClosed(e);

		_services.End();
		_services.Clear();
	}

	/// <summary>
	/// Shows the caret when the drop-down opens.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnDropDown(EventArgs e) {
		base.OnDropDown(e);

		_services.Begin();
	}

	/// <summary>
	/// Pastes the text on the clipboard into the control. 
	/// The control is placed in search mode if it is not already.
	/// </summary>
	private void DoPaste() {
		if (!DroppedDown) DroppedDown = true;
		if (!_inSearchMode) EnterSearchMode();
		_services.Paste();
	}

	/// <summary>
	/// Handles input keys.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnKeyDown(KeyEventArgs e) {
		if (DroppedDown) {
			if (e.Control && (e.KeyCode == Keys.V)) {
				// paste
				if (Clipboard.ContainsText()) {
					DoPaste();
					e.Handled = true;
					e.SuppressKeyPress = true;
					return;
				}
			}
		}

		if (e.KeyCode == Keys.Tab) {
			Message fake = new Message() { HWnd = Handle, Msg = 0x0100, WParam = new IntPtr((int)e.KeyData), LParam = IntPtr.Zero, Result = IntPtr.Zero };
			ProcessCmdKey(ref fake, e.KeyData);
			e.Handled = true;
			e.SuppressKeyPress = true;
			return;
		}

		_services.HandleKeyDown(e);

		if (!e.Handled) base.OnKeyDown(e);
	}

	/// <summary>
	/// Handles input keys.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnKeyPress(KeyPressEventArgs e) {
		if (!Char.IsControl(e.KeyChar)) {
			if (!DroppedDown) DroppedDown = true;
		}

		_services.HandleKeyPress(e);

		if (!e.Handled) base.OnKeyPress(e);
	}

	/// <summary>
	/// Draws the text that has been entered into the control by the user.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnPaintContent(DropDownPaintEventArgs e) {
		if (DroppedDown) {
			_services.DrawText(e.Graphics);
		}
		else {
			base.OnPaintContent(e);
		}
	}

	/// <summary>
	/// Handles mouse events.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnMouseDown(MouseEventArgs e) {
		if (DroppedDown && GetTextBoxBounds().Contains(e.Location)) {
			if (e.Button == MouseButtons.Left) {
				DropDownControl.Capture = true;
			}
		}

		if (_services.HandleMouseDown(e)) return;

		base.OnMouseDown(e);
	}

	/// <summary>
	/// Handles mouse events.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnMouseUp(MouseEventArgs e) {
		if (DroppedDown) {
			if (e.Button == MouseButtons.Left) {
				DropDownControl.Capture = false;
			}

			return;
		}

		base.OnMouseUp(e);
	}

	/// <summary>
	/// Handles mouse events.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnMouseMove(MouseEventArgs e) {
		if (_services.HandleMouseMove(e)) return;

		base.OnMouseMove(e);
	}

	/// <summary>
	/// Handles mouse events.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnMouseClick(MouseEventArgs e) {
		if (_services.HandleMouseClick(e)) return;

		base.OnMouseClick(e);
	}

	void _services_ContextMenuOpening(object sender, EventArgs e) {
		DropDownControl.AutoClose = false;
	}

	void _services_ContextMenuClosed(object sender, EventArgs e) {
		DropDownControl.AutoClose = true;
	}

	void _services_TextChanged(object sender, EventArgs e) {
		// cancel any existing search operation
		if (_cts != null) {
			_cts.Cancel();
			_cts.Dispose();
			_cts = null;
		}

		if (_services.Length > 0) {
			if (!_inSearchMode) EnterSearchMode();

			if (_services.Length >= MinSearchTermLength) {
				// start async search operation
				BeginUpdate();
				Nodes.Clear();
				Nodes.Add(new ComboTreeNode("Searching...") { Selectable = false, FontStyle = FontStyle.Italic });
				EndUpdate();

				_cts = new CancellationTokenSource();
				ComboTreeNodeCollection results = new ComboTreeNodeCollection(null);

				var task = Task.Factory.StartNew(() => OnPerformSearch(new PerformSearchEventArgs(_services.Text, _cts.Token, results)), _cts.Token);

				task.ContinueWith(t => {
					if (t.IsFaulted) {
						results.Clear();
						string errorText = t.Exception.InnerExceptions.Select(x => x.Message).FirstOrDefault() ?? "an error occured";
						results.Add(new ComboTreeNode(String.Format("({0})", errorText)) { Selectable = false, FontStyle = FontStyle.Italic });
					}

					if (!t.IsCanceled) {
						ApplySearchResults(results);
					}
				});
			}
			else {
				// wait until the search term is long enough
				BeginUpdate();
				Nodes.Clear();
				string msg = String.Format("(type at least {0} characters)", MinSearchTermLength);
				Nodes.Add(new ComboTreeNode(msg) { Selectable = false, FontStyle = FontStyle.Italic });
				EndUpdate();
			}
		}
		else {
			if (_inSearchMode) LeaveSearchMode(false);
		}
	}

	void DropDownControl_MouseMove(object sender, MouseEventArgs e) {
		Point converted = PointToClient(DropDownControl.PointToScreen(e.Location));
		OnMouseMove(new MouseEventArgs(e.Button, e.Clicks, converted.X, converted.Y, e.Delta));
	}

	void DropDownControl_KeyPress(object sender, KeyPressEventArgs e) {
		if (!e.Handled) OnKeyPress(e);
	}

	void DropDownControl_KeyDown(object sender, KeyEventArgs e) {
		if (!DropDownControl.ProcessKeys) {
			switch (e.KeyCode) {
				case Keys.F4:
				case Keys.Escape:
				case Keys.Up:
				case Keys.Down:
				case Keys.PageDown:
				case Keys.PageUp:
				case Keys.Enter:
					DropDownControl.ProcessKey(e.KeyCode, e.Modifiers);
					e.Handled = true;
					e.SuppressKeyPress = true;
					break;
			}

			if (!e.Handled) OnKeyDown(e);
		}
	}
}

/// <summary>
/// Arguments for the <see cref="DropDownSearchBox.PerformSearch"/> event.
/// </summary>
[Serializable]
public class PerformSearchEventArgs : HandledEventArgs {

	/// <summary>
	/// Gets the search term entered by the user.
	/// </summary>
	public string SearchTerm { get; private set; }
	/// <summary>
	/// Gets the <see cref="System.Threading.CancellationToken"/> that is used to cancel the search operation.
	/// </summary>
	public CancellationToken CancellationToken { get; private set; }
	/// <summary>
	/// Gets the collection that will contain the search results.
	/// </summary>
	public ComboTreeNodeCollection Results { get; private set; }

	/// <summary>
	/// Initialises a new instance of the <see cref="PerformSearchEventArgs"/> class using the specified values.
	/// </summary>
	/// <param name="searchTerm"></param>
	/// <param name="token"></param>
	/// <param name="results"></param>
	public PerformSearchEventArgs(string searchTerm, CancellationToken token, ComboTreeNodeCollection results) : base(false) {
		SearchTerm = searchTerm;
		CancellationToken = token;
		Results = results;
	}
}

/// <summary>
/// Arguments for the <see cref="DropDownSearchBox.CommitSearch"/> event.
/// </summary>
[Serializable]
public class CommitSearchEventArgs : HandledEventArgs {

	/// <summary>
	/// Gets the search result node that was selected by the user.
	/// </summary>
	public ComboTreeNode Result { get; private set; }

	/// <summary>
	/// Initialises a new instance of the <see cref="CommitSearchEventArgs"/> class using the specified values.
	/// </summary>
	/// <param name="result"></param>
	public CommitSearchEventArgs(ComboTreeNode result) : base(false) {
		Result = result;
	}
}