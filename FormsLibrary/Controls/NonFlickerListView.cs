using System;
using System.Drawing;
using System.Windows.Forms;

namespace FormsLibrary.Controls
{
    /// <summary>
    /// An improved list view control with reduced flickering.
    /// </summary>
    [ToolboxBitmap(typeof(ListView))]
    public class NonFlickerListView : ListView
    {
        private ListViewItem[] m_InvalidatedItems;

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.Controls.NonFlickerListView"/> instance.
        /// </summary>
        public NonFlickerListView()
        {
            SetStyles(ExtendedListViewStyles.LVS_EX_DOUBLEBUFFER |
                      ExtendedListViewStyles.LVS_EX_BORDERSELECT);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public virtual void UpdateItems(params ListViewItem[] items)
        {
            m_InvalidatedItems = items;
            Update();
            m_InvalidatedItems = null;
        }

        /// <summary>
        /// Overrides <see cref="System.Windows.Forms.Control.WndProc"/>.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WindowMessage.WM_ERASEBKGND:
                    if (m_InvalidatedItems != null)
                    {
                        PaintNonItemRegion();
                        m.Msg = (int)WindowMessage.WM_NULL;
                    }
                    break;
                case (int)WindowMessage.WM_PAINT:
                    if (m_InvalidatedItems != null)
                    {
                        RECT controlBounds = RECT.FromRectangle(Bounds);
                        NativeMethods.ValidateRect(Utility.GetHandleRef(this), ref controlBounds);
                        using (Region invalidatedRegion = GetInvalidatedRegion())
                        {
                            Invalidate();
                        }
                    }
                    break;
                default:
                    break;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Gets the region of the control excluding all items that do not need to be redrawn.
        /// </summary>
        /// <returns></returns>
        private Region GetInvalidatedRegion()
        {
            Region invalidatedRegion = new Region(Bounds);
            foreach (ListViewItem item in Items)
            {
                bool needsUpdating = Array.Exists(m_InvalidatedItems, arrayItem => item == arrayItem);
                if (!needsUpdating)
                {
                    invalidatedRegion.Exclude(item.Bounds);
                }
            }

            return invalidatedRegion;
        }

        /// <summary>
        /// Paints all parts of the control that do not contain items with the control's back colour.
        /// </summary>
        private void PaintNonItemRegion()
        {
            using (Graphics graphics = Graphics.FromHwnd(Handle))
            using (Region region = new Region(ClientRectangle))
            using (Brush brush = new SolidBrush(BackColor))
            {
                foreach (ListViewItem item in Items)
                {
                    region.Exclude(item.Bounds);
                }

                graphics.FillRegion(brush, region);
            }
        }

        /// <summary>
        /// Sets extended list view styles for the current instance.
        /// </summary>
        /// <param name="styles">The styles to set.</param>
        private void SetStyles(ExtendedListViewStyles styles)
        {
            // Get current styles.
            IntPtr result = NativeMethods.SendMessage(Utility.GetHandleRef(this),
                                                     (int)ListViewMessage.LVM_GETEXTENDEDLISTVIEWSTYLE,
                                                     IntPtr.Zero, IntPtr.Zero);
            ExtendedListViewStyles newStyles = (ExtendedListViewStyles)result | styles;

            NativeMethods.SendMessage(Utility.GetHandleRef(this),
                                     (int)ListViewMessage.LVM_SETEXTENDEDLISTVIEWSTYLE,
                                     IntPtr.Zero, new IntPtr((int)newStyles));
        }
    }
}