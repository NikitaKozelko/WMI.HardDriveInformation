using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMI.HardDriveInformation
{
    class Program
    {
        static void Main(string[] args)
        {
            DiskSpec props = new DiskSpec();
            props.PrintSpec();
        }
    }
}
