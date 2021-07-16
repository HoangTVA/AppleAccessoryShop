using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BusinessObject
{
    public partial class Apple_Accessory_StoreContext : DbContext
    {
        public Apple_Accessory_StoreContext()
        {
        }

        public Apple_Accessory_StoreContext(DbContextOptions<Apple_Accessory_StoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblComment> TblComments { get; set; }
        public virtual DbSet<TblOrder> TblOrders { get; set; }
        public virtual DbSet<TblOrderDetail> TblOrderDetails { get; set; }
        public virtual DbSet<TblProduct> TblProducts { get; set; }
        public virtual DbSet<TblRole> TblRoles { get; set; }
        public virtual DbSet<TblUser> TblUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=SE150990;uid=phuchqse1;pwd=Phuc@05012001;database=Apple_Accessory_Store");
               // optionsBuilder.UseSqlServer("Server=(local);uid=Quang;pwd=230201;database=Apple_Accessory_Store");
               
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblComment>(entity =>
            {
                entity.HasKey(e => e.CommentId);

                entity.ToTable("tblComments");

                entity.Property(e => e.CommentId)
                    .ValueGeneratedNever()
                    .HasColumnName("commentID");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasColumnName("description");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblComments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_tblComments_tblUsers");
            });

            modelBuilder.Entity<TblOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.ToTable("tblOrders");

                entity.Property(e => e.OrderId)
                    .ValueGeneratedNever()
                    .HasColumnName("orderID");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("orderDate");

                entity.Property(e => e.Total)
                    .HasColumnType("money")
                    .HasColumnName("total");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .HasColumnName("address");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_tblOrders_tblUsers");
            });

            modelBuilder.Entity<TblOrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId });

                entity.ToTable("tblOrderDetail");

                entity.Property(e => e.OrderId).HasColumnName("orderID");

                entity.Property(e => e.ProductId).HasColumnName("productID");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("money")
                    .HasColumnName("unitPrice");

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_tblOrderDetail_tblOrders");

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_tblOrderDetail_tblProducts");
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.ToTable("tblProducts");

                entity.Property(e => e.ProductId)
                    .ValueGeneratedNever()
                    .HasColumnName("productID");

                entity.Property(e => e.CommentId).HasColumnName("commentID");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(50)
                    .HasColumnName("productName");

                entity.Property(e => e.ProductImage)
                    .HasMaxLength(50)
                    .HasColumnName("productImage");

                entity.Property(e => e.ProductPrice)
                    .HasColumnType("money")
                    .HasColumnName("productPrice");

                entity.Property(e => e.UnitInStock).HasColumnName("unitInStock");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.TblProducts)
                    .HasForeignKey(d => d.CommentId)
                    .HasConstraintName("FK_tblProducts_tblComments");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("tblRoles");

                entity.Property(e => e.RoleId)
                    .HasMaxLength(10)
                    .HasColumnName("roleID")
                    .IsFixedLength(true);

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .HasColumnName("roleName");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("tblUsers");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("userID");

                entity.Property(e => e.RoleId)
                    .HasMaxLength(10)
                    .HasColumnName("roleID")
                    .IsFixedLength(true);

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(50)
                    .HasColumnName("userEmail");

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(10)
                    .HasColumnName("userPassword")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblUsers)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_tblUsers_tblRoles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
