
namespace QuanLyHopDongVaKySo.CLIENT.Models
{
    public class SignatureModel
    {
        public string SignObject { get; set; }
        public int SignZIndex { get; set; }
        public int SignPenSize { get; set; }
        public string SignPenColor { get; set; }
        public string SignStartMessage { get; set; }
        public string SignErrorMessage { get; set; }
        public string SignSuccessMessage { get; set; }
        public string SignBackGroundImage { get; set; }
        public string SignRefreshImage { get; set; }
        public string SignPenCursor { get; set; }
        public int SignWidth { get; set; }
        public int SignHeight { get; set; }
        public int SignRequiredPoints { get; set; }
        public string SignBackColor { get; set; }
        public string SignBorderColor { get; set; }
        public string SignBorderStyle { get; set; }
        public int SignBorderWidth { get; set; }
        public bool SignSmooth { get; set; }
        public bool SignTransparent { get; set; }
        public float SignImageScaleFactor { get; set; }
    }

}
