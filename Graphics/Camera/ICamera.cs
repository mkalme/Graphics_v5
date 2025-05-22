namespace Graphics {
    public interface ICamera {
        int WidthPx { get; set; }
        int HeightPx { get; set; }

        Ray[,] Cast();
    }
}
