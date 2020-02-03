using System.Windows.Forms;

namespace ImageProcessing
{
    partial class MainForm
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
            this.buttonBrowseFolder = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.imageViewer1 = new ImageProcessing.ImageViewer();
            this.flowLayoutPanelMain = new ImageProcessing.ThumbnailFlowLayoutPanel();
            this.DocumentTypeTxt = new System.Windows.Forms.TextBox();
            this.StartDocNumTxt = new System.Windows.Forms.TextBox();
            this.EndDocNumTxt = new System.Windows.Forms.TextBox();
            this.TotNumPagesTxt = new System.Windows.Forms.TextBox();
            this.RunningDocNum = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.DocTypeLbl = new System.Windows.Forms.Label();
            this.StartNumLbl = new System.Windows.Forms.Label();
            this.EndDocNumLbl = new System.Windows.Forms.Label();
            this.TotNumPagesLbl = new System.Windows.Forms.Label();
            this.RunningDocNumlbl = new System.Windows.Forms.Label();
            //this.BOOKNOLBL = new System.Windows.Forms.Label();
            //this.BookNoTxt = new System.Windows.Forms.TextBox();
            this.SROCODElbl = new System.Windows.Forms.Label();
            this.SROCODETXT = new System.Windows.Forms.TextBox();
            this.YearLbl = new System.Windows.Forms.Label();
            this.YearTxt = new System.Windows.Forms.TextBox();
            this.trackBarSize = new System.Windows.Forms.TrackBar();
            this.Cropbtn = new System.Windows.Forms.Button();
            this.leftbtn = new System.Windows.Forms.Button();
            this.rightbtn = new System.Windows.Forms.Button();
            this.topbtn = new System.Windows.Forms.Button();
            this.bottombtn = new System.Windows.Forms.Button();
            this.Reorderbtn = new System.Windows.Forms.Button();
            this.SelectedTifbtn = new System.Windows.Forms.Button();
            this.OutsideCropbtn = new System.Windows.Forms.Button();
            this.Copybtn = new System.Windows.Forms.Button();
            this.Rescanbtn = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.ResetAll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.StartTimeTxt = new System.Windows.Forms.Label();
            this.EndTimelbl = new System.Windows.Forms.Label();
            this.EndTimeTxt = new System.Windows.Forms.Label();
            this.CropTxt = new System.Windows.Forms.TextBox();
            this.pagenationtxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.KeyPreview = true;
            this.KeyPress +=
                new KeyPressEventHandler(this.Form1_KeyDown);
            this.folderbrowse = new System.Windows.Forms.Button();
            this.panelMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonBrowseFolder
            // 
            this.buttonBrowseFolder.Location = new System.Drawing.Point(5, 2);
            this.buttonBrowseFolder.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBrowseFolder.Name = "buttonBrowseFolder";
            this.buttonBrowseFolder.Size = new System.Drawing.Size(100, 28);
            this.buttonBrowseFolder.TabIndex = 1;
            this.buttonBrowseFolder.Text = "Browse...";
            this.buttonBrowseFolder.UseVisualStyleBackColor = true;
            this.buttonBrowseFolder.Click += new System.EventHandler(this.buttonBrowseFolder_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(113, 1);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 28);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Controls.Add(this.tableLayoutPanel1);
            this.panelMain.Location = new System.Drawing.Point(16, 119);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1151, 580);
            this.panelMain.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.imageViewer1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanelMain, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1151, 580);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // imageViewer1
            // 
            this.imageViewer1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.imageViewer1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.imageViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer1.Image = null;
            this.imageViewer1.ImageLocation = null;
            this.imageViewer1.IsActive = false;
            this.imageViewer1.IsThumbnail = false;
            this.imageViewer1.Location = new System.Drawing.Point(579, 4);
            this.imageViewer1.Margin = new System.Windows.Forms.Padding(4);
            this.imageViewer1.Name = "imageViewer1";
            this.imageViewer1.Size = new System.Drawing.Size(568, 572);
            this.imageViewer1.TabIndex = 0;
            this.imageViewer1.Load += new System.EventHandler(this.imageViewer1_Load);
            this.imageViewer1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseDown);
            this.imageViewer1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseMove);
            this.imageViewer1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseUp);
            // 
            // flowLayoutPanelMain
            // 
            this.flowLayoutPanelMain.AutoScroll = true;
            this.flowLayoutPanelMain.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.flowLayoutPanelMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanelMain.CausesValidation = false;
            this.flowLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelMain.Location = new System.Drawing.Point(4, 4);
            this.flowLayoutPanelMain.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanelMain.Name = "flowLayoutPanelMain";
            this.flowLayoutPanelMain.Size = new System.Drawing.Size(567, 572);
            this.flowLayoutPanelMain.TabIndex = 0;
            this.flowLayoutPanelMain.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanelMain_Paint);
            // 
            // DocumentTypeTxt
            // 
            this.DocumentTypeTxt.Location = new System.Drawing.Point(291, 4);
            this.DocumentTypeTxt.Name = "DocumentTypeTxt";
            this.DocumentTypeTxt.Size = new System.Drawing.Size(31, 22);
            this.DocumentTypeTxt.TabIndex = 7;
            this.DocumentTypeTxt.Text = "R";
            this.DocumentTypeTxt.TextChanged += new System.EventHandler(this.DocumentTypeTxt_TextChanged);
            // 
            // StartDocNumTxt
            // 
            this.StartDocNumTxt.Location = new System.Drawing.Point(500, 41);
            this.StartDocNumTxt.Name = "StartDocNumTxt";
            this.StartDocNumTxt.Size = new System.Drawing.Size(300, 22);
            this.StartDocNumTxt.TabIndex = 8;
         //   this.StartDocNumTxt.TextChanged += new System.EventHandler(this.StartDocNumTxt_TextChanged);
            // 
            // EndDocNumTxt
            // 
            this.EndDocNumTxt.Location = new System.Drawing.Point(760, 5);
            this.EndDocNumTxt.Name = "EndDocNumTxt";
            this.EndDocNumTxt.Size = new System.Drawing.Size(47, 22);
            this.EndDocNumTxt.TabIndex = 9;
            this.EndDocNumTxt.TextChanged += new System.EventHandler(this.EndDocNumTxt_TextChanged);
            // 
            // TotNumPagesTxt
            // 
            this.TotNumPagesTxt.Location = new System.Drawing.Point(859, 4);
            this.TotNumPagesTxt.Name = "TotNumPagesTxt";
            this.TotNumPagesTxt.Size = new System.Drawing.Size(59, 22);
            this.TotNumPagesTxt.TabIndex = 10;
            this.TotNumPagesTxt.TextChanged += new System.EventHandler(this.TotNumPagesTxt_TextChanged);
            // 
            // RunningDocNum
            // 
            this.RunningDocNum.Location = new System.Drawing.Point(991, 7);
            this.RunningDocNum.Name = "RunningDocNum";
            this.RunningDocNum.Size = new System.Drawing.Size(57, 22);
            this.RunningDocNum.TabIndex = 11;
            this.RunningDocNum.TextChanged += new System.EventHandler(this.RunningDocNum_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 38);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Tiff";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(93, 38);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // DocTypeLbl
            // 
            this.DocTypeLbl.AutoSize = true;
            this.DocTypeLbl.Location = new System.Drawing.Point(220, 7);
            this.DocTypeLbl.Name = "DocTypeLbl";
            this.DocTypeLbl.Size = new System.Drawing.Size(65, 17);
            this.DocTypeLbl.TabIndex = 14;
            this.DocTypeLbl.Text = "DocType";
            this.DocTypeLbl.UseMnemonic = false;
            // 
            // StartNumLbl
            // 
            this.StartNumLbl.AutoSize = true;
            this.StartNumLbl.Location = new System.Drawing.Point(420, 48);
            this.StartNumLbl.Name = "StartNumLbl";
            this.StartNumLbl.Size = new System.Drawing.Size(38, 17);
            this.StartNumLbl.TabIndex = 15;
            this.StartNumLbl.Text = "ReOrder";
            this.StartNumLbl.UseMnemonic = false;
            this.StartNumLbl.Click += new System.EventHandler(this.StartNum_Click);
            // 
            // EndDocNumLbl
            // 
            this.EndDocNumLbl.AutoSize = true;
            this.EndDocNumLbl.Location = new System.Drawing.Point(721, 8);
            this.EndDocNumLbl.Name = "EndDocNumLbl";
            this.EndDocNumLbl.Size = new System.Drawing.Size(33, 17);
            this.EndDocNumLbl.TabIndex = 16;
            this.EndDocNumLbl.Text = "End";
            this.EndDocNumLbl.UseMnemonic = false;
            this.EndDocNumLbl.Click += new System.EventHandler(this.EndDocNumLbl_Click);
            // 
            // TotNumPagesLbl
            // 
            this.TotNumPagesLbl.AutoSize = true;
            this.TotNumPagesLbl.Location = new System.Drawing.Point(813, 8);
            this.TotNumPagesLbl.Name = "TotNumPagesLbl";
            this.TotNumPagesLbl.Size = new System.Drawing.Size(40, 17);
            this.TotNumPagesLbl.TabIndex = 17;
            this.TotNumPagesLbl.Text = "Total";
            this.TotNumPagesLbl.UseMnemonic = false;
            // 
            // RunningDocNumlbl
            // 
            this.RunningDocNumlbl.AutoSize = true;
            this.RunningDocNumlbl.Location = new System.Drawing.Point(924, 7);
            this.RunningDocNumlbl.Name = "RunningDocNumlbl";
            this.RunningDocNumlbl.Size = new System.Drawing.Size(61, 17);
            this.RunningDocNumlbl.TabIndex = 18;
            this.RunningDocNumlbl.Text = "Running";
            this.RunningDocNumlbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.RunningDocNumlbl.UseMnemonic = false;
            // 
            // BOOKNOLBL
            // 
            //this.BOOKNOLBL.AutoSize = true;
            //this.BOOKNOLBL.Location = new System.Drawing.Point(480, 8);
            //this.BOOKNOLBL.Name = "BOOKNOLBL";
            //this.BOOKNOLBL.Size = new System.Drawing.Size(69, 17);
            //this.BOOKNOLBL.TabIndex = 19;
            //this.BOOKNOLBL.Text = "BOOKNO";
            //// 
            //// BookNoTxt
            //// 
            //this.BookNoTxt.Location = new System.Drawing.Point(555, 4);
            //this.BookNoTxt.Name = "BookNoTxt";
            //this.BookNoTxt.Size = new System.Drawing.Size(63, 22);
            //this.BookNoTxt.TabIndex = 20;
            //this.BookNoTxt.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // SROCODElbl
            // 
            this.SROCODElbl.AutoSize = true;
            this.SROCODElbl.Location = new System.Drawing.Point(5, 2);
            this.SROCODElbl.Name = "SROCODElbl";
            this.SROCODElbl.Size = new System.Drawing.Size(77, 17);
            this.SROCODElbl.TabIndex = 21;
            this.SROCODElbl.Text = "File Name";
            // 
            // SROCODETXT
            // 
            this.SROCODETXT.Location = new System.Drawing.Point(113, 1);
            this.SROCODETXT.Name = "SROCODETXT";
            this.SROCODETXT.Size = new System.Drawing.Size(500, 22);
            this.SROCODETXT.TabIndex = 22;
            // 
            // YearLbl
            // 
            this.YearLbl.AutoSize = true;
            this.YearLbl.Location = new System.Drawing.Point(1054, 8);
            this.YearLbl.Name = "YearLbl";
            this.YearLbl.Size = new System.Drawing.Size(38, 17);
            this.YearLbl.TabIndex = 24;
            this.YearLbl.Text = "Year";
            this.YearLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.YearLbl.UseMnemonic = false;
            this.YearLbl.Click += new System.EventHandler(this.label2_Click);
            // 
            // YearTxt
            // 
            this.YearTxt.Location = new System.Drawing.Point(1098, 7);
            this.YearTxt.Name = "YearTxt";
            this.YearTxt.Size = new System.Drawing.Size(57, 22);
            this.YearTxt.TabIndex = 23;
            this.YearTxt.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // trackBarSize
            // 
            this.trackBarSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarSize.AutoSize = false;
            this.trackBarSize.LargeChange = 1;
            this.trackBarSize.Location = new System.Drawing.Point(1108, 83);
            this.trackBarSize.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarSize.Maximum = 2;
            this.trackBarSize.Name = "trackBarSize";
            this.trackBarSize.Size = new System.Drawing.Size(55, 28);
            this.trackBarSize.TabIndex = 5;
            this.trackBarSize.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarSize.Value = 2;
            this.trackBarSize.Scroll += new System.EventHandler(this.trackBarSize_Scroll);
            this.trackBarSize.ValueChanged += new System.EventHandler(this.trackBarSize_ValueChanged);
            // 
            // Cropbtn
            // 
            this.Cropbtn.Location = new System.Drawing.Point(336, 41);
            this.Cropbtn.Name = "Cropbtn";
            this.Cropbtn.Size = new System.Drawing.Size(75, 23);
            this.Cropbtn.TabIndex = 25;
            this.Cropbtn.Text = "Erase";
            this.Cropbtn.UseVisualStyleBackColor = true;
            this.Cropbtn.Click += new System.EventHandler(this.Cropbtn_Click);
            // 
            // Reorder
            // 
            this.Reorderbtn.Location = new System.Drawing.Point(810, 41);
            this.Reorderbtn.Name = "ReOrder";
            this.Reorderbtn.Size = new System.Drawing.Size(75, 23);
            this.Reorderbtn.TabIndex = 25;
            this.Reorderbtn.Text = "ReOrder";
            this.Reorderbtn.UseVisualStyleBackColor = true;
            this.Reorderbtn.Click += new System.EventHandler(this.Reorderbtn_Click);
            // 
            // SelectedTif
            // 
            this.SelectedTifbtn.Location = new System.Drawing.Point(885, 41);
            this.SelectedTifbtn.Name = "SelectedTif";
            this.SelectedTifbtn.Size = new System.Drawing.Size(95, 23);
            this.SelectedTifbtn.TabIndex = 25;
            this.SelectedTifbtn.Text = "SelectedTif";
            this.SelectedTifbtn.UseVisualStyleBackColor = true;
            this.SelectedTifbtn.Click += new System.EventHandler(this.SelectedTif_Click);
            // 
            // OutsideCropbtn
            // 
            this.OutsideCropbtn.Location = new System.Drawing.Point(985, 41);
            this.OutsideCropbtn.Name = "OutsideCropbtn";
            this.OutsideCropbtn.Size = new System.Drawing.Size(85, 23);
            this.OutsideCropbtn.TabIndex = 25;
            this.OutsideCropbtn.Text = "OuterCrop";
            this.OutsideCropbtn.UseVisualStyleBackColor = true;
            this.OutsideCropbtn.Click += new System.EventHandler(this.OutsideCropbtn_Click);
            // 
            // Copybtn
            // 
            this.Copybtn.Location = new System.Drawing.Point(1070, 41);
            this.Copybtn.Name = "Copybtn";
            this.Copybtn.Size = new System.Drawing.Size(60, 25);
            this.Copybtn.TabIndex = 25;
            this.Copybtn.Text = "Copy";
            this.Copybtn.UseVisualStyleBackColor = true;
            this.Copybtn.Click += new System.EventHandler(this.Copybtn_Click);
            // 
            // Leftbtn
            // 
            this.leftbtn.Location = new System.Drawing.Point(620, 1);
            this.leftbtn.Name = "Leftbtn";
            this.leftbtn.Size = new System.Drawing.Size(75, 28);
            this.leftbtn.TabIndex = 25;
            this.leftbtn.Text = "Left";
            this.leftbtn.UseVisualStyleBackColor = true;
            this.leftbtn.Click += new System.EventHandler(this.leftbtn_Click);
            // 
            // Copybtn
            // 
            this.rightbtn.Location = new System.Drawing.Point(695, 1);
            this.rightbtn.Name = "Rightbtn";
            this.rightbtn.Size = new System.Drawing.Size(75, 28);
            this.rightbtn.TabIndex = 25;
            this.rightbtn.Text = "Right";
            this.rightbtn.UseVisualStyleBackColor = true;
            this.rightbtn.Click += new System.EventHandler(this.rightbtn_Click);
            // 
            // Copybtn
            // 
            this.topbtn.Location = new System.Drawing.Point(770, 1);
            this.topbtn.Name = "Topbtn";
            this.topbtn.Size = new System.Drawing.Size(75, 28);
            this.topbtn.TabIndex = 25;
            this.topbtn.Text = "Top";
            this.topbtn.UseVisualStyleBackColor = true;
            this.topbtn.Click += new System.EventHandler(this.topbtn_Click);
            // 
            // Copybtn
            // 
            this.bottombtn.Location = new System.Drawing.Point(850, 1);
            this.bottombtn.Name = "Bottonbtn";
            this.bottombtn.Size = new System.Drawing.Size(75, 28);
            this.bottombtn.TabIndex = 25;
            this.bottombtn.Text = "Bottom";
            this.bottombtn.UseVisualStyleBackColor = true;
            this.bottombtn.Click += new System.EventHandler(this.bottombtn_Click);
            // 
            // RescanCropbtn
            // 
            this.Rescanbtn.Location = new System.Drawing.Point(1130, 41);
            this.Rescanbtn.Name = "Rescanbtn";
            this.Rescanbtn.Size = new System.Drawing.Size(70, 25);
            this.Rescanbtn.TabIndex = 25;
            this.Rescanbtn.Text = "Rescan";
            this.Rescanbtn.UseVisualStyleBackColor = true;
            this.Rescanbtn.Click += new System.EventHandler(this.Rescanbtn_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(454, 88);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(442, 28);
            this.progressBar1.TabIndex = 27;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // ResetAll
            // 
            this.ResetAll.Location = new System.Drawing.Point(255, 40);
            this.ResetAll.Name = "ResetAll";
            this.ResetAll.Size = new System.Drawing.Size(75, 23);
            this.ResetAll.TabIndex = 28;
            this.ResetAll.Text = "Refresh";
            this.ResetAll.UseVisualStyleBackColor = true;
            this.ResetAll.Click += new System.EventHandler(this.ResetAll_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 31;
            this.label2.Text = "StartTime:";
            // 
            // StartTimeTxt
            // 
            this.StartTimeTxt.AutoSize = true;
            this.StartTimeTxt.Location = new System.Drawing.Point(93, 95);
            this.StartTimeTxt.Name = "StartTimeTxt";
            this.StartTimeTxt.Size = new System.Drawing.Size(0, 17);
            this.StartTimeTxt.TabIndex = 32;
            // 
            // EndTimelbl
            // 
            this.EndTimelbl.AutoSize = true;
            this.EndTimelbl.Location = new System.Drawing.Point(213, 95);
            this.EndTimelbl.Name = "EndTimelbl";
            this.EndTimelbl.Size = new System.Drawing.Size(72, 17);
            this.EndTimelbl.TabIndex = 33;
            this.EndTimelbl.Text = "EndTime: ";
            // 
            // EndTimeTxt
            // 
            this.EndTimeTxt.AutoSize = true;
            this.EndTimeTxt.Location = new System.Drawing.Point(291, 98);
            this.EndTimeTxt.Name = "EndTimeTxt";
            this.EndTimeTxt.Size = new System.Drawing.Size(0, 17);
            this.EndTimeTxt.TabIndex = 34;
            // 
            // CropTxt
            // 
            this.CropTxt.Location = new System.Drawing.Point(336, 41);
            this.CropTxt.Name = "CropTxt";
            this.CropTxt.Size = new System.Drawing.Size(75, 22);
            this.CropTxt.TabIndex = 35;
            // 
            // pagenationtxt
            // 
            this.pagenationtxt.Location = new System.Drawing.Point(174, 40);
            this.pagenationtxt.Name = "pagenationtxt";
            this.pagenationtxt.Size = new System.Drawing.Size(75, 22);
            this.pagenationtxt.TabIndex = 36;
            this.pagenationtxt.Text = "10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(939, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 37;
            this.label1.Text = "Not Started";
            // 
            // folderbrowse
            // 
            this.folderbrowse.Location = new System.Drawing.Point(499, 40);
            this.folderbrowse.Margin = new System.Windows.Forms.Padding(4);
            this.folderbrowse.Name = "folderbrowse";
            this.folderbrowse.Size = new System.Drawing.Size(100, 28);
            this.folderbrowse.TabIndex = 38;
            this.folderbrowse.Text = "Folder";
            this.folderbrowse.UseVisualStyleBackColor = true;
            this.folderbrowse.Click += new System.EventHandler(this.folderbrowse_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1200, 700);
           // this.Controls.Add(this.folderbrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pagenationtxt);
          ///  this.Controls.Add(this.CropTxt);
            this.Controls.Add(this.EndTimeTxt);
            this.Controls.Add(this.EndTimelbl);
            this.Controls.Add(this.StartTimeTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ResetAll);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Cropbtn);
            this.Controls.Add(this.Reorderbtn);
            this.Controls.Add(this.SelectedTifbtn);
            this.Controls.Add(this.OutsideCropbtn);
            this.Controls.Add(this.Copybtn);
            this.Controls.Add(this.Rescanbtn);
            this.Controls.Add(this.leftbtn);
            this.Controls.Add(this.rightbtn);
            this.Controls.Add(this.topbtn);
            this.Controls.Add(this.bottombtn);
            //this.Controls.Add(this.YearLbl);
            //this.Controls.Add(this.YearTxt);
            this.Controls.Add(this.SROCODETXT);
            this.Controls.Add(this.SROCODElbl);
            //this.Controls.Add(this.BookNoTxt);
            //this.Controls.Add(this.BOOKNOLBL);
           // this.Controls.Add(this.RunningDocNumlbl);
          //  this.Controls.Add(this.TotNumPagesLbl);
            //this.Controls.Add(this.EndDocNumLbl);
            this.Controls.Add(this.StartNumLbl);
          //  this.Controls.Add(this.DocTypeLbl);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            //this.Controls.Add(this.RunningDocNum);
            //this.Controls.Add(this.TotNumPagesTxt);
            //this.Controls.Add(this.EndDocNumTxt);
            this.Controls.Add(this.StartDocNumTxt);
           // this.Controls.Add(this.DocumentTypeTxt);
            this.Controls.Add(this.trackBarSize);
            this.Controls.Add(this.panelMain);
            //this.Controls.Add(this.buttonCancel);
            //this.Controls.Add(this.buttonBrowseFolder);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Rejection Tool 6.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ThumbnailFlowLayoutPanel flowLayoutPanelMain;
        private System.Windows.Forms.Button buttonBrowseFolder;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.TrackBar trackBarSize;
        private System.Windows.Forms.TextBox DocumentTypeTxt;
        private System.Windows.Forms.TextBox StartDocNumTxt;
        private System.Windows.Forms.TextBox EndDocNumTxt;
        private System.Windows.Forms.TextBox TotNumPagesTxt;
        private System.Windows.Forms.TextBox RunningDocNum;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label DocTypeLbl;
        private System.Windows.Forms.Label StartNumLbl;
        private System.Windows.Forms.Label EndDocNumLbl;
        private System.Windows.Forms.Label TotNumPagesLbl;
        private System.Windows.Forms.Label RunningDocNumlbl;
        //private System.Windows.Forms.Label BOOKNOLBL;
        //private System.Windows.Forms.TextBox BookNoTxt;
        private System.Windows.Forms.Label SROCODElbl;
        private System.Windows.Forms.TextBox SROCODETXT;
        private System.Windows.Forms.Label YearLbl;
        private System.Windows.Forms.TextBox YearTxt;
        private System.Windows.Forms.Button Cropbtn;
        private System.Windows.Forms.Button Reorderbtn;
        private System.Windows.Forms.Button SelectedTifbtn;
        private System.Windows.Forms.Button OutsideCropbtn;
        private System.Windows.Forms.Button Copybtn;
        private System.Windows.Forms.Button Rescanbtn;
        private System.Windows.Forms.Button leftbtn;
        private System.Windows.Forms.Button rightbtn;
        private System.Windows.Forms.Button topbtn;
        private System.Windows.Forms.Button bottombtn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button ResetAll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label StartTimeTxt;
        private System.Windows.Forms.Label EndTimelbl;
        private System.Windows.Forms.Label EndTimeTxt;
        private System.Windows.Forms.TextBox CropTxt;
        private System.Windows.Forms.TextBox pagenationtxt;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ImageViewer imageViewer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button folderbrowse;
    }
}

