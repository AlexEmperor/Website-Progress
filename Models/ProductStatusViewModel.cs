namespace Website_Progress.Models
{
    public enum ProductStatusViewModel
    {
        [Display(Name = "Разработка")]
        Development,

        [Display(Name = "Проектирование")]
        Project,

        [Display(Name = "Производство")]
        Manufacturing,

        [Display(Name = "Тестируется")]
        Testing,

        [Display(Name = "В наличии")]
        Production
    }
}
