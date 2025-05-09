using Transactional_Outbox_Pattern_Use_Api.Services;
using Quartz;
using Transactional_Outbox_Pattern_Use_Api.Data;
using Microsoft.EntityFrameworkCore;
using Transactional_Outbox_Pattern_Use_Api.Jobs;
using Transactional_Outbox_Pattern_Use_Api.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
     sqlOptions => sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
    );
});

builder.Services.AddTransient<EmployeeService>();
builder.Services.AddTransient<EmailService>();


builder.Services.AddQuartz(q =>
{
    // Use Microsoft Dependency Injection for job creation
    q.UseMicrosoftDependencyInjectionJobFactory();

    // Define a job key for the SendEmailsJob
    var jobKey = new JobKey(nameof(SendEmailsJob));

    // Register the job with the DI container
    q.AddJob<SendEmailsJob>(opts => opts.WithIdentity(jobKey));

    // Create a trigger for the job
    q.AddTrigger(opts => opts
        .ForJob(jobKey) // Link the trigger to the job
        .WithIdentity($"{nameof(SendEmailsJob)}-trigger") // Unique trigger name
        .WithCronSchedule("0/10 * * * * ?")); // Run every 10 seconds
});

// Add the Quartz Hosted Service to manage the job lifecycle
builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true; // Wait for jobs to complete on shutdown
});


builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
