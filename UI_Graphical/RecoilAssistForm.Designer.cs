using RecoilAssist.Patterns;

namespace RecoilAssist.UI_Graphical
{
    partial class RecoilAssistForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecoilAssistForm));
            this.PatternGridView = new System.Windows.Forms.DataGridView();
            this.PatternTypeComboBox = new System.Windows.Forms.ComboBox();
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Function = new System.Windows.Forms.DataGridViewImageColumn();
            this.A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.B = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActiveCheckbox = new RecoilAssist.UI_Graphical.MyCheckBox();
            this.directionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stopTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Patterns = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PatternGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Patterns)).BeginInit();
            this.SuspendLayout();
            // 
            // PatternGridView
            // 
            this.PatternGridView.AutoGenerateColumns = false;
            this.PatternGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PatternGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Function,
            this.directionDataGridViewTextBoxColumn,
            this.startTimeDataGridViewTextBoxColumn,
            this.stopTimeDataGridViewTextBoxColumn,
            this.A,
            this.B,
            this.C});
            this.PatternGridView.DataSource = this.Patterns;
            this.PatternGridView.Location = new System.Drawing.Point(12, 39);
            this.PatternGridView.Name = "PatternGridView";
            this.PatternGridView.Size = new System.Drawing.Size(763, 314);
            this.PatternGridView.TabIndex = 0;
            // 
            // PatternTypeComboBox
            // 
            this.PatternTypeComboBox.FormattingEnabled = true;
            this.PatternTypeComboBox.Location = new System.Drawing.Point(12, 12);
            this.PatternTypeComboBox.Name = "PatternTypeComboBox";
            this.PatternTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.PatternTypeComboBox.TabIndex = 1;
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(139, 10);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddButton.TabIndex = 2;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(220, 10);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveButton.TabIndex = 4;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(665, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "Active";
            // 
            // Function
            // 
            this.Function.HeaderText = "Function";
            this.Function.Name = "Function";
            this.Function.ReadOnly = true;
            this.Function.Width = 120;
            // 
            // A
            // 
            this.A.DataPropertyName = "A";
            this.A.HeaderText = "A";
            this.A.Name = "A";
            // 
            // B
            // 
            this.B.DataPropertyName = "B";
            this.B.HeaderText = "B";
            this.B.Name = "B";
            // 
            // C
            // 
            this.C.DataPropertyName = "C";
            this.C.HeaderText = "C";
            this.C.Name = "C";
            // 
            // ActiveCheckbox
            // 
            this.ActiveCheckbox.Appearance = System.Windows.Forms.Appearance.Button;
            this.ActiveCheckbox.AutoSize = true;
            this.ActiveCheckbox.Location = new System.Drawing.Point(727, 10);
            this.ActiveCheckbox.Name = "ActiveCheckbox";
            this.ActiveCheckbox.Padding = new System.Windows.Forms.Padding(6);
            this.ActiveCheckbox.Size = new System.Drawing.Size(47, 23);
            this.ActiveCheckbox.TabIndex = 3;
            this.ActiveCheckbox.Text = "Active";
            this.ActiveCheckbox.UseVisualStyleBackColor = true;
            this.ActiveCheckbox.CheckedChanged += new System.EventHandler(this.ActiveCheckbox_CheckedChanged);
            // 
            // directionDataGridViewTextBoxColumn
            // 
            this.directionDataGridViewTextBoxColumn.DataPropertyName = "Direction";
            this.directionDataGridViewTextBoxColumn.HeaderText = "Direction";
            this.directionDataGridViewTextBoxColumn.Name = "directionDataGridViewTextBoxColumn";
            // 
            // startTimeDataGridViewTextBoxColumn
            // 
            this.startTimeDataGridViewTextBoxColumn.DataPropertyName = "StartTime";
            this.startTimeDataGridViewTextBoxColumn.HeaderText = "StartTime";
            this.startTimeDataGridViewTextBoxColumn.Name = "startTimeDataGridViewTextBoxColumn";
            // 
            // stopTimeDataGridViewTextBoxColumn
            // 
            this.stopTimeDataGridViewTextBoxColumn.DataPropertyName = "StopTime";
            this.stopTimeDataGridViewTextBoxColumn.HeaderText = "StopTime";
            this.stopTimeDataGridViewTextBoxColumn.Name = "stopTimeDataGridViewTextBoxColumn";
            // 
            // Patterns
            // 
            this.Patterns.DataSource = typeof(RecoilAssist.Patterns.MovementPattern);
            // 
            // RecoilAssistForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 365);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.ActiveCheckbox);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.PatternTypeComboBox);
            this.Controls.Add(this.PatternGridView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RecoilAssistForm";
            this.Text = "Recoil Assist";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RecoilAssistForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.PatternGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Patterns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource Patterns;
        private System.Windows.Forms.DataGridView PatternGridView;
        private System.Windows.Forms.ComboBox PatternTypeComboBox;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private MyCheckBox ActiveCheckbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewImageColumn Function;
        private System.Windows.Forms.DataGridViewTextBoxColumn directionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stopTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn A;
        private System.Windows.Forms.DataGridViewTextBoxColumn B;
        private System.Windows.Forms.DataGridViewTextBoxColumn C;
    }
}