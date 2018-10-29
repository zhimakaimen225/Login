using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Video.DirectShow;
using AForge.Video;

namespace Gd
{
    public class Camera
    {
        DataCenter dataCenter;

        static readonly int ORG_OUT_RIDIUM = 50;
        static readonly int ORG_IN_RIDIUM = 30;

        FilterInfoCollection videoCaptureDevices = null;
        VideoCaptureDevice cam = null;
        PictureBox pictureBox;
        int camDeviceIndex = 0;
        
        int orgWidth = 0;
        int orgHeight = 0;

        int finderFrameX = 0;
        int finderFrameY = 0;
        int finderFrameWidth = 0;
        int finderFrameHeight = 0;
        int zoom = 1;   //缩小倍数
        int maxZoom = 6;

        int outRidium;
        int inRidium;

        int panStep = 10;
        int tiltStep = 10;

        Bitmap boxBmp = new Bitmap(240, 180);
        Rectangle destRect = new Rectangle(0, 0, 240, 180);

        bool hMirror = false;
        bool vMirror = false;
        bool light = false;

        bool saveImage = false;

        public Camera(PictureBox pb, DataCenter dc)
        {
            dataCenter = dc;
            pictureBox = pb;
            try
            {
                videoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                ///---摄像头数量大于0
                if (videoCaptureDevices.Count > 0)
                {
                    ///---实例化对象
                    cam = new VideoCaptureDevice(videoCaptureDevices[camDeviceIndex].MonikerString);
                    ///---绑定事件
                    cam.NewFrame += new NewFrameEventHandler(Cam_NewFrame);
                }

                //PrintAllProperties();
                //cam.DisplayPropertyPage(IntPtr.Zero);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        ~Camera()
        {
            try
            {
                if (cam != null)
                {
                    ///---关闭摄像头
                    if (cam.IsRunning)
                    {
                        cam.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetCamera(int index)
        {
            if (videoCaptureDevices.Count > 0)
            {
                ///---实例化对象
                cam = new VideoCaptureDevice(videoCaptureDevices[index].MonikerString);
                ///---绑定事件
                cam.NewFrame += new NewFrameEventHandler(Cam_NewFrame);

                camDeviceIndex = index;
            }
        }

        private void Cam_NewFrame(object obj, NewFrameEventArgs eventArgs)
        {
            if (orgWidth == 0)
            {
                zoom = 1;
                //改变camera要修改orgWidth，orgHeight为0
                orgWidth = eventArgs.Frame.Size.Width;
                orgHeight = eventArgs.Frame.Size.Height;

                //middleBmp = null;
                //middleBmp = new Bitmap(orgWidth, orgHeight);
                finderFrameWidth = orgWidth;
                finderFrameHeight = orgHeight;

                int finderCenterX = finderFrameWidth / 2;
                int finderCenterY = finderFrameHeight / 2;
                //算出新取景框左上角
                finderFrameX = finderCenterX - finderFrameWidth / 2;
                finderFrameY = finderCenterY - finderFrameHeight / 2;
                //算出两个圆的半径
                outRidium = ORG_OUT_RIDIUM;
                inRidium = ORG_IN_RIDIUM;
            }

            //pictureBox.Image = (Bitmap)eventArgs.Frame.Clone(new RectangleF(60, 60, 300, 240), PixelFormat.Format24bppRgb);
            //Bitmap middleBmp = (Bitmap)eventArgs.Frame.Clone(new RectangleF(60, 60, 240, 180), PixelFormat.Format24bppRgb);
            Bitmap middleBmp = (Bitmap)eventArgs.Frame.Clone(new RectangleF(finderFrameX, finderFrameY, finderFrameWidth, finderFrameHeight), PixelFormat.Format24bppRgb);

            //Graphics g = Graphics.FromImage(middleBmp);
            Graphics g = Graphics.FromImage(boxBmp);
            g.DrawImage(middleBmp, destRect);

            RotateFlipType horizontalMirror = (hMirror == true ? RotateFlipType.RotateNoneFlipX : RotateFlipType.RotateNoneFlipNone);
            RotateFlipType verticalMirror = (vMirror == true ? RotateFlipType.RotateNoneFlipY : RotateFlipType.RotateNoneFlipNone);
            boxBmp.RotateFlip(horizontalMirror | verticalMirror);

            string timeStr = "";
            string orgFilePath = "";
            string processedFilePath = "";
            string orgFileName = "";
            string processedFileName = "";
            if (saveImage)
            {
                timeStr = DateTime.Now.ToString("yyyyMMddhhmmss");
                orgFilePath = "..\\..\\capture\\";
                orgFileName = string.Format("{0}.png", timeStr);
                boxBmp.Save(orgFilePath + orgFileName, System.Drawing.Imaging.ImageFormat.Png);
            }


            Pen p = new Pen(Color.Red);

            //Point p1 = new Point(0, finderFrameHeight / 2);
            //Point p2 = new Point(finderFrameWidth, finderFrameHeight / 2);
            //g.DrawLine(p, p1, p2);

            //p1.X = finderFrameWidth / 2;
            //p1.Y = 0;
            //p2.X = finderFrameWidth / 2;
            //p2.Y = finderFrameHeight;
            //g.DrawLine(p, p1, p2);

            //g.DrawEllipse(p, finderFrameWidth / 2 - outRidium, finderFrameHeight / 2 - outRidium, outRidium * 2, outRidium * 2);
            //g.DrawEllipse(p, finderFrameWidth / 2 - inRidium, finderFrameHeight / 2 - inRidium, inRidium * 2, inRidium * 2);

            Point p1 = new Point(0, pictureBox.Height / 2);
            Point p2 = new Point(pictureBox.Width, pictureBox.Height / 2);
            g.DrawLine(p, p1, p2);

            p1.X = pictureBox.Width / 2;
            p1.Y = 0;
            p2.X = pictureBox.Width / 2;
            p2.Y = pictureBox.Height;
            g.DrawLine(p, p1, p2);

            g.DrawEllipse(p, pictureBox.Width / 2 - outRidium, pictureBox.Height / 2 - outRidium, outRidium * 2, outRidium * 2);
            g.DrawEllipse(p, pictureBox.Width / 2 - inRidium, pictureBox.Height / 2 - inRidium, inRidium * 2, inRidium * 2);

            g.Dispose();

            if (saveImage)
            {
                processedFilePath = "..\\..\\capture\\";
                processedFileName = string.Format("{0}_crosshair.png", timeStr);
                boxBmp.Save(processedFilePath + processedFileName, System.Drawing.Imaging.ImageFormat.Png);
                dataCenter.SaveImageInfo(timeStr, orgFileName, processedFileName);
                saveImage = false;
            }
            //pictureBox.Image = middleBmp;

            pictureBox.Image = boxBmp;
            ///---throw new NotImplementedException();
        }

        public void Start()
        {
            try
            {
                cam.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Stop()
        {
            try
            {
                if (cam.IsRunning)
                {
                    cam.Stop();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void PrintAllProperties()
        {
            int minValue = 0;
            int maxValue = 0;
            int stepSize = 1;
            int defaultValue = 0;
            CameraControlFlags controlFlags = 0;
            bool success;

            //for (int i = 0; i <= 6; i++)
            //{

            //    success = cam.GetCameraPropertyRange((CameraControlProperty)i, out minValue, out maxValue, out stepSize, out defaultValue, out controlFlags);
            //}

            success = cam.GetCameraProperty(0, out minValue, out controlFlags);
        }

        public void ZoomIn()
        {
            if (zoom >= maxZoom)
            {
                return;
            }

            zoom++;
            //算出当前取景框中心
            int finderCenterX = finderFrameX + finderFrameWidth / 2;
            int finderCenterY = finderFrameY + finderFrameHeight / 2;
            //算出新取景框宽高
            finderFrameWidth = orgWidth / zoom;
            finderFrameHeight = orgHeight / zoom;
            //算出新取景框左上角
            finderFrameX = finderCenterX - finderFrameWidth / 2;
            finderFrameY = finderCenterY - finderFrameHeight / 2;
            //算出两个圆的半径
            //outRidium = ORG_OUT_RIDIUM / zoom;
            //inRidium = ORG_IN_RIDIUM / zoom;
        }

        public void ZoomOut()
        {
            if (zoom <= 1)
            {
                return;
            }

            zoom--;
            //算出当前取景框中心
            int finderCenterX = finderFrameX + finderFrameWidth / 2;
            int finderCenterY = finderFrameY + finderFrameHeight / 2;
            //算出新取景框宽高
            finderFrameWidth = orgWidth / zoom;
            finderFrameHeight = orgHeight / zoom;
            //算出新取景框左上角
            finderFrameX = finderCenterX - finderFrameWidth / 2;
            if (finderFrameX < 0)
            {
                //int diff = finderFrameX * -1;
                finderFrameX = 0;
                //finderCenterX += diff;
            }
            if (finderFrameX + finderFrameWidth > orgWidth)
            {
                finderFrameX = orgWidth - finderFrameWidth;
            }

            finderFrameY = finderCenterY - finderFrameHeight / 2;
            if (finderFrameY < 0)
            {
                //int diff = finderFrameY * -1;
                finderFrameY = 0;
                //finderFrameY += diff;
            }
            if (finderFrameY + finderFrameHeight > orgHeight)
            {
                finderFrameY = orgHeight - finderFrameHeight;
            }
            //算出两个圆的半径
            //outRidium = ORG_OUT_RIDIUM / zoom;
            //inRidium = ORG_IN_RIDIUM / zoom;
        }

        public void MoveLeft()
        {
            finderFrameX -= panStep;
            if (finderFrameX < 0)
            {
                 finderFrameX = 0;
            }            
        }
        
        public void MoveRight()
        {
            finderFrameX += panStep;
            if (finderFrameX > orgWidth - finderFrameWidth)
            {
                finderFrameX = orgWidth - finderFrameWidth;
            }
        }

        public void MoveUp()
        {
            finderFrameY -= tiltStep;
            if (finderFrameY < 0)
            {
                finderFrameY = 0;
            }
        }

        public void MoveDown()
        {
            finderFrameY += tiltStep;
            if (finderFrameY > orgHeight - finderFrameHeight)
            {
                finderFrameY = orgHeight - finderFrameHeight;
            }
        }

        public void HMirror()
        {
            hMirror = !hMirror;
            finderFrameX = orgWidth - (finderFrameX + finderFrameWidth);
        }

        public void VMirror()
        {
            vMirror = !vMirror;
            finderFrameY = orgHeight - (finderFrameY + finderFrameHeight);
        }

        public void CaptureImage()
        {
            saveImage = true;
        }

        public void SaveSetting()
        {
            if (dataCenter != null)
            {
                string timeStr = DateTime.Now.ToString("yyyyMMddhhmmss");
                dataCenter.SaveCameraSetting(timeStr, zoom, orgWidth, orgHeight, finderFrameX, finderFrameY, hMirror, vMirror, light);
            }
        }


        public void Setting()
        {
            //CameraSettingForm settingForm = new CameraSettingForm();
            CameraSetting settingForm = new CameraSetting(camDeviceIndex);
            settingForm.cameraIndex = camDeviceIndex;
            //if (settingForm.ShowDialog() == DialogResult.OK && settingForm.cameraIndex != camDeviceIndex)
            settingForm.ShowDialog();
            {
                Stop();
                SetCamera(settingForm.cameraIndex);
                Start();
            }
        }
    }
}
