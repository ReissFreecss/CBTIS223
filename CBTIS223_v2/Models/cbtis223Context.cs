using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CBTIS223_v2.Models
{
    public partial class cbtis223Context : DbContext
    {
        public cbtis223Context()
        {
        }

        public cbtis223Context(DbContextOptions<cbtis223Context> options)
            : base(options)
        {
        }

        public async Task<List<Escuela>> getEscuelas() { 
            return await this.Escuelas.ToListAsync();
        }
        public virtual DbSet<Escuela> Escuelas { get; set; } = null!;
        public virtual DbSet<EstudiantesPractica> EstudiantesPracticas { get; set; } = null!;
        public virtual DbSet<EstudiantesServicio> EstudiantesServicios { get; set; } = null!;
        public virtual DbSet<Institucione> Instituciones { get; set; } = null!;
        public virtual DbSet<PracticasProfesionale> PracticasProfesionales { get; set; } = null!;
        public virtual DbSet<ServicioSocial> ServicioSocials { get; set; } = null!;
        public virtual DbSet<Usuario> Usuario { get; set; } = null!;
        
        public async Task<IList<Institucione>> GetInstituciones() {
            IList<Institucione> instituciones = await Instituciones.ToListAsync();
            return instituciones;
        }

        public async Task<IList<EstudiantesServicio>> GetEstudiantes()
        {
            IList<EstudiantesServicio> estudiantes = await EstudiantesServicios.ToListAsync();
            return estudiantes;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=3306;database=cbtis223;user=root;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Escuela>(entity =>
            {
                entity.HasKey(e => e.NombreEscuela)
                    .HasName("PRIMARY");

                entity.ToTable("escuela");

                entity.Property(e => e.NombreEscuela)
                    .HasMaxLength(200)
                    .HasColumnName("nombre_escuela");

                entity.Property(e => e.NombreDirector)
                    .HasMaxLength(200)
                    .HasColumnName("nombre_director");

                entity.Property(e => e.NombreDirectorGeneral)
                    .HasMaxLength(200)
                    .HasColumnName("nombre_director_general");

                entity.Property(e => e.NombreEncargadoEstatal)
                    .HasMaxLength(200)
                    .HasColumnName("nombre_encargado_estatal");
            });

            modelBuilder.Entity<EstudiantesPractica>(entity =>
            {
                entity.HasKey(e => e.EstudianteNc)
                    .HasName("PRIMARY");

                entity.ToTable("estudiantes_practicas");

                entity.Property(e => e.EstudianteNc)
                    .HasMaxLength(15)
                    .HasColumnName("estudiante_nc");

                entity.HasOne(d => d.EstudianteNcNavigation)
                    .WithOne(p => p.EstudiantesPractica)
                    .HasForeignKey<EstudiantesPractica>(d => d.EstudianteNc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("estudiantes_practicas_ibfk_1");
            });

            modelBuilder.Entity<EstudiantesServicio>(entity =>
            {
                entity.HasKey(e => e.NumeroControl)
                    .HasName("PRIMARY");

                entity.ToTable("estudiantes_servicio");

                entity.Property(e => e.NumeroControl)
                    .HasMaxLength(15)
                    .HasColumnName("numero_control");

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(255)
                    .HasColumnName("apellido_materno");

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(255)
                    .HasColumnName("apellido_paterno");

                entity.Property(e => e.Curp)
                    .HasMaxLength(18)
                    .HasColumnName("curp");

                entity.Property(e => e.Especialidad)
                    .HasMaxLength(50)
                    .HasColumnName("especialidad");

                entity.Property(e => e.Ciclo)
                   .HasMaxLength(50)
                   .HasColumnName("ciclo");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Institucione>(entity =>
            {
                entity.HasKey(e => e.IdInstitucion)
                    .HasName("PRIMARY");

                entity.ToTable("instituciones");

                entity.Property(e => e.IdInstitucion)
                    .ValueGeneratedNever()
                    .HasColumnName("id_institucion");

                entity.Property(e => e.Institucion)
                    .HasMaxLength(255)
                    .HasColumnName("institucion");

                entity.Property(e => e.Supervisor)
                    .HasMaxLength(255)
                    .HasColumnName("supervisor");

                entity.Property(e => e.TipoInstitucion)
                    .HasMaxLength(200)
                    .HasColumnName("tipo_institucion");

                entity.Property(e => e.UbicacionInstitucion)
                    .HasMaxLength(255)
                    .HasColumnName("ubicacion_institucion");
            });

            modelBuilder.Entity<PracticasProfesionale>(entity =>
            {
                entity.HasKey(e => e.EstudianteNc)
                    .HasName("PRIMARY");

                entity.ToTable("practicas_profesionales");

                entity.HasIndex(e => e.IdInstiPracticas, "id_insti_practicas");

                entity.Property(e => e.EstudianteNc)
                    .HasMaxLength(15)
                    .HasColumnName("estudiante_nc");

                entity.Property(e => e.FechaInicioPracticas).HasColumnName("fecha_inicio_practicas");

                entity.Property(e => e.FechaTerminoPracticas).HasColumnName("fecha_termino_practicas");
                
                entity.Property(e => e.actividad_practicas)
                    .HasMaxLength(300)
                    .HasColumnName("actividad_practicas");

                entity.Property(e => e.IdInstiPracticas).HasColumnName("id_insti_practicas");

                entity.HasOne(d => d.EstudianteNcNavigation)
                    .WithOne(p => p.PracticasProfesionale)
                    .HasForeignKey<PracticasProfesionale>(d => d.EstudianteNc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("practicas_profesionales_ibfk_1");

                entity.HasOne(d => d.IdInstiPracticasNavigation)
                    .WithMany(p => p.PracticasProfesionales)
                    .HasForeignKey(d => d.IdInstiPracticas)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("practicas_profesionales_ibfk_2");
            });

            modelBuilder.Entity<ServicioSocial>(entity =>
            {
                entity.HasKey(e => e.EstudianteNc)
                    .HasName("PRIMARY");

                entity.ToTable("servicio_social");

                entity.HasIndex(e => e.IdInstiServicio, "id_insti_servicio");

                entity.Property(e => e.EstudianteNc)
                    .HasMaxLength(15)
                    .HasColumnName("estudiante_nc");

                entity.Property(e => e.FechaInicioServicio).HasColumnName("fecha_inicio_servicio");

                entity.Property(e => e.FechaTerminoServicio).HasColumnName("fecha_termino_servicio");

                entity.Property(e => e.IdInstiServicio).HasColumnName("id_insti_servicio");
                entity.Property(e => e.actividad_servicio).HasColumnName("actividad_servicio");


                entity.HasOne(d => d.EstudianteNcNavigation)
                    .WithOne(p => p.ServicioSocial)
                    .HasForeignKey<ServicioSocial>(d => d.EstudianteNc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("servicio_social_ibfk_1");

                entity.HasOne(d => d.IdInstiServicioNavigation)
                    .WithMany(p => p.ServicioSocials)
                    .HasForeignKey(d => d.IdInstiServicio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("servicio_social_ibfk_2");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(100)
                    .HasColumnName("contraseña");

                entity.Property(e => e.Correo)
                    .HasMaxLength(255)
                    .HasColumnName("correo");

                entity.Property(e => e.token_recovery)
                    .HasMaxLength(255)
                    .HasColumnName("token_recovery");
                //Token para recuperacion

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .HasColumnName("nombre");

                entity.Property(e => e.Rol)
                    .HasMaxLength(35)
                    .HasColumnName("rol");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
