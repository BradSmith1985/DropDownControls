// DataGridView Column Types for Drop-Down Controls
// Bradley Smith - 2015/4/14

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Globalization;
using System.ComponentModel;

namespace DropDownControls {

	/// <summary>
	/// A <see cref="DataGridView"/> column type based on the 
	/// <see cref="ComboTreeBox"/> control.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Both the underlying values and formatted values of cells in the column 
	/// are path strings. If the column is bound to a data source, the data 
	/// source values must also be path strings. You can decide which options 
	/// to use when constructing path strings.
	/// </para>
	/// <para>
	/// Some of the more advanced features of the <see cref="ComboTreeBox"/> 
	/// control (such as checkboxes) are not supported by this column type.
	/// </para>
	/// </remarks>
	public class ComboTreeBoxColumn : DropDownColumnBase {

		/// <summary>
		/// Gets or sets the index of the default image to use for nodes when expanded.
		/// </summary>
		[DefaultValue(0), Description("The index of the default image to use for nodes when expanded."), Category("Appearance")]
		public int ExpandedImageIndex {
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the name of the default image to use for nodes when expanded.
		/// </summary>
		[DefaultValue(""), Description("The name of the default image to use for nodes when expanded."), Category("Appearance")]
		public string ExpandedImageKey {
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the index of the default image to use for nodes.
		/// </summary>
		[DefaultValue(0), Description("The index of the default image to use for nodes."), Category("Appearance")]
		public int ImageIndex {
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the name of the default image to use for nodes.
		/// </summary>
		[DefaultValue(""), Description("The name of the default image to use for nodes."), Category("Appearance")]
		public string ImageKey {
			get;
			set;
		}
		/// <summary>
		/// Gets or sets an ImageList component which provides the images displayed beside nodes in the control.
		/// </summary>
		[DefaultValue(null), Description("An ImageList component which provides the images displayed beside nodes in the control."), Category("Appearance")]
		public ImageList Images {
			get;
			set;
		}
		/// <summary>
		/// Gets the collection of top-level nodes contained by the column.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The collection of top-level nodes contained by the control."), Category("Data")]
		public ComboTreeNodeCollection Nodes { get; private set; }
		/// <summary>
		/// Gets or sets the string used to separate nodes in the Path property.
		/// </summary>
		[DefaultValue(ComboTreeBox.DEFAULT_PATH_SEPARATOR), Description("The string used to separate nodes in the path string."), Category("Behavior")]
		public string PathSeparator {
			get;
			set;
		}
		/// <summary>
		/// Determines whether the full path to the selected node is displayed in the column.
		/// </summary>
		[DefaultValue(false), Description("Determines whether the path to the selected node is displayed in the column."), Category("Appearance")]
		public bool ShowPath {
			get;
			set;
		}
		/// <summary>
		/// Determines whether the <see cref="ComboTreeNode.Name"/> property of the nodes is used to construct the path string. 
		/// The default behaviour is to use the <see cref="ComboTreeNode.Text"/> property.
		/// </summary>
		[DefaultValue(false), Description("Determines whether the Name property of the nodes is used to construct the path string. The default behaviour is to use the Text property."), Category("Behavior")]
		public bool UseNodeNamesForPath {
			get;
			set;
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="ComboTreeBoxColumn"/> class.
		/// </summary>
		public ComboTreeBoxColumn() {
			PathSeparator = ComboTreeBox.DEFAULT_PATH_SEPARATOR;
			UseNodeNamesForPath = false;
			Nodes = new ComboTreeNodeCollection(null);
			ExpandedImageIndex = ImageIndex = 0;
			ExpandedImageKey = ImageKey = String.Empty;
			CellTemplate = new ComboTreeBoxCell();
		}

		/// <summary>
		/// Returns a copy of the column.
		/// </summary>
		/// <returns></returns>
		public override object Clone() {
			ComboTreeBoxColumn that = (ComboTreeBoxColumn)base.Clone();
			that.Nodes.AddRange(this.Nodes);
			that.UseNodeNamesForPath = this.UseNodeNamesForPath;
			that.PathSeparator = this.PathSeparator;
			that.ShowPath = this.ShowPath;
			that.Images = this.Images;
			that.ImageIndex = this.ImageIndex;
			that.ImageKey = this.ImageKey;
			that.ExpandedImageIndex = this.ExpandedImageIndex;
			that.ExpandedImageKey = this.ExpandedImageKey;
			return that;
		}
	}

	/// <summary>
	/// Custom cell type to accompany <see cref="ComboTreeBoxColumn"/>.
	/// </summary>
	public class ComboTreeBoxCell : DropDownCellBase {

		private ComboTreeNodeCollection _nodes;

		/// <summary>
		/// Gets a collection of nodes to use instead of the owning column's 
		/// nodes.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ComboTreeNodeCollection Nodes {
			get {
				if (UseColumnNodes)
					return (OwningColumn != null) ? ((ComboTreeBoxColumn)OwningColumn).Nodes : null;
				else
					return _nodes;
			}
		}
		/// <summary>
		/// Gets or sets a value indicating whether to use the 
		/// <see cref="Nodes"/> collection for the cell instead of the owning 
		/// column's nodes.
		/// </summary>
		public bool UseColumnNodes { get; set; }
		/// <summary>
		/// Gets the type of editing control to use for the cell.
		/// </summary>
		public override Type EditType {
			get {
				return typeof(ComboTreeBoxEditingControl);
			}
		}
		/// <summary>
		/// Gets the type of the cell's formatted values.
		/// </summary>
		public override Type FormattedValueType {
			get {
				return typeof(String);
			}
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="ComboTreeBoxCell"/> class.
		/// </summary>
		public ComboTreeBoxCell() {
			UseColumnNodes = true;
			_nodes = new ComboTreeNodeCollection(null);
		}

		/// <summary>
		/// Parses the formatted value from the editing control. 
		/// For this cell type, the formatted value and the underlying value are the same.
		/// </summary>
		/// <param name="formattedValue"></param>
		/// <param name="cellStyle"></param>
		/// <param name="formattedValueTypeConverter"></param>
		/// <param name="valueTypeConverter"></param>
		/// <returns></returns>
		public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter) {
			return formattedValue;
		}

		/// <summary>
		/// Gets the formatted value for the cell. 
		/// For this cell type, the formatted value and the underlying value are the same.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="rowIndex"></param>
		/// <param name="cellStyle"></param>
		/// <param name="valueTypeConverter"></param>
		/// <param name="formattedValueTypeConverter"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context) {
			return value ?? String.Empty;
		}

		/// <summary>
		/// Returns a copy of the cell.
		/// </summary>
		/// <returns></returns>
		public override object Clone() {
			ComboTreeBoxCell that = (ComboTreeBoxCell)base.Clone();
			that.UseColumnNodes = this.UseColumnNodes;
			that._nodes.AddRange(this._nodes);
			return that;
		}

		/// <summary>
		/// Initialises the editing control for the cell.
		/// </summary>
		/// <param name="rowIndex"></param>
		/// <param name="initialFormattedValue"></param>
		/// <param name="dataGridViewCellStyle"></param>
		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle) {
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

			ComboTreeBoxEditingControl control = DataGridView.EditingControl as ComboTreeBoxEditingControl;
			ComboTreeBoxColumn column = (ComboTreeBoxColumn)OwningColumn;

			if (control != null) {
				control.BeginUpdate();
				control.Nodes.Clear();
				control.ImageIndex = column.ImageIndex;
				control.ImageKey = column.ImageKey;
				control.ExpandedImageIndex = column.ExpandedImageIndex;
				control.ExpandedImageKey = column.ExpandedImageKey;
				control.Images = column.Images;
				control.Nodes.AddRange(Nodes);
				control.Path = Convert.ToString(initialFormattedValue);
				control.EndUpdate();
			}
		}

		/// <summary>
		/// Paints the cell.
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="clipBounds"></param>
		/// <param name="cellBounds"></param>
		/// <param name="rowIndex"></param>
		/// <param name="cellState"></param>
		/// <param name="value"></param>
		/// <param name="formattedValue"></param>
		/// <param name="errorText"></param>
		/// <param name="cellStyle"></param>
		/// <param name="advancedBorderStyle"></param>
		/// <param name="paintParts"></param>
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts) {
			base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

			BeforePaintContent(graphics, cellBounds, cellState, cellStyle, paintParts);

			if (paintParts.HasFlag(DataGridViewPaintParts.ContentForeground)) {
				ComboTreeBoxColumn column = (ComboTreeBoxColumn)OwningColumn;
				ComboTreeNode node = Nodes.ParsePath(Convert.ToString(formattedValue), column.PathSeparator, column.UseNodeNamesForPath);
				string displayValue;

				if (column.ShowPath)
					displayValue = Convert.ToString(formattedValue);
				else
					displayValue = (node != null) ? node.Text : String.Empty;

				Image img = ComboTreeBox.GetNodeImage(node, column.Images, column.ImageIndex, column.ImageKey, column.ExpandedImageIndex, column.ExpandedImageKey);

				Rectangle contentBounds = cellBounds;
				contentBounds.Width -= 17;
				TextFormatFlags flags = ComboTreeBox.TEXT_FORMAT_FLAGS | TextFormatFlags.PreserveGraphicsClipping;

				Rectangle imgBounds = (img == null) 
					? new Rectangle(Point.Add(contentBounds.Location, new Size(1,0)), Size.Empty) 
					: new Rectangle(contentBounds.Left + 4, contentBounds.Top + (contentBounds.Height / 2 - img.Height / 2), img.Width, img.Height);

				Rectangle txtBounds = new Rectangle(imgBounds.Right, contentBounds.Top, contentBounds.Right - imgBounds.Right - 3, contentBounds.Height);

				if (img != null) graphics.DrawImage(img, imgBounds);

				TextRenderer.DrawText(graphics, displayValue, cellStyle.Font, txtBounds, cellStyle.ForeColor, flags);
			}

			AfterPaintContent(graphics, clipBounds, cellBounds, rowIndex, cellStyle, advancedBorderStyle, paintParts);
		}
	}

