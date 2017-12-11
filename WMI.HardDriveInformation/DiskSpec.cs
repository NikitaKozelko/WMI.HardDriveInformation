using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WMI.HardDriveInformation
{
    internal class DiskSpec
    {
        private String PhysicalName { get; set; }
        private String DiskName { get; set; }
        private String DiskModel { get; set; }
        private String DiskInterface { get; set; }
        private String SerialNumber { get; set; }
        private UInt64 Size { get; set; }
        private UInt32 MediaSignature { get; set; }
        private String MediaStatus { get; set; }
        private String DriveName { get; set; }
        private String DriveId { get; set; }
        private bool DriveCompressed { get; set; }
        private UInt32 DriveType { get; set; }
        private String FileSystem { get; set; }
        private UInt64 NonFreeSpace { get; set; }
        private UInt64 FreeSpace { get; set; }
        private UInt64 TotalSpace { get; set; }
        private UInt64 HdNonFreeSpace { get; set; }
        private UInt64 HdFreeSpace { get; set; }
        private UInt64 HdTotalSpace { get; set; }
        private UInt32 DriveMediaType { get; set; }
        private String VolumeName { get; set; }
        private String VolumeSerial { get; set; }

        public int MyProperty { get; set; }

        public void PrintSpec()
        {
            var driveQuery = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject d in driveQuery.Get())
            {
                //var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", d.Path.RelativePath);
                //var partitionQuery = new ManagementObjectSearcher(partitionQueryText);

                //foreach (ManagementObject p in partitionQuery.Get())
                //{
                //    var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition",
                //        p.Path.RelativePath);
                //    var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
                string strComputer = ".";
                ManagementScope namespaceScope = new ManagementScope("\\\\" + strComputer + "\\ROOT\\CIMV2");
                ObjectQuery diskQuery = new ObjectQuery("SELECT * FROM Win32_LogicalDisk ");
                ManagementObjectSearcher logicalDriveQuery = new ManagementObjectSearcher(namespaceScope, diskQuery);

                foreach (ManagementObject ld in logicalDriveQuery.Get())
                {
                    GetDiskInfo(ld);
                    PrintLocalDiskSpecs();
                    Console.ReadKey();
                }
                //}
                GetHardDriveInfo(d);
                PrintMainSpecs();
                Console.ReadLine();
            }
        }

        public void PrintMainSpecs()
        {
            Console.WriteLine("PhysicalName: {0}", PhysicalName);
            Console.WriteLine("DiskName: {0}", DiskName);
            Console.WriteLine("DiskModel: {0}", DiskModel);
            Console.WriteLine("DiskInterface: {0}", DiskInterface);
            Console.WriteLine("SerialNumber: {0}", SerialNumber.Trim());
            Console.WriteLine("FreeSpace: {0} bytes", HdFreeSpace);
            Console.WriteLine("Size: {0}", Size);
            Console.WriteLine("MediaSignature: {0}", MediaSignature);
            Console.WriteLine("MediaStatus: {0}", MediaStatus);
        }

        public void PrintLocalDiskSpecs()
        {
            Console.WriteLine("DriveName: {0}", DriveName);
            Console.WriteLine("DriveCompressed: {0}", DriveCompressed);
            Console.WriteLine("DriveType: {0}", DriveType);
            Console.WriteLine("FileSystem: {0}", FileSystem);
            Console.WriteLine("FreeSpace: {0} bytes", FreeSpace);
            Console.WriteLine("Non-FreeSpace: {0} bytes", NonFreeSpace);
            Console.WriteLine("TotalSpace: {0} bytes", TotalSpace);
            Console.WriteLine("DriveMediaType: {0}", DriveMediaType);
            Console.WriteLine("VolumeName: {0}", VolumeName);
            Console.WriteLine("VolumeSerial: {0}", VolumeSerial);

            Console.WriteLine();
        }

        void GetHardDriveInfo(ManagementObject d)
        {
            PhysicalName = Convert.ToString(d.Properties["Name"].Value);
            DiskName = Convert.ToString(d.Properties["Caption"].Value);
            DiskModel = Convert.ToString(d.Properties["Model"].Value);

            HdTotalSpace = Convert.ToUInt64(d.Properties["Size"].Value); // in bytes
            HdNonFreeSpace = HdTotalSpace - HdFreeSpace;
            DiskInterface = Convert.ToString(d.Properties["InterfaceType"].Value);
            SerialNumber = Convert.ToString(d.Properties["SerialNumber"].Value); // bool
            Size = Convert.ToUInt64(d.Properties["Size"].Value); // Fixed hard disk media
            MediaSignature = Convert.ToUInt32(d.Properties["Signature"].Value); // int32
            MediaStatus = Convert.ToString(d.Properties["Status"].Value); // OK
        }

        void GetDiskInfo(ManagementObject ld)
        {
            DriveName = Convert.ToString(ld.Properties["Name"].Value);
            DriveCompressed = Convert.ToBoolean(ld.Properties["Compressed"].Value);
            DriveType = Convert.ToUInt32(ld.Properties["DriveType"].Value);
            FileSystem = Convert.ToString(ld.Properties["FileSystem"].Value); // NTFS
            FreeSpace = Convert.ToUInt64(ld.Properties["FreeSpace"].Value); // in bytes
            TotalSpace = Convert.ToUInt64(ld.Properties["Size"].Value); // in bytes
            NonFreeSpace = TotalSpace - FreeSpace;
            DriveMediaType = Convert.ToUInt32(ld.Properties["MediaType"].Value); // c: 12
            VolumeName = Convert.ToString(ld.Properties["VolumeName"].Value); // System
            VolumeSerial = Convert.ToString(ld.Properties["VolumeSerialNumber"].Value);
            HdFreeSpace += FreeSpace;
        }
    }
}