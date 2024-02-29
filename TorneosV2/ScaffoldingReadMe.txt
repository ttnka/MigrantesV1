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


/*
    <p>
        <a asp-page="./Register" asp-route-returnUrl="@Model.ReturnUrl">Register as a new user</a>
    </p>
    <p>
        <a id="resend-confirmation" asp-page="./ResendEmailConfirmation">Resend email confirmation</a>
    </p>

    <div class="col-md-6 col-md-offset-2">
        <section>
            <h3>Use another service to log in.</h3>
            <hr />
            @{
                if ((Model.ExternalLogins?.Count ?? 0) == 0)
                {
                    <div>
                        <p>
                            There are no external authentication services configured. See this <a href="https://go.microsoft.com/fwlink/?LinkID=532715">
                                article
                                about setting up this ASP.NET application to support logging in via external services
                            </a>.
                        </p>
                    </div>
                }
                else
                {
                    <form id="external-account" asp-page="./ExternalLogin" asp-route-returnUrl="@Model.ReturnUrl" method="post" class="form-horizontal">
                        <div>
                            <p>
                                @foreach (var provider in Model.ExternalLogins!)
                                {
                                    <button type="submit" class="btn btn-primary" name="provider" value="@provider.Name" title="Log in using your @provider.DisplayName account">@provider.DisplayName</button>
                                }
                            </p>
                        </div>
                    </form>
                }
            }
        </section>
    </div>
    */