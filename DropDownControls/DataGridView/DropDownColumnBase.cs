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
	/// Base class for custom <see cref="DataGridView"/> column types.
	/// </summary>
	[ToolboxBitmapAttribute(typeof(DataGridViewComboBoxColumn), "DataGridViewComboBoxColumn.bmp")]
	public abstract class DropDownColumnBase : DataGridViewColumn {

		/// <summary>
		/// Gets a value indicating whether buffered painting is supported. 
		/// Used when rendering cells.
		/// </summary>
		internal bool BufferedPaintingSupported { get; private set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		protected DropDownColumnBase() {
			BufferedPaintingSupported = BufferedPainting.BufferedPainter<ComboBoxState>.IsSupported();
		}
	}

	/// <summary>
	/// Base class for custom <see cref="DataGridView"/> cell types.
	/// </summary>
	public abstract class DropDownCellBase : DataGridViewCell {

		private bool _wasCurrentCell;

		/// <summary>
		/// Paints common elements behind the cell content.
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="cellBounds"></param>
		/// <param name="cellState"></param>
		/// <param name="cellStyle"></param>
		/// <param name="paintParts"></param>
		protected void BeforePaintContent(Graphics graphics, Rectangle cellBounds, DataGridViewElementStates cellState, DataGridViewCellStyle cellStyle, DataGridViewPaintParts paintParts) {
			if (paintParts.HasFlag(DataGridViewPaintParts.Background)) {
				using (Brush brush = new SolidBrush(cellStyle.BackColor)) {
					graphics.FillRectangle(brush, cellBounds);
				}
			}

			if (paintParts.HasFlag(DataGridViewPaintParts.SelectionBackground) && cellState.HasFlag(DataGridViewElementStates.Selected)) {
				using (Brush brush = new SolidBrush(cellStyle.SelectionBackColor)) {
					graphics.FillRectangle(brush, cellBounds);
				}
			}

			if (paintParts.HasFlag(DataGridViewPaintParts.ContentBackground)) {
				ComboBoxState state = ComboBoxState.Normal;
				ButtonState legacyState = ButtonState.Normal;

				if ((DataGridView.Site == null) || !DataGridView.Site.DesignMode) {
					if (cellState.HasFlag(DataGridViewElementStates.ReadOnly)) {
						state = ComboBoxState.Disabled;
						legacyState = ButtonState.Inactive;
					}
					else if (cellBounds.Contains(DataGridView.PointToClient(DataGridView.MousePosition))) {
						if (DataGridView.MouseButtons.HasFlag(MouseButtons.Left)) {
							state = ComboBoxState.Pressed;
							legacyState = ButtonState.Pushed;
						}
						else {
							state = ComboBoxState.Hot;
						}
					}
				}

				Rectangle comboBounds = cellBounds;
				comboBounds.Width--;

				if (((DropDownColumnBase)OwningColumn).BufferedPaintingSupported) {
					GroupedComboBox.DrawComboBox(graphics, comboBounds, state);
				}
				else {
					DropDownControlBase.DrawLegacyComboBox(graphics, comboBounds, DropDownControlBase.GetComboButtonBounds(comboBounds), cellStyle.BackColor, legacyState);
				}
			}
		}

		/// <summary>
		/// Paints common elements in front of the cell content.
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="clipBounds"></param>
		/// <param name="cellBounds"></param>
		/// <param name="rowIndex"></param>
		/// <param name="cellStyle"></param>
		/// <param name="advancedBorderStyle"></param>
		/// <param name="paintParts"></param>
		protected void AfterPaintContent(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts) {
			if (paintParts.HasFlag(DataGridViewPaintParts.ErrorIcon)) {
				base.PaintErrorIcon(graphics, clipBounds, cellBounds, GetErrorText(rowIndex));
			}

			if (paintParts.HasFlag(DataGridViewPaintParts.Border)) {
				base.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}

			if (paintParts.HasFlag(DataGridViewPaintParts.Focus) && (DataGridView.CurrentCell == this)) {
				Rectangle focusBounds = cellBounds;
				focusBounds.Width--;
				focusBounds.Height--;
				ControlPaint.DrawFocusRectangle(graphics, focusBounds);
			}
		}

		protected override void OnMouseEnter(int rowIndex) {
			base.OnMouseEnter(rowIndex);
			DataGridView.InvalidateCell(OwningColumn.Index, rowIndex);
		}

		protected override void OnMouseLeave(int rowIndex) {
			base.OnMouseLeave(rowIndex);
			DataGridView.InvalidateCell(OwningColumn.Index, rowIndex);
		}

		protected override void OnMouseDown(DataGridViewCellMouseEventArgs e) {
			_wasCurrentCell = (DataGridView.CurrentCellAddress == new Point(e.ColumnIndex, e.RowIndex));
			base.OnMouseDown(e);
			DataGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);
		}

		protected override void OnMouseUp(DataGridViewCellMouseEventArgs e) {
			base.OnMouseUp(e);
			DataGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);
		}

		protected override void OnMouseClick(DataGridViewCellMouseEventArgs e) {
			if (!ReadOnly && _wasCurrentCell) DataGridView.BeginEdit(true);
			base.OnMouseClick(e);
		}
	}
}
