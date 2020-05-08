using System;
using System.Windows.Forms;

namespace PriceHistory
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class View : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ListBox listBoxFunds;
		private System.Windows.Forms.DateTimePicker dateTimePickerStart;
		private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
		private System.Windows.Forms.Label labelStartDate;
		private System.Windows.Forms.Label labelEndDate;
		private System.Windows.Forms.Button buttonCalculate;
		private Controller _controller;

		public View()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_controller=new Controller();
			_controller.InitializeFundTable();

			listBoxFunds.DataSource=_controller.FundTable.Keys;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.listBoxFunds = new System.Windows.Forms.ListBox();
			this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
			this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
			this.buttonCalculate = new System.Windows.Forms.Button();
			this.labelStartDate = new System.Windows.Forms.Label();
			this.labelEndDate = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// listBoxFunds
			// 
			this.listBoxFunds.Location = new System.Drawing.Point(16, 24);
			this.listBoxFunds.Name = "listBoxFunds";
			this.listBoxFunds.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxFunds.Size = new System.Drawing.Size(200, 238);
			this.listBoxFunds.TabIndex = 0;
			// 
			// dateTimePickerStart
			// 
			this.dateTimePickerStart.Location = new System.Drawing.Point(232, 48);
			this.dateTimePickerStart.Name = "dateTimePickerStart";
			this.dateTimePickerStart.TabIndex = 1;
			// 
			// dateTimePickerEnd
			// 
			this.dateTimePickerEnd.Location = new System.Drawing.Point(232, 104);
			this.dateTimePickerEnd.Name = "dateTimePickerEnd";
			this.dateTimePickerEnd.TabIndex = 2;
			// 
			// buttonCalculate
			// 
			this.buttonCalculate.Location = new System.Drawing.Point(288, 184);
			this.buttonCalculate.Name = "buttonCalculate";
			this.buttonCalculate.TabIndex = 3;
			this.buttonCalculate.Text = "Calculate";
			this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
			// 
			// labelStartDate
			// 
			this.labelStartDate.Location = new System.Drawing.Point(232, 24);
			this.labelStartDate.Name = "labelStartDate";
			this.labelStartDate.TabIndex = 4;
			this.labelStartDate.Text = "Start Date:";
			// 
			// labelEndDate
			// 
			this.labelEndDate.Location = new System.Drawing.Point(232, 80);
			this.labelEndDate.Name = "labelEndDate";
			this.labelEndDate.TabIndex = 5;
			this.labelEndDate.Text = "End Date:";
			// 
			// View
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(456, 277);
			this.Controls.Add(this.labelEndDate);
			this.Controls.Add(this.labelStartDate);
			this.Controls.Add(this.buttonCalculate);
			this.Controls.Add(this.dateTimePickerEnd);
			this.Controls.Add(this.dateTimePickerStart);
			this.Controls.Add(this.listBoxFunds);
			this.Name = "View";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Correlation Calculator";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new View());
		}

		private void buttonCalculate_Click(object sender, System.EventArgs e)
		{
			//Update user input
			_controller.UpdateSelectedFunds(listBoxFunds.SelectedItems);
			_controller.StartDate=dateTimePickerStart.Value;
			_controller.EndDate=dateTimePickerEnd.Value;

			//Calculate
			_controller.Calculate();
		}
	}
}
