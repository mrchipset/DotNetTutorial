using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace COMInterp
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("78446B84-E254-4452-8AFE-CC16B7AF53D7")]
    public interface IComInterp
    {
        int GetData();
        void SetData(int data);
        int Add(int a, int b);
    }

    [ComVisible(true)]
    [Guid("FDC8090A-2A5E-491E-BF56-71A328BB7648")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("COMInterp.COMInterp")]
    public class ComInterp : IComInterp
    {
        private int data;
        public int GetData()
        {
            return data;
        }

        public void SetData(int data)
        {
            this.data = data;
        }

        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}
