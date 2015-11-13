namespace Apintec.Modules.Cameras
{
    public class CameraInstanceInfo
    {
        public string Vendor { get; internal set; }
        public uint Index { get; internal set; }
        public int Sequence { get; internal set; }
        public string IpAddress { get; internal set; }
        public bool IsInstance { get; internal set; }
        public Camera Instance { get; internal set; }
        public CameraInstanceInfo(string vendor, uint index, int sequence, string ipAddr)
        {
            Vendor = vendor;
            Index = index;
            Sequence = sequence;
            IsInstance = false;
            IpAddress = ipAddr;
        }
    }
}
