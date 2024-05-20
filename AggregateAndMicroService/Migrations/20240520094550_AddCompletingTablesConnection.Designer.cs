﻿// <auto-generated />
using System;
using System.Collections.Generic;
using AggregateAndMicroService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AggregateAndMicroService.Migrations
{
    [DbContext(typeof(LearningContext))]
    [Migration("20240520094550_AddCompletingTablesConnection")]
    partial class AddCompletingTablesConnection
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AggregateAndMicroService.Domain.Course.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ComplexProperty<Dictionary<string, object>>("StageCount", "AggregateAndMicroService.Domain.Course.Course.StageCount#StageCount", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasDefaultValue(0)
                                .HasColumnName("StageCount");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Status", "AggregateAndMicroService.Domain.Course.Course.Status#CourseStatus", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("Status");
                        });

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("AggregateAndMicroService.Domain.Course.Stage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<Guid?>("Previous")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ComplexProperty<Dictionary<string, object>>("Duration", "AggregateAndMicroService.Domain.Course.Stage.Duration#StageDuration", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<TimeSpan>("Value")
                                .HasColumnType("interval")
                                .HasColumnName("Duration");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Type", "AggregateAndMicroService.Domain.Course.Stage.Type#StageType", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("Type");
                        });

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Stages");
                });

            modelBuilder.Entity("AggregateAndMicroService.Domain.CourseProgress.CourseCompleting", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.ComplexProperty<Dictionary<string, object>>("Progress", "AggregateAndMicroService.Domain.CourseProgress.CourseCompleting.Progress#Progress", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasDefaultValue(0)
                                .HasColumnName("Progress");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("StagesCountData", "AggregateAndMicroService.Domain.CourseProgress.CourseCompleting.StagesCountData#StagesCountData", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("CompletedStages")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasDefaultValue(0)
                                .HasColumnName("CompletedStages");

                            b1.Property<int>("TotalStages")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasDefaultValue(0)
                                .HasColumnName("TotalStages");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Status", "AggregateAndMicroService.Domain.CourseProgress.CourseCompleting.Status#CompleteStatus", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("Status");
                        });

                    b.HasKey("Id");

                    b.ToTable("CourseCompleting");
                });

            modelBuilder.Entity("AggregateAndMicroService.Domain.CourseProgress.StageCourseCompleting", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseCompletingId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("StageId")
                        .HasColumnType("uuid");

                    b.ComplexProperty<Dictionary<string, object>>("StageProgress", "AggregateAndMicroService.Domain.CourseProgress.StageCourseCompleting.StageProgress#StageProgress", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasDefaultValue(0)
                                .HasColumnName("StageProgress");
                        });

                    b.HasKey("Id");

                    b.HasIndex("CourseCompletingId");

                    b.ToTable("StageCourseCompletings");
                });

            modelBuilder.Entity("AggregateAndMicroService.Domain.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<int>("CompletedCoursesCount")
                        .HasColumnType("integer");

                    b.Property<int>("CourseInProgressCount")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AggregateAndMicroService.Domain.Course.Stage", b =>
                {
                    b.HasOne("AggregateAndMicroService.Domain.Course.Course", "Course")
                        .WithMany("Stages")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("AggregateAndMicroService.Domain.CourseProgress.StageCourseCompleting", b =>
                {
                    b.HasOne("AggregateAndMicroService.Domain.CourseProgress.CourseCompleting", "CourseCompleting")
                        .WithMany("StageCourseCompletings")
                        .HasForeignKey("CourseCompletingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CourseCompleting");
                });

            modelBuilder.Entity("AggregateAndMicroService.Domain.Course.Course", b =>
                {
                    b.Navigation("Stages");
                });

            modelBuilder.Entity("AggregateAndMicroService.Domain.CourseProgress.CourseCompleting", b =>
                {
                    b.Navigation("StageCourseCompletings");
                });
#pragma warning restore 612, 618
        }
    }
}
