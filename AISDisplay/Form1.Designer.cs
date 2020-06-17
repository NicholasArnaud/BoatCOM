using System;
using System.Drawing;
using System.Windows.Forms;

namespace AISDisplay
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
        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {

            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "CloseReason", e.CloseReason);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Cancel", e.Cancel);
            messageBoxCS.AppendLine();
            MessageBox.Show(messageBoxCS.ToString(), "FormClosing Event");
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.AISDataTable = new System.Windows.Forms.DataGridView();
            this._Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._MMSI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Heading = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._BRG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Range = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._COG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._SOG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Lat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Lon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._UTCDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yourDataTable = new System.Windows.Forms.DataGridView();
            this.YourVesselName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YourVesselMMSI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YourVesselHeading = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YourVesselCOG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YourVesselSOG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YourVesselLat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YourVesselLon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.AISDataTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yourDataTable)).BeginInit();
            this.SuspendLayout();
            // 
            // AISDataTable
            // 
            this.AISDataTable.AllowUserToAddRows = false;
            this.AISDataTable.AllowUserToDeleteRows = false;
            this.AISDataTable.AllowUserToResizeColumns = false;
            this.AISDataTable.AllowUserToResizeRows = false;
            this.AISDataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AISDataTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.AISDataTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.AISDataTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.AISDataTable.ColumnHeadersHeight = 46;
            this.AISDataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.AISDataTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._Name,
            this._MMSI,
            this._Heading,
            this._BRG,
            this._Range,
            this._COG,
            this._SOG,
            this._Lat,
            this._Lon,
            this._UTCDateTime});
            this.AISDataTable.Enabled = false;
            this.AISDataTable.Location = new System.Drawing.Point(0, 120);
            this.AISDataTable.Margin = new System.Windows.Forms.Padding(4);
            this.AISDataTable.MultiSelect = false;
            this.AISDataTable.Name = "AISDataTable";
            this.AISDataTable.ReadOnly = true;
            this.AISDataTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.AISDataTable.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.AISDataTable.RowTemplate.Height = 25;
            this.AISDataTable.RowTemplate.ReadOnly = true;
            this.AISDataTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AISDataTable.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.AISDataTable.Size = new System.Drawing.Size(2588, 1283);
            this.AISDataTable.TabIndex = 0;
            this.AISDataTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // _Name
            // 
            this._Name.HeaderText = "Name";
            this._Name.MinimumWidth = 75;
            this._Name.Name = "_Name";
            this._Name.ReadOnly = true;
            this._Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _MMSI
            // 
            this._MMSI.HeaderText = "MMSI";
            this._MMSI.MinimumWidth = 75;
            this._MMSI.Name = "_MMSI";
            this._MMSI.ReadOnly = true;
            this._MMSI.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _Heading
            // 
            this._Heading.HeaderText = "Heading";
            this._Heading.MinimumWidth = 75;
            this._Heading.Name = "_Heading";
            this._Heading.ReadOnly = true;
            this._Heading.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _BRG
            // 
            this._BRG.HeaderText = "BRG";
            this._BRG.MinimumWidth = 75;
            this._BRG.Name = "_BRG";
            this._BRG.ReadOnly = true;
            this._BRG.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _Range
            // 
            this._Range.HeaderText = "Range";
            this._Range.MinimumWidth = 75;
            this._Range.Name = "_Range";
            this._Range.ReadOnly = true;
            this._Range.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _COG
            // 
            this._COG.HeaderText = "COG";
            this._COG.MinimumWidth = 75;
            this._COG.Name = "_COG";
            this._COG.ReadOnly = true;
            this._COG.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _SOG
            // 
            this._SOG.HeaderText = "SOG";
            this._SOG.MinimumWidth = 75;
            this._SOG.Name = "_SOG";
            this._SOG.ReadOnly = true;
            this._SOG.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _Lat
            // 
            this._Lat.HeaderText = "Lat";
            this._Lat.MinimumWidth = 75;
            this._Lat.Name = "_Lat";
            this._Lat.ReadOnly = true;
            this._Lat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _Lon
            // 
            this._Lon.HeaderText = "Lon";
            this._Lon.MinimumWidth = 75;
            this._Lon.Name = "_Lon";
            this._Lon.ReadOnly = true;
            this._Lon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _UTCDateTime
            // 
            this._UTCDateTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this._UTCDateTime.HeaderText = "UTCDateTime";
            this._UTCDateTime.MinimumWidth = 75;
            this._UTCDateTime.Name = "_UTCDateTime";
            this._UTCDateTime.ReadOnly = true;
            this._UTCDateTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this._UTCDateTime.Width = 109;
            // 
            // yourDataTable
            // 
            this.yourDataTable.AllowUserToAddRows = false;
            this.yourDataTable.AllowUserToDeleteRows = false;
            this.yourDataTable.AllowUserToResizeColumns = false;
            this.yourDataTable.AllowUserToResizeRows = false;
            this.yourDataTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.yourDataTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.yourDataTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.yourDataTable.ColumnHeadersHeight = 60;
            this.yourDataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.yourDataTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.YourVesselName,
            this.YourVesselMMSI,
            this.YourVesselHeading,
            this.YourVesselCOG,
            this.YourVesselSOG,
            this.YourVesselLat,
            this.YourVesselLon});
            this.yourDataTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.yourDataTable.Enabled = false;
            this.yourDataTable.Location = new System.Drawing.Point(0, 0);
            this.yourDataTable.Margin = new System.Windows.Forms.Padding(4);
            this.yourDataTable.MultiSelect = false;
            this.yourDataTable.Name = "yourDataTable";
            this.yourDataTable.ReadOnly = true;
            this.yourDataTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yourDataTable.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.yourDataTable.RowTemplate.Height = 25;
            this.yourDataTable.RowTemplate.ReadOnly = true;
            this.yourDataTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.yourDataTable.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.yourDataTable.Size = new System.Drawing.Size(2588, 120);
            this.yourDataTable.TabIndex = 1;
            this.yourDataTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick_1);
            // 
            // YourVesselName
            // 
            this.YourVesselName.HeaderText = "Name";
            this.YourVesselName.MinimumWidth = 75;
            this.YourVesselName.Name = "YourVesselName";
            this.YourVesselName.ReadOnly = true;
            this.YourVesselName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // YourVesselMMSI
            // 
            this.YourVesselMMSI.HeaderText = "MMSI";
            this.YourVesselMMSI.MinimumWidth = 75;
            this.YourVesselMMSI.Name = "YourVesselMMSI";
            this.YourVesselMMSI.ReadOnly = true;
            this.YourVesselMMSI.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // YourVesselHeading
            // 
            this.YourVesselHeading.HeaderText = "Heading";
            this.YourVesselHeading.MinimumWidth = 75;
            this.YourVesselHeading.Name = "YourVesselHeading";
            this.YourVesselHeading.ReadOnly = true;
            this.YourVesselHeading.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // YourVesselCOG
            // 
            this.YourVesselCOG.HeaderText = "COG";
            this.YourVesselCOG.MinimumWidth = 75;
            this.YourVesselCOG.Name = "YourVesselCOG";
            this.YourVesselCOG.ReadOnly = true;
            this.YourVesselCOG.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // YourVesselSOG
            // 
            this.YourVesselSOG.HeaderText = "SOG";
            this.YourVesselSOG.MinimumWidth = 75;
            this.YourVesselSOG.Name = "YourVesselSOG";
            this.YourVesselSOG.ReadOnly = true;
            this.YourVesselSOG.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // YourVesselLat
            // 
            this.YourVesselLat.HeaderText = "Lat";
            this.YourVesselLat.MinimumWidth = 75;
            this.YourVesselLat.Name = "YourVesselLat";
            this.YourVesselLat.ReadOnly = true;
            this.YourVesselLat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // YourVesselLon
            // 
            this.YourVesselLon.HeaderText = "Lon";
            this.YourVesselLon.MinimumWidth = 75;
            this.YourVesselLon.Name = "YourVesselLon";
            this.YourVesselLon.ReadOnly = true;
            this.YourVesselLon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(2588, 1174);
            this.ControlBox = false;
            this.Controls.Add(this.yourDataTable);
            this.Controls.Add(this.AISDataTable);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "Form1";
            this.Text = "AIS Display";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AISDataTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yourDataTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView AISDataTable;
        private DataGridViewTextBoxColumn _Name;
        private DataGridViewTextBoxColumn _MMSI;
        private DataGridViewTextBoxColumn _Heading;
        private DataGridViewTextBoxColumn _BRG;
        private DataGridViewTextBoxColumn _Range;
        private DataGridViewTextBoxColumn _COG;
        private DataGridViewTextBoxColumn _SOG;
        private DataGridViewTextBoxColumn _Lat;
        private DataGridViewTextBoxColumn _Lon;
        private DataGridViewTextBoxColumn _UTCDateTime;
        private DataGridView yourDataTable;
        private DataGridViewTextBoxColumn YourVesselName;
        private DataGridViewTextBoxColumn YourVesselMMSI;
        private DataGridViewTextBoxColumn YourVesselHeading;
        private DataGridViewTextBoxColumn YourVesselCOG;
        private DataGridViewTextBoxColumn YourVesselSOG;
        private DataGridViewTextBoxColumn YourVesselLat;
        private DataGridViewTextBoxColumn YourVesselLon;
        private Timer timer1;
    }
}

