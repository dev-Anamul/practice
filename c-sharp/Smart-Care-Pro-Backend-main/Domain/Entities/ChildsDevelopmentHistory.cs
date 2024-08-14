using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Utilities.Constants;

/*
 * Created by   : Stephan
 * Date created : 25.12.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Entities
{
   /// <summary>
   /// ChildsDevelopmentHistory entity.
   /// </summary>
   public class ChildsDevelopmentHistory : EncounterBaseModel
   {
      /// <summary>
      /// Primary key of the table ChildsDevelopmentHistories.
      /// </summary>
      [Key]
      public Guid InteractionId { get; set; }

      /// <summary>
      /// Feeding history of the child.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [StringLength(1000)]
      [Display(Name = "Feeding History")]
      public string FeedingHistory { get; set; }

      /// <summary>
      /// Social smile.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "Smallint")]
      [Display(Name = "Social Smile")]
      public int SocialSmile { get; set; }

      /// <summary>
      /// Head holding.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "Smallint")]
      [Display(Name = "Head Holding")]
      public int HeadHolding { get; set; }

      /// <summary>
      /// Turn toward sound origin.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "Smallint")]
      [Display(Name = "Turn Toward Sound Origin")]
      public int TurnTowardSoundOrigin { get; set; }

      /// <summary>
      /// Grasp toy.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "Smallint")]
      [Display(Name = "Grasp Toy")]
      public int GraspToy { get; set; }

      /// <summary>
      /// Follow objects with eyes.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Follow objects with eyes")]
      public int FollowObjectsWithEyes { get; set; }

      /// <summary>
      /// Rolls over.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Rolls over")]
      public int RollsOver { get; set; }

      /// <summary>
      /// Babbles.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Babbles")]
      public int Babbles { get; set; }

      /// <summary>
      /// Takes objects to mouth.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Takes objects to mouth")]
      public int TakesObjectsToMouth { get; set; }

      /// <summary>
      /// Repeats syllables.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Repeats syllables")]
      public int RepeatsSyllables { get; set; }

      /// <summary>
      /// Moves objects.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Moves objects")]
      public int MovesObjects { get; set; }

      /// <summary>
      /// Plays peek-a-boo.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Plays peek-a-boo")]
      public int PlaysPeekaBoo { get; set; }

      /// <summary>
      /// Responds to own name.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Response to own name")]
      public int RespondsToOwnName { get; set; }

      /// <summary>
      /// Takes steps with support.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Takes steps with support")]
      public int TakesStepsWithSupport { get; set; }

      /// <summary>
      /// Picks up small objects.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Picks up small objects")]
      public int PicksUpSmallObjects { get; set; }

      /// <summary>
      /// Imitates simple gestures.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Imitates simple gestures")]
      public int ImitatesSimpleGestures { get; set; }

      /// <summary>
      /// Says two or three words.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Says two or three words")]
      public int SaysTwoToThreeWords { get; set; }

      /// <summary>
      /// Sitting.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "Smallint")]
      [Display(Name = "Sitting")]
      public int Sitting { get; set; }

      /// <summary>
      /// Standing.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "Smallint")]
      [Display(Name = "Standing")]
      public int Standing { get; set; }

      /// <summary>
      /// Walking.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "Smallint")]
      [Display(Name = "Walking")]
      public int Walking { get; set; }

      /// <summary>
      /// Talking.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      [Column(TypeName = "Smallint")]
      [Display(Name = "Talking")]
      public int Talking { get; set; }

      /// <summary>
      /// Drinks from cup.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Drinks from cup")]
      public int DrinksFromCup { get; set; }

      /// <summary>
      /// Says Seven or ten words.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Says Seven or ten words")]
      public int SaysSevenToTenWords { get; set; }

      /// <summary>
      /// Points to body parts.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Points to body parts")]
      public int PointsToBodyParts { get; set; }

      /// <summary>
      /// Starts to run.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Starts to run")]
      public int StartsToRun { get; set; }

      /// <summary>
      /// Points picture on request.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Points picture on request")]
      public int PointsPictureOnRequest { get; set; }

      /// <summary>
      /// Sings.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Sings")]
      public int Sings { get; set; }

      /// <summary>
      /// Build tower with box.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Build tower with box")]
      public int BuildTowerWithBox { get; set; }

      /// <summary>
      /// Jumps and run.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Jumps and run")]
      public int JumpsAndRun { get; set; }

      /// <summary>
      /// Begins to dress and undress.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Begins to dress and undress")]
      public int BeginsToDressAndUndress { get; set; }

      /// <summary>
      /// Groups similar objects.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Groups similar objects")]
      public int GroupsSimilarObjects { get; set; }

      /// <summary>
      /// Plays with other children.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Plays with other children")]
      public int PlaysWithOtherChildren { get; set; }

      /// <summary>
      /// Says first name and short story.
      /// </summary>
      [Column(TypeName = "Smallint")]
      [Display(Name = "Says first name and short story")]
      public int SaysFirstNameAndShortStory { get; set; }

      /// <summary>
      /// Foreign Key. Primary key of the table Clients.
      /// </summary>
      [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
      public Guid ClientId { get; set; }

      /// <summary>
      /// Navigation property.
      /// </summary>
      [ForeignKey("ClientId")]
      [JsonIgnore]
      public virtual Client Client { get; set; }
   }
}