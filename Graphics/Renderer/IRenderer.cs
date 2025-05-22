using System;
using System.Drawing;

namespace Graphics {
    public interface IRenderer {
        Scene Scene { get; set; }

        bool Render(LockedBitmap output);
    }
}
