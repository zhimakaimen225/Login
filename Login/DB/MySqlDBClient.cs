using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Gd
{
    class MySqlDBClient : DBClient
    {
        public const String CONNECT_STRING = "server=localhost;userid=root;password=123456;database=jddatabase";

        MySqlConnection connection;

        public MySqlDBClient()
        {
            Open();
        }

        public bool Open()
        {
            try
            {
                connection = new MySqlConnection(CONNECT_STRING);
                connection.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show("open database failed");
                return false;
            }

            return true;
        }

        public void Close()
        {
            connection.Close();
        }

        public override List<UserInfo> GetAllUserInfo()
        {
            List<UserInfo> userInfoList = new List<UserInfo>();

            try
            {
                string sql = "select * from LoginTable";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read() == true)
                {
                    UserInfo curInfo = new UserInfo();
                    curInfo.userName = reader[0].ToString();
                    curInfo.password = reader[1].ToString();
                    curInfo.grade = Convert.ToInt32(reader[2].ToString());
                    curInfo.id = Convert.ToInt32(reader[3].ToString());
                    curInfo.number = Convert.ToInt32(reader[4].ToString());
                    curInfo.address = reader[5].ToString();
                    curInfo.tel = reader[6].ToString();
                    curInfo.company = reader[7].ToString();
                    curInfo.summary = reader[8].ToString();

                    userInfoList.Add(curInfo);
                }
                reader.Close();

                return userInfoList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
         
        public override string GetPassword(int userNumber)
        {
            try
            {
                string sql = string.Format("select LoginPassword from LoginTable where LoginNumber = {0}", userNumber);
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                string password;
                if (reader.Read() == true)
                {
                    password = reader.GetString(0);
                    reader.Close();
                    return password;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public override bool UpdateUserInfo(UserInfo userInfo)
        {
            try
            {
                string sql = string.Format("Update LoginTable Set LoginUserName = \"{0}\", LoginPassword = \"{1}\", LoginGrade = {2}, LoginNumber = {3}, LoginAddress = \"{4}\", LoginTel = \"{5}\", LoginCompany = \"{6}\", LoginAddress = \"{7}\" where LoginId = {8}",
                                        userInfo.userName, userInfo.password, userInfo.grade, userInfo.number, userInfo.address, userInfo.tel, userInfo.company, userInfo.summary, userInfo.id);

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        
        }

        public override List<SampleUnitInfo> GetAllSampleUnitInfo()
        {
            List<SampleUnitInfo> sampleUnitInfoList = new List<SampleUnitInfo>();

            try
            {
                string sql = "select * from SampleUnitTable";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read() == true)
                {
                    SampleUnitInfo curInfo = new SampleUnitInfo();
                    curInfo.id = Convert.ToInt32(reader[0]);
                    curInfo.name = reader[1].ToString();
                    curInfo.address = reader[2].ToString();
                    curInfo.tel = reader[3].ToString();
                    curInfo.summary = reader[4].ToString();

                    sampleUnitInfoList.Add(curInfo);
                }
                reader.Close();

                return sampleUnitInfoList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public override bool SaveImageInfo(ImageInfo imageInfo)
        {
            try
            {
                string sql = string.Format("insert into ImageCaptureTable (ImageCaptureTime, ImageCaptureOrgName, ImageCaptureProcessedName) values('{0}','{1}','{2}')",
                                            imageInfo.captureTime, imageInfo.orgName, imageInfo.processedName);
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public override bool SaveCameraSetInfo(CameraSetInfo cameraSetInfo)
        {
            try
            {
                string sql = string.Format("insert into CameraSetTable (CameraSetTime, CameraSetZoom, CameraSetOrgWidth, CameraSetOrgHeight, CameraSetHorizontalLocation, CameraSetVerticalLocation, CameraSetHorizontalMirror, CameraSetVerticalMirror, CameraSetLight) values('{0}', {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})",
                                            cameraSetInfo.timeStr, cameraSetInfo.zoom, cameraSetInfo.orgWidth, cameraSetInfo.orgHeight, cameraSetInfo.finderFrameX, cameraSetInfo.finderFrameY, cameraSetInfo.hMirror, cameraSetInfo.vMirror, cameraSetInfo.light);
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