	/// <summary>
	/// Editing control to accompany <see cref="ComboTreeBoxCell"/>.
	/// </summary>
	internal class ComboTreeBoxEditingControl : ComboTreeBox, IDataGridViewEditingControl {

		/// <summary>
		/// Constructor.
		/// </summary>
		public ComboTreeBoxEditingControl() {
			TabStop = false;
		}

		#region IDataGridViewEditingControl Members

		void IDataGridViewEditingControl.ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle) {
			// font
			Font = dataGridViewCellStyle.Font;

			// background colour
			if (dataGridViewCellStyle.BackColor.A < 255) {
				Color opaqueBackColor = Color.FromArgb(255, dataGridViewCellStyle.BackColor);
				BackColor = opaqueBackColor;
				((IDataGridViewEditingControl)this).EditingControlDataGridView.EditingPanel.BackColor = opaqueBackColor;
			}
			else {
				BackColor = dataGridViewCellStyle.BackColor;
			}

			// foreground colour
			ForeColor = dataGridViewCellStyle.ForeColor;
		}

		DataGridView IDataGridViewEditingControl.EditingControlDataGridView {
			get;
			set;
		}

		object IDataGridViewEditingControl.EditingControlFormattedValue {
			get {
				return ((IDataGridViewEditingControl)this).GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
			}
			set {
				string valueStr = value as string;				
				if (valueStr != null) Path = valueStr;
			}
		}

