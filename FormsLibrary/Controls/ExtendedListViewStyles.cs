using System;

namespace FormsLibrary.Controls
{
    /// <summary>
    /// Specifies the extended list view style message codes.
    /// </summary>
    [Flags]
    internal enum ExtendedListViewStyles
    {
        LVS_EX_GRIDLINES = 0x00000001,
        LVS_EX_SUBITEMIMAGES = 0x00000002,
        LVS_EX_CHECKBOXES = 0x00000004,
        LVS_EX_TRACKSELECT = 0x00000008,
        LVS_EX_HEADERDRAGDROP = 0x00000010,
        LVS_EX_FULLROWSELECT = 0x00000020,
        LVS_EX_ONECLICKACTIVATE = 0x00000040,
        LVS_EX_TWOCLICKACTIVATE = 0x00000080,
        LVS_EX_FLATSB = 0x00000100,
        LVS_EX_REGIONAL = 0x00000200,
        LVS_EX_INFOTIP = 0x00000400,
        LVS_EX_UNDERLINEHOT = 0x00000800,
        LVS_EX_UNDERLINECOLD = 0x00001000,
        LVS_EX_MULTIWORKAREAS = 0x00002000,
        LVS_EX_LABELTIP = 0x00004000,
        LVS_EX_BORDERSELECT = 0x00008000,
        LVS_EX_DOUBLEBUFFER = 0x00010000,
        LVS_EX_HIDELABELS = 0x00020000,
        LVS_EX_SINGLEROW = 0x00040000,
        LVS_EX_SNAPTOGRID = 0x00080000,
        LVS_EX_SIMPLESELECT = 0x00100000  
    }
}