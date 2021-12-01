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
	/// <see cref="GroupedComboBox"/> control.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Unlike the <see cref="DataGridViewComboBoxColumn"/>, the items in the 
	/// combo box for this column type may only come from a data source. 
	/// However, you can override the data source for cells on an individual 
	/// basis.
	/// </para>
	/// <para>
	/// The formatted value type for this column is always 
	/// <see cref="System.String"/>. You do not have to explicitly set the 
	/// <see cref="DisplayMember"/>, <see cref="ValueMember"/> or 
	/// <see cref="GroupMember"/> properties.
	/// </para>
	/// </remarks>
	public class GroupedComboBoxColumn : DropDownColumnBase {

		/// <summary>
		/// Gets or sets the name of the property of a list item which provides its display value.
		/// </summary>
		[Category("Data"), Description("The name of the property of a list item which provides its display value.")]
		[DefaultValue("")]
		[TypeConverterAttribute("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
		public string DisplayMember { get; set; }
		/// <summary>
		/// Gets or sets the name of the property of a list item which provides its underlying value.
		/// </summary>
		[Category("Data"), Description("The name of the property of a list item which provides its underlying value.")]
		[DefaultValue("")]
		[TypeConverterAttribute("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
		public string ValueMember { get; set; }
		/// <summary>
		/// Gets or sets the name of the property of a list item which provides its grouping value.
		/// </summary>
		[Category("Data"), Description("The name of the property of a list item which provides its grouping value.")]
		[DefaultValue("")]
		[TypeConverterAttribute("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
		public string GroupMember { get; set; }
		/// <summary>
		/// Gets or sets the data source that populates the items in the combo box.
		/// </summary>
		[Category("Data"), Description("The data source that populates the items in the combo box.")]
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[AttributeProvider(typeof(IListSource))]
		public object DataSource { get; set; }

		/// <summary>
		/// Initialises a new instance of the <see cref="GroupedComboBoxColumn"/> class.
		/// </summary>
		public GroupedComboBoxColumn() {
			DisplayMember = String.Empty;
			ValueMember = String.Empty;
			GroupMember = String.Empty;
			CellTemplate = new GroupedComboBoxCell();
		}

		/// <summary>
		/// Returns a copy of the column.
		/// </summary>
		/// <returns></returns>
		public override object Clone() {
			GroupedComboBoxColumn that = (GroupedComboBoxColumn)base.Clone();
			that.DisplayMember = this.DisplayMember;
			that.ValueMember = this.ValueMember;
			that.GroupMember = this.GroupMember;
			that.DataSource = this.DataSource;
			return that;
		}
	}

	/// <summary>
	/// Custom cell type to accompany <see cref="GroupedComboBoxColumn"/>.
	/// </summary>
	public class GroupedComboBoxCell : DropDownCellBase {

		private string _displayMember;
		private string _valueMember;
		private string _groupMember;
		private object _dataSource;

		/// <summary>
		/// Gets or sets a value which overrides the owning column's 
		/// <see cref="GroupedComboBoxColumn.DisplayMember"/> property.
		/// </summary>
		public string DisplayMember {
			get {
				return _displayMember ?? ((OwningColumn != null) ? ((GroupedComboBoxColumn)OwningColumn).DisplayMember : null);
			}
			set {
				_displayMember = value;
			}
		}
		/// <summary>
		/// Gets or sets a value which overrides the owning column's 
		/// <see cref="GroupedComboBoxColumn.ValueMember"/> property.
		/// </summary>
		public string ValueMember {
			get {
				return _valueMember ?? ((OwningColumn != null) ? ((GroupedComboBoxColumn)OwningColumn).ValueMember : null);
			}
			set {
				_valueMember = value;
			}
		}
		/// <summary>
		/// Gets or sets a value which overrides the owning column's 
		/// <see cref="GroupedComboBoxColumn.GroupMember"/> property.
		/// </summary>
		public string GroupMember {
			get {
				return _groupMember ?? ((OwningColumn != null) ? ((GroupedComboBoxColumn)OwningColumn).GroupMember : null);
			}
			set {
				_groupMember = value;
			}
		}
		/// <summary>
		/// Gets or sets a value which overrides the owning column's 
		/// <see cref="GroupedComboBoxColumn.DataSource"/> property.
		/// </summary>
		public object DataSource {
			get {
				return _dataSource ?? ((OwningColumn != null) ? ((GroupedComboBoxColumn)OwningColumn).DataSource : null);
			}
			set {
				_dataSource = value;
			}
		}
		/// <summary>
		/// Gets the type of editing control used by the cell.
		/// </summary>
		public override Type EditType {
			get {
				return typeof(GroupedComboBoxEditingControl);
			}
		}
		/// <summary>
		/// Gets the type of the formatted value for the cell, which is 
		/// <see cref="System.String"/>.
		/// </summary>
		public override Type FormattedValueType {
			get {
				return typeof(String);
			}
		}

		/// <summary>
		/// Parses a formatted value from the editing control. 
		/// This works by matching the editing control's text against the 
		/// display values for the list items.
		/// </summary>
		/// <param name="formattedValue"></param>
		/// <param name="cellStyle"></param>
		/// <param name="formattedValueTypeConverter"></param>
		/// <param name="valueTypeConverter"></param>
		/// <returns></returns>
		public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter) {
			if (DataSource != null) {
				object listItem = null;
				Func<object, String> dispFormatter;

				if (!String.IsNullOrEmpty(DisplayMember)) {
					PropertyDescriptor pd = ListBindingHelper.GetListItemProperties(DataSource)[DisplayMember];
					dispFormatter = o => Convert.ToString(pd.GetValue(o));
				}
				else {
					dispFormatter = o => Convert.ToString(o);
				}

				foreach (object item in (IList)ListBindingHelper.GetList(DataSource)) {
					if (String.Compare(Convert.ToString(formattedValue), dispFormatter(item), true, CultureInfo.CurrentCulture) == 0) {
						listItem = item;
						break;
					}
				}

				if ((listItem != null) && !(listItem is DBNull)) {
					Func<object, object> valueFormatter;

					if (!String.IsNullOrEmpty(ValueMember)) {
						PropertyDescriptor pd = ListBindingHelper.GetListItemProperties(DataSource)[ValueMember];
						valueFormatter = o => pd.GetValue(o);
					}
					else {
						valueFormatter = o => o;
					}

					return valueFormatter(listItem);
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the formatted value for the cell.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="rowIndex"></param>
		/// <param name="cellStyle"></param>
		/// <param name="valueTypeConverter"></param>
		/// <param name="formattedValueTypeConverter"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context) {
			if (DataSource != null) {
				object listItem = null;

				if (!String.IsNullOrEmpty(ValueMember)) {
					PropertyDescriptor pd = ListBindingHelper.GetListItemProperties(DataSource)[ValueMember];
					foreach (object item in (IList)ListBindingHelper.GetList(DataSource)) {
						if (Object.Equals(value, pd.GetValue(item))) {
							listItem = item;
							break;
						}
					}
				}
				else {
					listItem = value;
				}

				if (!String.IsNullOrEmpty(DisplayMember)) {
					PropertyDescriptor pd = ListBindingHelper.GetListItemProperties(DataSource)[DisplayMember];
					return pd.GetValue(listItem);
				}
				else {
					return Convert.ToString(listItem);
				}
			}
			
			return String.Empty;
		}

		/// <summary>
		/// Returns a copy of the cell.
		/// </summary>
		/// <returns></returns>
		public override object Clone() {
			GroupedComboBoxCell that = (GroupedComboBoxCell)base.Clone();
			that.DisplayMember = this.DisplayMember;
			that.ValueMember = this.ValueMember;
			that.GroupMember = this.GroupMember;
			that.DataSource = this.DataSource;
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

			GroupedComboBoxEditingControl control = DataGridView.EditingControl as GroupedComboBoxEditingControl;

			if (control != null) {
				control.BeginUpdate();
				control.DataSource = null;
				control.Items.Clear();
				control.DisplayMember = DisplayMember;
				control.ValueMember = ValueMember;
				control.GroupMember = GroupMember;
				control.DataSource = DataSource;
				control.Text = Convert.ToString(initialFormattedValue);
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
				Rectangle contentBounds = cellBounds;
				contentBounds.Width -= 17;
				TextFormatFlags flags = TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.PreserveGraphicsClipping;
				TextRenderer.DrawText(graphics, Convert.ToString(formattedValue), cellStyle.Font, contentBounds, cellStyle.ForeColor, flags);
			}

			AfterPaintContent(graphics, clipBounds, cellBounds, rowIndex, cellStyle, advancedBorderStyle, paintParts);
		}
	}

	/// <summary>
	/// Editing control to accompany <see cref="GroupedComboBoxCell"/>.
	/// </summary>
	/// <remarks>
	/// Implementation based on <see cref="DataGridViewComboBoxEditingControl"/>.
	/// </remarks>
	internal class GroupedComboBoxEditingControl : GroupedComboBox, IDataGridViewEditingControl {

		/// <summary>
		/// Constructor.
		/// </summary>
		public GroupedComboBoxEditingControl() {
			TabStop = false;
			DropDownStyle = ComboBoxStyle.DropDownList;
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

				if (valueStr != null) {
					this.Text = valueStr;

					if (String.Compare(valueStr, Text, true, CultureInfo.CurrentCulture) != 0) {
						this.SelectedIndex = -1;
					}
				}
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
			return Text;
		}

		void IDataGridViewEditingControl.PrepareEditingControlForEdit(bool selectAll) {
			if (selectAll) SelectAll();
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

		protected override void OnSelectedIndexChanged(EventArgs e) {
			base.OnSelectedIndexChanged(e);
			if (SelectedIndex != -1) NotifyDataGridViewOfValueChange();
		}
	}
}
