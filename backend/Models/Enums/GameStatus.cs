
using System.ComponentModel;

namespace Models.Enums
{
    public enum GameStatus
    {
        [Description("None")]
        None = 0,
        [Description("Away")]
        Away = 1,
        [Description("Pending")]
        Pending = 2,
        [Description("Finished")]
        Finished = 3,
        [Description("Invalid")]
        Invalid = 4,
    }
}
