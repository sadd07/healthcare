using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Healthcare.Models;

public class ScheduleDto
{
    public int Id { get; set; }

    public required int DayId { get; set; }

    public required String From { get; set; }

    public required String To { get; set; }

    public string FormattedSchedule => 
        $"{DayId} {From:hh:mm tt} - {To:hh:mm tt}";

}