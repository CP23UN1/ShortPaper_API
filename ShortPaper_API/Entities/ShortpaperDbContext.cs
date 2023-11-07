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

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Committee> Committees { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<FileStatus> FileStatuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=Jajah36674!;database=shortpaper_db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("comments");

            entity.HasIndex(e => e.UserId, "fk_comments_users1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comments)
                .HasMaxLength(500)
                .HasColumnName("comments");
            entity.Property(e => e.CreatedDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("\n")
                .HasColumnType("datetime")
                .HasColumnName("created_datetime");
            entity.Property(e => e.UpdatedDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_datetime");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_comments_users1");
        });

        modelBuilder.Entity<Committee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("committees");

            entity.HasIndex(e => e.StudentId, "fk_committees_users1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstCommitteeId).HasColumnName("first_committee_id");
            entity.Property(e => e.SecondCommitteeId).HasColumnName("second_committee_id");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.ThirdCommitteeId).HasColumnName("third_committee_id");

            entity.HasOne(d => d.Student).WithMany(p => p.Committees)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_committees_users1");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("files");

            entity.HasIndex(e => e.StatusId, "fk_files_file_status1_idx");

            entity.HasIndex(e => e.UserId, "fk_files_users_idx");

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
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_datetime");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Status).WithMany(p => p.Files)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_files_file_status1");

            entity.HasOne(d => d.User).WithMany(p => p.Files)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_files_users");
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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

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
            entity.Property(e => e.ProjectSubjectId)
                .HasMaxLength(100)
                .HasColumnName("project_subject_id");
            entity.Property(e => e.ProjectTopic)
                .HasMaxLength(200)
                .HasColumnName("project_topic");
            entity.Property(e => e.RegisteredSubjectId)
                .HasMaxLength(100)
                .HasColumnName("registered_subject_id");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .HasColumnName("role");
            entity.Property(e => e.StudentId)
                .HasMaxLength(11)
                .HasColumnName("student_id");
            entity.Property(e => e.Year)
                .HasMaxLength(4)
                .HasColumnName("year");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
