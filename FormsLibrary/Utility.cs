using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FormsLibrary
{
    /// <summary>
    /// Contains commonly used properties and methods.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Draws a horizontal rule on the given graphics surface.
        /// </summary>
        /// <param name="surface">The GDI+ surface on which to draw the rule.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rule.</param>
        /// <param name="y">The y-coordiante of the upper-left corner of the rule.</param>
        /// <param name="width">The width of the rule.</param>
        public static void DrawHorizontalRule(Graphics surface, int x, int y, int width)
        {
            DrawHorizontalRule(surface, x, y, width, SystemColors.ButtonShadow, Color.White);
        }

        /// <summary>
        /// Draws a horizontal rule on the given graphics surface.
        /// </summary>
        /// <param name="surface">The GDI+ surface on which to draw the rule.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rule.</param>
        /// <param name="y">The y-coordiante of the upper-left corner of the rule.</param>
        /// <param name="width">The width of the rule.</param>
        /// <param name="topColour">The colour of the top part of the rule.</param>
        /// <param name="bottomColour">The colour of the bottom part of the rule.</param>
        public static void DrawHorizontalRule(Graphics surface, int x, int y, int width, Color topColour, Color bottomColour)
        {
            int left = x;
            int right = width;
            int top = y;
            int bottom = y + 1;
            Pen topPen = new Pen(topColour);
            Pen bottomPen = new Pen(bottomColour);
            surface.DrawLine(topPen, new Point(left, top), new Point(right, top));
            surface.DrawLine(bottomPen, new Point(left, bottom), new Point(right, bottom));
        }

        /// <summary>
        /// Invokes a method on the thread that created the given control.
        /// </summary>
        /// <param name="control">The control on whose thread the method should be called.</param>
        /// <param name="method">The method to be called.</param>
        public static void UIInvoke(Control control, MethodInvoker method)
        {
            if (!control.IsDisposed && control.InvokeRequired)
            {
                control.Invoke(method);
            }
            else
            {
                method();
            }
        }

        /// <summary>
        /// Gets a handle reference for a window.
        /// </summary>
        /// <param name="window">The window for which a handle reference should be created.</param>
        /// <returns>A <see cref="System.Runtime.InteropServices.HandleRef"/> for the given window.</returns>
        public static HandleRef GetHandleRef(IWin32Window window)
        {
            return new HandleRef(window, window.Handle);
        }

        /// <summary>
        /// Causes a form's taskbar button to flash.
        /// </summary>
        /// <param name="targetWindow">The window whose taskbar entry to flash.</param>
        public static void Flash(IWin32Window targetWindow)
        {
            NativeMethods.FlashWindow(GetHandleRef(targetWindow), true);
        }

        /// <summary>
        /// Causes a form's taskbar button to flash.
        /// </summary>
        /// <param name="targetWindowHandle">The handle of the window whose taskbar entry to flash.</param>
        public static void Flash(IntPtr targetWindowHandle)
        {
            NativeMethods.FlashWindow(targetWindowHandle, true);
        }

        /// <summary>
        /// Gets the name of an assembly.
        /// </summary>
        /// <param name="assembly">The assembly whose name to get.</param>
        /// <returns>The name of the assembly.</returns>
        internal static string GetAssemblyName(Assembly assembly)
        {
            AssemblyTitleAttribute attribute = GetAttribute<AssemblyTitleAttribute>(assembly, false);
            return attribute != null ? attribute.Title : string.Empty;
        }

        /// <summary>
        /// Searches for and returns the first attribute of the specified type on the target member.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to search for.</typeparam>
        /// <param name="attributeProvider">The <see cref="System.Reflection.ICustomAttributeProvider"/>
        /// whose attributes are to be searched.</param>
        /// <param name="inherit"><c>true</c> to search the member's inhertiance chain for the attribute; <c>false</c> otherwise.</param>
        /// <returns>The first attribute of the specified type found on the target member; <c>null</c> if none were found.</returns>
        internal static T GetAttribute<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
        {
            return GetAttribute<T>(attributeProvider, inherit, 0);
        }

        /// <summary>
        /// Searches for and returns the first attribute of the specified type on the target member.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to search for.</typeparam>
        /// <param name="attributeProvider">The <see cref="System.Reflection.ICustomAttributeProvider"/>
        /// whose attributes are to be searched.</param>
        /// <param name="inherit"><c>true</c> to search the member's inhertiance chain for the attribute; <c>false</c> otherwise.</param>
        /// <param name="index">The zero-based index of the attribute to return; 0 returns the first attribute found, 1 the second, etc.</param>
        /// <returns>The first attribute of the specified type found on the target member; <c>null</c> if none were found.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The given index was larger than the number of attributes found minus 1.</exception>
        internal static T GetAttribute<T>(ICustomAttributeProvider attributeProvider, bool inherit, int index) where T : Attribute
        {
            return GetAttribute(attributeProvider, typeof(T), inherit, index) as T;
        }

        /// <summary>
        /// Searches for and returns the first attribute of the specified type on the target member.
        /// </summary>
        /// <param name="attributeProvider">The <see cref="System.Reflection.ICustomAttributeProvider"/>
        /// whose attributes are to be searched.</param>
        /// <param name="attributeType">The <see cref="System.Type"/> that represents the type of the attribute to search for.</param>
        /// <param name="inherit"><c>true</c> to search the member's inhertiance chain for the attribute; <c>false</c> otherwise.</param>
        /// <returns>The first attribute of the specified type found on the target member; <c>null</c> if none were found.</returns>
        internal static Attribute GetAttribute(ICustomAttributeProvider attributeProvider, Type attributeType, bool inherit)
        {
            return GetAttribute(attributeProvider, attributeType, inherit, 0);
        }

        /// <summary>
        /// Searches for and returns the first attribute of the specified type on the target member.
        /// </summary>
        /// <param name="attributeProvider">The <see cref="System.Reflection.ICustomAttributeProvider"/>
        /// whose attributes are to be searched.</param>
        /// <param name="attributeType">The <see cref="System.Type"/> that represents the type of the attribute to search for.</param>
        /// <param name="inherit"><c>true</c> to search the member's inhertiance chain for the attribute; <c>false</c> otherwise.</param>
        /// <param name="index">The zero-based index of the attribute to return; 0 returns the first attribute found, 1 the second, etc.</param>
        /// <returns>The first attribute of the specified type found on the target member; <c>null</c> if none were found.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The given index was larger than the number of attributes found minus 1.</exception>
        internal static Attribute GetAttribute(ICustomAttributeProvider attributeProvider, Type attributeType, bool inherit, int index)
        {
            bool hasAttribute = attributeProvider.IsDefined(attributeType, inherit);
            if (!hasAttribute)
            {
                return null;
            }

            object[] attributes = attributeProvider.GetCustomAttributes(attributeType, inherit);
            if (attributes != null)
            {
                return attributes[index] as Attribute;
            }
            else
            {
                return null;
            }
        }
    }
}