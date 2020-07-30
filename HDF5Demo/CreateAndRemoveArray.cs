using HDF.PInvoke;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HDF5Demo
{
    class CreateAndRemoveArray : IDisposable
    {
        public CreateAndRemoveArray()
        {
            H5.open();
            fileId = H5F.open(path, H5F.ACC_CREAT | H5F.ACC_RDWR);
            CreateData();
        }

        public void Dispose()
        {
            H5F.close(fileId);
            H5.close();
        }

        private long fileId;
        private string path = "testRemoveArray.h5";

        public void CreateData()
        {
            string groupName = "test";
            string dataName = "data";
            int ret = H5O.exists_by_name(fileId, groupName);
            long grpId = -1;
            if (ret < 0)
            {
                grpId = H5G.create(fileId, groupName);
            }
            else
            {
                grpId = H5G.open(fileId, groupName);
            }

            if (grpId > 0)
            {
                ulong[] dims = { 100 };
                long spaceId = H5S.create_simple(dims.Length, dims, dims);
                long typeId = H5T.copy(H5T.NATIVE_DOUBLE);
                H5T.set_order(typeId, H5T.order_t.LE);
                long datasetId = H5D.create(grpId, dataName, typeId, spaceId);
                typeId = H5T.copy(H5T.NATIVE_DOUBLE);
                var array = new double[100];

                double[] arr = new double[dims[0]];
                int n = 0;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < (int)dims[0]; j++)
                    {
                        arr[j] = j;
                    }
                }

                int size = Marshal.SizeOf(arr[0]) * arr.Length;
                IntPtr buff = Marshal.AllocHGlobal(size);
                try
                {
                    Marshal.Copy(arr, 0, buff, arr.Length);
                    H5D.write(datasetId, H5T.NATIVE_DOUBLE, H5S.ALL, H5S.ALL, H5P.DEFAULT, buff);
                }
                finally
                {
                    Marshal.FreeHGlobal(buff);
                }
                H5D.close(datasetId);
                H5S.close(spaceId);
                H5G.close(grpId);
            }
        }

        public void DeleteObj(string name)
        {
            H5L.delete(fileId, name);
        }
    }
}
