using RadElement.Core.Data;
using RadElement.Core.Domain;
using RadElement.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace RadElement.Data
{
    public partial class RadElementDbContext : DbContext, IRadElementDbContext
    {
        private readonly IConfigurationManager configurationManager;

        public RadElementDbContext(DbContextOptions<RadElementDbContext> options,IConfigurationManager configurationManager) : base(options)
        {
            this.configurationManager = configurationManager;
        }

        public virtual DbSet<Code> Code { get; set; }
        public virtual DbSet<CodeRef> CodeRef { get; set; }
        public virtual DbSet<CodeSystem> CodeSystem { get; set; }
        public virtual DbSet<Editor> Editor { get; set; }
        public virtual DbSet<Element> Element { get; set; }
        public virtual DbSet<ElementSet> ElementSet { get; set; }
        public virtual DbSet<ElementSetRef> ElementSetRef { get; set; }
        public virtual DbSet<ElementValue> ElementValue { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<ImageRef> ImageRef { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(configurationManager.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Code>(entity =>
            {
                entity.ToTable("Code");

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

            modelBuilder.Entity<CodeRef>(entity =>
            {
                entity.ToTable("CodeRef");

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

            modelBuilder.Entity<CodeSystem>(entity =>
            {
                entity.HasKey(e => e.Abbrev);

                entity.ToTable("CodeSystem");

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
                entity.ToTable("Editor");

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
                entity.ToTable("Element");

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

            modelBuilder.Entity<ElementSet>(entity =>
            {
                entity.ToTable("ElementSet");

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

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("text");

                entity.Property(e => e.ParentId)
                    .HasColumnName("parentID")
                    .HasColumnType("mediumint(9)");
            });

            modelBuilder.Entity<ElementSetRef>(entity =>
            {
                entity.ToTable("ElementSetRef");

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

            modelBuilder.Entity<ElementValue>(entity =>
            {
                entity.ToTable("ElementValue");

                entity.HasIndex(e => e.ElementId)
                    .HasName("elementID");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("mediumint(9)");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasColumnType("text");

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
                entity.ToTable("Image");

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

            modelBuilder.Entity<ImageRef>(entity =>
            {
                entity.ToTable("ImageRef");

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
