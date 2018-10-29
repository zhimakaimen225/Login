using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Gd
{
    public partial class EditSampleUnitForm : Form
    {
        private MySqlConnection connection = null;
        private MySqlDataAdapter da = null;
        MySqlCommand command = null;
        DataSet ds = new DataSet();
        public const String CONNECT_STRING = "server=localhost;userid=root;password=123456;database=jddatabase";
        private string sql_select;
        private string org_sql_select;
        private MySqlTransaction trans;
        DataCenter dataCenter;
        PrintHelper printHelper = null;
        private static readonly string title = "送检样品单位汇总";
        public EditSampleUnitForm(DataCenter dataCenterP)
        {
            InitializeComponent();
            dataCenter = dataCenterP;
            printHelper = new PrintHelper();
            Init();
        }

        public void Init()
        {
            if (Open() == false)
            {
                return;
            }

            org_sql_select = string.Format("select * from SampleUnitTable");
            sql_select = org_sql_select;

            try
            {
                command = new MySqlCommand(sql_select, connection);
                da = new MySqlDataAdapter(command);
                da.Fill(ds, "table");
                dataGridViewCompany.DataSource = ds.Tables[0];
                dataGridViewCompany.AutoGenerateColumns = true;

                //dataGridViewUser.Columns[0].ReadOnly = true;

                trans = connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据表初始化失败");
                return;
            }

            dataGridViewCompany.Columns["SampleUnitId"].ReadOnly = true;
            //dataGridViewCompany.Columns["LoginId"].Visible = false;

            buttonApply.Enabled = false;
            buttonCancel.Enabled = false;
        }

        public bool Open()
        {
            try
            {
                connection = new MySqlConnection(CONNECT_STRING);
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开数据库错误");
                return false;
            }

            return true;
        }

        public void RefreshDataGridView()
        {
            try
            {
                command = null;
                command = new MySqlCommand(sql_select, connection);
                da = null;
                da = new MySqlDataAdapter(command);
                ds.Clear();
                da.Fill(ds, "table");
                dataGridViewCompany.DataSource = ds.Tables[0];
                dataGridViewCompany.AutoGenerateColumns = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("刷新失败");
                return;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlCommandBuilder scb = new MySqlCommandBuilder(da);
                da.InsertCommand = scb.GetInsertCommand();
                da.Update(ds, "table");
            }
            catch (Exception ex)
            {
                MessageBox.Show("增加数据失败");
                return;
            }

            buttonApply.Enabled = true;
            buttonCancel.Enabled = true;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ds.AcceptChanges();
                MySqlCommandBuilder scb = new MySqlCommandBuilder(da);
                ds.Tables[0].Rows[dataGridViewCompany.CurrentRow.Index].Delete();
                da.DeleteCommand = scb.GetDeleteCommand();
                da.Update(ds, "table");
            }
            catch
            {
                MessageBox.Show("删除数据失败");
                return;
            }

            buttonApply.Enabled = true;
            buttonCancel.Enabled = true;
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlCommandBuilder scb = new MySqlCommandBuilder(da);
                da.UpdateCommand = scb.GetUpdateCommand();
                da.Update(ds, "table");
            }
            catch (Exception ex)
            {
                MessageBox.Show("修改数据失败");
                return;
            }

            buttonApply.Enabled = true;
            buttonCancel.Enabled = true;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            try
            {
                trans.Commit();
                RefreshDataGridView();
                trans = connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                MessageBox.Show("提交变动失败");
                return;
            }

            buttonApply.Enabled = false;
            buttonCancel.Enabled = false;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                trans.Rollback();
                RefreshDataGridView();
                trans = connection.BeginTransaction();
            }
            catch
            {
                MessageBox.Show("取消变动失败");
                return;
            }
            
            buttonApply.Enabled = false;
            buttonCancel.Enabled = false;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (textBoxId.Text != "")
            {
                sql_select = org_sql_select + " where SampleUnitId = " + textBoxId.Text;
            }

            if (textBoxName.Text != "")
            {
                sql_select += " and SampleUnitName = \"" + textBoxName.Text + "\"";
            }

            try
            {
                command = null;
                command = new MySqlCommand(sql_select, connection);
                da = null;
                da = new MySqlDataAdapter(command);
                ds.Clear();
                da.Fill(ds, "table");
                dataGridViewCompany.DataSource = ds.Tables[0];
                dataGridViewCompany.AutoGenerateColumns = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("查找失败");
                return;
            }
        }

        private void EditSampleUnitForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                trans.Rollback();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("取消提交数据失败或关闭数据连接时出错");
            }

            return;
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            List<SampleUnitInfo> list = dataCenter.GetAllSampleInfo();
            DataTable dt = CreateDataTable(list);

            printHelper.Print(dt, title);
        }

        private DataTable CreateDataTable(List<SampleUnitInfo> list)
        {
            DataTable dt = new DataTable();

            //创建列
            dt.Columns.Add("单位编号", typeof(int));
            dt.Columns.Add("单位名称", typeof(String));
            dt.Columns.Add("单位地址", typeof(String));
            dt.Columns.Add("           单位电话", typeof(String));
            dt.Columns.Add("           单位简介", typeof(String));

            foreach(SampleUnitInfo info in list)
            {
                //创建行
                dt.Rows.Add(info.id, info.name, info.address, info.tel, info.summary);
            }

            return dt;
        }

        private void buttonPrintPreview_Click(object sender, EventArgs e)
        {
            List<SampleUnitInfo> list = dataCenter.GetAllSampleInfo();
            DataTable dt = CreateDataTable(list);

            printHelper.PrintPriview(dt, title);
        }

        private void buttonPrintSetting_Click(object sender, EventArgs e)
        {
            printHelper.PrintSetting();
        }
    }
}
