﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Routine.APi.Data;

namespace Routine.APi.Migrations
{
    [DbContext(typeof(RoutineDbContext))]
    partial class RoutineDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Routine.APi.Entities.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Introduction")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c0e917f9-1640-4ffd-8dfe-d3946913a180"),
                            Introduction = "Great Company",
                            Name = "Microsoft"
                        },
                        new
                        {
                            Id = new Guid("543d0978-084e-43f5-82f8-6417ffce0c0c"),
                            Introduction = "Don't be evil",
                            Name = "Google"
                        },
                        new
                        {
                            Id = new Guid("50817877-f6f8-4161-9926-20817ac3bd08"),
                            Introduction = "fubao Company",
                            Name = "Alipapa"
                        });
                });

            modelBuilder.Entity("Routine.APi.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmployeeNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ca268a19-0f39-4d8b-b8d6-5bace54f8027"),
                            CompanyId = new Guid("c0e917f9-1640-4ffd-8dfe-d3946913a180"),
                            DateOfBirth = new DateTime(1955, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "M001",
                            FirstName = "William",
                            Gender = 1,
                            LastName = "Gates"
                        },
                        new
                        {
                            Id = new Guid("265348d2-1276-4ada-ae33-4c1b8348edce"),
                            CompanyId = new Guid("c0e917f9-1640-4ffd-8dfe-d3946913a180"),
                            DateOfBirth = new DateTime(1998, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "M024",
                            FirstName = "Kent",
                            Gender = 1,
                            LastName = "Back"
                        },
                        new
                        {
                            Id = new Guid("47b70abc-98b8-4fdc-b9fa-5dd6716f6e6b"),
                            CompanyId = new Guid("543d0978-084e-43f5-82f8-6417ffce0c0c"),
                            DateOfBirth = new DateTime(1986, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "G003",
                            FirstName = "Mary",
                            Gender = 0,
                            LastName = "King"
                        },
                        new
                        {
                            Id = new Guid("059e2fcb-e5a4-4188-9b46-06184bcb111b"),
                            CompanyId = new Guid("543d0978-084e-43f5-82f8-6417ffce0c0c"),
                            DateOfBirth = new DateTime(1977, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "G007",
                            FirstName = "Kevin",
                            Gender = 1,
                            LastName = "Richardson"
                        },
                        new
                        {
                            Id = new Guid("a868ff18-3398-4598-b420-4878974a517a"),
                            CompanyId = new Guid("50817877-f6f8-4161-9926-20817ac3bd08"),
                            DateOfBirth = new DateTime(1964, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "A001",
                            FirstName = "Jack",
                            Gender = 1,
                            LastName = "Ma"
                        },
                        new
                        {
                            Id = new Guid("2c3bb40c-5907-4eb7-bb2c-7d62edb430c9"),
                            CompanyId = new Guid("50817877-f6f8-4161-9926-20817ac3bd08"),
                            DateOfBirth = new DateTime(1997, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeNo = "A201",
                            FirstName = "Lorraine",
                            Gender = 0,
                            LastName = "Shaw"
                        });
                });

            modelBuilder.Entity("Routine.APi.Entities.Employee", b =>
                {
                    b.HasOne("Routine.APi.Entities.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
