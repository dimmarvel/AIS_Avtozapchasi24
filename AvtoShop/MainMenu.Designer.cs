namespace AvtoShop
{
    partial class MainMenu
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
            this.PurchaseButton = new System.Windows.Forms.Button();
            this.ProductsButton = new System.Windows.Forms.Button();
            this.SalesStatisticsButton = new System.Windows.Forms.Button();
            this.SalesButton = new System.Windows.Forms.Button();
            this.StorageButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PurchaseButton
            // 
            this.PurchaseButton.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PurchaseButton.Location = new System.Drawing.Point(256, 93);
            this.PurchaseButton.Name = "PurchaseButton";
            this.PurchaseButton.Size = new System.Drawing.Size(272, 75);
            this.PurchaseButton.TabIndex = 1;
            this.PurchaseButton.Text = "Закупки";
            this.PurchaseButton.UseVisualStyleBackColor = true;
            this.PurchaseButton.Click += new System.EventHandler(this.PurchaseButton_Click);
            // 
            // ProductsButton
            // 
            this.ProductsButton.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ProductsButton.Location = new System.Drawing.Point(256, 174);
            this.ProductsButton.Name = "ProductsButton";
            this.ProductsButton.Size = new System.Drawing.Size(272, 75);
            this.ProductsButton.TabIndex = 2;
            this.ProductsButton.Text = "Товары";
            this.ProductsButton.UseVisualStyleBackColor = true;
            this.ProductsButton.Click += new System.EventHandler(this.ProductsButton_Click);
            // 
            // SalesStatisticsButton
            // 
            this.SalesStatisticsButton.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SalesStatisticsButton.Location = new System.Drawing.Point(256, 255);
            this.SalesStatisticsButton.Name = "SalesStatisticsButton";
            this.SalesStatisticsButton.Size = new System.Drawing.Size(272, 75);
            this.SalesStatisticsButton.TabIndex = 3;
            this.SalesStatisticsButton.Text = "Статистика продаж";
            this.SalesStatisticsButton.UseVisualStyleBackColor = true;
            this.SalesStatisticsButton.Click += new System.EventHandler(this.SalesStatisticsButton_Click);
            // 
            // SalesButton
            // 
            this.SalesButton.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SalesButton.Location = new System.Drawing.Point(256, 336);
            this.SalesButton.Name = "SalesButton";
            this.SalesButton.Size = new System.Drawing.Size(272, 75);
            this.SalesButton.TabIndex = 4;
            this.SalesButton.Text = "Продажи";
            this.SalesButton.UseVisualStyleBackColor = true;
            this.SalesButton.Click += new System.EventHandler(this.SalesButton_Click);
            // 
            // StorageButton
            // 
            this.StorageButton.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StorageButton.Location = new System.Drawing.Point(256, 12);
            this.StorageButton.Name = "StorageButton";
            this.StorageButton.Size = new System.Drawing.Size(272, 75);
            this.StorageButton.TabIndex = 5;
            this.StorageButton.Text = "Склад";
            this.StorageButton.UseVisualStyleBackColor = true;
            this.StorageButton.Click += new System.EventHandler(this.StorageButton_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.StorageButton);
            this.Controls.Add(this.SalesButton);
            this.Controls.Add(this.SalesStatisticsButton);
            this.Controls.Add(this.ProductsButton);
            this.Controls.Add(this.PurchaseButton);
            this.Name = "MainMenu";
            this.Text = "Автозапчасти24";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button PurchaseButton;
        private System.Windows.Forms.Button ProductsButton;
        private System.Windows.Forms.Button SalesStatisticsButton;
        private System.Windows.Forms.Button SalesButton;
        private System.Windows.Forms.Button StorageButton;
    }
}

