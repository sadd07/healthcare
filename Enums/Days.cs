using System.ComponentModel.DataAnnotations;

namespace Healthcare.Enums;

public enum Days
{
    [Display(Name = "SU")]
    Sunday = 1,
    
    [Display(Name = "MO")]
    Monday = 2,
    
    [Display(Name = "TU")]
    Tuesday = 3,
    
    [Display(Name = "WE")]
    Wednesday = 4,
    
    [Display(Name = "TH")]
    Thursday = 5,
    
    [Display(Name = "FR")]
    Friday = 6,
    
    [Display(Name = "SA")]
    Saturday = 7
}
