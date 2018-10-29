using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gd
{
    public enum DBType
    {
        DBType_mysql,
        DBType_access
    }
    public class DataCenter
    {
        DBClient dbClient;

        public static DBType dbType = DBType.DBType_mysql;

        public DataCenter()
        {
            InitDB();
        }

        void InitDB()
        {
            switch (dbType)
            {
                case DBType.DBType_mysql:
                    dbClient = new MySqlDBClient();
                    break;
                case DBType.DBType_access:
                    dbClient = null;
                    break;
            }

            return;
        }

        public List<UserInfo> GetAllUserInfo()
        {
            if (dbClient != null)
            {
                return dbClient.GetAllUserInfo();
            }
            else
            {
                return null;
            }
        }

        public string GetPassword(int userNumber)
        {
            if (dbClient != null)
            {
                return dbClient.GetPassword(userNumber);
            }
            else
            {
                return null;
            }
        }

        public bool UpdateUserInfo(UserInfo userInfo)
        {
            if (dbClient != null)
            {
                return dbClient.UpdateUserInfo(userInfo);
            }
            else
            {
                return false;
            }
        }

        public List<SampleUnitInfo> GetAllSampleInfo()
        {
            if (dbClient != null)
            {
                return dbClient.GetAllSampleUnitInfo();
            }
            else
            {
                return null;
            }
        }

        public bool SaveImageInfo(string timeStr, string orgName, string processedName)
        {
            ImageInfo imageInfo = new ImageInfo();
            imageInfo.captureTime = timeStr;
            imageInfo.orgName = orgName;
            imageInfo.processedName = processedName;

            if (dbClient != null)
            {
                return dbClient.SaveImageInfo(imageInfo);
            }
            else
            {
                return false;
            }
        }

        public bool SaveCameraSetting(string timeStr, 
                                         int zoom, 
                                         int orgWidth,  
                                         int orgHeight,
                                         int finderFrameX,
                                         int finderFrameY,
                                         bool hMirror,
                                         bool vMirror,
                                         bool light)
        {
            CameraSetInfo info = new CameraSetInfo();
            info.timeStr = timeStr;
            info.zoom = zoom;
            info.orgWidth = orgWidth;
            info.orgHeight = orgHeight;
            info.finderFrameX = finderFrameX;
            info.finderFrameY = finderFrameY;
            info.hMirror = hMirror;
            info.vMirror = vMirror;
            info.light = light;

            if (dbClient != null)
            {
                return dbClient.SaveCameraSetInfo(info);
            }
            else
            {
                return false;
            }
        }
    }
}
