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
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.label6 = new System.Windows.Forms.Label();
			this.rbVS = new System.Windows.Forms.RadioButton();
			this.rbPlain = new System.Windows.Forms.RadioButton();
			this.cmbNormal = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.chkRetainGroups = new System.Windows.Forms.CheckBox();
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
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 69);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(160, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "GroupedComboBox (editable)";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(133, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "GroupedComboBox (list)";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 122);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "ComboTreeBox (normal)";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 180);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(151, 13);
			this.label4.TabIndex = 13;
			this.label4.Text = "ComboTreeBox (checkboxes)";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 151);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(128, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "ComboTreeBox (images)";
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
			this.label6.Location = new System.Drawing.Point(12, 97);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(69, 13);
			this.label6.TabIndex = 6;
			this.label6.Text = "Visual styles";
			// 
			// rbVS
			// 
			this.rbVS.AutoSize = true;
			this.rbVS.Checked = true;
			this.rbVS.Location = new System.Drawing.Point(211, 96);
			this.rbVS.Name = "rbVS";
			this.rbVS.Size = new System.Drawing.Size(41, 17);
			this.rbVS.TabIndex = 7;
			this.rbVS.TabStop = true;
			this.rbVS.Text = "On";
			this.rbVS.UseVisualStyleBackColor = true;
			// 
			// rbPlain
			// 
			this.rbPlain.AutoSize = true;
			this.rbPlain.Location = new System.Drawing.Point(256, 96);
			this.rbPlain.Name = "rbPlain";
			this.rbPlain.Size = new System.Drawing.Size(42, 17);
			this.rbPlain.TabIndex = 8;
			this.rbPlain.Text = "Off";
			this.rbPlain.UseVisualStyleBackColor = true;
			// 
			// cmbNormal
			// 
			this.cmbNormal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbNormal.FormattingEnabled = true;
			this.cmbNormal.Location = new System.Drawing.Point(211, 12);
			this.cmbNormal.Name = "cmbNormal";
			this.cmbNormal.Size = new System.Drawing.Size(200, 21);
			this.cmbNormal.Sorted = true;
			this.cmbNormal.TabIndex = 1;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 15);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(63, 13);
			this.label7.TabIndex = 0;
			this.label7.Text = "ComboBox";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(12, 208);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(158, 13);
			this.label8.TabIndex = 15;
			this.label8.Text = "ComboTreeBox (checkbox list)";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(12, 236);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(171, 13);
			this.label9.TabIndex = 18;
			this.label9.Text = "DropDownSearchBox (list items)";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(12, 287);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(193, 13);
			this.label10.TabIndex = 20;
			this.label10.Text = "DropDownSearchBox (external data)";
			// 
			// chkRetainGroups
			// 
			this.chkRetainGroups.AutoSize = true;
			this.chkRetainGroups.Location = new System.Drawing.Point(211, 262);
			this.chkRetainGroups.Name = "chkRetainGroups";
			this.chkRetainGroups.Size = new System.Drawing.Size(181, 17);
			this.chkRetainGroups.TabIndex = 22;
			this.chkRetainGroups.Text = "Show groups in search results";
			this.chkRetainGroups.UseVisualStyleBackColor = true;
			// 
			// dsbExternal
			// 
			this.dsbExternal.DropDownHeight = 300;
			this.dsbExternal.DroppedDown = false;
			this.dsbExternal.Location = new System.Drawing.Point(211, 285);
			this.dsbExternal.Name = "dsbExternal";
			this.dsbExternal.SelectedNode = null;
			this.dsbExternal.ShowGlyphs = false;
			this.dsbExternal.Size = new System.Drawing.Size(200, 22);
			this.dsbExternal.TabIndex = 21;
			// 
			// dsbListItems
			// 
			this.dsbListItems.DropDownHeight = 300;
			this.dsbListItems.DroppedDown = false;
			this.dsbListItems.Location = new System.Drawing.Point(211, 234);
			this.dsbListItems.Name = "dsbListItems";
			this.dsbListItems.SelectedNode = null;
			this.dsbListItems.ShowGlyphs = false;
			this.dsbListItems.Size = new System.Drawing.Size(200, 22);
			this.dsbListItems.TabIndex = 19;
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
			this.dataGridView1.Location = new System.Drawing.Point(10, 313);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(401, 192);
			this.dataGridView1.TabIndex = 17;
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
			this.ctbFlatChecks.Location = new System.Drawing.Point(211, 206);
			this.ctbFlatChecks.Name = "ctbFlatChecks";
			this.ctbFlatChecks.SelectedNode = null;
			this.ctbFlatChecks.ShowCheckBoxes = true;
			this.ctbFlatChecks.Size = new System.Drawing.Size(200, 22);
			this.ctbFlatChecks.TabIndex = 16;
			// 
			// ctbImages
			// 
			this.ctbImages.DroppedDown = false;
			this.ctbImages.ExpandedImageIndex = 1;
			this.ctbImages.Images = this.imageList;
			this.ctbImages.Location = new System.Drawing.Point(211, 149);
			this.ctbImages.Name = "ctbImages";
			this.ctbImages.SelectedNode = null;
			this.ctbImages.Size = new System.Drawing.Size(200, 22);
			this.ctbImages.TabIndex = 12;
			// 
			// ctbCheckboxes
			// 
			this.ctbCheckboxes.DroppedDown = false;
			this.ctbCheckboxes.Location = new System.Drawing.Point(211, 178);
			this.ctbCheckboxes.Name = "ctbCheckboxes";
			this.ctbCheckboxes.SelectedNode = null;
			this.ctbCheckboxes.ShowCheckBoxes = true;
			this.ctbCheckboxes.Size = new System.Drawing.Size(200, 22);
			this.ctbCheckboxes.TabIndex = 14;
			// 
			// gcbList
			// 
			this.gcbList.DataSource = null;
			this.gcbList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.gcbList.FormattingEnabled = true;
			this.gcbList.Location = new System.Drawing.Point(211, 39);
			this.gcbList.Name = "gcbList";
			this.gcbList.Size = new System.Drawing.Size(200, 23);
			this.gcbList.TabIndex = 3;
			// 
			// gcbEditable
			// 
			this.gcbEditable.DataSource = null;
			this.gcbEditable.FormattingEnabled = true;
			this.gcbEditable.Location = new System.Drawing.Point(211, 66);
			this.gcbEditable.Name = "gcbEditable";
			this.gcbEditable.Size = new System.Drawing.Size(200, 23);
			this.gcbEditable.TabIndex = 5;
			// 
			// ctbNormal
			// 
			this.ctbNormal.DroppedDown = false;
			this.ctbNormal.Location = new System.Drawing.Point(211, 120);
			this.ctbNormal.Name = "ctbNormal";
			this.ctbNormal.SelectedNode = null;
			this.ctbNormal.Size = new System.Drawing.Size(200, 22);
			this.ctbNormal.TabIndex = 10;
			// 
			// DemoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(423, 517);
			this.Controls.Add(this.chkRetainGroups);
			this.Controls.Add(this.dsbExternal);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.dsbListItems);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.ctbFlatChecks);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.cmbNormal);
			this.Controls.Add(this.rbPlain);
			this.Controls.Add(this.rbVS);
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
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ComboTreeBox ctbNormal;
		private GroupedComboBox gcbEditable;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private GroupedComboBox gcbList;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private ComboTreeBox ctbCheckboxes;
		private System.Windows.Forms.Label label5;
		private ComboTreeBox ctbImages;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.RadioButton rbVS;
		private System.Windows.Forms.RadioButton rbPlain;
		private System.Windows.Forms.ComboBox cmbNormal;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private ComboTreeBox ctbFlatChecks;
		private System.Windows.Forms.DataGridView dataGridView1;
		private DropDownControls.GroupedComboBoxColumn Column1;
		private DropDownControls.ComboTreeBoxColumn Column2;
		private System.Windows.Forms.Label label9;
		private DropDownSearchBox dsbListItems;
		private System.Windows.Forms.Label label10;
		private DropDownSearchBox dsbExternal;
		private System.Windows.Forms.CheckBox chkRetainGroups;
	}
}