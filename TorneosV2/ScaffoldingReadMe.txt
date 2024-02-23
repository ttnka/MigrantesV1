Primero es Necesario

1) Agregar Nuget con Radzen y demas aplicaciones
1.1) Confirugar Radzen

2) Asignar una base de datos tipo MySql
2.1) Cambia la conexcion en Programa.cs para conectar a la base de datos

3) Crear las clases necesarias
3.1) Spiner para clase

4) Constantes es necesario configurar
4.1) Todas las variables de donde se colgara el proyecto comos ervidor y esos valores

5) Rutina de arranque
5.1) Se creen grupos de organizaciones, para empezar el grupo general para que entren todas las org.




public class Escuela
{
    [Key] // Esto indica que Id es la clave primaria
    public string Id { get; private set; }

    public string Nombre { get; private set; }

    // Agregamos una propiedad de navegación para los salones
    public virtual ICollection<Salon> Salones { get; set; } = new List<Salon>();

    public Escuela(string id, string nombre)
    {
        Id = id;
        Nombre = nombre;
    }

    // Cambiamos el método para agregar un salón
    public void AgregarSalon(Salon salon)
    {
        Salones.Add(salon);
        salon.AsignarEscuela(this);
    }
}

public class Salon
{
    [Key]
    public string Id { get; private set; }
    public string Nombre { get; private set; }

    [ForeignKey("EscuelaId")] // Esto indica que EscuelaId es una clave foránea
    public string EscuelaId { get; private set; }

    // Propiedad de navegación
    public virtual Escuela Escuela { get; private set; }

    // Agregamos una propiedad de navegación para los alumnos
    public virtual ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();

    public Salon(string id, string nombre, string escuelaId)
    {
        Id = id;
        Nombre = nombre;
        EscuelaId = escuelaId;
    }

    public void AsignarEscuela(Escuela escuela)
    {
        Escuela = escuela;
        escuela.AgregarSalon(this);
    }

    public void AgregarAlumno(Alumno alumno)
    {
        Alumnos.Add(alumno);
        alumno.AsignarSalon(this);
    }
}

public class Alumno
{
    [Key]
    public string Id { get; private set; }
    public string Nombre { get; private set; }

    [ForeignKey("SalonId")] // Esto indica que SalonId es una clave foránea
    public string SalonId { get; private set; }

    // Propiedad de navegación
    public virtual Salon Salon { get; private set; }

    public Alumno(string id, string nombre, string salonId)
    {
        Id = id;
        Nombre = nombre;
        SalonId = salonId;
    }

    public void AsignarSalon(Salon salon)
    {
        Salon = salon;
        salon.AgregarAlumno(this);
    }
}


@if (EsNuevoUser == true)
                    {
                        <RadzenLabel Text="Password" />
                    }
                    else
                    {
                        <RadzenTextBox style="width: 90%;" Name="Pass" @bind-Value="NuevoUser.Pass"
                                       Placeholder="e-Mail" MaxLength="75" Change="CheckPass" /> <br />
                        <RadzenRequiredValidator Component="Mail" Text="Email es requerido" />

                    }

@if (EsNuevoUser == false)
            {
                <div class="row">
                    <div class="col-md-4 align-items-center d-flex">
                        <RadzenLabel Text="Repite Password" />
                    </div>
                    <div class="col-md-8">
                        <RadzenTextBox style="width: 90%;" Name="Confirm" @bind-Value="NuevoUser.Confirm"
                           Placeholder="Repite tu Password" MaxLength="75" Change="CheckPass" /> <br />
                        <RadzenRequiredValidator Component="Confirm" Text="Confirma es requerido" />

                    </div>
                </div>      
            }
