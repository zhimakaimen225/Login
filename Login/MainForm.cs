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
    public partial class MainForm : Form
    {
        DataCenter dataCenter;
        UserInfo curUserInfo;
        Camera camera;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            dataCenter = new DataCenter();

            LoginForm loginForm = new LoginForm(dataCenter);
            //loginForm.ShowDialog();
            //if (loginForm.DialogResult == DialogResult.Cancel)
            //{
            //    Close();
            //}

            curUserInfo = loginForm.GetCurUserInfo();

            InitCamera();
        }

        private void 登录管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (curUserInfo.grade > 2)
            {
                return;
            }

            EditLoginForm editLoginForm = new EditLoginForm(curUserInfo);
            editLoginForm.ShowDialog();
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePwdForm changePwdForm = new ChangePwdForm(curUserInfo, dataCenter);
            changePwdForm.ShowDialog();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void 送检单位管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (curUserInfo.grade > 2)
            {
                return;
            }

            EditSampleUnitForm editSampleUnitForm = new EditSampleUnitForm(dataCenter);
            editSampleUnitForm.ShowDialog();
        }

        private void InitCamera()
        {
            camera = new Camera(pictureBoxCam, dataCenter);
            camera.Start();
            //camera.PrintAllProperties();
        }
        
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (camera != null)
            {
                camera.Stop();
            }

        }

        private void buttonZoomIn_Click(object sender, EventArgs e)
        {
            camera.ZoomIn();
        }

        private void buttonZoomOut_Click(object sender, EventArgs e)
        {
            camera.ZoomOut();
        }

        private void buttonMoveLeft_Click(object sender, EventArgs e)
        {
            camera.MoveLeft();
        }

        private void buttonMoveRight_Click(object sender, EventArgs e)
        {
            camera.MoveRight();
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            camera.MoveUp();
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            camera.MoveDown();
        }

        private void buttonHMirror_Click(object sender, EventArgs e)
        {
            camera.HMirror();
        }

        private void buttonVMirror_Click(object sender, EventArgs e)
        {
            camera.VMirror();
        }

        private void buttonCaptureImage_Click(object sender, EventArgs e)
        {
            camera.CaptureImage();
        }

        private void buttonSaveSetting_Click(object sender, EventArgs e)
        {
            camera.SaveSetting();
        }

        private void buttonCameraSetting_Click(object sender, EventArgs e)
        {
            camera.Setting();
        }
    }
}
