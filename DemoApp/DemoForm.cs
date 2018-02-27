using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Diagnostics;
using DropDownControls;

namespace DemoApp {

	public partial class DemoForm : Form {

		DataTable _table;

		public DemoForm() {
			InitializeComponent();

			rbPlain.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
			rbVS.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);

			dsbListItems.PerformSearch += dsbListItems_PerformSearch;

			// define our collection of list items
			var groupedItems = new[] {
				new { Group = "Gases", Value = 1, Display = "Helium", ToolTip = "Lighter than air" },
				new { Group = "Gases", Value = 2, Display = "Hydrogen", ToolTip = "Explosive" },
				new { Group = "Gases", Value = 3, Display = "Oxygen", ToolTip = "Vital for animal life" },
				new { Group = "Gases", Value = 4, Display = "Argon", ToolTip = "Inert" },
				new { Group = "Metals", Value = 5, Display = "Iron", ToolTip = "Heavy and metallic" },
				new { Group = "Metals", Value = 6, Display = "Lithium", ToolTip = "Explodes in water" },
				new { Group = "Metals", Value = 7, Display = "Copper", ToolTip = "Good electrical conductor" },
				new { Group = "Metals", Value = 8, Display = "Gold", ToolTip = "Precious metal" },
				new { Group = "Metals", Value = 9, Display = "Silver", ToolTip = "Anti-bacterial" },
				new { Group = "Radioactive", Value = 10, Display = "Uranium", ToolTip = "Used in fission" },
				new { Group = "Radioactive", Value = 11, Display = "Plutonium", ToolTip = "Man-made" },
				new { Group = "Radioactive", Value = 12, Display = "Americium", ToolTip = "Used in smoke detectors" },
				new { Group = "Radioactive", Value = 13, Display = "Radon", ToolTip = "Radioactive gas" }
			};

			Action<ComboTreeNodeCollection> addNodesHelper = nodes => {
				foreach (var grp in groupedItems.GroupBy(x => x.Group)) {
					ComboTreeNode parent = nodes.Add(grp.Key);
					foreach (var item in grp) {
						ComboTreeNode child = parent.Nodes.Add(item.Display);
						child.ToolTip = item.ToolTip;
					}
				}
			};

			// anonymous method delegate for transforming the above into nodes
			Action<ComboTreeBox> addNodes = ctb => {
				addNodesHelper(ctb.Nodes);
				ctb.Sort();
				ctb.SelectedNode = ctb.Nodes[0].Nodes[0];
			};

			// normal combobox
			cmbNormal.ValueMember = "Value";
			cmbNormal.DisplayMember = "Display";
			cmbNormal.DataSource = new BindingSource(groupedItems, String.Empty);

			// grouped comboboxes
			gcbList.ValueMember = "Value";
			gcbList.DisplayMember = "Display";
			gcbList.GroupMember = "Group";
			gcbList.DataSource = new BindingSource(groupedItems, String.Empty);

			gcbEditable.ValueMember = "Value";
			gcbEditable.DisplayMember = "Display";
			gcbEditable.GroupMember = "Group";
			gcbEditable.SortComparer = new DemoComparer();
			gcbEditable.DataSource = new BindingSource(groupedItems, String.Empty);

			// combotreeboxes
			addNodes(ctbNormal);
			addNodes(ctbImages);

			addNodes(ctbCheckboxes);
			ctbCheckboxes.Nodes[0].Nodes[1].Nodes.Add(new ComboTreeNode("Deuterium"));
			ctbCheckboxes.Nodes[0].Nodes[1].Nodes.Add(new ComboTreeNode("Tritium"));
			ctbCheckboxes.CheckedNodes = new ComboTreeNode[] {
						 ctbCheckboxes.Nodes[1].Nodes[0],
						 ctbCheckboxes.Nodes[1].Nodes[1]
					 };

			foreach (var item in groupedItems) {
				ctbFlatChecks.Nodes.Add(item.Display);
			}

