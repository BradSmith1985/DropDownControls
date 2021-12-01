namespace DemoApp {
	partial class TimePickerForm {
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
			this.dropDownTimePicker1 = new DropDownTimePicker();
			this.dropDownTimePicker2 = new DropDownTimePicker();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// dropDownTimePicker1
			// 
			this.dropDownTimePicker1.DroppedDown = false;
			this.dropDownTimePicker1.Location = new System.Drawing.Point(12, 12);
			this.dropDownTimePicker1.Name = "dropDownTimePicker1";
			this.dropDownTimePicker1.Size = new System.Drawing.Size(160, 23);
			this.dropDownTimePicker1.TabIndex = 0;
			// 
			// dropDownTimePicker2
			// 
			this.dropDownTimePicker2.DroppedDown = false;
			this.dropDownTimePicker2.Location = new System.Drawing.Point(12, 41);
			this.dropDownTimePicker2.Name = "dropDownTimePicker2";
			this.dropDownTimePicker2.ShowCheckBox = false;
			this.dropDownTimePicker2.Size = new System.Drawing.Size(160, 23);
			this.dropDownTimePicker2.TabIndex = 1;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(12, 70);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(160, 20);
			this.textBox1.TabIndex = 2;
			// 
			// TimePickerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.dropDownTimePicker2);
			this.Controls.Add(this.dropDownTimePicker1);
			this.Name = "TimePickerForm";
			this.Text = "TimePickerForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DropDownTimePicker dropDownTimePicker1;
		private DropDownTimePicker dropDownTimePicker2;
		private System.Windows.Forms.TextBox textBox1;
	}
}