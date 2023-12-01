using System;
using MoverCandidateTest.WatchHands.Utility;

namespace MoverCandidateTest.WatchHands.Service;

public class CalculateLeastAngleService: ICalculateLeastAngleService
{
    public double CalculateLeastAngle(DateTime dateTimeFromRequest)
    {
        var dateTime = dateTimeFromRequest.ParseRequestModelDateTime();
        
        //I could simplify this by using direct values instead of multiple operations, but I've chosen this approach for better readability.
        var hourAngle = dateTime.Hour * 360 / 12 + dateTime.Minute * 30 / 60 + dateTime.Second * 0.5 / 60;
        var minuteAngle = dateTime.Minute * 360 / 60 + dateTime.Second * 6 / 60 ;

        var angleDifference = Math.Abs(hourAngle - minuteAngle);
        var leastAngle = Math.Min(angleDifference, 360 - angleDifference);

        return Math.Round(leastAngle);
    }
}