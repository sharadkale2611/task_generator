using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using task_generator.AI;
using task_generator.AI.Fake;
using task_generator.Data;
using task_generator.Dto;
using task_generator.Mappings;
using task_generator.Middlewares;
using task_generator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection"))
);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: true)
        );

        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;

        // 🔥 ADD THIS (CRITICAL FIX)
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// custom services
builder.Services.AddScoped<ITechStackService, TechStackService>();
builder.Services.AddScoped<IProjectCategoryService, ProjectCategoryService>();
builder.Services.AddScoped<IProjectDomainService, ProjectDomainService>();
builder.Services.AddScoped<IEpicService, EpicService>();
builder.Services.AddScoped<ISprintService, SprintService>();
builder.Services.AddScoped<IWorkItemService, WorkItemService>();
builder.Services.AddScoped<IWorkItemAssignmentService, WorkItemAssignmentService>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEvaluationService, EvaluationService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// external API services
builder.Services.AddHttpClient<IGitHubService, GitHubService>();

// Remove this Later - reference only
builder.Services.AddScoped<IProjectGeneratorService, ProjectGeneratorService>();
builder.Services.AddScoped<IAiClient, FakeAiClient>();

// Prompt Builders
builder.Services.AddScoped<IEpicCreateResponse, FakeEpicCreateResponse>();
builder.Services.AddScoped<ISprintCreateResponse, FakeSprintCreateResponse>();
builder.Services.AddScoped<ITaskCreateResponse, FakeTaskCreateResponse>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var response = HttpResponseDto<object>.FailureResponse(
            "Validation failed",
            errors
        );

        return new BadRequestObjectResult(response);
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:3000") // frontend URL
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
//app.UseMiddleware<ResponseWrapperMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");
app.UseAuthorization();

app.MapControllers();

app.Run();
