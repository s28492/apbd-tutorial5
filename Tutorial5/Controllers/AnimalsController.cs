using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tutorial5.Models;
using Tutorial5.Models.DTOs;

namespace Tutorial5.Controllers;

[ApiController]
// [Route("api/animals")]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AnimalsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult GetAnimals(string orderBy = "name")
    {
        // Otwieramy połączenie
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        string controlOrderBy = "name";
        // Defincja command
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        if (new List<string> { "name", "description", "category", "area" }.Contains(orderBy))
        {
            controlOrderBy = orderBy;
        }
        command.CommandText = $"SELECT * FROM Animal ORDER BY {controlOrderBy}";
        // Wykonanie zapytania
        var reader = command.ExecuteReader();

        List<Animal> animals = new List<Animal>();

        var idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        var nameOrdinal = reader.GetOrdinal("Name");
        var descriptionOrdinal = reader.GetOrdinal("Description");
        var categoryOrdinal = reader.GetOrdinal("Category");
        var areaOrdinal = reader.GetOrdinal("Area");

        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = (int)reader.GetInt32(idAnimalOrdinal),
                Name = reader.GetString(nameOrdinal).ToString(),
                Description = reader.GetString(descriptionOrdinal).ToString(),
                Category = reader.GetString(categoryOrdinal).ToString(),
                Area = reader.GetString(areaOrdinal).ToString()
            });
        }

        // var animals = _repository.GetAnimals();

        return Ok(animals);
    }


    [HttpPost]
    public IActionResult AddAnimal(AddAnimal addAnimal)
    {
        // Otwieramy połączenie
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        // Defincja command
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "INSERT INTO Animal VALUES(@animalName,@animalDescription,@animalCategory,@animalArea)";
        command.Parameters.AddWithValue("@animalName", addAnimal.Name);
        command.Parameters.AddWithValue("@animalDescription", addAnimal.Description);
        command.Parameters.AddWithValue("@animalCategory", addAnimal.Category);
        command.Parameters.AddWithValue("@animalArea", addAnimal.Area);
        // Wykonanie zapytania
        command.ExecuteNonQuery();

        // _repository.AddAnimal(addAnimal);

        return Created();
    }
    
    
    
    [HttpPut("{id}")]
    public IActionResult UpdateAnimal(int id, UpdateAnimal updateAnimal)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        string sqlQuery = "UPDATE Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal";

        SqlCommand command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@IdAnimal", id);
        command.Parameters.AddWithValue("@Name", updateAnimal.Name);
        command.Parameters.AddWithValue("@Description", updateAnimal.Description);
        command.Parameters.AddWithValue("@Category", updateAnimal.Category);
        command.Parameters.AddWithValue("@Area", updateAnimal.Area);

        int affectedRows = command.ExecuteNonQuery();
        if (affectedRows > 0)
        {
            return Ok();
        }
        return NotFound();
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteAnimal(int id)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        SqlCommand command = new SqlCommand("DELETE FROM Animal WHERE IdAnimal = @IdAnimal", connection);
        command.Parameters.AddWithValue("@IdAnimal", id);

        int affectedRows = command.ExecuteNonQuery();
        if (affectedRows > 0)
        {
            return Ok();
        }
        return NotFound();
    }
}