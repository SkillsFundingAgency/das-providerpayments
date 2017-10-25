namespace EventPublisher
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ProviderInput = new System.Windows.Forms.NumericUpDown();
            this.LearnerInput = new System.Windows.Forms.NumericUpDown();
            this.AccountInput = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.StartDatePicker = new System.Windows.Forms.DateTimePicker();
            this.EndDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.StandardInput = new System.Windows.Forms.NumericUpDown();
            this.FrameworkInput = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.ProgrammeInput = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.PathwayInput = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.CostInput = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ApprenticeshipIdInput = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.PriorityInput = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ProviderInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LearnerInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StandardInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FrameworkInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgrammeInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PathwayInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CostInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprenticeshipIdInput)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Apprenticeship Id";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 89);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Provider";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 121);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Learner";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 154);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Account";
            // 
            // ProviderInput
            // 
            this.ProviderInput.Location = new System.Drawing.Point(169, 86);
            this.ProviderInput.Margin = new System.Windows.Forms.Padding(4);
            this.ProviderInput.Maximum = new decimal(new int[] {
            -1,
            2147483647,
            0,
            0});
            this.ProviderInput.Name = "ProviderInput";
            this.ProviderInput.Size = new System.Drawing.Size(160, 22);
            this.ProviderInput.TabIndex = 7;
            // 
            // LearnerInput
            // 
            this.LearnerInput.Location = new System.Drawing.Point(169, 118);
            this.LearnerInput.Margin = new System.Windows.Forms.Padding(4);
            this.LearnerInput.Maximum = new decimal(new int[] {
            -1,
            2147483647,
            0,
            0});
            this.LearnerInput.Name = "LearnerInput";
            this.LearnerInput.Size = new System.Drawing.Size(160, 22);
            this.LearnerInput.TabIndex = 8;
            // 
            // AccountInput
            // 
            this.AccountInput.Location = new System.Drawing.Point(169, 150);
            this.AccountInput.Margin = new System.Windows.Forms.Padding(4);
            this.AccountInput.Name = "AccountInput";
            this.AccountInput.Size = new System.Drawing.Size(159, 22);
            this.AccountInput.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 238);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Start date";
            // 
            // StartDatePicker
            // 
            this.StartDatePicker.Location = new System.Drawing.Point(169, 230);
            this.StartDatePicker.Margin = new System.Windows.Forms.Padding(4);
            this.StartDatePicker.Name = "StartDatePicker";
            this.StartDatePicker.Size = new System.Drawing.Size(207, 22);
            this.StartDatePicker.TabIndex = 11;
            // 
            // EndDatePicker
            // 
            this.EndDatePicker.Location = new System.Drawing.Point(169, 262);
            this.EndDatePicker.Margin = new System.Windows.Forms.Padding(4);
            this.EndDatePicker.Name = "EndDatePicker";
            this.EndDatePicker.Size = new System.Drawing.Size(207, 22);
            this.EndDatePicker.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 270);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "End date";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 337);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Standard";
            // 
            // StandardInput
            // 
            this.StandardInput.Location = new System.Drawing.Point(169, 335);
            this.StandardInput.Margin = new System.Windows.Forms.Padding(4);
            this.StandardInput.Maximum = new decimal(new int[] {
            -1,
            2147483647,
            0,
            0});
            this.StandardInput.Name = "StandardInput";
            this.StandardInput.Size = new System.Drawing.Size(160, 22);
            this.StandardInput.TabIndex = 15;
            // 
            // FrameworkInput
            // 
            this.FrameworkInput.Location = new System.Drawing.Point(169, 382);
            this.FrameworkInput.Margin = new System.Windows.Forms.Padding(4);
            this.FrameworkInput.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.FrameworkInput.Name = "FrameworkInput";
            this.FrameworkInput.Size = new System.Drawing.Size(160, 22);
            this.FrameworkInput.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 384);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 17);
            this.label8.TabIndex = 16;
            this.label8.Text = "Framework";
            // 
            // ProgrammeInput
            // 
            this.ProgrammeInput.Location = new System.Drawing.Point(169, 414);
            this.ProgrammeInput.Margin = new System.Windows.Forms.Padding(4);
            this.ProgrammeInput.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ProgrammeInput.Name = "ProgrammeInput";
            this.ProgrammeInput.Size = new System.Drawing.Size(160, 22);
            this.ProgrammeInput.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 416);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 17);
            this.label9.TabIndex = 18;
            this.label9.Text = "Programme";
            // 
            // PathwayInput
            // 
            this.PathwayInput.Location = new System.Drawing.Point(169, 446);
            this.PathwayInput.Margin = new System.Windows.Forms.Padding(4);
            this.PathwayInput.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.PathwayInput.Name = "PathwayInput";
            this.PathwayInput.Size = new System.Drawing.Size(160, 22);
            this.PathwayInput.TabIndex = 21;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 448);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 17);
            this.label10.TabIndex = 20;
            this.label10.Text = "Pathway";
            // 
            // CostInput
            // 
            this.CostInput.DecimalPlaces = 2;
            this.CostInput.Location = new System.Drawing.Point(169, 513);
            this.CostInput.Margin = new System.Windows.Forms.Padding(4);
            this.CostInput.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.CostInput.Name = "CostInput";
            this.CostInput.Size = new System.Drawing.Size(160, 22);
            this.CostInput.TabIndex = 23;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 516);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 17);
            this.label11.TabIndex = 22;
            this.label11.Text = "Cost";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(20, 560);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(357, 28);
            this.SaveButton.TabIndex = 24;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ApprenticeshipIdInput
            // 
            this.ApprenticeshipIdInput.Location = new System.Drawing.Point(169, 16);
            this.ApprenticeshipIdInput.Margin = new System.Windows.Forms.Padding(4);
            this.ApprenticeshipIdInput.Maximum = new decimal(new int[] {
            -1,
            2147483647,
            0,
            0});
            this.ApprenticeshipIdInput.Name = "ApprenticeshipIdInput";
            this.ApprenticeshipIdInput.Size = new System.Drawing.Size(160, 22);
            this.ApprenticeshipIdInput.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 477);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 17);
            this.label12.TabIndex = 25;
            this.label12.Text = "Priority";
            // 
            // PriorityInput
            // 
            this.PriorityInput.Location = new System.Drawing.Point(169, 477);
            this.PriorityInput.Margin = new System.Windows.Forms.Padding(4);
            this.PriorityInput.Name = "PriorityInput";
            this.PriorityInput.Size = new System.Drawing.Size(159, 22);
            this.PriorityInput.TabIndex = 26;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 609);
            this.Controls.Add(this.PriorityInput);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.ApprenticeshipIdInput);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.CostInput);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.PathwayInput);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ProgrammeInput);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.FrameworkInput);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.StandardInput);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.EndDatePicker);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.StartDatePicker);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.AccountInput);
            this.Controls.Add(this.LearnerInput);
            this.Controls.Add(this.ProviderInput);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Commitment Event Publisher";
            ((System.ComponentModel.ISupportInitialize)(this.ProviderInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LearnerInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StandardInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FrameworkInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgrammeInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PathwayInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CostInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApprenticeshipIdInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown ProviderInput;
        private System.Windows.Forms.NumericUpDown LearnerInput;
        private System.Windows.Forms.TextBox AccountInput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker StartDatePicker;
        private System.Windows.Forms.DateTimePicker EndDatePicker;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown StandardInput;
        private System.Windows.Forms.NumericUpDown FrameworkInput;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown ProgrammeInput;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown PathwayInput;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown CostInput;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.NumericUpDown ApprenticeshipIdInput;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox PriorityInput;
    }
}

