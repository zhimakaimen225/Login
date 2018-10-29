using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace Gd
{
    public partial class PrintForm : Form
    {
        public PrintForm(List<SampleUnitInfo> sampleUnitInfoList)
        {
            InitializeComponent();
        }

        private void buttonPrintSetting_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument1 = new PrintDocument();
            //设置打印用的纸张,当设置为Custom的时候，可以自定义纸张的大小
            printDocument1.DefaultPageSettings.PaperSize = new PaperSize("Custum", 500, 500);
            //注册PrintPage事件，打印每一页时会触发该事件
            printDocument1.PrintPage += new PrintPageEventHandler(this.PrintDocument_PrintPage);

            //初始化打印对话框对象
            PrintDialog printDialog1 = new PrintDialog();
            //将PrintDialog.UseEXDialog属性设置为True，才可显示出打印对话框
            printDialog1.UseEXDialog = true;
            //将printDocument1对象赋值给打印对话框的Document属性
            printDialog1.Document = printDocument1;
            //打开打印对话框
            DialogResult result = printDialog1.ShowDialog();
            if (result == DialogResult.OK)
                printDocument1.Print();//开始打印
        }

        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //设置打印内容及其字体，颜色和位置
            e.Graphics.DrawString("Hello World！", new Font(new FontFamily("黑体"), 24), System.Drawing.Brushes.Red, 50, 50);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            //实例化打印对象
            PrintDocument printDocument1 = new PrintDocument();
            //设置打印用的纸张,当设置为Custom的时候，可以自定义纸张的大小
            printDocument1.DefaultPageSettings.PaperSize = new PaperSize("Custum", 500, 500);
            //注册PrintPage事件，打印每一页时会触发该事件
            printDocument1.PrintPage += new PrintPageEventHandler(this.PrintDocument_PrintPage);

            //初始化打印预览对话框对象
            PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
            //将printDocument1对象赋值给打印预览对话框的Document属性
            printPreviewDialog1.Document = printDocument1;
            //打开打印预览对话框
            DialogResult result = printPreviewDialog1.ShowDialog();
            if (result == DialogResult.OK)
                printDocument1.Print();//开始打印
        }
    }
}
