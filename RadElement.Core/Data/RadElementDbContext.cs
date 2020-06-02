using RadElement.Core.Domain;
using RadElement.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace RadElement.Core.Data
{
    /// <summary>
    /// Repository class for data related operations
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public partial class RadElementDbContext : DbContext
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
        /// Initializes a new instance of the <see cref="RadElementDbContext"/> class.
        /// </summary>
        public RadElementDbContext() { }

        /// <summary>
        /// Gets the code.
        /// </summary>
        public virtual DbSet<Code> Code { get; set; }

        /// <summary>
        /// Gets the code reference.
        /// </summary>
        public virtual DbSet<CodeRef> CodeRef { get; set; }

        /// <summary>
        /// Gets the code system.
        /// </summary>
        public virtual DbSet<CodeSystem> CodeSystem { get; set; }

        /// <summary>
        /// Gets the editor.
        /// </summary>
        public virtual DbSet<Editor> Editor { get; set; }

        /// <summary>
        /// Gets the element.
        /// </summary>
        public virtual DbSet<Element> Element { get; set; }

        /// <summary>
        /// Gets the element set.
        /// </summary>
        public virtual DbSet<ElementSet> ElementSet { get; set; }

        /// <summary>
        /// Gets the element set reference.
        /// </summary>
        public virtual DbSet<ElementSetRef> ElementSetRef { get; set; }

        /// <summary>
        /// Gets the element value.
        /// </summary>
        public virtual DbSet<ElementValue> ElementValue { get; set; }

        /// <summary>
        /// Gets the image.
        /// </summary>
        public virtual DbSet<Image> Image { get; set; }

        /// <summary>
        /// Gets the image reference.
        /// </summary>
        public virtual DbSet<ImageRef> ImageRef { get; set; }

        /// <summary>
        /// Gets the person.
        /// </summary>
        public virtual DbSet<Person> Person { get; set; }

        /// <summary>
        /// Gets the person role element reference.
        /// </summary>
        public virtual DbSet<PersonRoleElementRef> PersonRoleElementRef { get; set; }

        /// <summary>
        /// Gets the person role element set reference.
        /// </summary>
        public virtual DbSet<PersonRoleElementSetRef> PersonRoleElementSetRef { get; set; }

        /// <summary>
        /// Gets the organization.
        /// </summary>
        public virtual DbSet<Organization> Organization { get; set; }

        /// <summary>
        /// Gets the organization role element reference.
        /// </summary>
        public virtual DbSet<OrganizationRoleElementRef> OrganizationRoleElementRef { get; set; }

        /// <summary>
        /// Gets the organization role element set reference.
        /// </summary>
        public virtual DbSet<OrganizationRoleElementSetRef> OrganizationRoleElementSetRef { get; set; }

        /// <summary>
        /// Gets or sets the index code system.
        /// </summary>
        public virtual DbSet<IndexCodeSystem> IndexCodeSystem { get; set; }

        /// <summary>
        /// Gets or sets the index code.
        /// </summary>
        /// <value>
        public virtual DbSet<IndexCode> IndexCode { get; set; }

        /// <summary>
        /// Gets or sets the index code element reference.
        /// </summary>
        public virtual DbSet<IndexCodeElementRef> IndexCodeElementRef { get; set; }

        /// <summary>
        /// Gets or sets the index code element set reference.
        /// </summary>
        public virtual DbSet<IndexCodeElementSetRef> IndexCodeElementSetRef { get; set; }

        /// <summary>
        /// Gets or sets the index code element value reference.
        /// </summary>
        public virtual DbSet<IndexCodeElementValueRef> IndexCodeElementValueRef { get; set; }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        public virtual DbSet<Reference> Reference { get; set; }
        /// <summary>
        /// 
        /// Gets or sets the reference reference.
        /// </summary>
        public virtual DbSet<ReferenceRef> ReferenceRef { get; set; }

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
            modelBuilder.Entity<CodeSystem>(entity =>
            {
                entity.HasKey(e => e.Abbrev);
            });
            modelBuilder.Entity<IndexCode>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<IndexCodeSystem>(entity =>
            {
                entity.HasKey(e => e.Abbrev);
            });
            modelBuilder.Entity<IndexCodeElementRef>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<IndexCodeElementSetRef>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<IndexCodeElementValueRef>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
