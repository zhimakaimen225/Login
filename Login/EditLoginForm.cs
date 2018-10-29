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
    public partial class EditLoginForm : Form
    {
        private UserInfo curUserInfo;
        private MySqlConnection connection = null;
        private MySqlDataAdapter da = null;
        MySqlCommand command = null;
        DataSet ds = new DataSet();
        public const String CONNECT_STRING = "server=localhost;userid=root;password=123456;database=jddatabase";
        private string sql_select;
        private string org_sql_select;
        private MySqlTransaction trans;
        public EditLoginForm(UserInfo userInfo)
        {
            InitializeComponent();
            curUserInfo = userInfo;
            Init();
        }

        public void Init()
        {
            if (Open() == false)
            {
                return;
            }

            if (curUserInfo.grade == 0)     //超级用户
            {
                org_sql_select = string.Format("select LoginUserName as \"用户名\", LoginPassword, LoginId, LoginGrade, LoginNumber, LoginAddress, LoginTel, LoginCompany, LoginSummary from LoginTable where LoginGrade >= {0}", curUserInfo.grade);
            }
            else
            {
                org_sql_select = string.Format("select LoginUserName, LoginId, LoginGrade, LoginNumber, LoginAddress, LoginTel, LoginCompany, LoginSummary from LoginTable where LoginGrade >= {0}", curUserInfo.grade);
            }
            sql_select = org_sql_select;
            
            try
            {
                command = new MySqlCommand(sql_select, connection);
                da = new MySqlDataAdapter(command);
                da.Fill(ds, "table");
                dataGridViewUser.DataSource = ds.Tables[0];
                dataGridViewUser.AutoGenerateColumns = true;

                //dataGridViewUser.Columns[0].ReadOnly = true;

                trans = connection.BeginTransaction();
            }
            catch(Exception ex)
            {
                MessageBox.Show("数据表初始化失败");
                return;
            }

            //dataGridViewUser.Columns[idColIndex].Visible = false;
            dataGridViewUser.Columns["LoginId"].Visible = false;

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
                dataGridViewUser.DataSource = ds.Tables[0];
                dataGridViewUser.AutoGenerateColumns = true;
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
            catch(Exception ex)
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
                ds.Tables[0].Rows[dataGridViewUser.CurrentRow.Index].Delete();
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
            catch(Exception ex)
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
            catch(Exception ex)
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
            if (textBoxNumber.Text != "")
            {
                sql_select = org_sql_select + " and LoginNumber = " + textBoxNumber.Text;
            }

            if (textBoxName.Text != "")
            {
                sql_select += " and LoginUserName = \"" + textBoxName.Text + "\"";  
            }

            try
            {
                command = null;
                command = new MySqlCommand(sql_select, connection);
                da = null;
                da = new MySqlDataAdapter(command);
                ds.Clear();
                da.Fill(ds, "table");
                dataGridViewUser.DataSource = ds.Tables[0];
                dataGridViewUser.AutoGenerateColumns = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("查找失败");
                return;
            }
        }

        private void EditLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                trans.Rollback();
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("取消提交数据失败或关闭数据连接时出错");
            }

            return;
        }
    }
}
