namespace BitMexSampleBot
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnBuy = new System.Windows.Forms.Button();
            this.btnSell = new System.Windows.Forms.Button();
            this.nudQty = new System.Windows.Forms.NumericUpDown();
            this.chkCancelWhileOrdering = new System.Windows.Forms.CheckBox();
            this.btnCancelOpenOrders = new System.Windows.Forms.Button();
            this.ddlOrderType = new System.Windows.Forms.ComboBox();
            this.ddlNetwork = new System.Windows.Forms.ComboBox();
            this.ddlSymbol = new System.Windows.Forms.ComboBox();
            this.dgvCandles = new System.Windows.Forms.DataGridView();
            this.ddlCandleTimes = new System.Windows.Forms.ComboBox();
            this.gbCandles = new System.Windows.Forms.GroupBox();
            this.btnShowHideCols = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.nudCandleRefreshTime = new System.Windows.Forms.NumericUpDown();
            this.lblMA2 = new System.Windows.Forms.Label();
            this.nudMA2 = new System.Windows.Forms.NumericUpDown();
            this.lblMA1 = new System.Windows.Forms.Label();
            this.nudMA1 = new System.Windows.Forms.NumericUpDown();
            this.chkUpdateCandles = new System.Windows.Forms.CheckBox();
            this.tmrCandleUpdater = new System.Windows.Forms.Timer(this.components);
            this.rdoBuy = new System.Windows.Forms.RadioButton();
            this.rdoSell = new System.Windows.Forms.RadioButton();
            this.rdoSwitch = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAutoMatedTradingStop = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.nudRetryAttempts = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblAutoUnrealizedROEPercent = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nudAutoMarketTakeProfitPercent = new System.Windows.Forms.NumericUpDown();
            this.chkAutoMarketTakeProfits = new System.Windows.Forms.CheckBox();
            this.ddlAutoOrderType = new System.Windows.Forms.ComboBox();
            this.nudAutoQuantity = new System.Windows.Forms.NumericUpDown();
            this.btnAutomatedTrading = new System.Windows.Forms.Button();
            this.tmrAutoTradeExecution = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stsAPIValid = new System.Windows.Forms.ToolStripStatusLabel();
            this.stsAccountBalance = new System.Windows.Forms.ToolStripStatusLabel();
            this.stsOTProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.btnAccountBalance = new System.Windows.Forms.Button();
            this.nudStopPercent = new System.Windows.Forms.NumericUpDown();
            this.btnManualSetStop = new System.Windows.Forms.Button();
            this.txtAPIKey = new System.Windows.Forms.TextBox();
            this.txtAPISecret = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBuyOverTimeOrder = new System.Windows.Forms.Button();
            this.btnSellOverTimeOrder = new System.Windows.Forms.Button();
            this.tmrTradeOverTime = new System.Windows.Forms.Timer(this.components);
            this.nudOverTimeContracts = new System.Windows.Forms.NumericUpDown();
            this.nudOverTimeInterval = new System.Windows.Forms.NumericUpDown();
            this.nudOverTimeIntervalCount = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblOverTimeSummary = new System.Windows.Forms.Label();
            this.btnOverTimeStop = new System.Windows.Forms.Button();
            this.btnBulkTest = new System.Windows.Forms.Button();
            this.btnBulkShift = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.gbShowHideCols = new System.Windows.Forms.GroupBox();
            this.dgShowHideCols = new System.Windows.Forms.DataGridView();
            this.label15 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCandles)).BeginInit();
            this.gbCandles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCandleRefreshTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMA2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMA1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRetryAttempts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAutoMarketTakeProfitPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAutoQuantity)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStopPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOverTimeContracts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOverTimeInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOverTimeIntervalCount)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            this.gbShowHideCols.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgShowHideCols)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBuy
            // 
            this.btnBuy.BackColor = System.Drawing.Color.LimeGreen;
            this.btnBuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuy.FlatAppearance.BorderSize = 0;
            this.btnBuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuy.ForeColor = System.Drawing.Color.White;
            this.btnBuy.Location = new System.Drawing.Point(128, 109);
            this.btnBuy.Name = "btnBuy";
            this.btnBuy.Size = new System.Drawing.Size(75, 23);
            this.btnBuy.TabIndex = 0;
            this.btnBuy.Text = "Buy";
            this.btnBuy.UseVisualStyleBackColor = false;
            this.btnBuy.Click += new System.EventHandler(this.btnBuy_Click);
            // 
            // btnSell
            // 
            this.btnSell.BackColor = System.Drawing.Color.Orange;
            this.btnSell.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSell.FlatAppearance.BorderSize = 0;
            this.btnSell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSell.ForeColor = System.Drawing.Color.White;
            this.btnSell.Location = new System.Drawing.Point(209, 109);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(75, 23);
            this.btnSell.TabIndex = 1;
            this.btnSell.Text = "Sell";
            this.btnSell.UseVisualStyleBackColor = false;
            this.btnSell.Click += new System.EventHandler(this.btnSell_Click);
            // 
            // nudQty
            // 
            this.nudQty.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudQty.Location = new System.Drawing.Point(129, 83);
            this.nudQty.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudQty.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudQty.Name = "nudQty";
            this.nudQty.Size = new System.Drawing.Size(67, 20);
            this.nudQty.TabIndex = 2;
            this.nudQty.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // chkCancelWhileOrdering
            // 
            this.chkCancelWhileOrdering.AutoSize = true;
            this.chkCancelWhileOrdering.Checked = true;
            this.chkCancelWhileOrdering.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCancelWhileOrdering.Location = new System.Drawing.Point(76, 147);
            this.chkCancelWhileOrdering.Name = "chkCancelWhileOrdering";
            this.chkCancelWhileOrdering.Size = new System.Drawing.Size(132, 17);
            this.chkCancelWhileOrdering.TabIndex = 3;
            this.chkCancelWhileOrdering.Text = "Cancel While Ordering";
            this.chkCancelWhileOrdering.UseVisualStyleBackColor = true;
            // 
            // btnCancelOpenOrders
            // 
            this.btnCancelOpenOrders.BackColor = System.Drawing.Color.Tomato;
            this.btnCancelOpenOrders.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelOpenOrders.FlatAppearance.BorderSize = 0;
            this.btnCancelOpenOrders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelOpenOrders.ForeColor = System.Drawing.Color.White;
            this.btnCancelOpenOrders.Location = new System.Drawing.Point(209, 141);
            this.btnCancelOpenOrders.Name = "btnCancelOpenOrders";
            this.btnCancelOpenOrders.Size = new System.Drawing.Size(75, 23);
            this.btnCancelOpenOrders.TabIndex = 4;
            this.btnCancelOpenOrders.Text = "Cancel";
            this.btnCancelOpenOrders.UseVisualStyleBackColor = false;
            this.btnCancelOpenOrders.Click += new System.EventHandler(this.btnCancelOpenOrders_Click);
            // 
            // ddlOrderType
            // 
            this.ddlOrderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlOrderType.FormattingEnabled = true;
            this.ddlOrderType.Items.AddRange(new object[] {
            "Market",
            "Limit Post Only"});
            this.ddlOrderType.Location = new System.Drawing.Point(129, 5);
            this.ddlOrderType.Name = "ddlOrderType";
            this.ddlOrderType.Size = new System.Drawing.Size(155, 21);
            this.ddlOrderType.TabIndex = 5;
            // 
            // ddlNetwork
            // 
            this.ddlNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlNetwork.FormattingEnabled = true;
            this.ddlNetwork.Items.AddRange(new object[] {
            "TestNet",
            "RealNet"});
            this.ddlNetwork.Location = new System.Drawing.Point(129, 30);
            this.ddlNetwork.Name = "ddlNetwork";
            this.ddlNetwork.Size = new System.Drawing.Size(155, 21);
            this.ddlNetwork.TabIndex = 6;
            this.ddlNetwork.SelectedIndexChanged += new System.EventHandler(this.ddlNetwork_SelectedIndexChanged);
            // 
            // ddlSymbol
            // 
            this.ddlSymbol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSymbol.FormattingEnabled = true;
            this.ddlSymbol.Location = new System.Drawing.Point(129, 55);
            this.ddlSymbol.Name = "ddlSymbol";
            this.ddlSymbol.Size = new System.Drawing.Size(155, 21);
            this.ddlSymbol.TabIndex = 7;
            this.ddlSymbol.SelectedIndexChanged += new System.EventHandler(this.ddlSymbol_SelectedIndexChanged);
            // 
            // dgvCandles
            // 
            this.dgvCandles.AllowUserToAddRows = false;
            this.dgvCandles.AllowUserToDeleteRows = false;
            this.dgvCandles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCandles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCandles.Location = new System.Drawing.Point(6, 46);
            this.dgvCandles.Name = "dgvCandles";
            this.dgvCandles.ReadOnly = true;
            this.dgvCandles.RowHeadersVisible = false;
            this.dgvCandles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvCandles.Size = new System.Drawing.Size(1227, 417);
            this.dgvCandles.TabIndex = 8;
            // 
            // ddlCandleTimes
            // 
            this.ddlCandleTimes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCandleTimes.FormattingEnabled = true;
            this.ddlCandleTimes.Items.AddRange(new object[] {
            "1m",
            "5m",
            "1h",
            "1d"});
            this.ddlCandleTimes.Location = new System.Drawing.Point(6, 19);
            this.ddlCandleTimes.Name = "ddlCandleTimes";
            this.ddlCandleTimes.Size = new System.Drawing.Size(69, 21);
            this.ddlCandleTimes.TabIndex = 9;
            this.ddlCandleTimes.SelectedIndexChanged += new System.EventHandler(this.ddlCandleTimes_SelectedIndexChanged);
            // 
            // gbCandles
            // 
            this.gbCandles.Controls.Add(this.btnShowHideCols);
            this.gbCandles.Controls.Add(this.label13);
            this.gbCandles.Controls.Add(this.nudCandleRefreshTime);
            this.gbCandles.Controls.Add(this.lblMA2);
            this.gbCandles.Controls.Add(this.nudMA2);
            this.gbCandles.Controls.Add(this.lblMA1);
            this.gbCandles.Controls.Add(this.nudMA1);
            this.gbCandles.Controls.Add(this.chkUpdateCandles);
            this.gbCandles.Controls.Add(this.dgvCandles);
            this.gbCandles.Controls.Add(this.ddlCandleTimes);
            this.gbCandles.Location = new System.Drawing.Point(7, 6);
            this.gbCandles.Name = "gbCandles";
            this.gbCandles.Size = new System.Drawing.Size(1239, 469);
            this.gbCandles.TabIndex = 10;
            this.gbCandles.TabStop = false;
            this.gbCandles.Text = "Candles";
            // 
            // btnShowHideCols
            // 
            this.btnShowHideCols.BackColor = System.Drawing.Color.LimeGreen;
            this.btnShowHideCols.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShowHideCols.FlatAppearance.BorderSize = 0;
            this.btnShowHideCols.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowHideCols.ForeColor = System.Drawing.Color.White;
            this.btnShowHideCols.Location = new System.Drawing.Point(715, 8);
            this.btnShowHideCols.Name = "btnShowHideCols";
            this.btnShowHideCols.Size = new System.Drawing.Size(134, 32);
            this.btnShowHideCols.TabIndex = 40;
            this.btnShowHideCols.Text = "Show/Hide Columns";
            this.btnShowHideCols.UseVisualStyleBackColor = false;
            this.btnShowHideCols.Click += new System.EventHandler(this.btnShowHideCols_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(255, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 13);
            this.label13.TabIndex = 39;
            this.label13.Text = "Seconds";
            // 
            // nudCandleRefreshTime
            // 
            this.nudCandleRefreshTime.Location = new System.Drawing.Point(205, 21);
            this.nudCandleRefreshTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCandleRefreshTime.Name = "nudCandleRefreshTime";
            this.nudCandleRefreshTime.Size = new System.Drawing.Size(42, 20);
            this.nudCandleRefreshTime.TabIndex = 17;
            this.nudCandleRefreshTime.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nudCandleRefreshTime.ValueChanged += new System.EventHandler(this.nudCandleRefreshTime_ValueChanged);
            // 
            // lblMA2
            // 
            this.lblMA2.AutoSize = true;
            this.lblMA2.Location = new System.Drawing.Point(475, 23);
            this.lblMA2.Name = "lblMA2";
            this.lblMA2.Size = new System.Drawing.Size(29, 13);
            this.lblMA2.TabIndex = 16;
            this.lblMA2.Text = "MA2";
            // 
            // nudMA2
            // 
            this.nudMA2.Location = new System.Drawing.Point(426, 20);
            this.nudMA2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMA2.Name = "nudMA2";
            this.nudMA2.Size = new System.Drawing.Size(42, 20);
            this.nudMA2.TabIndex = 15;
            this.nudMA2.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblMA1
            // 
            this.lblMA1.AutoSize = true;
            this.lblMA1.Location = new System.Drawing.Point(391, 23);
            this.lblMA1.Name = "lblMA1";
            this.lblMA1.Size = new System.Drawing.Size(29, 13);
            this.lblMA1.TabIndex = 14;
            this.lblMA1.Text = "MA1";
            // 
            // nudMA1
            // 
            this.nudMA1.Location = new System.Drawing.Point(342, 20);
            this.nudMA1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMA1.Name = "nudMA1";
            this.nudMA1.Size = new System.Drawing.Size(42, 20);
            this.nudMA1.TabIndex = 13;
            this.nudMA1.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // chkUpdateCandles
            // 
            this.chkUpdateCandles.AutoSize = true;
            this.chkUpdateCandles.Location = new System.Drawing.Point(105, 21);
            this.chkUpdateCandles.Name = "chkUpdateCandles";
            this.chkUpdateCandles.Size = new System.Drawing.Size(94, 17);
            this.chkUpdateCandles.TabIndex = 12;
            this.chkUpdateCandles.Text = "Update Every ";
            this.chkUpdateCandles.UseVisualStyleBackColor = true;
            this.chkUpdateCandles.CheckedChanged += new System.EventHandler(this.chkUpdateCandles_CheckedChanged);
            // 
            // tmrCandleUpdater
            // 
            this.tmrCandleUpdater.Interval = 10000;
            this.tmrCandleUpdater.Tick += new System.EventHandler(this.tmrCandleUpdater_Tick);
            // 
            // rdoBuy
            // 
            this.rdoBuy.AutoSize = true;
            this.rdoBuy.Checked = true;
            this.rdoBuy.Location = new System.Drawing.Point(24, 15);
            this.rdoBuy.Name = "rdoBuy";
            this.rdoBuy.Size = new System.Drawing.Size(43, 17);
            this.rdoBuy.TabIndex = 11;
            this.rdoBuy.TabStop = true;
            this.rdoBuy.Text = "Buy";
            this.rdoBuy.UseVisualStyleBackColor = true;
            this.rdoBuy.CheckedChanged += new System.EventHandler(this.rdoBuy_CheckedChanged);
            // 
            // rdoSell
            // 
            this.rdoSell.AutoSize = true;
            this.rdoSell.Location = new System.Drawing.Point(24, 39);
            this.rdoSell.Name = "rdoSell";
            this.rdoSell.Size = new System.Drawing.Size(42, 17);
            this.rdoSell.TabIndex = 12;
            this.rdoSell.Text = "Sell";
            this.rdoSell.UseVisualStyleBackColor = true;
            // 
            // rdoSwitch
            // 
            this.rdoSwitch.AutoSize = true;
            this.rdoSwitch.Location = new System.Drawing.Point(24, 67);
            this.rdoSwitch.Name = "rdoSwitch";
            this.rdoSwitch.Size = new System.Drawing.Size(57, 17);
            this.rdoSwitch.TabIndex = 13;
            this.rdoSwitch.Text = "Switch";
            this.rdoSwitch.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAutoMatedTradingStop);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.nudRetryAttempts);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.lblAutoUnrealizedROEPercent);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.nudAutoMarketTakeProfitPercent);
            this.groupBox1.Controls.Add(this.chkAutoMarketTakeProfits);
            this.groupBox1.Controls.Add(this.ddlAutoOrderType);
            this.groupBox1.Controls.Add(this.nudAutoQuantity);
            this.groupBox1.Controls.Add(this.btnAutomatedTrading);
            this.groupBox1.Controls.Add(this.rdoSell);
            this.groupBox1.Controls.Add(this.rdoSwitch);
            this.groupBox1.Controls.Add(this.rdoBuy);
            this.groupBox1.Location = new System.Drawing.Point(897, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(361, 184);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Automated Trading";
            // 
            // btnAutoMatedTradingStop
            // 
            this.btnAutoMatedTradingStop.BackColor = System.Drawing.Color.Tomato;
            this.btnAutoMatedTradingStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAutoMatedTradingStop.FlatAppearance.BorderSize = 0;
            this.btnAutoMatedTradingStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoMatedTradingStop.ForeColor = System.Drawing.Color.White;
            this.btnAutoMatedTradingStop.Location = new System.Drawing.Point(232, 135);
            this.btnAutoMatedTradingStop.Name = "btnAutoMatedTradingStop";
            this.btnAutoMatedTradingStop.Size = new System.Drawing.Size(119, 43);
            this.btnAutoMatedTradingStop.TabIndex = 42;
            this.btnAutoMatedTradingStop.Text = "Stop";
            this.btnAutoMatedTradingStop.UseVisualStyleBackColor = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(150, 76);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 13);
            this.label14.TabIndex = 41;
            this.label14.Text = "Retry Attempts";
            this.label14.Visible = false;
            // 
            // nudRetryAttempts
            // 
            this.nudRetryAttempts.Location = new System.Drawing.Point(240, 70);
            this.nudRetryAttempts.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRetryAttempts.Name = "nudRetryAttempts";
            this.nudRetryAttempts.Size = new System.Drawing.Size(42, 20);
            this.nudRetryAttempts.TabIndex = 40;
            this.nudRetryAttempts.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudRetryAttempts.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(141, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 13);
            this.label12.TabIndex = 39;
            this.label12.Text = "Auto Order Type";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(155, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 13);
            this.label11.TabIndex = 38;
            this.label11.Text = "Auto Quantity";
            // 
            // lblAutoUnrealizedROEPercent
            // 
            this.lblAutoUnrealizedROEPercent.AutoSize = true;
            this.lblAutoUnrealizedROEPercent.Location = new System.Drawing.Point(237, 122);
            this.lblAutoUnrealizedROEPercent.Name = "lblAutoUnrealizedROEPercent";
            this.lblAutoUnrealizedROEPercent.Size = new System.Drawing.Size(0, 13);
            this.lblAutoUnrealizedROEPercent.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(233, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Unrealized Mark ROE %";
            // 
            // nudAutoMarketTakeProfitPercent
            // 
            this.nudAutoMarketTakeProfitPercent.DecimalPlaces = 2;
            this.nudAutoMarketTakeProfitPercent.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudAutoMarketTakeProfitPercent.Location = new System.Drawing.Point(168, 102);
            this.nudAutoMarketTakeProfitPercent.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudAutoMarketTakeProfitPercent.Name = "nudAutoMarketTakeProfitPercent";
            this.nudAutoMarketTakeProfitPercent.Size = new System.Drawing.Size(60, 20);
            this.nudAutoMarketTakeProfitPercent.TabIndex = 19;
            this.nudAutoMarketTakeProfitPercent.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // chkAutoMarketTakeProfits
            // 
            this.chkAutoMarketTakeProfits.AutoSize = true;
            this.chkAutoMarketTakeProfits.Checked = true;
            this.chkAutoMarketTakeProfits.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoMarketTakeProfits.Location = new System.Drawing.Point(26, 104);
            this.chkAutoMarketTakeProfits.Name = "chkAutoMarketTakeProfits";
            this.chkAutoMarketTakeProfits.Size = new System.Drawing.Size(126, 17);
            this.chkAutoMarketTakeProfits.TabIndex = 16;
            this.chkAutoMarketTakeProfits.Text = "Market take profits at";
            this.chkAutoMarketTakeProfits.UseVisualStyleBackColor = true;
            // 
            // ddlAutoOrderType
            // 
            this.ddlAutoOrderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlAutoOrderType.FormattingEnabled = true;
            this.ddlAutoOrderType.Items.AddRange(new object[] {
            "Market",
            "Limit Post Only"});
            this.ddlAutoOrderType.Location = new System.Drawing.Point(240, 18);
            this.ddlAutoOrderType.Name = "ddlAutoOrderType";
            this.ddlAutoOrderType.Size = new System.Drawing.Size(98, 21);
            this.ddlAutoOrderType.TabIndex = 15;
            // 
            // nudAutoQuantity
            // 
            this.nudAutoQuantity.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudAutoQuantity.Location = new System.Drawing.Point(240, 45);
            this.nudAutoQuantity.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudAutoQuantity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudAutoQuantity.Name = "nudAutoQuantity";
            this.nudAutoQuantity.Size = new System.Drawing.Size(67, 20);
            this.nudAutoQuantity.TabIndex = 15;
            this.nudAutoQuantity.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // btnAutomatedTrading
            // 
            this.btnAutomatedTrading.BackColor = System.Drawing.Color.LimeGreen;
            this.btnAutomatedTrading.FlatAppearance.BorderSize = 0;
            this.btnAutomatedTrading.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutomatedTrading.ForeColor = System.Drawing.Color.White;
            this.btnAutomatedTrading.Location = new System.Drawing.Point(107, 135);
            this.btnAutomatedTrading.Name = "btnAutomatedTrading";
            this.btnAutomatedTrading.Size = new System.Drawing.Size(119, 43);
            this.btnAutomatedTrading.TabIndex = 14;
            this.btnAutomatedTrading.Text = "Start";
            this.btnAutomatedTrading.UseVisualStyleBackColor = false;
            this.btnAutomatedTrading.Click += new System.EventHandler(this.btnAutomatedTrading_Click);
            // 
            // tmrAutoTradeExecution
            // 
            this.tmrAutoTradeExecution.Interval = 5000;
            this.tmrAutoTradeExecution.Tick += new System.EventHandler(this.tmrAutoTradeExecution_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stsAPIValid,
            this.stsAccountBalance,
            this.stsOTProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 724);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1281, 22);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stsAPIValid
            // 
            this.stsAPIValid.Name = "stsAPIValid";
            this.stsAPIValid.Size = new System.Drawing.Size(108, 17);
            this.stsAPIValid.Text = "API keys are invalid";
            // 
            // stsAccountBalance
            // 
            this.stsAccountBalance.Name = "stsAccountBalance";
            this.stsAccountBalance.Size = new System.Drawing.Size(60, 17);
            this.stsAccountBalance.Text = "Balance: 0";
            // 
            // stsOTProgress
            // 
            this.stsOTProgress.Name = "stsOTProgress";
            this.stsOTProgress.Size = new System.Drawing.Size(100, 16);
            this.stsOTProgress.Visible = false;
            // 
            // btnAccountBalance
            // 
            this.btnAccountBalance.BackColor = System.Drawing.Color.LimeGreen;
            this.btnAccountBalance.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAccountBalance.FlatAppearance.BorderSize = 0;
            this.btnAccountBalance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccountBalance.ForeColor = System.Drawing.Color.White;
            this.btnAccountBalance.Location = new System.Drawing.Point(747, 88);
            this.btnAccountBalance.Name = "btnAccountBalance";
            this.btnAccountBalance.Size = new System.Drawing.Size(134, 44);
            this.btnAccountBalance.TabIndex = 16;
            this.btnAccountBalance.Text = "Update Balance";
            this.btnAccountBalance.UseVisualStyleBackColor = false;
            this.btnAccountBalance.Click += new System.EventHandler(this.btnAccountBalance_Click);
            // 
            // nudStopPercent
            // 
            this.nudStopPercent.DecimalPlaces = 2;
            this.nudStopPercent.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudStopPercent.Location = new System.Drawing.Point(128, 173);
            this.nudStopPercent.Name = "nudStopPercent";
            this.nudStopPercent.Size = new System.Drawing.Size(74, 20);
            this.nudStopPercent.TabIndex = 17;
            this.nudStopPercent.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnManualSetStop
            // 
            this.btnManualSetStop.BackColor = System.Drawing.Color.DarkGray;
            this.btnManualSetStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnManualSetStop.FlatAppearance.BorderSize = 0;
            this.btnManualSetStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManualSetStop.ForeColor = System.Drawing.Color.White;
            this.btnManualSetStop.Location = new System.Drawing.Point(209, 172);
            this.btnManualSetStop.Name = "btnManualSetStop";
            this.btnManualSetStop.Size = new System.Drawing.Size(75, 23);
            this.btnManualSetStop.TabIndex = 18;
            this.btnManualSetStop.Text = "Set Stop";
            this.btnManualSetStop.UseVisualStyleBackColor = false;
            this.btnManualSetStop.Click += new System.EventHandler(this.btnManualSetStop_Click);
            // 
            // txtAPIKey
            // 
            this.txtAPIKey.Location = new System.Drawing.Point(575, 36);
            this.txtAPIKey.Name = "txtAPIKey";
            this.txtAPIKey.Size = new System.Drawing.Size(306, 20);
            this.txtAPIKey.TabIndex = 19;
            this.txtAPIKey.UseSystemPasswordChar = true;
            this.txtAPIKey.TextChanged += new System.EventHandler(this.txtAPIKey_TextChanged);
            // 
            // txtAPISecret
            // 
            this.txtAPISecret.Location = new System.Drawing.Point(575, 62);
            this.txtAPISecret.Name = "txtAPISecret";
            this.txtAPISecret.Size = new System.Drawing.Size(306, 20);
            this.txtAPISecret.TabIndex = 20;
            this.txtAPISecret.UseSystemPasswordChar = true;
            this.txtAPISecret.TextChanged += new System.EventHandler(this.txtAPISecret_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(525, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Key";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(525, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Secret";
            // 
            // btnBuyOverTimeOrder
            // 
            this.btnBuyOverTimeOrder.BackColor = System.Drawing.Color.LimeGreen;
            this.btnBuyOverTimeOrder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuyOverTimeOrder.FlatAppearance.BorderSize = 0;
            this.btnBuyOverTimeOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuyOverTimeOrder.ForeColor = System.Drawing.Color.White;
            this.btnBuyOverTimeOrder.Location = new System.Drawing.Point(349, 5);
            this.btnBuyOverTimeOrder.Name = "btnBuyOverTimeOrder";
            this.btnBuyOverTimeOrder.Size = new System.Drawing.Size(141, 27);
            this.btnBuyOverTimeOrder.TabIndex = 24;
            this.btnBuyOverTimeOrder.Text = "Buy Over Time";
            this.btnBuyOverTimeOrder.UseVisualStyleBackColor = false;
            this.btnBuyOverTimeOrder.Click += new System.EventHandler(this.btnBuyOverTimeOrder_Click);
            // 
            // btnSellOverTimeOrder
            // 
            this.btnSellOverTimeOrder.BackColor = System.Drawing.Color.DarkOrange;
            this.btnSellOverTimeOrder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSellOverTimeOrder.FlatAppearance.BorderSize = 0;
            this.btnSellOverTimeOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSellOverTimeOrder.ForeColor = System.Drawing.Color.White;
            this.btnSellOverTimeOrder.Location = new System.Drawing.Point(349, 35);
            this.btnSellOverTimeOrder.Name = "btnSellOverTimeOrder";
            this.btnSellOverTimeOrder.Size = new System.Drawing.Size(141, 27);
            this.btnSellOverTimeOrder.TabIndex = 25;
            this.btnSellOverTimeOrder.Text = "Sell Over Time";
            this.btnSellOverTimeOrder.UseVisualStyleBackColor = false;
            this.btnSellOverTimeOrder.Click += new System.EventHandler(this.btnSellOverTimeOrder_Click);
            // 
            // tmrTradeOverTime
            // 
            this.tmrTradeOverTime.Tick += new System.EventHandler(this.tmrTradeOverTime_Tick);
            // 
            // nudOverTimeContracts
            // 
            this.nudOverTimeContracts.Location = new System.Drawing.Point(356, 114);
            this.nudOverTimeContracts.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudOverTimeContracts.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudOverTimeContracts.Name = "nudOverTimeContracts";
            this.nudOverTimeContracts.Size = new System.Drawing.Size(67, 20);
            this.nudOverTimeContracts.TabIndex = 26;
            this.nudOverTimeContracts.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudOverTimeContracts.ValueChanged += new System.EventHandler(this.nudOverTimeContracts_ValueChanged);
            // 
            // nudOverTimeInterval
            // 
            this.nudOverTimeInterval.Location = new System.Drawing.Point(440, 114);
            this.nudOverTimeInterval.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudOverTimeInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudOverTimeInterval.Name = "nudOverTimeInterval";
            this.nudOverTimeInterval.Size = new System.Drawing.Size(47, 20);
            this.nudOverTimeInterval.TabIndex = 27;
            this.nudOverTimeInterval.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudOverTimeInterval.ValueChanged += new System.EventHandler(this.nudOverTimeInterval_ValueChanged);
            // 
            // nudOverTimeIntervalCount
            // 
            this.nudOverTimeIntervalCount.Location = new System.Drawing.Point(503, 114);
            this.nudOverTimeIntervalCount.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudOverTimeIntervalCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudOverTimeIntervalCount.Name = "nudOverTimeIntervalCount";
            this.nudOverTimeIntervalCount.Size = new System.Drawing.Size(47, 20);
            this.nudOverTimeIntervalCount.TabIndex = 28;
            this.nudOverTimeIntervalCount.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.nudOverTimeIntervalCount.ValueChanged += new System.EventHandler(this.nudOverTimeIntervalCount_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(353, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Contracts Per";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(438, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Seconds";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(505, 98);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "X Times";
            // 
            // lblOverTimeSummary
            // 
            this.lblOverTimeSummary.Location = new System.Drawing.Point(352, 144);
            this.lblOverTimeSummary.Name = "lblOverTimeSummary";
            this.lblOverTimeSummary.Size = new System.Drawing.Size(262, 49);
            this.lblOverTimeSummary.TabIndex = 22;
            this.lblOverTimeSummary.Text = "Over Time Summary";
            // 
            // btnOverTimeStop
            // 
            this.btnOverTimeStop.BackColor = System.Drawing.Color.Tomato;
            this.btnOverTimeStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOverTimeStop.FlatAppearance.BorderSize = 0;
            this.btnOverTimeStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOverTimeStop.ForeColor = System.Drawing.Color.White;
            this.btnOverTimeStop.Location = new System.Drawing.Point(349, 65);
            this.btnOverTimeStop.Name = "btnOverTimeStop";
            this.btnOverTimeStop.Size = new System.Drawing.Size(141, 27);
            this.btnOverTimeStop.TabIndex = 31;
            this.btnOverTimeStop.Text = "Stop";
            this.btnOverTimeStop.UseVisualStyleBackColor = false;
            this.btnOverTimeStop.Click += new System.EventHandler(this.btnOverTimeStop_Click);
            // 
            // btnBulkTest
            // 
            this.btnBulkTest.BackColor = System.Drawing.Color.LimeGreen;
            this.btnBulkTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBulkTest.FlatAppearance.BorderSize = 0;
            this.btnBulkTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBulkTest.ForeColor = System.Drawing.Color.White;
            this.btnBulkTest.Location = new System.Drawing.Point(525, 5);
            this.btnBulkTest.Name = "btnBulkTest";
            this.btnBulkTest.Size = new System.Drawing.Size(175, 27);
            this.btnBulkTest.TabIndex = 32;
            this.btnBulkTest.Text = "BulkTest";
            this.btnBulkTest.UseVisualStyleBackColor = false;
            this.btnBulkTest.Click += new System.EventHandler(this.btnBulkTest_Click);
            // 
            // btnBulkShift
            // 
            this.btnBulkShift.BackColor = System.Drawing.Color.LimeGreen;
            this.btnBulkShift.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBulkShift.FlatAppearance.BorderSize = 0;
            this.btnBulkShift.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBulkShift.ForeColor = System.Drawing.Color.White;
            this.btnBulkShift.Location = new System.Drawing.Point(706, 5);
            this.btnBulkShift.Name = "btnBulkShift";
            this.btnBulkShift.Size = new System.Drawing.Size(175, 27);
            this.btnBulkShift.TabIndex = 33;
            this.btnBulkShift.Text = "Bulk Shift";
            this.btnBulkShift.UseVisualStyleBackColor = false;
            this.btnBulkShift.Click += new System.EventHandler(this.btnBulkShift_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 202);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1250, 507);
            this.tabControl1.TabIndex = 34;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gbCandles);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1242, 481);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Candles";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvOrders);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1242, 481);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Orders";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvOrders
            // 
            this.dgvOrders.AllowUserToAddRows = false;
            this.dgvOrders.AllowUserToDeleteRows = false;
            this.dgvOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOrders.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOrders.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvOrders.Location = new System.Drawing.Point(7, 17);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOrders.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvOrders.Size = new System.Drawing.Size(1225, 458);
            this.dgvOrders.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Select Order Type";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(39, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Select Network";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(45, 58);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Select Symbol";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(73, 84);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Quantity";
            // 
            // gbShowHideCols
            // 
            this.gbShowHideCols.Controls.Add(this.dgShowHideCols);
            this.gbShowHideCols.Location = new System.Drawing.Point(887, 10);
            this.gbShowHideCols.Name = "gbShowHideCols";
            this.gbShowHideCols.Size = new System.Drawing.Size(348, 669);
            this.gbShowHideCols.TabIndex = 39;
            this.gbShowHideCols.TabStop = false;
            this.gbShowHideCols.Visible = false;
            // 
            // dgShowHideCols
            // 
            this.dgShowHideCols.AllowUserToAddRows = false;
            this.dgShowHideCols.AllowUserToDeleteRows = false;
            this.dgShowHideCols.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgShowHideCols.Location = new System.Drawing.Point(10, 17);
            this.dgShowHideCols.Name = "dgShowHideCols";
            this.dgShowHideCols.ReadOnly = true;
            this.dgShowHideCols.Size = new System.Drawing.Size(326, 646);
            this.dgShowHideCols.TabIndex = 0;
            this.dgShowHideCols.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgShowHideCols_CellClick);
            this.dgShowHideCols.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgShowHideCols_CellFormatting);
            this.dgShowHideCols.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgShowHideCols_CellPainting);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(45, 175);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(69, 13);
            this.label15.TabIndex = 40;
            this.label15.Text = "Stop Percent";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1281, 746);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnBulkShift);
            this.Controls.Add(this.btnBulkTest);
            this.Controls.Add(this.btnOverTimeStop);
            this.Controls.Add(this.lblOverTimeSummary);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudOverTimeIntervalCount);
            this.Controls.Add(this.nudOverTimeInterval);
            this.Controls.Add(this.nudOverTimeContracts);
            this.Controls.Add(this.btnSellOverTimeOrder);
            this.Controls.Add(this.btnBuyOverTimeOrder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAPISecret);
            this.Controls.Add(this.txtAPIKey);
            this.Controls.Add(this.btnManualSetStop);
            this.Controls.Add(this.nudStopPercent);
            this.Controls.Add(this.btnAccountBalance);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ddlSymbol);
            this.Controls.Add(this.ddlNetwork);
            this.Controls.Add(this.ddlOrderType);
            this.Controls.Add(this.btnCancelOpenOrders);
            this.Controls.Add(this.chkCancelWhileOrdering);
            this.Controls.Add(this.nudQty);
            this.Controls.Add(this.btnSell);
            this.Controls.Add(this.btnBuy);
            this.Controls.Add(this.gbShowHideCols);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "BitMex Bot Version 2.2";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCandles)).EndInit();
            this.gbCandles.ResumeLayout(false);
            this.gbCandles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCandleRefreshTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMA2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMA1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRetryAttempts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAutoMarketTakeProfitPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAutoQuantity)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStopPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOverTimeContracts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOverTimeInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOverTimeIntervalCount)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            this.gbShowHideCols.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgShowHideCols)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBuy;
        private System.Windows.Forms.Button btnSell;
        private System.Windows.Forms.NumericUpDown nudQty;
        private System.Windows.Forms.CheckBox chkCancelWhileOrdering;
        private System.Windows.Forms.Button btnCancelOpenOrders;
        private System.Windows.Forms.ComboBox ddlOrderType;
        private System.Windows.Forms.ComboBox ddlNetwork;
        private System.Windows.Forms.ComboBox ddlSymbol;
        private System.Windows.Forms.DataGridView dgvCandles;
        private System.Windows.Forms.ComboBox ddlCandleTimes;
        private System.Windows.Forms.GroupBox gbCandles;
        private System.Windows.Forms.Timer tmrCandleUpdater;
        private System.Windows.Forms.CheckBox chkUpdateCandles;
        private System.Windows.Forms.Label lblMA2;
        private System.Windows.Forms.NumericUpDown nudMA2;
        private System.Windows.Forms.Label lblMA1;
        private System.Windows.Forms.NumericUpDown nudMA1;
        private System.Windows.Forms.RadioButton rdoBuy;
        private System.Windows.Forms.RadioButton rdoSell;
        private System.Windows.Forms.RadioButton rdoSwitch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAutomatedTrading;
        private System.Windows.Forms.ComboBox ddlAutoOrderType;
        private System.Windows.Forms.NumericUpDown nudAutoQuantity;
        private System.Windows.Forms.Timer tmrAutoTradeExecution;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel stsAPIValid;
        private System.Windows.Forms.ToolStripStatusLabel stsAccountBalance;
        private System.Windows.Forms.Button btnAccountBalance;
        private System.Windows.Forms.NumericUpDown nudStopPercent;
        private System.Windows.Forms.Button btnManualSetStop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudAutoMarketTakeProfitPercent;
        private System.Windows.Forms.CheckBox chkAutoMarketTakeProfits;
        private System.Windows.Forms.Label lblAutoUnrealizedROEPercent;
        private System.Windows.Forms.TextBox txtAPIKey;
        private System.Windows.Forms.TextBox txtAPISecret;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBuyOverTimeOrder;
        private System.Windows.Forms.Button btnSellOverTimeOrder;
        private System.Windows.Forms.Timer tmrTradeOverTime;
        private System.Windows.Forms.NumericUpDown nudOverTimeContracts;
        private System.Windows.Forms.NumericUpDown nudOverTimeInterval;
        private System.Windows.Forms.NumericUpDown nudOverTimeIntervalCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblOverTimeSummary;
        private System.Windows.Forms.ToolStripProgressBar stsOTProgress;
        private System.Windows.Forms.Button btnOverTimeStop;
        private System.Windows.Forms.Button btnBulkTest;
        private System.Windows.Forms.Button btnBulkShift;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudCandleRefreshTime;
        private System.Windows.Forms.GroupBox gbShowHideCols;
        private System.Windows.Forms.DataGridView dgShowHideCols;
        private System.Windows.Forms.Button btnShowHideCols;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown nudRetryAttempts;
        private System.Windows.Forms.Button btnAutoMatedTradingStop;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DataGridView dgvOrders;
    }
}

