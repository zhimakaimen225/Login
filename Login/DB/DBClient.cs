using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gd
{

    abstract class DBClient
    {
        public abstract List<UserInfo> GetAllUserInfo();
        public abstract string GetPassword(int userNumber);
        public abstract bool UpdateUserInfo(UserInfo userInfo);

        public abstract List<SampleUnitInfo> GetAllSampleUnitInfo();

        public abstract bool SaveImageInfo(ImageInfo imageInfo);

        public abstract bool SaveCameraSetInfo(CameraSetInfo cameraSetInfo);

    }
}
