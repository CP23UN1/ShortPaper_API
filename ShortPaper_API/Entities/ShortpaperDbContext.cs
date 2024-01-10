using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShortPaper_API.Entities;

public partial class ShortpaperDbContext : DbContext
{
    public ShortpaperDbContext()
    {
    }

    public ShortpaperDbContext(DbContextOptions<ShortpaperDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Announcement> Announcements { get; set; }

    public virtual DbSet<AnnouncementFile> AnnouncementFiles { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Committee> Committees { get; set; }

    public virtual DbSet<Shortpaper> Shortpapers { get; set; }

    public virtual DbSet<ShortpaperFile> ShortpaperFiles { get; set; }

    public virtual DbSet<ShortpaperFileType> ShortpaperFileTypes { get; set; }

    public virtual DbSet<ShortpapersHasCommittee> ShortpapersHasCommittees { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<TopicCategoriesHasShortpaper> TopicCategoriesHasShortpapers { get; set; }

    public virtual DbSet<TopicCategory> TopicCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=Jajah36674!;database=shortpaper_db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PRIMARY");

            entity.ToTable("admins");

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
        });

        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.AnnouncementId).HasName("PRIMARY");

            entity.ToTable("announcements");

            entity.Property(e => e.AnnouncementId).HasColumnName("announcement_id");
            entity.Property(e => e.Content)
                .HasMaxLength(500)
                .HasColumnName("content");
            entity.Property(e => e.CreatedDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_datetime");
            entity.Property(e => e.ExpiredDatetime)
                .HasColumnType("datetime")
                .HasColumnName("expired_datetime");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .HasColumnName("image_url");
            entity.Property(e => e.Schedule)
                .HasMaxLength(100)
                .HasColumnName("schedule");
        });

        modelBuilder.Entity<AnnouncementFile>(entity =>
        {
            entity.HasKey(e => new { e.AnnouncementFileId, e.AnnouncementId }).HasName("PRIMARY");

            entity.ToTable("announcement_files");

            entity.HasIndex(e => e.AnnouncementId, "fk_announcement_files_announcements1_idx");

            entity.Property(e => e.AnnouncementFileId)
                .ValueGeneratedOnAdd()
                .HasColumnName("announcement_file_id");
            entity.Property(e => e.AnnouncementId).HasColumnName("announcement_id");
            entity.Property(e => e.CreatedDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_datetime");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .HasColumnName("file_name");
            entity.Property(e => e.FileSize)
                .HasMaxLength(50)
                .HasColumnName("file_size");
            entity.Property(e => e.FileType)
                .HasMaxLength(50)
                .HasColumnName("file_type");

            entity.HasOne(d => d.Announcement).WithMany(p => p.AnnouncementFiles)
                .HasForeignKey(d => d.AnnouncementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_announcement_files_announcements1");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => new { e.CommentId, e.FileId }).HasName("PRIMARY");

            entity.ToTable("comments");

            entity.HasIndex(e => e.FileId, "fk_comments_files1_idx");

            entity.Property(e => e.CommentId)
                .ValueGeneratedOnAdd()
                .HasColumnName("comment_id");
            entity.Property(e => e.FileId).HasColumnName("file_id");
            entity.Property(e => e.CommentContent)
                .HasMaxLength(100)
                .HasColumnName("comment_content");
            entity.Property(e => e.CreatedDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_datetime");
            entity.Property(e => e.UpdatedDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_datetime");
        });

        modelBuilder.Entity<Committee>(entity =>
        {
            entity.HasKey(e => e.CommitteeId).HasName("PRIMARY");

            entity.ToTable("committees");

            entity.Property(e => e.CommitteeId).HasColumnName("committee_id");
            entity.Property(e => e.AlternativeEmail)
                .HasMaxLength(45)
                .HasColumnName("alternative_email");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.IsAdvisor)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_advisor");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(10)
                .HasColumnName("phonenumber");
        });

        modelBuilder.Entity<Shortpaper>(entity =>
        {
            entity.HasKey(e => new { e.ShortpaperId, e.StudentId, e.SubjectId }).HasName("PRIMARY");

            entity.ToTable("shortpapers");

            entity.HasIndex(e => e.StudentId, "fk_shortpapers_students1_idx");

            entity.HasIndex(e => e.SubjectId, "fk_shortpapers_subjects1_idx");

            entity.Property(e => e.ShortpaperId)
                .ValueGeneratedOnAdd()
                .HasColumnName("shortpaper_id");
            entity.Property(e => e.StudentId)
                .HasMaxLength(11)
                .HasColumnName("student_id");
            entity.Property(e => e.SubjectId)
                .HasMaxLength(6)
                .HasColumnName("subject_id");
            entity.Property(e => e.ShortpaperTopic)
                .HasMaxLength(100)
                .HasColumnName("shortpaper_topic");

            entity.HasOne(d => d.Student).WithMany(p => p.Shortpapers)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shortpapers_students1");

            entity.HasOne(d => d.Subject).WithMany(p => p.Shortpapers)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shortpapers_subjects1");
        });

        modelBuilder.Entity<ShortpaperFile>(entity =>
        {
            entity.HasKey(e => new { e.ShortpaperFileId, e.ShortpaperId, e.ShortpaperFileTypeId }).HasName("PRIMARY");

            entity.ToTable("shortpaper_files");

            entity.HasIndex(e => e.ShortpaperFileTypeId, "fk_files_shortpaper_file_types1_idx");

            entity.HasIndex(e => e.ShortpaperId, "fk_files_shortpapers1_idx");

            entity.Property(e => e.ShortpaperFileId)
                .ValueGeneratedOnAdd()
                .HasColumnName("shortpaper_file_id");
            entity.Property(e => e.ShortpaperId).HasColumnName("shortpaper_id");
            entity.Property(e => e.ShortpaperFileTypeId).HasColumnName("shortpaper_file_type_id");
            entity.Property(e => e.CreatedDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_datetime");
            entity.Property(e => e.ExplanationVideo)
                .HasMaxLength(100)
                .HasColumnName("explanation_video");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .HasColumnName("file_name");
            entity.Property(e => e.FileSize)
                .HasMaxLength(50)
                .HasColumnName("file_size");
            entity.Property(e => e.FileType)
                .HasMaxLength(50)
                .HasColumnName("file_type");
            entity.Property(e => e.Remark)
                .HasMaxLength(100)
                .HasColumnName("remark");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'not_approve'")
                .HasColumnType("enum('approved','not_approve')")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_datetime");
            entity.Property(e => e.UpdatedStatusDatetime)
                .HasColumnType("datetime")
                .HasColumnName("updated_status_datetime");

            entity.HasOne(d => d.ShortpaperFileType).WithMany(p => p.ShortpaperFiles)
                .HasForeignKey(d => d.ShortpaperFileTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_files_shortpaper_file_types1");
        });

        modelBuilder.Entity<ShortpaperFileType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PRIMARY");

            entity.ToTable("shortpaper_file_types");

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<ShortpapersHasCommittee>(entity =>
        {
            entity.HasKey(e => new { e.ShortpaperId, e.CommitteeId }).HasName("PRIMARY");

            entity.ToTable("shortpapers_has_committees");

            entity.HasIndex(e => e.CommitteeId, "fk_shortpapers_has_committees_committees1_idx");

            entity.HasIndex(e => e.ShortpaperId, "fk_shortpapers_has_committees_shortpapers_idx");

            entity.Property(e => e.ShortpaperId).HasColumnName("shortpaper_id");
            entity.Property(e => e.CommitteeId).HasColumnName("committee_id");

            entity.HasOne(d => d.Committee).WithMany(p => p.ShortpapersHasCommittees)
                .HasForeignKey(d => d.CommitteeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shortpapers_has_committees_committees1");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PRIMARY");

            entity.ToTable("students");

            entity.Property(e => e.StudentId)
                .HasMaxLength(11)
                .HasColumnName("student_id");
            entity.Property(e => e.AlternativeEmail)
                .HasMaxLength(50)
                .HasColumnName("alternative_email");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(10)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Year)
                .HasMaxLength(6)
                .HasColumnName("year");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PRIMARY");

            entity.ToTable("subjects");

            entity.Property(e => e.SubjectId)
                .HasMaxLength(6)
                .HasColumnName("subject_id");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(50)
                .HasColumnName("subject_name");
        });

        modelBuilder.Entity<TopicCategoriesHasShortpaper>(entity =>
        {
            entity.HasKey(e => new { e.CategoryId, e.ShortpaperId }).HasName("PRIMARY");

            entity.ToTable("topic_categories_has_shortpapers");

            entity.HasIndex(e => e.ShortpaperId, "fk_topic_categories_has_shortpapers_shortpapers1_idx");

            entity.HasIndex(e => e.CategoryId, "fk_topic_categories_has_shortpapers_topic_categories1_idx");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.ShortpaperId).HasColumnName("shortpaper_id");

            entity.HasOne(d => d.Category).WithMany(p => p.TopicCategoriesHasShortpapers)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_topic_categories_has_shortpapers_topic_categories1");
        });

        modelBuilder.Entity<TopicCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("topic_categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
