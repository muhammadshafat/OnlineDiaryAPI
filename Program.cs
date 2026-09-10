using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5294");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SqlConnectionFactory>();

builder.Services.AddScoped<StudentRepository>();
builder.Services.AddScoped<ClassRepository>();
builder.Services.AddScoped<TeacherRepository>();
builder.Services.AddScoped<SubjectRepository>();
builder.Services.AddScoped<TeachesRepository>();
builder.Services.AddScoped<DiaryRepository>();
builder.Services.AddScoped<NoteRepository>();
builder.Services.AddScoped<ParentRepository>();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<UpdateDiaryRepository>();
builder.Services.AddScoped<PrincipalRepository>();

// ✅ ADD THIS LINE (IMPORTANT FIX)
builder.Services.AddScoped<ComplaintRepository, ComplaintRepository>();
builder.Services.AddScoped<GardenerRepository>();
builder.Services.AddScoped<GardenRepository>();
builder.Services.AddScoped<FlowerRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();