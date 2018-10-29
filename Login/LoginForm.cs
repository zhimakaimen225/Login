using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gd
{
    public partial class LoginForm : Form
    {
        public static readonly string[] gradeString = new string[4] { "管理员", "工程师", "厂家", "操作员" };
        DataCenter dataCenter;
        List<UserInfo> userInfos;
        int curIndex = 0;
        bool loginSuccess = false;
        
        public LoginForm(DataCenter dataCenterP)
        {
            InitializeComponent();

            //this.ControlBox = false;

            dataCenter = dataCenterP;

            userInfos = dataCenter.GetAllUserInfo();
            if (userInfos != null)
            {
                foreach(UserInfo userInfo in userInfos)
                {
                    comboBoxUserName.Items.Add(userInfo.number);
                }
            }
            else
            {
                //显示警告
                MessageBox.Show("用户名读取错误");
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            int userNumber = Convert.ToInt32(comboBoxUserName.Text.Trim());
            string password = textBoxPassword.Text.Trim();

            string loginPassword = dataCenter.GetPassword(userNumber);
            if (password == loginPassword)
            {
                //关闭窗口
                loginSuccess = true;
                DialogResult = DialogResult.OK;
                //Hide();
                Close();
            }
            else
            {
                //显示警告
                MessageBox.Show("密码不正确，请重新输入");
                textBoxPassword.Text = "";
                textBoxPassword.Focus();
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = !canBeClosed;
            if (!loginSuccess)
            {
                //Owner.Close();
                DialogResult = DialogResult.Cancel;
            }
        }

        private void comboBoxUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            curIndex = comboBoxUserName.SelectedIndex;
            string userName = userInfos[curIndex].userName;
            int userRight = userInfos[curIndex].grade;

            labelUserName.Text = userName;
            labelUserRight.Text = gradeString[userRight];
        }

        public UserInfo GetCurUserInfo()
        {
            return userInfos[curIndex];
        }
    }
}
