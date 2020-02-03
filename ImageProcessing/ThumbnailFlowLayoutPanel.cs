using System;
using System.Windows.Forms;
using System.Drawing;

namespace ImageProcessing
{
    public class ThumbnailFlowLayoutPanel : FlowLayoutPanel
    {
        protected override Point ScrollToControl(Control activeControl)
        {
            return this.AutoScrollPosition;
        }
    }
}
