﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using server.Models;

namespace server.Migrations
{
    [DbContext(typeof(HostingContext))]
    partial class HostingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("server.Models.Report", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<bool>("IsSolved")
                        .HasColumnType("bit")
                        .HasColumnName("is_solved");

                    b.Property<int?>("ReportTypeID")
                        .HasColumnType("int");

                    b.Property<int?>("ServerID")
                        .HasColumnType("int");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ReportTypeID");

                    b.HasIndex("ServerID");

                    b.HasIndex("UserID");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("server.Models.ReportType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("ID");

                    b.ToTable("ReportTypes");
                });

            modelBuilder.Entity("server.Models.Server", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IPAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ip_address");

                    b.Property<string>("Processor")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("processor");

                    b.Property<int>("RAMCapacity")
                        .HasColumnType("int")
                        .HasColumnName("ram_capacity");

                    b.Property<int>("SSDCapacity")
                        .HasColumnType("int")
                        .HasColumnName("ssd_capacity");

                    b.HasKey("ID");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("server.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FullName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("full_name");

                    b.Property<string>("Password")
                        .HasMaxLength(72)
                        .HasColumnType("nvarchar(72)")
                        .HasColumnName("password");

                    b.Property<int>("Priority")
                        .HasColumnType("int")
                        .HasColumnName("priority");

                    b.Property<string>("Username")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)")
                        .HasColumnName("username");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("server.Models.UserServer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ServerID")
                        .HasColumnType("int");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ServerID");

                    b.HasIndex("UserID");

                    b.ToTable("UserServers");
                });

            modelBuilder.Entity("server.Models.Report", b =>
                {
                    b.HasOne("server.Models.ReportType", "ReportType")
                        .WithMany("Reports")
                        .HasForeignKey("ReportTypeID");

                    b.HasOne("server.Models.Server", "Server")
                        .WithMany("Reports")
                        .HasForeignKey("ServerID");

                    b.HasOne("server.Models.User", "User")
                        .WithMany("Reports")
                        .HasForeignKey("UserID");

                    b.Navigation("ReportType");

                    b.Navigation("Server");

                    b.Navigation("User");
                });

            modelBuilder.Entity("server.Models.UserServer", b =>
                {
                    b.HasOne("server.Models.Server", "Server")
                        .WithMany("UserServer")
                        .HasForeignKey("ServerID");

                    b.HasOne("server.Models.User", "User")
                        .WithMany("UserServer")
                        .HasForeignKey("UserID");

                    b.Navigation("Server");

                    b.Navigation("User");
                });

            modelBuilder.Entity("server.Models.ReportType", b =>
                {
                    b.Navigation("Reports");
                });

            modelBuilder.Entity("server.Models.Server", b =>
                {
                    b.Navigation("Reports");

                    b.Navigation("UserServer");
                });

            modelBuilder.Entity("server.Models.User", b =>
                {
                    b.Navigation("Reports");

                    b.Navigation("UserServer");
                });
#pragma warning restore 612, 618
        }
    }
}
