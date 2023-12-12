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

    public virtual DbSet<Announcement> Announcements { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<FileStatus> FileStatuses { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //local
        //=> optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=Jajah36674!;database=shortpaper_db;");
        //server
        => optionsBuilder.UseMySQL("server=192.168.0.2;port=3306;user=admin;password=cp23un1;database=shortpaper_db;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("announcements");

            entity.HasIndex(e => e.FileId, "fk_announcements_files1_idx");

            entity.HasIndex(e => e.AuthorId, "fk_announcements_users1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_datetime");
            entity.Property(e => e.FileId).HasColumnName("file_id");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'draft'")
                .HasColumnType("enum('published','draft','archived')")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .HasColumnName("title");

            entity.HasOne(d => d.Author).WithMany(p => p.Announcements)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_announcements_users1");

            entity.HasOne(d => d.File).WithMany(p => p.Announcements)
                .HasForeignKey(d => d.FileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_announcements_files1");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("comments");

            entity.HasIndex(e => e.ReplyCommentid, "fk_comments_comments1_idx");

            entity.HasIndex(e => e.FileId, "fk_comments_files1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comments).HasColumnName("comments");
            entity.Property(e => e.CreatedDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("\n")
                .HasColumnType("datetime")
                .HasColumnName("created_datetime");
            entity.Property(e => e.FileId).HasColumnName("file_id");
            entity.Property(e => e.ReplyCommentid).HasColumnName("reply_commentid");
            entity.Property(e => e.UpdatedDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_datetime");

            entity.HasOne(d => d.File).WithMany(p => p.Comments)
                .HasForeignKey(d => d.FileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_comments_files1");

            entity.HasOne(d => d.ReplyComment).WithMany(p => p.InverseReplyComment)
                .HasForeignKey(d => d.ReplyCommentid)
                .HasConstraintName("fk_comments_comments1");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("files");

            entity.HasIndex(e => e.StatusId, "fk_files_file_status1_idx");

            entity.HasIndex(e => e.ProjectId, "fk_files_project1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_datetime");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.ExplanationVideo)
                .HasMaxLength(200)
                .HasColumnName("explanation_video");
            entity.Property(e => e.Filename)
                .HasMaxLength(100)
                .HasColumnName("filename");
            entity.Property(e => e.Filesize)
                .HasMaxLength(50)
                .HasColumnName("filesize");
            entity.Property(e => e.Filetype)
                .HasMaxLength(50)
                .HasColumnName("filetype");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_datetime");

            entity.HasOne(d => d.Project).WithMany(p => p.Files)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_files_project1");

            entity.HasOne(d => d.Status).WithMany(p => p.Files)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_files_file_status1");
        });

        modelBuilder.Entity<FileStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PRIMARY");

            entity.ToTable("file_status");

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Article)
                .HasColumnType("bit(1)")
                .HasColumnName("article");
            entity.Property(e => e.BOne)
                .HasColumnType("bit(1)")
                .HasColumnName("b_one");
            entity.Property(e => e.Copyright)
                .HasColumnType("bit(1)")
                .HasColumnName("copyright");
            entity.Property(e => e.Final)
                .HasColumnType("bit(1)")
                .HasColumnName("final");
            entity.Property(e => e.PaperOne)
                .HasColumnType("bit(1)")
                .HasColumnName("paper_one");
            entity.Property(e => e.PaperTwo)
                .HasColumnType("bit(1)")
                .HasColumnName("paper_two");
            entity.Property(e => e.Plagiarism)
                .HasColumnType("bit(1)")
                .HasColumnName("plagiarism");
            entity.Property(e => e.Robbery)
                .HasColumnType("bit(1)")
                .HasColumnName("robbery");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PRIMARY");

            entity.ToTable("projects");

            entity.HasIndex(e => e.StudentId, "fk_project_users1_idx");

            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.CommitteeFirst).HasColumnName("committee_first");
            entity.Property(e => e.CommitteeSecond).HasColumnName("committee_second");
            entity.Property(e => e.CommitteeThird).HasColumnName("committee_third");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.Topic)
                .HasMaxLength(200)
                .HasColumnName("topic");

            entity.HasOne(d => d.Student).WithMany(p => p.Projects)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_project_users1");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("subjects");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SubjectId)
                .HasMaxLength(6)
                .HasColumnName("subject_id");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(100)
                .HasColumnName("subject_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.RegisteredSubjectid, "fk_users_subjects1_idx");

            entity.HasIndex(e => e.ShortpaperSubjectid, "fk_users_subjects2_idx");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .HasColumnName("phone_number");
            entity.Property(e => e.RegisteredSubjectid).HasColumnName("registered_subjectid");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .HasDefaultValueSql("'student'")
                .HasColumnName("role");
            entity.Property(e => e.ShortpaperSubjectid).HasColumnName("shortpaper_subjectid");
            entity.Property(e => e.StudentId)
                .HasMaxLength(11)
                .HasColumnName("student_id");
            entity.Property(e => e.Year)
                .HasMaxLength(4)
                .HasColumnName("year");

            entity.HasOne(d => d.RegisteredSubject).WithMany(p => p.UserRegisteredSubjects)
                .HasForeignKey(d => d.RegisteredSubjectid)
                .HasConstraintName("fk_users_subjects1");

            entity.HasOne(d => d.ShortpaperSubject).WithMany(p => p.UserShortpaperSubjects)
                .HasForeignKey(d => d.ShortpaperSubjectid)
                .HasConstraintName("fk_users_subjects2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
