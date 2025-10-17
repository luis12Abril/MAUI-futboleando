using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace futboleandoAPIS.Models;

public partial class DbA85d0bFutboleandobdContext : DbContext
{
    public DbA85d0bFutboleandobdContext()
    {
    }

    public DbA85d0bFutboleandobdContext(DbContextOptions<DbA85d0bFutboleandobdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Arbitro> Arbitros { get; set; }

    public virtual DbSet<Arbitrocolegio> Arbitrocolegios { get; set; }

    public virtual DbSet<Avisofutboleando> Avisofutboleandos { get; set; }

    public virtual DbSet<Boton> Botons { get; set; }

    public virtual DbSet<Campo> Campos { get; set; }

    public virtual DbSet<Campocolegio> Campocolegios { get; set; }

    public virtual DbSet<Colegioarbitro> Colegioarbitros { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Comunicado> Comunicados { get; set; }

    public virtual DbSet<Configuracion> Configuracions { get; set; }

    public virtual DbSet<Equipo> Equipos { get; set; }

    public virtual DbSet<Equipocolegio> Equipocolegios { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Estatusjuego> Estatusjuegos { get; set; }

    public virtual DbSet<Gol> Gols { get; set; }

    public virtual DbSet<Jornadum> Jornada { get; set; }

    public virtual DbSet<Juego> Juegos { get; set; }

    public virtual DbSet<Jugador> Jugadors { get; set; }

    public virtual DbSet<Liga> Ligas { get; set; }

    public virtual DbSet<Municipio> Municipios { get; set; }

    public virtual DbSet<Pagina> Paginas { get; set; }

    public virtual DbSet<Paginatipousuario> Paginatipousuarios { get; set; }

    public virtual DbSet<Paginatipousuarioboton> Paginatipousuariobotons { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Programacioncolegio> Programacioncolegios { get; set; }

    public virtual DbSet<Publicidad> Publicidads { get; set; }

    public virtual DbSet<Tipousuario> Tipousuarios { get; set; }

    public virtual DbSet<Torneo> Torneos { get; set; }

    public virtual DbSet<Ultimoscinco> Ultimoscincos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Usuariotorneo> Usuariotorneos { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("server=sql5052.site4now.net;database=db_a85d0b_futboleandobd;uid=db_a85d0b_futboleandobd_admin;pwd=Labt1970");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Arbitro>(entity =>
        {
            entity.HasKey(e => e.Idarbitro);

            entity.ToTable("ARBITRO");

            entity.Property(e => e.Idarbitro).HasColumnName("IDARBITRO");
            entity.Property(e => e.Apmaterno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APMATERNO");
            entity.Property(e => e.Appaterno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APPATERNO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Torneo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TORNEO");
        });

        modelBuilder.Entity<Arbitrocolegio>(entity =>
        {
            entity.HasKey(e => e.Idarbitrocolegio);

            entity.ToTable("ARBITROCOLEGIO");

            entity.Property(e => e.Idarbitrocolegio).HasColumnName("IDARBITROCOLEGIO");
            entity.Property(e => e.Apmaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("APMATERNO");
            entity.Property(e => e.Appaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("APPATERNO");
            entity.Property(e => e.Codigoactual)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CODIGOACTUAL");
            entity.Property(e => e.Fnacimiento)
                .HasColumnType("datetime")
                .HasColumnName("FNACIMIENTO");
            entity.Property(e => e.Fotoarbitro)
                .IsUnicode(false)
                .HasColumnName("FOTOARBITRO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idcolegioarbitro).HasColumnName("IDCOLEGIOARBITRO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Nomusuario)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NOMUSUARIO");
            entity.Property(e => e.Peso).HasColumnName("PESO");
        });

        modelBuilder.Entity<Avisofutboleando>(entity =>
        {
            entity.HasKey(e => e.Idavisofutboleando);

            entity.ToTable("AVISOFUTBOLEANDO");

            entity.Property(e => e.Idavisofutboleando).HasColumnName("IDAVISOFUTBOLEANDO");
            entity.Property(e => e.Fechamensaje).HasColumnName("FECHAMENSAJE");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Mensaje)
                .IsUnicode(false)
                .HasColumnName("MENSAJE");
            entity.Property(e => e.Titulomensaje)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TITULOMENSAJE");
        });

        modelBuilder.Entity<Boton>(entity =>
        {
            entity.HasKey(e => e.Idboton);

            entity.ToTable("BOTON");

            entity.Property(e => e.Idboton).HasColumnName("IDBOTON");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Campo>(entity =>
        {
            entity.HasKey(e => e.Idcampo);

            entity.ToTable("CAMPO");

            entity.Property(e => e.Idcampo).HasColumnName("IDCAMPO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Torneo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TORNEO");
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("UBICACION");
        });

        modelBuilder.Entity<Campocolegio>(entity =>
        {
            entity.HasKey(e => e.Idcampocolegio);

            entity.ToTable("CAMPOCOLEGIO");

            entity.Property(e => e.Idcampocolegio).HasColumnName("IDCAMPOCOLEGIO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idcolegioarbitro).HasColumnName("IDCOLEGIOARBITRO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("UBICACION");
        });

        modelBuilder.Entity<Colegioarbitro>(entity =>
        {
            entity.HasKey(e => e.Idcolegioarbitro);

            entity.ToTable("COLEGIOARBITRO");

            entity.Property(e => e.Idcolegioarbitro).HasColumnName("IDCOLEGIOARBITRO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idpresidente).HasColumnName("IDPRESIDENTE");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.Idcomentario);

            entity.ToTable("COMENTARIO");

            entity.Property(e => e.Idcomentario).HasColumnName("IDCOMENTARIO");
            entity.Property(e => e.Comentario1)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("COMENTARIO");
            entity.Property(e => e.Fechacomentario)
                .HasColumnType("datetime")
                .HasColumnName("FECHACOMENTARIO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idjuego).HasColumnName("IDJUEGO");
            entity.Property(e => e.Idusuario).HasColumnName("IDUSUARIO");
        });

        modelBuilder.Entity<Comunicado>(entity =>
        {
            entity.HasKey(e => e.Idcomunicado);

            entity.ToTable("COMUNICADO");

            entity.Property(e => e.Idcomunicado).HasColumnName("IDCOMUNICADO");
            entity.Property(e => e.Comunicadocorto)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("COMUNICADOCORTO");
            entity.Property(e => e.Comunicadolargo)
                .IsUnicode(false)
                .HasColumnName("COMUNICADOLARGO");
            entity.Property(e => e.Fechacomunicado).HasColumnName("FECHACOMUNICADO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");
        });

        modelBuilder.Entity<Configuracion>(entity =>
        {
            entity.HasKey(e => e.Idconfiguracion);

            entity.ToTable("CONFIGURACION");

            entity.Property(e => e.Idconfiguracion)
                .ValueGeneratedNever()
                .HasColumnName("IDCONFIGURACION");
            entity.Property(e => e.Switch1).HasColumnName("SWITCH1");
            entity.Property(e => e.Switch2).HasColumnName("SWITCH2");
            entity.Property(e => e.Switch3).HasColumnName("SWITCH3");
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasKey(e => e.Idequipo);

            entity.ToTable("EQUIPO");

            entity.Property(e => e.Idequipo).HasColumnName("IDEQUIPO");
            entity.Property(e => e.Claequipo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CLAEQUIPO");
            entity.Property(e => e.Difgoles).HasColumnName("DIFGOLES");
            entity.Property(e => e.Empatados).HasColumnName("EMPATADOS");
            entity.Property(e => e.Empatadosganados).HasColumnName("EMPATADOSGANADOS");
            entity.Property(e => e.Empatadosperdidos).HasColumnName("EMPATADOSPERDIDOS");
            entity.Property(e => e.Fotoequipo)
                .IsUnicode(false)
                .HasColumnName("FOTOEQUIPO");
            entity.Property(e => e.Ganados).HasColumnName("GANADOS");
            entity.Property(e => e.Ganadosadmo).HasColumnName("GANADOSADMO");
            entity.Property(e => e.Golesafavor).HasColumnName("GOLESAFAVOR");
            entity.Property(e => e.Golesencontra).HasColumnName("GOLESENCONTRA");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");
            entity.Property(e => e.Jugados).HasColumnName("JUGADOS");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Perdidos).HasColumnName("PERDIDOS");
            entity.Property(e => e.Perdidosadmo).HasColumnName("PERDIDOSADMO");
            entity.Property(e => e.Puntos).HasColumnName("PUNTOS");
            entity.Property(e => e.Puntosextras).HasColumnName("PUNTOSEXTRAS");
            entity.Property(e => e.Representante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("REPRESENTANTE");
            entity.Property(e => e.Torneo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TORNEO");
            entity.Property(e => e.Usuequipo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USUEQUIPO");
            entity.Property(e => e.Vigencia)
                .HasColumnType("datetime")
                .HasColumnName("VIGENCIA");
        });

        modelBuilder.Entity<Equipocolegio>(entity =>
        {
            entity.HasKey(e => e.Idequipocolegio);

            entity.ToTable("EQUIPOCOLEGIO");

            entity.Property(e => e.Idequipocolegio).HasColumnName("IDEQUIPOCOLEGIO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idcolegioarbitro).HasColumnName("IDCOLEGIOARBITRO");
            entity.Property(e => e.Idtorneocolegio).HasColumnName("IDTORNEOCOLEGIO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.Idestado);

            entity.ToTable("ESTADO");

            entity.Property(e => e.Idestado).HasColumnName("IDESTADO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(509)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Estatusjuego>(entity =>
        {
            entity.HasKey(e => e.Idestatusjuego);

            entity.ToTable("ESTATUSJUEGO");

            entity.Property(e => e.Idestatusjuego).HasColumnName("IDESTATUSJUEGO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Torneo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TORNEO");
        });

        modelBuilder.Entity<Gol>(entity =>
        {
            entity.HasKey(e => e.Idgol);

            entity.ToTable("GOL");

            entity.Property(e => e.Idgol).HasColumnName("IDGOL");
            entity.Property(e => e.Goles).HasColumnName("GOLES");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idequipo).HasColumnName("IDEQUIPO");
            entity.Property(e => e.Idjuego).HasColumnName("IDJUEGO");
            entity.Property(e => e.Idjugador).HasColumnName("IDJUGADOR");
        });

        modelBuilder.Entity<Jornadum>(entity =>
        {
            entity.HasKey(e => e.Idjornada);

            entity.ToTable("JORNADA");

            entity.Property(e => e.Idjornada).HasColumnName("IDJORNADA");
            entity.Property(e => e.Finiciojornada).HasColumnName("FINICIOJORNADA");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Torneo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TORNEO");
        });

        modelBuilder.Entity<Juego>(entity =>
        {
            entity.HasKey(e => e.Idjuego);

            entity.ToTable("JUEGO");

            entity.Property(e => e.Idjuego).HasColumnName("IDJUEGO");
            entity.Property(e => e.Cuentaparagoles).HasColumnName("CUENTAPARAGOLES");
            entity.Property(e => e.Cuentaparapuntos).HasColumnName("CUENTAPARAPUNTOS");
            entity.Property(e => e.Fhorario)
                .HasColumnType("datetime")
                .HasColumnName("FHORARIO");
            entity.Property(e => e.Golesequipo01).HasColumnName("GOLESEQUIPO01");
            entity.Property(e => e.Golesequipo02).HasColumnName("GOLESEQUIPO02");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idarbitro).HasColumnName("IDARBITRO");
            entity.Property(e => e.Idcampo).HasColumnName("IDCAMPO");
            entity.Property(e => e.Idequipo01).HasColumnName("IDEQUIPO01");
            entity.Property(e => e.Idequipo02).HasColumnName("IDEQUIPO02");
            entity.Property(e => e.Idestatusjuego).HasColumnName("IDESTATUSJUEGO");
            entity.Property(e => e.Idjornada).HasColumnName("IDJORNADA");
            entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");
            entity.Property(e => e.Peequipo01).HasColumnName("PEEQUIPO01");
            entity.Property(e => e.Peequipo02).HasColumnName("PEEQUIPO02");
            entity.Property(e => e.Puntosequipo01).HasColumnName("PUNTOSEQUIPO01");
            entity.Property(e => e.Puntosequipo02).HasColumnName("PUNTOSEQUIPO02");
            entity.Property(e => e.Resequipo01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("RESEQUIPO01");
            entity.Property(e => e.Resequipo02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("RESEQUIPO02");
            entity.Property(e => e.Torneo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TORNEO");
        });

        modelBuilder.Entity<Jugador>(entity =>
        {
            entity.HasKey(e => e.Idjugador);

            entity.ToTable("JUGADOR");

            entity.Property(e => e.Idjugador).HasColumnName("IDJUGADOR");
            entity.Property(e => e.Apmaterno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APMATERNO");
            entity.Property(e => e.Appaterno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APPATERNO");
            entity.Property(e => e.Fnacimiento).HasColumnName("FNACIMIENTO");
            entity.Property(e => e.Goles).HasColumnName("GOLES");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idequipo).HasColumnName("IDEQUIPO");
            entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Torneo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TORNEO");
        });

        modelBuilder.Entity<Liga>(entity =>
        {
            entity.HasKey(e => e.Idliga);

            entity.ToTable("LIGA");

            entity.Property(e => e.Idliga).HasColumnName("IDLIGA");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idmunicipio).HasColumnName("IDMUNICIPIO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Municipio>(entity =>
        {
            entity.HasKey(e => e.Idmunicipio);

            entity.ToTable("MUNICIPIO");

            entity.Property(e => e.Idmunicipio).HasColumnName("IDMUNICIPIO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idestado).HasColumnName("IDESTADO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Pagina>(entity =>
        {
            entity.HasKey(e => e.Idpagina);

            entity.ToTable("PAGINA");

            entity.Property(e => e.Idpagina).HasColumnName("IDPAGINA");
            entity.Property(e => e.Accion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ACCION");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Mensaje)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("MENSAJE");
            entity.Property(e => e.Ordenmenu).HasColumnName("ORDENMENU");
            entity.Property(e => e.Visible).HasColumnName("VISIBLE");
        });

        modelBuilder.Entity<Paginatipousuario>(entity =>
        {
            entity.HasKey(e => e.Idpaginatipousuario);

            entity.ToTable("PAGINATIPOUSUARIO");

            entity.Property(e => e.Idpaginatipousuario).HasColumnName("IDPAGINATIPOUSUARIO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idpagina).HasColumnName("IDPAGINA");
            entity.Property(e => e.Idtipousuario).HasColumnName("IDTIPOUSUARIO");
        });

        modelBuilder.Entity<Paginatipousuarioboton>(entity =>
        {
            entity.HasKey(e => e.Idpaginatipousuarioboton);

            entity.ToTable("PAGINATIPOUSUARIOBOTON");

            entity.Property(e => e.Idpaginatipousuarioboton).HasColumnName("IDPAGINATIPOUSUARIOBOTON");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idboton).HasColumnName("IDBOTON");
            entity.Property(e => e.Idpaginatipousuario).HasColumnName("IDPAGINATIPOUSUARIO");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.Idpersona);

            entity.ToTable("PERSONA");

            entity.Property(e => e.Idpersona).HasColumnName("IDPERSONA");
            entity.Property(e => e.Apmaterno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APMATERNO");
            entity.Property(e => e.Appaterno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APPATERNO");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CORREO");
            entity.Property(e => e.Fechanacimiento).HasColumnName("FECHANACIMIENTO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Telefono)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TELEFONO");
            entity.Property(e => e.Tieneusuario).HasColumnName("TIENEUSUARIO");
        });

        modelBuilder.Entity<Programacioncolegio>(entity =>
        {
            entity.HasKey(e => e.Idprogramacioncolegio);

            entity.ToTable("PROGRAMACIONCOLEGIO");

            entity.Property(e => e.Idprogramacioncolegio).HasColumnName("IDPROGRAMACIONCOLEGIO");
            entity.Property(e => e.Comentario)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("COMENTARIO");
            entity.Property(e => e.Fjuegocolegio)
                .HasColumnType("datetime")
                .HasColumnName("FJUEGOCOLEGIO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idarbitrocolegio).HasColumnName("IDARBITROCOLEGIO");
            entity.Property(e => e.Idcampocolegio).HasColumnName("IDCAMPOCOLEGIO");
            entity.Property(e => e.Idcolegioarbitro).HasColumnName("IDCOLEGIOARBITRO");
            entity.Property(e => e.Idequipocolegio01).HasColumnName("IDEQUIPOCOLEGIO01");
            entity.Property(e => e.Idequipocolegio02).HasColumnName("IDEQUIPOCOLEGIO02");
        });

        modelBuilder.Entity<Publicidad>(entity =>
        {
            entity.HasKey(e => e.Idpublicidad);

            entity.ToTable("PUBLICIDAD");

            entity.Property(e => e.Idpublicidad).HasColumnName("IDPUBLICIDAD");
            entity.Property(e => e.Foto)
                .IsUnicode(false)
                .HasColumnName("FOTO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");
            entity.Property(e => e.Orden).HasColumnName("ORDEN");
        });

        modelBuilder.Entity<Tipousuario>(entity =>
        {
            entity.HasKey(e => e.Idtipousuario);

            entity.ToTable("TIPOUSUARIO");

            entity.Property(e => e.Idtipousuario).HasColumnName("IDTIPOUSUARIO");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Torneo>(entity =>
        {
            entity.HasKey(e => e.Idtorneo);

            entity.ToTable("TORNEO");

            entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");
            entity.Property(e => e.Clavetorneo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CLAVETORNEO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idliga).HasColumnName("IDLIGA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Ordentorneo).HasColumnName("ORDENTORNEO");
            entity.Property(e => e.Visible).HasColumnName("VISIBLE");
            entity.Property(e => e.Visitas).HasColumnName("VISITAS");
            entity.Property(e => e.Visitascel).HasColumnName("VISITASCEL");
        });

        modelBuilder.Entity<Ultimoscinco>(entity =>
        {
            entity.HasKey(e => e.Idultimoscinco);

            entity.ToTable("ULTIMOSCINCO");

            entity.Property(e => e.Idultimoscinco).HasColumnName("IDULTIMOSCINCO");
            entity.Property(e => e.C2)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.C3)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.C4)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.C5)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Idequipo).HasColumnName("IDEQUIPO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Puntos).HasColumnName("PUNTOS");
            entity.Property(e => e.Ultimo)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("ULTIMO");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Idusuario);

            entity.ToTable("USUARIO");

            entity.Property(e => e.Idusuario).HasColumnName("IDUSUARIO");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONTRASEÑA");
            entity.Property(e => e.Fechaalta)
                .HasColumnType("datetime")
                .HasColumnName("FECHAALTA");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idarbitrocolegio).HasColumnName("IDARBITROCOLEGIO");
            entity.Property(e => e.Idpersona).HasColumnName("IDPERSONA");
            entity.Property(e => e.Idtipousuario).HasColumnName("IDTIPOUSUARIO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Origenalta)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ORIGENALTA");
            entity.Property(e => e.Token)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TOKEN");
            entity.Property(e => e.Visitas).HasColumnName("VISITAS");
            entity.Property(e => e.Visitascel).HasColumnName("VISITASCEL");
        });

        modelBuilder.Entity<Usuariotorneo>(entity =>
        {
            entity.HasKey(e => e.Idusuariotorneo);

            entity.ToTable("USUARIOTORNEO");

            entity.Property(e => e.Idusuariotorneo).HasColumnName("IDUSUARIOTORNEO");
            entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");
            entity.Property(e => e.Idtorneo).HasColumnName("IDTORNEO");
            entity.Property(e => e.Idusuario).HasColumnName("IDUSUARIO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
