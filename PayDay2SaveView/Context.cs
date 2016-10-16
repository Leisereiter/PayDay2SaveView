using PaydaySaveEditor.PD2;
using PayDay2SaveView.Formatter;

namespace PayDay2SaveView
{
    public class Context
    {
        public HeistDb HeistDb { set; get; }
        public CmdLineHelper Args { get; set; }
        public IFormatter Formatter { get; set; }
        public SaveFile SaveFile { get; set; }
    }
}