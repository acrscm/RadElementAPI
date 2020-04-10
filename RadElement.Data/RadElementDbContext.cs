using RadElement.Core.Data;
using RadElement.Core.Domain;
using RadElement.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace RadElement.Data
{
    /// <summary>
    /// Repository class for data related operations
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// <seealso cref="RadElement.Core.Data.IRadElementDbContext" />
    public partial class RadElementDbContext : DbContext, IRadElementDbContext
    {
        /// <summary>
        /// The configuration manager
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadElementDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="configurationManager">The configuration manager.</param>
        public RadElementDbContext(
            DbContextOptions<RadElementDbContext> options,
            IConfigurationManager configurationManager) : base(options)
        {
            this.configurationManager = configurationManager;
        }

        /// <summary>
        /// Gets the code.
        /// </summary>
        public DbSet<Code> Code { get; set; }

        /// <summary>
        /// Gets the code reference.
        /// </summary>
        public DbSet<CodeRef> CodeRef { get; set; }

        /// <summary>
        /// Gets the code system.
        /// </summary>
        public DbSet<CodeSystem> CodeSystem { get; set; }

        /// <summary>
        /// Gets the editor.
        /// </summary>
        public DbSet<Editor> Editor { get; set; }

        /// <summary>
        /// Gets the element.
        /// </summary>
        public DbSet<Element> Element { get; set; }

        /// <summary>
        /// Gets the element set.
        /// </summary>
        public DbSet<ElementSet> ElementSet { get; set; }

        /// <summary>
        /// Gets the element set reference.
        /// </summary>
        public DbSet<ElementSetRef> ElementSetRef { get; set; }

        /// <summary>
        /// Gets the element value.
        /// </summary>
        public DbSet<ElementValue> ElementValue { get; set; }

        /// <summary>
        /// Gets the image.
        /// </summary>
        public DbSet<Image> Image { get; set; }

        /// <summary>
        /// Gets the image reference.
        /// </summary>
        public DbSet<ImageRef> ImageRef { get; set; }

        /// <summary>
        /// <para>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </para>
        /// <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(configurationManager.ConnectionString);
            }
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
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
