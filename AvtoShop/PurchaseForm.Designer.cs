namespace AvtoShop
{
    partial class PurchaseForm
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
            this.buyButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.zapchastBox1 = new System.Windows.Forms.ComboBox();
            this.izgotovitelBox2 = new System.Windows.Forms.ComboBox();
            this.markBox3 = new System.Windows.Forms.ComboBox();
            this.countTextBox1 = new System.Windows.Forms.TextBox();
            this.priceLabel = new System.Windows.Forms.Label();
            this.checkPostavshikButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.soldpriceTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buyButton
            // 
            this.buyButton.Location = new System.Drawing.Point(59, 224);
            this.buyButton.Name = "buyButton";
            this.buyButton.Size = new System.Drawing.Size(148, 52);
            this.buyButton.TabIndex = 0;
            this.buyButton.Text = "Купить";
            this.buyButton.UseVisualStyleBackColor = true;
            this.buyButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(65, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Запчасть:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(65, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Изготовитель:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(65, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Марка:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(65, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Количество:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(65, 181);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "Цена:";
            // 
            // zapchastBox1
            // 
            this.zapchastBox1.FormattingEnabled = true;
            this.zapchastBox1.Location = new System.Drawing.Point(191, 64);
            this.zapchastBox1.Name = "zapchastBox1";
            this.zapchastBox1.Size = new System.Drawing.Size(197, 21);
            this.zapchastBox1.TabIndex = 6;
            this.zapchastBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // izgotovitelBox2
            // 
            this.izgotovitelBox2.FormattingEnabled = true;
            this.izgotovitelBox2.Location = new System.Drawing.Point(191, 37);
            this.izgotovitelBox2.Name = "izgotovitelBox2";
            this.izgotovitelBox2.Size = new System.Drawing.Size(197, 21);
            this.izgotovitelBox2.TabIndex = 7;
            this.izgotovitelBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // markBox3
            // 
            this.markBox3.FormattingEnabled = true;
            this.markBox3.Location = new System.Drawing.Point(191, 91);
            this.markBox3.Name = "markBox3";
            this.markBox3.Size = new System.Drawing.Size(197, 21);
            this.markBox3.TabIndex = 8;
            this.markBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // countTextBox1
            // 
            this.countTextBox1.Location = new System.Drawing.Point(191, 118);
            this.countTextBox1.Name = "countTextBox1";
            this.countTextBox1.Size = new System.Drawing.Size(197, 20);
            this.countTextBox1.TabIndex = 9;
            this.countTextBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.countTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // priceLabel
            // 
            this.priceLabel.AutoSize = true;
            this.priceLabel.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.priceLabel.Location = new System.Drawing.Point(188, 181);
            this.priceLabel.Name = "priceLabel";
            this.priceLabel.Size = new System.Drawing.Size(0, 17);
            this.priceLabel.TabIndex = 10;
            // 
            // checkPostavshikButton
            // 
            this.checkPostavshikButton.Location = new System.Drawing.Point(230, 224);
            this.checkPostavshikButton.Name = "checkPostavshikButton";
            this.checkPostavshikButton.Size = new System.Drawing.Size(148, 52);
            this.checkPostavshikButton.TabIndex = 11;
            this.checkPostavshikButton.Text = "Посмотреть все товары поставщиков";
            this.checkPostavshikButton.UseVisualStyleBackColor = true;
            this.checkPostavshikButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(65, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Цена продажи:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // soldpriceTextBox
            // 
            this.soldpriceTextBox.Location = new System.Drawing.Point(191, 147);
            this.soldpriceTextBox.Name = "soldpriceTextBox";
            this.soldpriceTextBox.Size = new System.Drawing.Size(197, 20);
            this.soldpriceTextBox.TabIndex = 13;
            this.soldpriceTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.soldpriceTextBox_KeyPress);
            // 
            // PurchaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 288);
            this.Controls.Add(this.soldpriceTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkPostavshikButton);
            this.Controls.Add(this.priceLabel);
            this.Controls.Add(this.countTextBox1);
            this.Controls.Add(this.markBox3);
            this.Controls.Add(this.izgotovitelBox2);
            this.Controls.Add(this.zapchastBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buyButton);
            this.Name = "PurchaseForm";
            this.Text = "Закупки";
            this.Load += new System.EventHandler(this.PurchaseForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buyButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox zapchastBox1;
        private System.Windows.Forms.ComboBox izgotovitelBox2;
        private System.Windows.Forms.ComboBox markBox3;
        private System.Windows.Forms.TextBox countTextBox1;
        private System.Windows.Forms.Label priceLabel;
        private System.Windows.Forms.Button checkPostavshikButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox soldpriceTextBox;
    }
}