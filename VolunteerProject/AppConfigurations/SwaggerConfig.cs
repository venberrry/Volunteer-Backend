namespace VolunteerProject.AppConfigurations;

public static class SwaggerConfig
{
    public static void SwaggerConfigurator(WebApplication webApplication)
    {
        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseDeveloperExceptionPage();
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Volunteer API v1");
                c.InjectStylesheet("/swagger-ui/dark-theme.css");
            });
        }
        else
        {
            webApplication.UseExceptionHandler("/Home/Error");
            webApplication.UseHsts();
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Volunteer API v1");
            });
        }
    }
}