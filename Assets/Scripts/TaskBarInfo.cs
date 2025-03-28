using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TaskbarInfo : MonoBehaviour
{
    // SHAppBarMessage ���� ����� ����ü
    private const int ABM_GETTASKBARPOS = 0x00000005;

    [StructLayout(LayoutKind.Sequential)]
    public struct APPBARDATA
    {
        public uint cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public uint uEdge;
        public RECT rc;
        public int lParam;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left, top, right, bottom;
    }

    [DllImport("shell32.dll", CallingConvention = CallingConvention.StdCall)]
    static extern uint SHAppBarMessage(uint dwMessage, ref APPBARDATA pData);

    public static int taskBarHeight = 0;

    void Awake()
    {
        APPBARDATA abd = new APPBARDATA();
        abd.cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA));

        uint ret = SHAppBarMessage(ABM_GETTASKBARPOS, ref abd);
        if (ret != 0)
        {
            RECT rect = abd.rc;
            int taskbarWidth = rect.right - rect.left;
            int taskbarHeight = rect.bottom - rect.top;
            taskBarHeight = taskbarHeight;

            // �۾� ǥ������ ȭ���� ��� ��ġ�� �ִ����� ���� ���̳� �ʺ� �ǹ̰� �޶��� �� ����
            Debug.Log($"Taskbar Rect: left={rect.left}, top={rect.top}, right={rect.right}, bottom={rect.bottom}");
            Debug.Log($"Taskbar Width: {taskbarWidth}, Taskbar Height: {taskbarHeight}");
        }
        else
        {
            Debug.LogWarning("�۾� ǥ���� ������ ������ �� �����ϴ�.");
        }
    }
}