			ctbFlatChecks.CheckedNodes = new ComboTreeNode[] {
				ctbFlatChecks.Nodes[0],
				ctbFlatChecks.Nodes[1]
			};

			// dropdownsearchboxes
			addNodes(dsbListItems);

			dsbExternal.BeginUpdate();
			dsbExternal.Nodes.Add("example");
			dsbExternal.Nodes.Add("nodes");
			dsbExternal.Nodes.Add("already");
			dsbExternal.Nodes.Add("in");
			dsbExternal.Nodes.Add("list");
			dsbExternal.EndUpdate();
			dsbExternal.PerformSearch += dsbExternal_PerformSearch;
			dsbExternal.SelectedNode = dsbExternal.Nodes[0];

			// datagridview columns
			Column1.ValueMember = "Value";
			Column1.DisplayMember = "Display";
			Column1.GroupMember = "Group";
			Column1.DataSource = new BindingSource(groupedItems, String.Empty);

			Column2.Images = imageList;
			Column2.ImageIndex = 0;
			Column2.ExpandedImageIndex = 1;
			addNodesHelper(Column2.Nodes);
		}

		void dsbListItems_PerformSearch(object sender, PerformSearchEventArgs e) {
			if (chkRetainGroups.Checked) {
				foreach (ComboTreeNode node in dsbListItems.AllNormalNodes) {
					e.CancellationToken.ThrowIfCancellationRequested();

					if (dsbListItems.DefaultSearchPredicate(node, e.SearchTerm)) {
						// get all ancestor nodes (including the result)
						Stack<ComboTreeNode> ancestors = new Stack<ComboTreeNode>();
						ComboTreeNode current = node;
						while (current != null) {
							ancestors.Push(current);
							current = current.Parent;
						}

						// copy ancestor nodes into search results (or re-use existing)
						ComboTreeNodeCollection collection = e.Results;
						while (ancestors.Any()) {
							current = ancestors.Pop();

							ComboTreeNode copy = e.Results.Find(x => dsbListItems.DefaultEquivalencePredicate(x, current), true);
							if (copy == null) collection.Add(copy = current.Clone());

							collection = copy.Nodes;
						}
					}
				}

				e.Handled = true;
			}
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);

			_table = new DataTable();
			_table.PrimaryKey = new DataColumn[] { _table.Columns.Add("Word") };

			try {
				using (StreamReader sr = new StreamReader(File.OpenRead("WordList.txt"))) {
					string line;
					while ((line = sr.ReadLine()) != null) {
						_table.Rows.Add(line.Trim());
					}
				}
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		[DebuggerHidden]
		void dsbExternal_PerformSearch(object sender, PerformSearchEventArgs e) {
			string adoFilter = String.Format("Word LIKE '{0}%'", e.SearchTerm.Replace("'", "''"));
			foreach (DataRow dr in _table.Select(adoFilter)) {
				e.CancellationToken.ThrowIfCancellationRequested();
				e.Results.Add(new ComboTreeNode(dr.Field<string>(0)));
			}
		}

		void radioButtons_CheckedChanged(object sender, EventArgs e) {
			ctbNormal.DrawWithVisualStyles = rbVS.Checked;
			ctbImages.DrawWithVisualStyles = rbVS.Checked;
			ctbCheckboxes.DrawWithVisualStyles = rbVS.Checked;
			ctbFlatChecks.DrawWithVisualStyles = rbVS.Checked;
			dsbListItems.DrawWithVisualStyles = rbVS.Checked;
			dsbExternal.DrawWithVisualStyles = rbVS.Checked;
		}

		/// <summary>
		/// Used to demonstrate the <see cref="GroupedComboBox.SortComparer"/> property.
		/// </summary>
		private class DemoComparer : IComparer {

			public int Compare(object x, object y) {
				return -Comparer.Default.Compare(x, y);
			}
		}
	}
}
