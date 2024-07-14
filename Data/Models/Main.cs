using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolyglotAPI.Data.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Course>? Courses { get; set; }
    }

    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(Language))]
        public int LanguageId { get; set; }
        public virtual Language? Language { get; set; }

        public virtual ICollection<Module>? Modules { get; set; }
    }

    public class Module
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Order { get; set; }

        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public virtual Course? Course { get; set; }

        public virtual ICollection<Lesson>? Lessons { get; set; }
        public virtual ICollection<Progress>? Progresses { get; set; }
        public virtual ICollection<Rating>? Ratings { get; set; }
    }

    public class Lesson
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public ContentType ContentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? Order { get; set; }

        [ForeignKey(nameof(Module))]
        public int ModuleId { get; set; }
        public virtual Module? Module { get; set; }
        
        public virtual ICollection<Question>? Questions { get; set; }
        public virtual ICollection<Rating>? Ratings { get; set; }
    }

    public enum QuestionType
    {
        Match = 1,
        Listen = 2,
        Translate =3,
        FillInTheBlank =4,
        TrueFalse =5,
        MultipleChoice =6,
        ReorderSentence =7
    }

    public class Question
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public QuestionType QuestionType { get; set; }
        public Guid Answer { get; set; }
        public int? Order { get; set; }

        [ForeignKey(nameof(Lesson))]
        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; }

        [ForeignKey(nameof(Audio))]
        public int? AudioId { get; set; }
        public virtual Audio Audio { get; set; }

        [ForeignKey(nameof(Image))]
        public int? ImageId { get; set; }
        public virtual Image Image { get; set; }

        public virtual ICollection<Option> Options { get; set; }
    }

    public class Option
    {
        [Key]
        public Guid Id { get; set; }
        public string Text { get; set; }

        [ForeignKey(nameof(Audio))]
        public int? AudioId { get; set; }
        public virtual Audio Audio { get; set; }

        [ForeignKey(nameof(Image))]
        public int? ImageId { get; set; }
        public virtual Image Image { get; set; }

        [ForeignKey(nameof(Question))]
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }

    public enum ContentType
    {
        Course = 1,
        Module = 2,
        Lesson = 3,
        Question = 4,
        Video = 5,
        Audio = 6,
        Quiz = 7,
    }

    public class Rating
    {
        [Key]
        public int RatingId { get; set; }

        [ForeignKey(nameof(UserProfile))]
        public Guid UserId { get; set; }
        public UserProfile User { get; set; }

        public ContentType ContentType { get; set; }

        public int ContentId { get; set; }
        public bool IsThumbsUp { get; set; }
        public DateTime RatedAt { get; set; }

        [ForeignKey(nameof(Lesson))]
        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; }
    }

    public class Progress
    {
        [Key]
        public int ProgressId { get; set; }

        [ForeignKey(nameof(UserProfile))]
        public Guid UserId { get; set; }
        public UserProfile User { get; set; }

        [ForeignKey(nameof(Course))]
        public int? CourseId { get; set; }
        public Course Course { get; set; }

        [ForeignKey(nameof(Module))]
        public int? ModuleId { get; set; }
        public Module Module { get; set; }

        [ForeignKey(nameof(Lesson))]
        public int? LessonId { get; set; }
        public Module Lesson { get; set; }

        public int CompletionPercentage { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    

    public enum SubscriptionPlan
    {
        Basic,
        AdFree,
        Premium
    }

    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; }

        [ForeignKey(nameof(UserProfile))]
        public Guid UserId { get; set; }
        public UserProfile User { get; set; }

        public SubscriptionPlan PlanName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class Audio
    {
        [Key]
        public int Id { get; set; }
        public string UrlKey { get; set; }
        public string Transcript { get; set; }
        public string EnglishTranslation { get; set; }
        public string SentenceId { get; set; }
        public string LanguageCode { get; set; }

        public DateTime UploadedAt { get; set; }
    }

    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string UrlKey { get; set; } // UrlKey is FileName
        public string Context { get; set; }
        public string Description { get; set; }
        public string EnhancedDescription { get; set; }
        public DateTime UploadedAt { get; set; }
    }

    public class Badge
    {
        [Key]
        public int BadgeId { get; set; }

        [ForeignKey(nameof(UserProfile))]
        public Guid UserId { get; set; }
        public UserProfile User { get; set; }

        public string BadgeName { get; set; }
        public DateTime AwardedAt { get; set; }
    }

    public class Notification
    {
        [Key]
        public Guid NotificationId { get; set; }

        [ForeignKey(nameof(UserProfile))]
        public Guid UserId { get; set; }
        public UserProfile User { get; set; }

        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }

    public class Flashcard
    {
        [Key]
        public int FlashcardId { get; set; }

        [ForeignKey(nameof(UserProfile))]
        public Guid UserId { get; set; }
        public UserProfile User { get; set; }

        public string Front { get; set; }
        public string Back { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UserGeneratedContent
    {
        [Key]
        public Guid ContentId { get; set; }

        [ForeignKey(nameof(UserProfile))]
        public Guid UserId { get; set; }
        public UserProfile User { get; set; }

        public string Title { get; set; }
        public ContentType ContentType { get; set; } // e.g., lesson, exercise, quiz
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
