using System;
using MoverCandidateTest.WatchHands.Model;

namespace MoverCandidateTest.WatchHands.Utility;

public static class Extensions
{
    public static WatchHandsDateTime ParseRequestModelDateTime(this DateTime dateTime) =>
        new(dateTime.Hour > 11 ? dateTime.Hour - 12 : dateTime.Hour, dateTime.Minute, dateTime.Second);
}