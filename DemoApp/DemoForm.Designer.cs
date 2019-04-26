namespace DemoApp {
	partial class DemoForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DemoForm));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.label6 = new System.Windows.Forms.Label();
			this.rbVS = new System.Windows.Forms.RadioButton();
			this.rbPlain = new System.Windows.Forms.RadioButton();
			this.cmbList = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.chkRetainGroups = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.rbDisabled = new System.Windows.Forms.RadioButton();
			this.rbEnabled = new System.Windows.Forms.RadioButton();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.label12 = new System.Windows.Forms.Label();
			this.cmbEditable = new System.Windows.Forms.ComboBox();
			this.dsbExternal = new DropDownSearchBox();
			this.dsbListItems = new DropDownSearchBox();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.Column1 = new DropDownControls.GroupedComboBoxColumn();
			this.Column2 = new DropDownControls.ComboTreeBoxColumn();
			this.ctbFlatChecks = new ComboTreeBox();
			this.ctbImages = new ComboTreeBox();
			this.ctbCheckboxes = new ComboTreeBox();
			this.gcbList = new GroupedComboBox();
			this.gcbEditable = new GroupedComboBox();
			this.ctbNormal = new ComboTreeBox();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 119);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(160, 13);
			this.label1.TabIndex = 8;
			this.label1.Text = "GroupedComboBox (editable)";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 91);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(133, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "GroupedComboBox (list)";
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "Closed");
			this.imageList.Images.SetKeyName(1, "Open");
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 147);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(69, 13);
			this.label6.TabIndex = 10;
			this.label6.Text = "Visual styles";
			// 
			// rbVS
			// 
			this.rbVS.AutoSize = true;
			this.rbVS.Checked = true;
			this.rbVS.Location = new System.Drawing.Point(0, 0);
			this.rbVS.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
			this.rbVS.Name = "rbVS";
			this.rbVS.Size = new System.Drawing.Size(41, 17);
			this.rbVS.TabIndex = 0;
			this.rbVS.TabStop = true;
			this.rbVS.Text = "On";
			this.rbVS.UseVisualStyleBackColor = true;
			// 
			// rbPlain
			// 
			this.rbPlain.AutoSize = true;
			this.rbPlain.Location = new System.Drawing.Point(47, 0);
			this.rbPlain.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
			this.rbPlain.Name = "rbPlain";
			this.rbPlain.Size = new System.Drawing.Size(42, 17);
			this.rbPlain.TabIndex = 1;
			this.rbPlain.Text = "Off";
			this.rbPlain.UseVisualStyleBackColor = true;
			// 
			// cmbList
			// 
			this.cmbList.DropDownHeight = 150;
			this.cmbList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbList.FormattingEnabled = true;
			this.cmbList.IntegralHeight = false;
			this.cmbList.Location = new System.Drawing.Point(211, 35);
			this.cmbList.Name = "cmbList";
			this.cmbList.Size = new System.Drawing.Size(200, 21);
			this.cmbList.Sorted = true;
			this.cmbList.TabIndex = 3;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 38);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(87, 13);
			this.label7.TabIndex = 2;
			this.label7.Text = "ComboBox (list)";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.rbVS);
			this.flowLayoutPanel1.Controls.Add(this.rbPlain);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(211, 145);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(95, 17);
			this.flowLayoutPanel1.TabIndex = 11;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 170);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 13);
			this.label3.TabIndex = 12;
			this.label3.Text = "ComboTreeBox (normal)";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 228);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(151, 13);
			this.label4.TabIndex = 16;
			this.label4.Text = "ComboTreeBox (checkboxes)";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 199);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(128, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "ComboTreeBox (images)";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(12, 256);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(158, 13);
			this.label8.TabIndex = 18;
			this.label8.Text = "ComboTreeBox (checkbox list)";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(12, 284);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(171, 13);
			this.label9.TabIndex = 20;
			this.label9.Text = "DropDownSearchBox (list items)";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(12, 335);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(193, 13);
			this.label10.TabIndex = 23;
			this.label10.Text = "DropDownSearchBox (external data)";
			// 
			// chkRetainGroups
			// 
			this.chkRetainGroups.AutoSize = true;
			this.chkRetainGroups.Location = new System.Drawing.Point(211, 310);
			this.chkRetainGroups.Name = "chkRetainGroups";
			this.chkRetainGroups.Size = new System.Drawing.Size(181, 17);
			this.chkRetainGroups.TabIndex = 22;
			this.chkRetainGroups.Text = "Show groups in search results";
			this.chkRetainGroups.UseVisualStyleBackColor = true;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(12, 14);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(33, 13);
			this.label11.TabIndex = 0;
			this.label11.Text = "State";
			// 
			// rbDisabled
			// 
			this.rbDisabled.AutoSize = true;
			this.rbDisabled.Location = new System.Drawing.Point(73, 0);
			this.rbDisabled.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
			this.rbDisabled.Name = "rbDisabled";
			this.rbDisabled.Size = new System.Drawing.Size(70, 17);
			this.rbDisabled.TabIndex = 1;
			this.rbDisabled.Text = "Disabled";
			this.rbDisabled.UseVisualStyleBackColor = true;
			// 
			// rbEnabled
			// 
			this.rbEnabled.AutoSize = true;
			this.rbEnabled.Checked = true;
			this.rbEnabled.Location = new System.Drawing.Point(0, 0);
			this.rbEnabled.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
			this.rbEnabled.Name = "rbEnabled";
			this.rbEnabled.Size = new System.Drawing.Size(67, 17);
			this.rbEnabled.TabIndex = 0;
			this.rbEnabled.TabStop = true;
			this.rbEnabled.Text = "Enabled";
			this.rbEnabled.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.AutoSize = true;
			this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel2.Controls.Add(this.rbEnabled);
			this.flowLayoutPanel2.Controls.Add(this.rbDisabled);
			this.flowLayoutPanel2.Location = new System.Drawing.Point(211, 12);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(149, 17);
			this.flowLayoutPanel2.TabIndex = 1;
			this.flowLayoutPanel2.WrapContents = false;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(12, 65);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(114, 13);
			this.label12.TabIndex = 4;
			this.label12.Text = "ComboBox (editable)";
			// 
			// cmbEditable
			// 
			this.cmbEditable.DropDownHeight = 150;
			this.cmbEditable.FormattingEnabled = true;
			this.cmbEditable.IntegralHeight = false;
			this.cmbEditable.Location = new System.Drawing.Point(211, 62);
			this.cmbEditable.Name = "cmbEditable";
			this.cmbEditable.Size = new System.Drawing.Size(200, 21);
			this.cmbEditable.Sorted = true;
			this.cmbEditable.TabIndex = 5;
			// 
			// dsbExternal
			// 
			this.dsbExternal.DropDownHeight = 300;
			this.dsbExternal.DroppedDown = false;
			this.dsbExternal.Location = new System.Drawing.Point(211, 333);
			this.dsbExternal.Name = "dsbExternal";
			this.dsbExternal.SelectedNode = null;
			this.dsbExternal.ShowGlyphs = false;
			this.dsbExternal.Size = new System.Drawing.Size(200, 22);
			this.dsbExternal.TabIndex = 24;
			// 
			// dsbListItems
			// 
			this.dsbListItems.DropDownHeight = 300;
			this.dsbListItems.DroppedDown = false;
			this.dsbListItems.Location = new System.Drawing.Point(211, 282);
			this.dsbListItems.Name = "dsbListItems";
			this.dsbListItems.SelectedNode = null;
			this.dsbListItems.ShowGlyphs = false;
			this.dsbListItems.Size = new System.Drawing.Size(200, 22);
			this.dsbListItems.TabIndex = 21;
			// 
			// dataGridView1
			// 
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
			this.dataGridView1.Location = new System.Drawing.Point(10, 364);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(401, 141);
			this.dataGridView1.TabIndex = 25;
			// 
			// Column1
			// 
			this.Column1.DisplayMember = null;
			this.Column1.GroupMember = null;
			this.Column1.HeaderText = "GroupedComboBoxColumn";
			this.Column1.Name = "Column1";
			this.Column1.ValueMember = null;
			// 
			// Column2
			// 
			this.Column2.HeaderText = "ComboTreeBoxColumn";
			this.Column2.Name = "Column2";
			this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// ctbFlatChecks
			// 
			this.ctbFlatChecks.DroppedDown = false;
			this.ctbFlatChecks.Location = new System.Drawing.Point(211, 254);
			this.ctbFlatChecks.Name = "ctbFlatChecks";
			this.ctbFlatChecks.SelectedNode = null;
			this.ctbFlatChecks.ShowCheckBoxes = true;
			this.ctbFlatChecks.Size = new System.Drawing.Size(200, 22);
			this.ctbFlatChecks.TabIndex = 19;
			// 
			// ctbImages
			// 
			this.ctbImages.DroppedDown = false;
			this.ctbImages.ExpandedImageIndex = 1;
			this.ctbImages.Images = this.imageList;
			this.ctbImages.Location = new System.Drawing.Point(211, 197);
			this.ctbImages.Name = "ctbImages";
			this.ctbImages.SelectedNode = null;
			this.ctbImages.Size = new System.Drawing.Size(200, 22);
			this.ctbImages.TabIndex = 15;
			// 
			// ctbCheckboxes
			// 
			this.ctbCheckboxes.DroppedDown = false;
			this.ctbCheckboxes.Location = new System.Drawing.Point(211, 226);
			this.ctbCheckboxes.Name = "ctbCheckboxes";
			this.ctbCheckboxes.SelectedNode = null;
			this.ctbCheckboxes.ShowCheckBoxes = true;
			this.ctbCheckboxes.Size = new System.Drawing.Size(200, 22);
			this.ctbCheckboxes.TabIndex = 17;
			// 
			// gcbList
			// 
			this.gcbList.DataSource = null;
			this.gcbList.DropDownHeight = 150;
			this.gcbList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.gcbList.FormattingEnabled = true;
			this.gcbList.IntegralHeight = false;
			this.gcbList.Location = new System.Drawing.Point(211, 89);
			this.gcbList.Name = "gcbList";
			this.gcbList.Size = new System.Drawing.Size(200, 23);
			this.gcbList.TabIndex = 7;
			// 
			// gcbEditable
			// 
			this.gcbEditable.DataSource = null;
			this.gcbEditable.DropDownHeight = 150;
			this.gcbEditable.FormattingEnabled = true;
			this.gcbEditable.IntegralHeight = false;
			this.gcbEditable.Location = new System.Drawing.Point(211, 116);
			this.gcbEditable.Name = "gcbEditable";
			this.gcbEditable.Size = new System.Drawing.Size(200, 23);
			this.gcbEditable.TabIndex = 9;
			// 
			// ctbNormal
			// 
			this.ctbNormal.DroppedDown = false;
			this.ctbNormal.Location = new System.Drawing.Point(211, 168);
			this.ctbNormal.Name = "ctbNormal";
			this.ctbNormal.SelectedNode = null;
			this.ctbNormal.Size = new System.Drawing.Size(200, 22);
			this.ctbNormal.TabIndex = 13;
			// 
			// DemoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(423, 517);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.cmbEditable);
			this.Controls.Add(this.flowLayoutPanel2);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.chkRetainGroups);
			this.Controls.Add(this.dsbExternal);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.dsbListItems);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.ctbFlatChecks);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.cmbList);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.ctbImages);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.ctbCheckboxes);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.gcbList);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gcbEditable);
			this.Controls.Add(this.ctbNormal);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DemoForm";
			this.Text = "DropDownControls Demo";
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private GroupedComboBox gcbEditable;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private GroupedComboBox gcbList;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.RadioButton rbVS;
		private System.Windows.Forms.RadioButton rbPlain;
		private System.Windows.Forms.ComboBox cmbList;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.DataGridView dataGridView1;
		private DropDownControls.GroupedComboBoxColumn Column1;
		private DropDownControls.ComboTreeBoxColumn Column2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private ComboTreeBox ctbNormal;
		private System.Windows.Forms.Label label3;
		private ComboTreeBox ctbCheckboxes;
		private System.Windows.Forms.Label label4;
		private ComboTreeBox ctbImages;
		private System.Windows.Forms.Label label5;
		private ComboTreeBox ctbFlatChecks;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private DropDownSearchBox dsbListItems;
		private System.Windows.Forms.Label label10;
		private DropDownSearchBox dsbExternal;
		private System.Windows.Forms.CheckBox chkRetainGroups;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.RadioButton rbDisabled;
		private System.Windows.Forms.RadioButton rbEnabled;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox cmbEditable;
	}
}