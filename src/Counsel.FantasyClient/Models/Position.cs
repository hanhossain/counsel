using System;

namespace Counsel.FantasyClient.Models
{
    public enum Position
    {
        Defense,
        DefensiveBack,
        DefensiveLine,
        Kicker,
        Linebacker,
        Quarterback,
        RunningBack,
        TightEnd,
        WideReceiver
    }

    public static class PositionHelper
    {
        public static Position FromString(string position)
        {
            switch (position)
            {
                case "DEF":
                    return Position.Defense;

                case "DB":
                    return Position.DefensiveBack;

                case "DL":
                    return Position.DefensiveLine;

                case "K":
                    return Position.Kicker;

                case "LB":
                    return Position.Linebacker;

                case "QB":
                    return Position.Quarterback;

                case "RB":
                    return Position.RunningBack;

                case "TE":
                    return Position.TightEnd;

                case "WR":
                    return Position.WideReceiver;

                default:
                    throw new ArgumentOutOfRangeException(nameof(position));
            }
        }

        public static string ToString(Position position)
        {
            switch (position)
            {
                case Position.Defense:
                    return "DEF";

                case Position.DefensiveBack:
                    return "DB";

                case Position.DefensiveLine:
                    return "DL";

                case Position.Kicker:
                    return "K";

                case Position.Linebacker:
                    return "LB";

                case Position.Quarterback:
                    return "QB";

                case Position.RunningBack:
                    return "RB";

                case Position.TightEnd:
                    return "TE";

                case Position.WideReceiver:
                    return "WR";

                default:
                    throw new ArgumentOutOfRangeException(nameof(position));
            }
        }
    }
}
