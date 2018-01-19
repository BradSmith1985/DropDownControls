// Painting Controls With Fade Animations Using the Buffered Paint API
// Bradley Smith - 2011/10/23 (updated 2014/03/12)

using System;
using System.Drawing;
using System.Runtime.InteropServices;

internal static class Interop {

    [DllImport("uxtheme")]
    public static extern IntPtr BufferedPaintInit();

    [DllImport("uxtheme")]
    public static extern IntPtr BufferedPaintUnInit();

    [DllImport("uxtheme")]
    public static extern IntPtr BeginBufferedAnimation(
        IntPtr hwnd, 
        IntPtr hdcTarget, 
        ref Rectangle rcTarget, 
        BP_BUFFERFORMAT dwFormat, 
        IntPtr pPaintParams, 
        ref BP_ANIMATIONPARAMS pAnimationParams, 
        out IntPtr phdcFrom, 
        out IntPtr phdcTo
    );

    [DllImport("uxtheme")]
    public static extern IntPtr EndBufferedAnimation(IntPtr hbpAnimation, bool fUpdateTarget);

    [DllImport("uxtheme")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool BufferedPaintRenderAnimation(IntPtr hwnd, IntPtr hdcTarget);

    [DllImport("uxtheme")]
    public static extern IntPtr BufferedPaintStopAllAnimations(IntPtr hwnd);

	[StructLayout(LayoutKind.Sequential)]
	internal struct BP_ANIMATIONPARAMS {
		public Int32 cbSize, dwFlags;
		public BP_ANIMATIONSTYLE style;
		public Int32 dwDuration;
	}

	internal enum BP_BUFFERFORMAT {
		BPBF_COMPATIBLEBITMAP,
		BPBF_DIB,
		BPBF_TOPDOWNDIB,
		BPBF_TOPDOWNMONODIB
	}

	[Flags]
	internal enum BP_ANIMATIONSTYLE {
		BPAS_NONE = 0,
		BPAS_LINEAR = 1,
		BPAS_CUBIC = 2,
		BPAS_SINE = 3
	}
}