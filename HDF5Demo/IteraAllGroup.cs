using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HDF.PInvoke;

namespace HDF5Demo
{
    class IteraAllGroup : IDisposable
    {
        public IteraAllGroup()
        {
            H5.open();
            CreateGroups();
        }

        public void Iter()
        {
            long fileId = H5F.open(path, H5F.ACC_RDONLY);
            long grpId = H5G.open(fileId, "/");
            ulong idx = 0;
            int val = 0;
            IntPtr op_data = new IntPtr(val);
            H5L.iterate(grpId, H5.index_t.NAME, H5.iter_order_t.INC, ref idx, GroupIterCallback, op_data);
        }
        public void Dispose()
        {
            H5.close();
        }
        
        private const string path = "testIter.h5";
        private void CreateGroups()
        {
            long fileId = H5F.create(path, H5F.ACC_TRUNC);
            H5G.create(fileId, "grp1");
            H5G.create(fileId, "grp2");
            H5G.create(fileId, "grp1/subgrp1");
            ulong[] size = { 10, 10 };
            long dspace = H5S.create_simple(size.Length, size, size);
            long dtype = H5T.copy(H5T.NATIVE_FLOAT);
            long dset = H5D.create(fileId, "dataset", dtype, dspace);
            H5F.close(fileId);
        }

        private int GroupIterCallback(long group, IntPtr name, ref H5L.info_t info, IntPtr op_data)
        {
            string strName = Marshal.PtrToStringAnsi(name);
            byte[] byteName = Encoding.ASCII.GetBytes(strName);
            Console.WriteLine(strName);
            Console.WriteLine(group);
            ulong idx = 0;
            long objId = H5O.open(group, byteName);
            if (objId > 0)
            {
                Console.WriteLine(objId);
                H5O.info_t oInfo = new H5O.info_t();
                if (H5O.get_info(objId, ref oInfo) >= 0)
                {
                    if (oInfo.type == H5O.type_t.GROUP)
                    {
                        long grpId = H5G.open(group, byteName);

                        if (grpId > 0)
                        {
                            H5L.iterate(grpId, H5.index_t.NAME, H5.iter_order_t.INC, ref idx, GroupIterCallback, op_data);
                            H5G.close(grpId);
                        }
                    }
                }
                H5O.close(objId);
            }
           
            return (int)H5.H5IterationResult.CONT;
        }
    }
}
