namespace DataSourcesConnectionApp
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblSqlStatus = new System.Windows.Forms.Label();
            this.txtSqlConnectionString = new System.Windows.Forms.TextBox();
            this.btnConnectSql = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConnectSQLite = new System.Windows.Forms.Button();
            this.lblSQLiteStatus = new System.Windows.Forms.Label();
            this.txtSQLiteConnectionString = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSqlStatus
            // 
            this.lblSqlStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSqlStatus.AutoSize = true;
            this.lblSqlStatus.Location = new System.Drawing.Point(13, 27);
            this.lblSqlStatus.Name = "lblSqlStatus";
            this.lblSqlStatus.Size = new System.Drawing.Size(35, 13);
            this.lblSqlStatus.TabIndex = 0;
            this.lblSqlStatus.Text = "label1";
            // 
            // txtSqlConnectionString
            // 
            this.txtSqlConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSqlConnectionString.Location = new System.Drawing.Point(172, 23);
            this.txtSqlConnectionString.Name = "txtSqlConnectionString";
            this.txtSqlConnectionString.Size = new System.Drawing.Size(508, 20);
            this.txtSqlConnectionString.TabIndex = 1;
            // 
            // btnConnectSql
            // 
            this.btnConnectSql.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnectSql.Location = new System.Drawing.Point(686, 22);
            this.btnConnectSql.Name = "btnConnectSql";
            this.btnConnectSql.Size = new System.Drawing.Size(95, 23);
            this.btnConnectSql.TabIndex = 2;
            this.btnConnectSql.Text = "Подключение";
            this.btnConnectSql.UseVisualStyleBackColor = true;
            this.btnConnectSql.Click += new System.EventHandler(this.btnConnectSql_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.btnConnectSQLite);
            this.groupBox1.Controls.Add(this.lblSQLiteStatus);
            this.groupBox1.Controls.Add(this.txtSQLiteConnectionString);
            this.groupBox1.Controls.Add(this.btnConnectSql);
            this.groupBox1.Controls.Add(this.lblSqlStatus);
            this.groupBox1.Controls.Add(this.txtSqlConnectionString);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox1.Size = new System.Drawing.Size(792, 87);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // btnConnectSQLite
            // 
            this.btnConnectSQLite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnectSQLite.Location = new System.Drawing.Point(686, 54);
            this.btnConnectSQLite.Name = "btnConnectSQLite";
            this.btnConnectSQLite.Size = new System.Drawing.Size(94, 23);
            this.btnConnectSQLite.TabIndex = 5;
            this.btnConnectSQLite.Text = "Подключение";
            this.btnConnectSQLite.UseVisualStyleBackColor = true;
            this.btnConnectSQLite.Click += new System.EventHandler(this.btnConnectSQLite_Click);
            // 
            // lblSQLiteStatus
            // 
            this.lblSQLiteStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSQLiteStatus.AutoSize = true;
            this.lblSQLiteStatus.Location = new System.Drawing.Point(13, 59);
            this.lblSQLiteStatus.Name = "lblSQLiteStatus";
            this.lblSQLiteStatus.Size = new System.Drawing.Size(35, 13);
            this.lblSQLiteStatus.TabIndex = 3;
            this.lblSQLiteStatus.Text = "label1";
            // 
            // txtSQLiteConnectionString
            // 
            this.txtSQLiteConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSQLiteConnectionString.Location = new System.Drawing.Point(172, 55);
            this.txtSQLiteConnectionString.Name = "txtSQLiteConnectionString";
            this.txtSQLiteConnectionString.Size = new System.Drawing.Size(508, 20);
            this.txtSQLiteConnectionString.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 87);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(808, 126);
            this.MinimumSize = new System.Drawing.Size(808, 126);
            this.Name = "Form1";
            this.Text = "DB Connecter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblSqlStatus;
        private System.Windows.Forms.TextBox txtSqlConnectionString;
        private System.Windows.Forms.Button btnConnectSql;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnConnectSQLite;
        private System.Windows.Forms.Label lblSQLiteStatus;
        private System.Windows.Forms.TextBox txtSQLiteConnectionString;
    }
}

