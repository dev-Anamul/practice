using System.Runtime.Serialization;

namespace Domain.Entities
{
   [DataContract]
   public class Log
   {
      [DataMember(Name = "Id")]
      public int Id { get; set; }

      [DataMember(Name = "Message")]
      public string Message { get; set; }

      [DataMember(Name = "MessageTemplate")]
      public string MessageTemplate { get; set; }

      [DataMember(Name = "Level")]
      public string Level { get; set; }

      [DataMember(Name = "TimeStamp")]
      public DateTime CreateDate { get; set; }

      [DataMember(Name = "Exception")]
      public string Exception { get; set; }

      [DataMember(Name = "Properties")]
      public string Properties { get; set; }
   }
}