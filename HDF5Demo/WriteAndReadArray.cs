using HDF.PInvoke;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HDF5Demo
{
    class WriteAndReadArray : IDisposable
    {
        public WriteAndReadArray()
        {
            H5.open();
            fileId = H5F.open(path, H5F.ACC_CREAT | H5F.ACC_RDWR);
           
        }

        public void Dispose()
        {
            H5F.close(fileId);
            H5.close();
        }

        public bool WriteData(string groupName, string name, List<double> x, List<double> y)
        {
            
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
                ulong[] dims = { 2, (ulong)x.Count };
                long spaceId = H5S.create_simple(dims.Length, dims, dims);
                long typeId = H5T.copy(H5T.NATIVE_DOUBLE);
                H5T.set_order(typeId, H5T.order_t.LE);
                long datasetId = H5D.create(grpId, name, typeId, spaceId);
                typeId = H5T.copy(H5T.NATIVE_DOUBLE);
                var array = new double[2, x.Count];
               
                for (int col = 0; col < x.Count; col++)
                {
                    array[0, col] = x[col];
                    array[1, col] = y[col];
                }
                double[] arr = new double[2 * x.Count];
                int n = 0;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < x.Count; j++)
                    {
                        arr[n++] = array[i, j];
                    }
                }

                int size = Marshal.SizeOf(arr[0]) * arr.Length;
                IntPtr buff = Marshal.AllocHGlobal(size);
                try
                {
                    Marshal.Copy(arr, 0, buff, 2 * x.Count);
                    H5D.write(datasetId, H5T.NATIVE_DOUBLE, H5S.ALL, H5S.ALL, H5P.DEFAULT, buff);
                }finally
                {
                    Marshal.FreeHGlobal(buff);
                }
                H5D.close(datasetId);
                H5S.close(spaceId);
                H5G.close(grpId);
                return true;
            }
               
            return false;
        }


        public bool ReadData(string groupName, string name, ref List<double> x, ref List<double> y)
        {
            int ret = H5O.exists_by_name(fileId, groupName);
            
            if (ret < 0)
            {
                return false;
            }

            long grpId = H5G.open(fileId, groupName);
            if (grpId > 0)
            {
                long datasetId = H5D.open(grpId, name);
                if (datasetId < 0)
                {
                    return false;
                }
                long typeId = H5D.get_type(datasetId);
                long spaceId = H5D.get_space(datasetId);


                if (spaceId < 0)
                {
                    return false;
                }
                int n = H5S.get_simple_extent_ndims(spaceId);
                ulong[] dims = new ulong[n];
                ulong[] max_dims = new ulong[n];
                H5S.get_simple_extent_dims(spaceId, dims, max_dims);

                int all = 1;
                for (int i = 0; i < n; i++)
                {
                    all *= (int)dims[i];
                }
                double type_val = 0;
                int size = Marshal.SizeOf(type_val) * all;
                IntPtr buff = Marshal.AllocHGlobal(size);
                try
                {
                    int read = H5D.read(datasetId, H5T.NATIVE_DOUBLE, H5S.ALL, H5S.ALL, H5P.DEFAULT, buff);
                    double[] val = new double[all];
                    Marshal.Copy(buff, val, 0, all);
                    x.Clear();
                    y.Clear();
                    for (int i=0;i<all/2;i++)
                    {
                        x.Add(val[i]);
                        y.Add(val[i + all / 2]);
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(buff);
                }
                H5D.close(datasetId);
                H5S.close(spaceId);
                H5G.close(grpId);
                return true;
            }

            return false;
        }

        private long fileId;
        private string path = "testWriteArrat.h5";

    }
}
