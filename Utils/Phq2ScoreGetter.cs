namespace UplLogEmail.Utils;

public static class Phq2ScoreGetter
{
    public static string GetScore(
        int? totalScore,
        string interviewConducted,
        string interest,
        string down
    )
    {
        if (interviewConducted == "0")
        {
            return "99";
        }

        int interestValue = ParseScore(interest);
        int downValue = ParseScore(down);

        if (interestValue < 2 && downValue < 2)
        {
            return $"{interestValue + downValue}";
        }

        return totalScore.ToString() ?? "";
    }

    private static int ParseScore(string score)
    {
        if (!int.TryParse(score?[..1], out int value) || value == -1)
        {
            return 0;
        }
        return value;
    }
}
