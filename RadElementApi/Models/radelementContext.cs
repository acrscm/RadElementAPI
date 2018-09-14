﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RadElementApi.Models
{
    public partial class radelementContext : DbContext
    {
        public radelementContext()
        {
        }

        public radelementContext(DbContextOptions<radelementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Code> Code { get; set; }
        public virtual DbSet<Coderef> Coderef { get; set; }
        public virtual DbSet<Codesystem> Codesystem { get; set; }
        public virtual DbSet<Editor> Editor { get; set; }
        public virtual DbSet<Element> Element { get; set; }
        public virtual DbSet<Elementset> Elementset { get; set; }
        public virtual DbSet<Elementsetref> Elementsetref { get; set; }
        public virtual DbSet<Elementvalue> Elementvalue { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Imageref> Imageref { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;user id=snair;password=sujith123;persistsecurityinfo=True;database=radelement");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Code>(entity =>
            {
                entity.ToTable("code");

                entity.HasIndex(e => e.Code1)
                    .HasName("value");

                entity.HasIndex(e => e.System)
                    .HasName("scheme");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AccessionDate)
                    .HasColumnName("accessionDate")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Code1)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasColumnType("varchar(24)");

                entity.Property(e => e.Display)
                    .IsRequired()
                    .HasColumnName("display")
                    .HasColumnType("tinytext");

                entity.Property(e => e.System)
                    .IsRequired()
                    .HasColumnName("system")
                    .HasColumnType("varchar(24)");
            });

            modelBuilder.Entity<Coderef>(entity =>
            {
                entity.ToTable("coderef");

                entity.HasIndex(e => e.CodeId)
                    .HasName("codeID");

                entity.HasIndex(e => e.ElementId)
                    .HasName("elementID");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("mediumint(11)");

                entity.Property(e => e.CodeId)
                    .HasColumnName("codeID")
                    .HasColumnType("mediumint(9)");

                entity.Property(e => e.ElementId)
                    .HasColumnName("elementID")
                    .HasColumnType("mediumint unsigned");

                entity.Property(e => e.ValueCode)
                    .HasColumnName("valueCode")
                    .HasColumnType("varchar(64)");
            });

            modelBuilder.Entity<Codesystem>(entity =>
            {
                entity.HasKey(e => e.Abbrev);

                entity.ToTable("codesystem");

                entity.Property(e => e.Abbrev)
                    .HasColumnName("abbrev")
                    .HasColumnType("varchar(12)");

                entity.Property(e => e.CodeUrl)
                    .IsRequired()
                    .HasColumnName("codeURL")
                    .HasColumnType("tinytext");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("text");

                entity.Property(e => e.Oid)
                    .IsRequired()
                    .HasColumnName("oid")
                    .HasColumnType("text");

                entity.Property(e => e.SystemUrl)
                    .IsRequired()
                    .HasColumnName("systemURL")
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<Editor>(entity =>
            {
                entity.ToTable("editor");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(12)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<Element>(entity =>
            {
                entity.ToTable("element");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("mediumint unsigned");

                entity.Property(e => e.Definition)
                    .IsRequired()
                    .HasColumnName("definition")
                    .HasColumnType("text");

                entity.Property(e => e.Editor)
                    .IsRequired()
                    .HasColumnName("editor")
                    .HasColumnType("varchar(12)");

                entity.Property(e => e.Instructions)
                    .IsRequired()
                    .HasColumnName("instructions")
                    .HasColumnType("text");

                entity.Property(e => e.MaxCardinality)
                    .HasColumnName("maxCardinality")
                    .HasColumnType("smallint(6)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.MinCardinality)
                    .HasColumnName("minCardinality")
                    .HasColumnType("smallint(6)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasColumnName("question")
                    .HasColumnType("text");

                entity.Property(e => e.References)
                    .IsRequired()
                    .HasColumnName("references")
                    .HasColumnType("text");

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasColumnName("shortName")
                    .HasColumnType("varchar(24)");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source")
                    .HasColumnType("text");

                entity.Property(e => e.StatusDate)
                    .HasColumnName("statusDate")
                    .HasColumnType("date");

                entity.Property(e => e.StepValue).HasColumnName("stepValue");

                entity.Property(e => e.Synonyms)
                    .IsRequired()
                    .HasColumnName("synonyms")
                    .HasColumnType("text");

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasColumnName("unit")
                    .HasColumnType("text");

                entity.Property(e => e.ValueMax).HasColumnName("valueMax");

                entity.Property(e => e.ValueMin).HasColumnName("valueMin");

                entity.Property(e => e.ValueSet)
                    .HasColumnName("valueSet")
                    .HasColumnType("text");

                entity.Property(e => e.ValueSize)
                    .HasColumnName("valueSize")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("varchar(8)");

                entity.Property(e => e.VersionDate)
                    .HasColumnName("versionDate")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Elementset>(entity =>
            {
                entity.ToTable("elementset");

                entity.HasIndex(e => e.ParentId)
                    .HasName("parent");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ContactName)
                    .IsRequired()
                    .HasColumnName("contactName")
                    .HasColumnType("text");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("text");

                entity.Property(e => e.ParentId)
                    .HasColumnName("parentID")
                    .HasColumnType("mediumint(9)");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<Elementsetref>(entity =>
            {
                entity.ToTable("elementsetref");

                entity.HasIndex(e => e.ElementId)
                    .HasName("elementID");

                entity.HasIndex(e => e.ElementSetId)
                    .HasName("projectID");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("mediumint(9)");

                entity.Property(e => e.ElementId)
                    .HasColumnName("elementID")
                    .HasColumnType("mediumint(9)");

                entity.Property(e => e.ElementSetId)
                    .HasColumnName("elementSetID")
                    .HasColumnType("mediumint(9)");
            });

            modelBuilder.Entity<Elementvalue>(entity =>
            {
                entity.ToTable("elementvalue");

                entity.HasIndex(e => e.ElementId)
                    .HasName("elementID");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("mediumint(9)");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Definition)
                    .HasColumnName("definition")
                    .HasColumnType("text");

                entity.Property(e => e.ElementId)
                    .HasColumnName("elementID")
                    .HasColumnType("smallint(6)");

                entity.Property(e => e.Images)
                    .HasColumnName("images")
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("image");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("mediumint(9)");

                entity.Property(e => e.Caption)
                    .IsRequired()
                    .HasColumnName("caption")
                    .HasColumnType("text");

                entity.Property(e => e.LocalUrl)
                    .IsRequired()
                    .HasColumnName("localURL")
                    .HasColumnType("tinytext");

                entity.Property(e => e.SourceUrl)
                    .IsRequired()
                    .HasColumnName("sourceURL")
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<Imageref>(entity =>
            {
                entity.ToTable("imageref");

                entity.HasIndex(e => e.ElementId)
                    .HasName("elementID");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("mediumint(9)");

                entity.Property(e => e.ElementId)
                    .HasColumnName("elementID")
                    .HasColumnType("mediumint(9)");

                entity.Property(e => e.ElementValue)
                    .HasColumnName("elementValue")
                    .HasColumnType("varchar(24)");

                entity.Property(e => e.ImageId)
                    .HasColumnName("imageID")
                    .HasColumnType("mediumint(9)");
            });
        }
    }
}
