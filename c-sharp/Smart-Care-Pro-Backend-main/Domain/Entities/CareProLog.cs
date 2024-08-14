namespace Domain.Entities
{
   public class CareProLog
   {
      public int Id { get; set; }

      public DateTime? LogDate { get; set; }

      public String Location { get; set; }

      public string ClassName { get; set; }

      public string MethodName { get; set; }

      public string ErrorMessage { get; set; }

      public int? FacilityId { get; set; }

      public Guid? UserId { get; set; }
   }
}