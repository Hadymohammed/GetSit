using System.ComponentModel.DataAnnotations;

namespace GetSit.Data.enums
{
    public enum Facility
    {
        [Display(Name="Free Wifi")]
        FreeWifi=1,
        [Display(Name = "Unlimited Coffee")]
        UnlimitedCoffee,
        [Display(Name = "OpenAir View")]
        OpenAirView,
        [Display(Name = "Unlimited Internet")]
        UnlimitedInternet,
    }
}
