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
    public partial class ChangePwdForm : Form
    {
        private UserInfo userInfo;
        private DataCenter dataCenter;
        public ChangePwdForm(UserInfo userInfoP, DataCenter dataCenterP)
        {
            InitializeComponent();
            labelUserName.Text = userInfoP.userName;
            userInfo = userInfoP;
            dataCenter = dataCenterP;
        }

        public void SetUserInfo(UserInfo userInfoP)
        {
            userInfo = userInfoP;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (textBoxOldPwd.Text != userInfo.password)
            {
                MessageBox.Show("旧密码不正确，请重新输入");

                textBoxOldPwd.Text = "";
                textBoxNewPwd.Text = "";
                textBoxReset.Text = "";

                textBoxOldPwd.Focus();
            } 

            if (textBoxNewPwd.Text != textBoxReset.Text)
            {
                MessageBox.Show("两次输入的新密码不一致，请重新输入");
                textBoxNewPwd.Text = "";
                textBoxReset.Text = "";

                textBoxNewPwd.Focus();
            }

            userInfo.password = textBoxNewPwd.Text;
            if ((dataCenter != null) && (dataCenter.UpdateUserInfo(userInfo)))
            {
                MessageBox.Show("密码已成功修改");
                Close();
            }
            else
            {
                MessageBox.Show("密码未能成功修改");
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
