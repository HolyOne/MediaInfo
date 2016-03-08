using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

public class ListViewNF : System.Windows.Forms.ListView
{
    public ListViewNF()
    {
        //Activate double buffering
        this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

        //Enable the OnNotifyMessage event so we get a chance to filter out 
        // Windows messages before they get to the form's WndProc
        this.SetStyle(ControlStyles.EnableNotifyMessage, true);
    }

    /*
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct LVHITTESTINFO
    {
        public System.Drawing.Point pt;
        public int flags;
        public int iItem;
        public int iSubItem;
    }
    private const int LVM_FIRST = 0x1000;
    private const int LVM_GETITEMRECT = LVM_FIRST + 14;
    private const int LVM_GETCOLUMNWIDTH = LVM_FIRST + 29;
    private const int LVM_SUBITEMHITTEST = LVM_FIRST + 57; 
   private const int  WM_LBUTTONUP        =            0x0202;
 

    [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
    private static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, IntPtr lParam);
    // overloaded for wParam type
    [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam);

    private const int LVHT_EX_GROUP_HEADER = 0x10000000;
    
    private static Point LParamToPoint(IntPtr lParam)
    {

        return new Point(LowWord(lParam), HighWord(lParam));

    }

    private static int LowWord(IntPtr param)
    {

        uint lp = (uint)param;



        return (UInt16)lp;

    }

    private static int HighWord(IntPtr param)
    {

        uint lp = (uint)param;

        return (int)(lp >> 16);

    }


    [FlagsAttribute]
    enum LVHITTESTFLAGS
    {
        LVHT_NOWHERE = 0x0001,
        LVHT_ONITEMICON = 0x0002,
        LVHT_ONITEMLABEL = 0x0004,
        LVHT_ONITEMSTATEICON = 0x0008,
        LVHT_ONITEM = (LVHT_ONITEMICON | LVHT_ONITEMLABEL | LVHT_ONITEMSTATEICON),

        LVHT_ABOVE = 0x0008,
        LVHT_BELOW = 0x0010,
        LVHT_TORIGHT = 0x0020,
        LVHT_TOLEFT = 0x0040
    }


    protected override void WndProc(ref Message m)
    {
        //the link uses WM_LBUTTONDOWN but I found that it doesn't work
        if (m.Msg != 0x203) base.WndProc(ref m);

      //  base.WndProc(ref m);
    }
    */

    /*
    private bool checkFromDoubleClick = false;

    protected override void OnItemCheck(ItemCheckEventArgs ice)
    {
        if (this.checkFromDoubleClick)
        {
            ice.NewValue = ice.CurrentValue;
            this.checkFromDoubleClick = false;
        }
        else
            base.OnItemCheck(ice);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        // Is this a double-click?
        if ((e.Button == MouseButtons.Left) && (e.Clicks > 1))
         
            this.checkFromDoubleClick = true;
      
        base.OnMouseDown(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        this.checkFromDoubleClick = false;
        base.OnKeyDown(e);
    }
    */
    protected override void OnNotifyMessage(Message m)
    {
        //Filter out the WM_ERASEBKGND message
        if (m.Msg != 0x14)
        {
            base.OnNotifyMessage(m);
        }
    }
}
