using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using AForge.Video;

namespace Gd
{
    public partial class CameraSetting : Form
    {
        FilterInfoCollection videoCaptureDevices = null;
        VideoCaptureDevice cam = null;
        public int cameraIndex = -1;

        public CameraSetting(int index)
        {
            InitializeComponent();

            Init(index);
        }

        private void Init(int index)
        {
            videoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            for (int i = 0; i < videoCaptureDevices.Count; i++)
            {
                ///---实例化对象
                comboBoxCamera.Items.Add(videoCaptureDevices[i].Name);

            }
            comboBoxCamera.SelectedIndex = index;
            cameraIndex = index;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            cameraIndex = comboBoxCamera.SelectedIndex;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
