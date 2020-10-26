using System.Globalization;
using System.Text;
using UnityEngine;

public class Score
{
    private int _value;

    public int Value
    {
        get => _value;
        set
        {
            if (value >= 0)
                _value = value;
        }
    }

    public Score(){}

    public Score(string value)
    {
        if (!int.TryParse(value, NumberStyles.Integer, null, out _value))
        {
            Debug.LogWarning("Can't convert string to int");
        }
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("Score: {0}", _value);
        return stringBuilder.ToString();
    }

    public static Score operator +(Score score, int amount)
    {
        return new Score {Value = score.Value + amount};
    }

    public static Score operator -(Score score, int amount)
    {
        return new Score {Value = score.Value - amount};
    }

    public static Score operator ++(Score score)
    {
        return new Score {Value = score.Value++};
    }

    public static Score operator --(Score score)
    {
        return new Score {Value = score.Value--};
    }

    public static bool operator ==(Score score, int amount)
    {
        return score?.Value == amount;
    }

    public static bool operator !=(Score score, int amount)
    {
        return score?.Value != amount;
    }

    public static bool operator >(Score score, int amount)
    {
        return score?.Value > amount;
    }

    public static bool operator <(Score score, int amount)
    {
        return score?.Value < amount;
    }

    public static implicit operator Score(int value)
    {
        if (value < 0) value = 0;
        return new Score {Value = value};
    }

    public static explicit operator int(Score score)
    {
        return score.Value;
    }
}