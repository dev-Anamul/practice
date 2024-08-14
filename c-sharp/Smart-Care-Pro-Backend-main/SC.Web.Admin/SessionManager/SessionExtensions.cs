using Domain.Dto;
using Domain.Entities;
using Newtonsoft.Json;

namespace SC.Web.Admin.SessionManager
{
   internal static class SessionKey
   {
      public static readonly string CurrentUser = nameof(CurrentUser);
      public static readonly string UserID = nameof(UserID);
      public static readonly string CurrentAdmin = nameof(CurrentAdmin);
   }

   public static class SessionExtensions
   {
      public static void SetObjectAsJson(this ISession session, string key, object value)
      {
         session.SetString(key, JsonConvert.SerializeObject(value));
      }

      public static T? GetObjectFromJson<T>(this ISession session, string key)
      {
         var value = session.GetString(key);

         return string.IsNullOrEmpty(value) ? default : JsonConvert.DeserializeObject<T>(value);
      }

      public static void SetCurrentUsers(this ISession session, UserLoginSuccessDto CurrentUser)
      {
         session.SetObjectAsJson(SessionKey.CurrentUser, CurrentUser);
      }

      public static UserLoginSuccessDto? GetCurrentUsers(this ISession session)
      {
         return session.GetObjectFromJson<UserLoginSuccessDto?>(SessionKey.CurrentUser);
      }

      public static void SetUserID(this ISession session, Guid? guid)
      {
         session.SetString(SessionKey.UserID, guid.ToString());
      }

      public static string? GetUserID(this ISession session)
      {
         return session.GetString(SessionKey.UserID);
      }

      public static void SetCurrentAdmin(this ISession session, UserAccount currentAdmin)
      {
         session.SetObjectAsJson(SessionKey.CurrentAdmin, currentAdmin);
      }

      public static UserAccount? GetCurrentAdmin(this ISession session)
      {
         return session.GetObjectFromJson<UserAccount?>(SessionKey.CurrentAdmin);
      }
   }
}