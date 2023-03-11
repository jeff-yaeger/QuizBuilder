namespace QuizBuilder.Models.Data;

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    [JsonIgnore]
    public virtual ICollection<UserQuiz> UserQuizzes { get; set; }
}