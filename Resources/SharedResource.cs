// ملحوظة مهمة: الكلاس ده لازم يفضل في الـ namespace الأساسي "TalyWebsite" مش "TalyWebsite.Resources".
// لو حطيناه في namespace اسمه "Resources" ومطابق لـ ResourcesPath في Program.cs، نظام الترجمة في
// ASP.NET Core بيدور على المسار "TalyWebsite.Resources.Resources.SharedResource" (بيكرر Resources مرتين)
// بدل "TalyWebsite.Resources.SharedResource" الحقيقي، فمايلاقيش ملفات الـ resx وترجع النصوص من غير ترجمة.
namespace TalyWebsite
{
    public class SharedResource
    {
    }
}
