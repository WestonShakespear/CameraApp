using DirectShowLib;
using System.Runtime.InteropServices;

namespace shakespear.cameraapp.utilities;

public static class Utilities
{
    public static Platform GetPlatform()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Platform.WINDOWS;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return Platform.LINUX;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return Platform.OSX;
        }
        else
        {
            return Platform.OTHER;
        }
    }

    public static string ListPlatform()
    {
        return string.Format("Running on: {0}", RuntimeInformation.OSDescription);
    }

    public static CameraDetails[]? GetCameras()
    {
        CameraDetails[]? details = GetPlatform() switch
        {
            Platform.WINDOWS => GetDSHOWCameras(),
            Platform.LINUX => GetV4L2Cameras(),
            _ => null
        };

        return details;
    }

    private static CameraDetails[] GetDSHOWCameras()
    {
        DsDevice[] captureDevices;
        captureDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

        CameraDetails[] details = new CameraDetails[captureDevices.Length];
        int i = 0;

        foreach (DsDevice device in captureDevices)
        {
            details[i] = new CameraDetails(i++, device.Name, device.ClassID.ToString());
        }

        return details;
    }

    private static CameraDetails[] GetV4L2Cameras()
    {
        CameraDetails[] details = new CameraDetails[1];

        details[0] = new CameraDetails(-100, "", "");

        return details;
    }


    public static int IndexFromCatAndList(CameraDetails[] _details, string _catName)
    {
        for (int i = 0; i < _details.Length; i++)
        {
            if (_details[i].CheckCatName(_catName))
            {
                return _details[i].IndexFromCatName(_catName);
            }
        }
        return -1;
    }



    public struct CameraDetails
    {
        public int index;
        public string name, guid;

        public bool inUse = false;

        public CameraDetails(int _index, string _name, string _guid)
        {
            this.index = _index;
            this.name = _name;
            this.guid = _guid;
        }

        public string CatName()
        {
            return (this.index.ToString() + " " + this.name);
        }

        public bool CheckCatName(string _catName)
        {
            return ( this.IndexFromCatName(_catName) == this.index );
        }

        public int IndexFromCatName(string _catName)
        {
            return Int32.Parse(_catName.Substring(0, 1));
        }
    }

    public enum Platform{
        WINDOWS,
        LINUX,
        OSX,
        OTHER
    }
}