using System;
using MoverCandidateTest.WatchHands.Extension;

namespace MoverCandidateTest.WatchHands.Service;

public class CalculateLeastAngleService: ICalculateLeastAngleService
{
    public double CalculateLeastAngle(DateTime dateTimeFromRequest)
    {
        var dateTime = dateTimeFromRequest.ParseRequestModelDateTime();
        
        // I could make easier writing right values instead of multiple operations, but in this way is more readable
        var hourAngle = dateTime.Hour * 360 / 12 + dateTime.Minute * 30 / 60 + dateTime.Second * 0.5 / 60;
        var minuteAngle = dateTime.Minute * 360 / 60 + dateTime.Second * 6 / 60 ;

        var angleDifference = Math.Abs(hourAngle - minuteAngle);
        var leastAngle = Math.Min(angleDifference, 360 - angleDifference);

        return Math.Round(leastAngle);
    }
}