using System;

public static class DateTimeExtension
{
    /// <summary>
    /// 週の開始日を返す
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static DateTime FirstDayOfWeek(this DateTime source)
    {
        return source.AddDays(DayOfWeek.Sunday - source.DayOfWeek);
    }

    /// <summary>
    /// 週の最終日を返す
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static DateTime LastDayOfWeek(this DateTime source)
    {
        return source.AddDays(DayOfWeek.Saturday - source.DayOfWeek);
    }
}