		int IDataGridViewEditingControl.EditingControlRowIndex {
			get;
			set;
		}

		bool IDataGridViewEditingControl.EditingControlValueChanged {
			get;
			set;
		}

		bool IDataGridViewEditingControl.EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey) {
			if ((keyData & Keys.KeyCode) == Keys.Down ||
				(keyData & Keys.KeyCode) == Keys.Up ||
				(DroppedDown && ((keyData & Keys.KeyCode) == Keys.Escape) || (keyData & Keys.KeyCode) == Keys.Enter)) {
				return true;
			}

			return !dataGridViewWantsInputKey;
		}

		Cursor IDataGridViewEditingControl.EditingPanelCursor {
			get { return Cursors.Default; }
		}

		object IDataGridViewEditingControl.GetEditingControlFormattedValue(DataGridViewDataErrorContexts context) {
			return Path;
		}

		void IDataGridViewEditingControl.PrepareEditingControlForEdit(bool selectAll) {
			DroppedDown = true;
		}

		bool IDataGridViewEditingControl.RepositionEditingControlOnValueChange {
			get {
				return false;
			}
		}

		#endregion

		private void NotifyDataGridViewOfValueChange() {
			((IDataGridViewEditingControl)this).EditingControlValueChanged = true;
			((IDataGridViewEditingControl)this).EditingControlDataGridView.NotifyCurrentCellDirty(true);
		}

		protected override void OnSelectedNodeChanged(EventArgs e) {
			base.OnSelectedNodeChanged(e);
			if (SelectedNode != null) NotifyDataGridViewOfValueChange();
		}
	}
}
