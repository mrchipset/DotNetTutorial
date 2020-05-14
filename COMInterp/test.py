from win32com.client import Dispatch

dll = Dispatch("COMInterp.COMInterp")
a = 10
dll.SetData(a)
y = dll.GetData()
print(dll.Add(a, y))
